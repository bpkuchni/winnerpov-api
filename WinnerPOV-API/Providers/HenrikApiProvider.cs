using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using System;
using System.Diagnostics;


using WinnerPOV_API.Database;

namespace WinnerPOV_API.Providers
{
    /// <summary>
    /// This IValorantApiProvider implementation uses the Henrik API
    ///     https://app.swaggerhub.com/apis-docs/Henrik-3/HenrikDev-API/3.0.0#/
    /// </summary>
    public class HenrikApiProvider : IValorantApiProvider
    {

        private const string HenrikUrl = "https://api.henrikdev.xyz";

        private const string TeamID = "fa8fc079-76d3-4afd-a0f1-335fbcf1f28b";
        private const string TeamName = "winner pov";
        private const string TeamTag = "wubrg";

        private const string Affinity = "na";

        private readonly HttpClient _httpClient;
        private readonly ValorantContext _dbContext;

        public HenrikApiProvider()
        {
            _httpClient = new HttpClient();
            _dbContext = new ValorantContext();
        }

        /// <summary>
        /// Retrieves the following for the current season
        ///     wins
        ///     losses
        ///     score
        ///     ranking
        ///     Henrik Team ID
        ///     
        /// 'https://api.henrikdev.xyz/valorant/v1/premier/search?name={team_name}&tag={team_tag}'
        /// </summary>
        /// <returns></returns>
        public async Task DownloadTeamAsync()
        {
            try
            {

                string url = $"{HenrikUrl}/valorant/v1/premier/search?name={TeamName}&tag={TeamTag}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                string content = await response.Content.ReadAsStringAsync();
                dynamic? obj = JsonConvert.DeserializeObject(content);

                Season? currentSeason = _dbContext.Seasons.FirstOrDefault(it => it.StartDate < DateTime.Now && it.EndDate > DateTime.Now);
                if (currentSeason != null)
                {
                    currentSeason.Wins = (int?)obj.data[0].wins.Value;
                    currentSeason.Losses = (int?)obj.data[0].losses.Value;
                    currentSeason.Ranking = (int?)obj.data[0].ranking.Value;
                    currentSeason.Score = (int?)obj.data[0].score.Value;
                }

                _dbContext.SaveChanges();

                DownloadTeamHistoryAsync();
            }
            catch (Exception ex)
            {
                var lol = 5;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task DownloadTeamHistoryAsync()
        {
            string url = $"{HenrikUrl}/valorant/v1/premier/{TeamID}/history";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            dynamic? obj = JsonConvert.DeserializeObject(content);

            List<(int, string)> matchIds = new List<(int, string)>();

            //Find Mapping
            foreach (dynamic dynMatch in obj.data.league_matches)
            {
                string matchId = dynMatch.Id.Value;
                HenrikMatchMapping? mapping = _dbContext.HenrikMatchMappings.FirstOrDefault(it => it.HenrikId == matchId);

                if (mapping == null)
                {

                    mapping = new HenrikMatchMapping()
                    {
                        HenrikId = matchId,
                        Match = new Match()
                        {
                            Date = DateTime.Parse(dynMatch.started_at.Value)
                        }
                    };

                    _dbContext.SaveChanges();
                    matchIds.Add((mapping.MatchId, matchId));
                }


            }

            DownloadMatchAsync(matchIds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
        private async Task DownloadMatchAsync(List<(int, string)> matchIds)
        {
            foreach ((int, string) tuple in matchIds)
            {
                string url = $"{HenrikUrl}/valorant/v2/match/{tuple.Item2}";
                HttpResponseMessage response = await _httpClient.GetAsync(url);
                string content = await response.Content.ReadAsStringAsync();
                dynamic? obj = JsonConvert.DeserializeObject(content);

                bool isRedTeam = obj.data.teams.red.roster.id.Value == TeamID;

                dynamic? team = isRedTeam ? obj.data.teams.red : obj.data.teams.blue;
                dynamic? opponent = isRedTeam ? obj.data.teams.blue : obj.data.teams.red;
                dynamic? teamPlayers = isRedTeam ? obj.players.red : obj.players.blue;
                dynamic? metadata = obj.data.metadata;

                //Handle Map
                string map = metadata.map;
                Map mapObj = _dbContext.Maps.FirstOrDefault(it => it.Name.Equals(map, StringComparison.OrdinalIgnoreCase));
                if (mapObj == null)
                {
                    _dbContext.Maps.Add(new Map()
                    {
                        Name = map,
                    });
                }


                //Handle Match
                Match? match = _dbContext.Matches.Find(tuple.Item1);

                match.Duration = new DateTime((long)metadata.game_length.Value);
                match.Rounds = (int?)metadata.rounds_played.Value;
                match.OurScore = (int?)team.rounds_won.Value;
                match.TheirScore = (int?)team.rounds_lost.Value;
                match.OpponentTag = opponent.roster.tag.Value;
                match.OpponentName  = opponent.roster.name.Value;
                match.OpponentImageUrl = opponent.roster.customization.image.Value;

                _dbContext.SaveChanges();

                //Handle Players, Agents, Ranks
                foreach (dynamic player in teamPlayers)
                {
                    DownloadPlayerAsync(player, tuple.Item1);
                }
            }
        }

        /// <summary>
        /// 
        /// 
        /// 'https://api.henrikdev.xyz/valorant/v1/by-puuid/mmr/{affinity}/{player_id}' 
        /// </summary>
        /// <param name="henrikPlayerID"></param>
        /// <returns></returns>
        private async Task DownloadPlayerAsync(dynamic player, int matchId)
        {
            string playerHenrikId = player.puuid.Value;
            HenrikPlayerMapping? mapping = _dbContext.HenrikPlayerMappings.FirstOrDefault(it => it.HenrikId == playerHenrikId);
            if(mapping == null)
            {
                mapping = new HenrikPlayerMapping()
                {
                    HenrikId = playerHenrikId,
                    Player = new Player()
                };

                _dbContext.HenrikPlayerMappings.Add(mapping);
                _dbContext.SaveChanges();
            } 

            Player playerObj = mapping.Player;

            //Agent
            string agentName = player.character.Value;
            Agent? agent = _dbContext.Agents.FirstOrDefault(it => it.Name.Equals(agentName, StringComparison.OrdinalIgnoreCase));
            if(agent == null)
            {
                agent = new Agent() { Name = agentName, PortraitUrl = player.agent.full.Value, ThumbnailUrl = player.agent.small.Value };
                _dbContext.SaveChanges();
            }          

            //TODO - player images could be point in time with matches
            playerObj.ThumbnailUrl = player.assets.card.small.Value;
            playerObj.PortraitUrl = player.assets.card.large.Value;
            playerObj.LandscapeUrl = player.assets.card.wide.Value;


            playerObj.Level = (int?)player.level.Value;

            PlayerMatch stats = new PlayerMatch()
            {
                MatchId = matchId,
                PlayerId = mapping.Player.PlayerId,
                AgentId = agent.AgentId,
                Score = (int?)player.stats.score.Value,
                Kills = (int?)player.stats.kills.Value,
                Deaths = (int?)player.stats.deaths.Value,
                Assists = (int?)player.stats.assists.Value,
                BodyShots = (int?)player.stats.bodyshots.Value,
                HeadShots = (int?)player.stats.headshots.Value,
                LegShots = (int?)player.stats.legshots.Value,
                Dealt = (int?)player.damage_made.Value,
                Received = (int?)player.damage_received.Value,
            };

            string url = $"{HenrikUrl}/valorant/v1/by-puuid/mmr/{Affinity}/{playerHenrikId}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            dynamic? obj = JsonConvert.DeserializeObject(content);

            string rank = obj.data.currenttierpatched.Value;
            Rank? rankObj = _dbContext.Ranks.FirstOrDefault(it => it.Name.Equals(rank, StringComparison.OrdinalIgnoreCase));

            if(rankObj == null)
            {
                rankObj = new Rank()
                {
                    Name = rank,
                    IconSmallUrl = obj.data.images.small.Value,
                    IconBigUrl = obj.data.images.large.Value,
                };
            }

            playerObj.MatchmakingRating = (int?)obj.data.elo.Value;

            _dbContext.SaveChanges();
        }

    }
}
