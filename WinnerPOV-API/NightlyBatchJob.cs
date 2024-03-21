using WinnerPOV_API.Providers;

namespace WinnerPOV_API
{
    public class NightlyBatchJob
    {
        private readonly IValorantApiProvider _valorantApiProvider;
        private readonly Timer _timer;

        private const int Timeout = 86400000;

        public NightlyBatchJob()
        {
            _valorantApiProvider = new HenrikApiProvider();
            _timer = new Timer(DataDownload, null, 0, Timeout);

        }

        public void DataDownload(object? state)
        {
            _valorantApiProvider.DownloadTeamAsync();
        }

    }
}
