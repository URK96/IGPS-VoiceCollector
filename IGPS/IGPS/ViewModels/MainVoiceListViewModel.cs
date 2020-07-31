using IGPS.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace IGPS.ViewModels
{
    class MainVoiceListViewModel : BaseViewModel
    {
        public List<VoiceDataItem> Items { get; set; }

        public MainVoiceListViewModel(List<VoiceDataItem> list)
        {
            Title = AppResources.Main_MenuPage_RecordList;

            Items = list;
        }
    }
}
