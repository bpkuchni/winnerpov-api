using WinnerPOV_API.Controllers;

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
 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task DownloadTeamHistoryAsync()
        {
            //https://api.henrikdev.xyz/valorant/v1/premier/{team_id}/history
            //Gets us Match ID and Match Date

            //Can also get us tournament information
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
        public async Task DownloadMatchAsync(string matchID)
        {
            //'https://api.henrikdev.xyz/valorant/v2/match/{match_id} 

            /* 
             * map - Mapping Table
             * duration
             * date
             * rounds played
             * 
             * players
             *  agent - Mapping Table
             *  score
             *  kills
             *  deaths
             *  assists
             *  bodyshots
             *  headshots
             *  legshots
             *  damagedealt
             *  damagetaken
             * 
             * 
             * 
             * tournament id - Mapping Table?
             * matchup id - part of that table?
             * 
             * rounds - ignore
             * kills - ignore
             * coaches - ignore
             * observers - ignore
             * */
        }

        /// <summary>
        /// 
        /// 
        /// 'https://api.henrikdev.xyz/valorant/v1/account/{player_name}/{player_tag}'
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public async Task DownloadPlayerAsync(string name, string tag)
        {
            /*
             * level
             * thumbnail
             * portrait
             * landscape
             * 
             */

            DownloadPlayerAsync("");
        }

        /// <summary>
        /// 
        /// 
        /// 'https://api.henrikdev.xyz/valorant/v1/by-puuid/mmr/{affinity}/{player_id}' 
        /// </summary>
        /// <param name="henrikPlayerID"></param>
        /// <returns></returns>
        private async Task DownloadPlayerAsync(string henrikPlayerID)
        {
            /*
             * rank name
             * rank small url
             * rank big url
             * elo
             * 
             */
        }

    }
}
