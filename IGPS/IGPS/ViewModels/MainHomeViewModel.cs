using IGPS.Models;
using System;
using System.Collections.Generic;
using System.Text;
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

            CalcProgress();
        }

        private void CalcProgress()
        {
            int total = AppEnvironment.dataService.voiceDataItems.Count;
            int count = AppEnvironment.dataService.voiceDataItems.Count(n => n.IsRecorded);

            Progress = count / (float)total;
        }
    }
}
