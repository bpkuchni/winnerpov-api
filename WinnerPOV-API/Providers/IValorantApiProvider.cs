namespace WinnerPOV_API.Providers
{
    public interface IValorantApiProvider
    {
        Task DownloadMatchAsync(string matchID);
        Task DownloadPlayerAsync(string name, string tag);
        Task DownloadTeamAsync();
        Task DownloadTeamHistoryAsync();
    }
}