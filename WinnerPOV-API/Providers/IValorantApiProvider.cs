namespace WinnerPOV_API.Providers
{
    public interface IValorantApiProvider
    {
 
        Task DownloadLatestMatchesAsync();

        Task UpdatePlayersAsync();

        Task UpdateTeamAsync();

    }
}