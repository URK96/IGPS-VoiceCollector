using IGPS.Models;

using System.Linq;

namespace IGPS.ViewModels
{
    class MainHomeViewModel : BaseViewModel
    {
        public UserInfo User => AppEnvironment.authService.AuthenticatedUser;
        public float Progress { get; set; }
        public float ConvertProgress => Progress * 100;

        public MainHomeViewModel()
        {
            Title = AppResources.Main_MenuPage_Home;

            AppEnvironment.dataService = new Services.DataService();
        }

        internal void CalcProgress()
        {
            int total = 0;
            int count = 0;

            foreach (var values in AppEnvironment.dataService.voiceStatusData.Values)
            {
                total += values.Length;
                count += values.Count(n => (n == 3));
            }

            Progress = count / (float)total;
        }
    }
}
