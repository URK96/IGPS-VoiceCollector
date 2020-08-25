using IGPS.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace IGPS.ViewModels
{
    class FirstVoiceSetRecordViewModel : BaseViewModel
    {
        public int Stage { get; set; }
        public int Count { get; set; }

        public FirstVoiceSetRecordViewModel()
        {
            AppEnvironment.dataService = new DataService();

            Stage = 1;
            Count = 1;
        }
    }
}
