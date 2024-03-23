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
            //_valorantApiProvider = new HenrikApiProvider();
            //TODO: Delay until midnight cause its cool

            _timer = new Timer(DataDownloadAsync, null, 0, Timeout);

        }

        public async void DataDownloadAsync(object? state)
        {
            //TODO - Download Latest Matches
            //_valorantApiProvider.DownloadLatestMatchesAsync();

            ////TODO - Update Players
            //_valorantApiProvider.UpdatePlayersAsync();

            ////TODO - Update Team Stats
            //_valorantApiProvider.UpdateTeamAsync();
        }

    }
}
