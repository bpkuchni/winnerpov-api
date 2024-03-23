using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json;

using System;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

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

        public HenrikApiProvider(HttpClient client, ValorantContext dbContext)
        {
            _httpClient = client;
            _dbContext = dbContext;
        }

        public HenrikApiProvider(IHttpClientFactory clientFactory, ValorantContext dbContext)
        {
            _httpClient = clientFactory.CreateClient();
            _dbContext = dbContext;
        }

        #region Private

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
        private async Task DownloadMatchAsync(List<(int, string)> matchIds)
        {
            try
            {

                foreach ((int, string) tuple in matchIds)
                {
                    string url = $"{HenrikUrl}/valorant/v2/match/{tuple.Item2}";
                    HttpResponseMessage response = await _httpClient.GetAsync(url);
                    string content = await response.Content.ReadAsStringAsync();
                    dynamic? matchObj = JsonConvert.DeserializeObject(content);

                    bool isRedTeam = matchObj?.data.teams.red.roster.id.Value == TeamID;

                    dynamic? team = isRedTeam ? matchObj?.data.teams.red : matchObj?.data.teams.blue;
                    dynamic? opponent = isRedTeam ? matchObj?.data.teams.blue : matchObj?.data.teams.red;
                    dynamic? teamPlayers = isRedTeam ? matchObj?.data.players.red : matchObj?.data.players.blue;
                    dynamic? metadata = matchObj?.data.metadata;

                    //Handle Map
                    string mapName = metadata?.map;
                    int mapId = await AddOrGetMapAsync(mapName);

                    //Handle Match
                    Match? match = _dbContext.Matches.Find(tuple.Item1);
                    if (match != null)
                    {
                        match.MapId = mapId;
                        match.Duration = (int?)metadata?.game_length.Value;
                        match.Rounds = (int?)metadata?.rounds_played.Value;
                        match.OurScore = (int?)team?.rounds_won.Value;
                        match.TheirScore = (int?)team?.rounds_lost.Value;
                        match.OpponentTag = opponent?.roster.tag.Value;
                        match.OpponentName = opponent?.roster.name.Value;
                        match.OpponentImageUrl = opponent?.roster.customization.image.Value;

                        await _dbContext.SaveChangesAsync();
                    }


                    //Handle Players, Agents, Ranks
                    foreach (dynamic player in teamPlayers)
                    {
                        await DownloadPlayerMatchStatsAsync(player, tuple.Item1);
                    }
                }

            }
            catch (Exception ex)
            {
                var lol = 5;
            }
        }

        /// <summary>
        ///     Downloads player's match stats into database
        ///     Create new Henrik Player Mapping if needed
        ///     Create new Agent if needed
        /// </summary>
        /// <param name="player">dynamic object representing the player</param>
        /// <param name="matchId">our match id</param>
        /// <returns></returns>
        private async Task DownloadPlayerMatchStatsAsync(dynamic playerObj, int matchId)
        {
            //Create new player if need be
            string playerHenrikId = playerObj.puuid.Value;
            int playerId = await AddOrGetHenrikPlayerAsync(playerHenrikId);
            await UpdatePlayerAsync(playerObj, playerId);

            //Create new agent if need be
            string agentName = playerObj.character.Value;
            int agentId = await AddOrGetAgentAsync(agentName, playerObj.assets.agent.full.Value, playerObj.assets.agent.small.Value);

            //Create player stats object
            PlayerMatch stats = new PlayerMatch()
            {
                MatchId = matchId,
                PlayerId = playerId,
                AgentId = agentId,
                Score = (int?)playerObj.stats.score.Value,
                Kills = (int?)playerObj.stats.kills.Value,
                Deaths = (int?)playerObj.stats.deaths.Value,
                Assists = (int?)playerObj.stats.assists.Value,
                BodyShots = (int?)playerObj.stats.bodyshots.Value,
                HeadShots = (int?)playerObj.stats.headshots.Value,
                LegShots = (int?)playerObj.stats.legshots.Value,
                Dealt = (int?)playerObj.damage_made.Value,
                Received = (int?)playerObj.damage_received.Value,
            };

            //Update database
            _dbContext.Update(stats);
            _dbContext.PlayerMatches.Add(stats);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Add agent to database if not exist
        /// return agent id
        /// </summary>
        /// <param name="agentName">Name of Agent</param>
        /// <param name="agentPortraitUrl">Portrait Image Url</param>
        /// <param name="agentThumbnailUrl">Thumbnail Image Url</param>
        /// <returns>Agent ID</returns>
        private async Task<int> AddOrGetAgentAsync(string agentName, string agentPortraitUrl, string agentThumbnailUrl)
        {
            Agent? agent = _dbContext.Agents.FirstOrDefault(it => it.Name.Equals(agentName, StringComparison.OrdinalIgnoreCase));
            if (agent == null)
            {
                agent = new Agent() { Name = agentName, PortraitUrl = agentPortraitUrl, ThumbnailUrl = agentThumbnailUrl };
                _dbContext.Update(agent);
                await _dbContext.SaveChangesAsync();
            }

            return agent.AgentId;
        }

        /// <summary>
        /// Creates a new Player object with Henrik Mapping if one doesn't exist
        /// Returns the player id
        /// </summary>
        /// <param name="henrikPlayerId">Henrik Player ID</param>
        /// <returns>Player ID</returns>
        private async Task<int> AddOrGetHenrikPlayerAsync(string henrikPlayerId)
        {
            HenrikPlayerMapping? mapping = _dbContext.HenrikPlayerMappings.FirstOrDefault(it => it.HenrikId == henrikPlayerId);
            if (mapping == null)
            {
                mapping = new HenrikPlayerMapping()
                {
                    HenrikId = henrikPlayerId,
                    Player = new Player()
                };

                _dbContext.Update(mapping);
                await _dbContext.SaveChangesAsync();
            }

            return mapping.PlayerId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="henrikMatchId"></param>
        /// <param name="date"></param>
        /// <returns>Match ID and whether its new</returns>
        private async Task<(int, bool)> AddOrGetHenrikMatchAsync(string henrikMatchId, DateTime date)
        {
            HenrikMatchMapping? mapping = await _dbContext.HenrikMatchMappings.FirstOrDefaultAsync(it => it.HenrikId == henrikMatchId);
            bool isNewMatch = mapping == null;
            if (mapping == null)
            {
                mapping = new HenrikMatchMapping()
                {
                    HenrikId = henrikMatchId,
                    Match = new Match()
                    {
                        Date = date
                    }
                };

                _dbContext.Update(mapping);
                await _dbContext.SaveChangesAsync();
            }

            return (mapping.MatchId, isNewMatch);
        }

        private async Task<int> AddOrGetRankAsync(string rankName, string smallUrl, string bigUrl)
        {
            Rank? rank = _dbContext.Ranks.FirstOrDefault(it => it.Name.Equals(rankName, StringComparison.OrdinalIgnoreCase));

            if (rank == null)
            {
                rank = new Rank()
                {
                    Name = rankName,
                    IconSmallUrl = smallUrl,
                    IconBigUrl = bigUrl,
                };

                _dbContext.Update(rank);
                await _dbContext.SaveChangesAsync();
            }

            return rank.RankId;
        }

        private async Task<int> AddOrGetMapAsync(string mapName)
        {
            Map? map = await _dbContext.Maps.FirstOrDefaultAsync(it => it.Name.Equals(mapName, StringComparison.OrdinalIgnoreCase));
            if (map == null)
            {
                map = new Map()
                {
                    Name = mapName,
                };

                _dbContext.Update(map);
                await _dbContext.SaveChangesAsync();
            }

            return map.MapId;
        }

        /// <summary>
        /// Minified version of update player for matches only
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="playerObj"></param>
        /// <returns></returns>
        private async Task UpdatePlayerAsync(int playerId, dynamic playerObj)
        {
            Player? player = await _dbContext.Players.FindAsync(playerId);
            player.Name = playerObj.name.Value;
            player.Tag = playerObj.tag.Value;

            await _dbContext.SaveChangesAsync();
        }

        #endregion


        #region Public API
        public async Task DownloadLatestMatchesAsync()
        {
            string url = $"{HenrikUrl}/valorant/v1/premier/{TeamID}/history";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            dynamic? historyObj = JsonConvert.DeserializeObject(content);

            List<(int, string)> matchIds = new List<(int, string)>();

            //Find New Matches
            foreach (dynamic matchObj in historyObj.data.league_matches)
            {
                string henrikMatchId = matchObj.id.Value;
                DateTime date = matchObj.started_at.Value;
                (int, bool) result = await AddOrGetHenrikMatchAsync(henrikMatchId, date);

                if (result.Item2)
                {
                    matchIds.Add((result.Item1, henrikMatchId));
                }

            }

            await DownloadMatchAsync(matchIds);
        }

        public async Task UpdatePlayerAsync(int playerId)
        {
            Player? player = await _dbContext.Players.FindAsync(playerId);
            string henrikPlayerId = player.HenrikPlayerMappings.First().HenrikId;

            string playerUrl = $"{HenrikUrl}/valorant/v1/by-puuid/account/{henrikPlayerId}";
            HttpResponseMessage playerResponse = await _httpClient.GetAsync(playerUrl);
            string playerContent = await playerResponse.Content.ReadAsStringAsync();
            dynamic? playerObj = JsonConvert.DeserializeObject(playerContent);

            player.ThumbnailUrl = playerObj.card.small.Value;
            player.PortraitUrl = playerObj.card.large.Value;
            player.LandscapeUrl = playerObj.card.wide.Value;
            player.Level = (int?)playerObj.account_level.Value;
            player.Name = playerObj.name.Value;
            player.Tag = playerObj.tag.Value;

            //MMR Request
            string url = $"{HenrikUrl}/valorant/v1/by-puuid/mmr/{Affinity}/{player.HenrikPlayerMappings.First().HenrikId}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            dynamic? mmrObj = JsonConvert.DeserializeObject(content);

            player.MatchmakingRating = (int?)mmrObj?.data.elo.Value;

            //Rank
            string? rank = mmrObj?.data.currenttierpatched?.Value;
            int rankId = await AddOrGetRankAsync(rank, mmrObj?.data.images.small.Value, mmrObj?.data.images.large.Value);
            player.RankId = rankId;

            await _dbContext.SaveChangesAsync();
        }



        public async Task UpdatePlayersAsync()
        {
            foreach (Player player in _dbContext.Players)
            {
                await UpdatePlayerAsync(player.PlayerId);
            }
        }

        public async Task UpdatePlayerSeasonalStats()
        {
            Season season = await _dbContext.Seasons.FirstOrDefaultAsync(it => it.StartDate < DateTime.Now && it.EndDate > DateTime.Now);


            if (season == null)
            {
                foreach (Player player in _dbContext.Players)
                {
                    player.CurrentSeasonEligible = null;
                    player.CurrentSeasonScore = null;
                    await _dbContext.SaveChangesAsync();
                    return;
                }
            }


            List<Match> matches = await _dbContext.Matches.Where(it => it.Date < season.EndDate && it.Date > season.StartDate).ToListAsync();
            Dictionary<int, int> playerMatchCounts = new Dictionary<int, int>();
            Dictionary<int, int> playerMatchScores = new Dictionary<int, int>();
            foreach (Match match in matches)
            {
                int matchScore = 0;
                foreach (PlayerMatch playerMatch in match.PlayerMatches)
                {
                    matchScore += playerMatch.Score ?? 0;
                }
            }
        }

        public async Task UpdateTeamAsync()
        {
            string url = $"{HenrikUrl}/valorant/v1/premier/search?name={TeamName}&tag={TeamTag}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            string content = await response.Content.ReadAsStringAsync();
            dynamic? teamObj = JsonConvert.DeserializeObject(content);

            Season? currentSeason = await _dbContext.Seasons.FirstOrDefaultAsync(it => it.StartDate < DateTime.Now && it.EndDate > DateTime.Now);
            if (currentSeason != null)
            {
                currentSeason.Wins = (int?)teamObj.data[0].wins.Value;
                currentSeason.Losses = (int?)teamObj.data[0].losses.Value;
                currentSeason.Ranking = (int?)teamObj.data[0].ranking.Value;
                currentSeason.Score = (int?)teamObj.data[0].score.Value;
            }

            await _dbContext.SaveChangesAsync();
        }

        #endregion
    }
}
