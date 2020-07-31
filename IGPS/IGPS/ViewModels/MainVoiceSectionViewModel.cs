using IGPS.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows.Input;

namespace IGPS.ViewModels
{
    class MainVoiceSectionViewModel : BaseViewModel
    {
        public List<VoiceDataItem> VoiceList => AppEnvironment.dataService.voiceDataItems;
        public IEnumerable<IGrouping<int, VoiceDataItem>> SectionGroup => from item in VoiceList group item by item.Section;
        //public List<string> SectionCount { get; set; }

        public MainVoiceSectionViewModel()
        {
            Title = AppResources.Main_MenuPage_RecordSection;

            //SectionCount = new List<string>();

            //CalcCount();
        }

        private void CalcCount()
        {
            foreach (var group in SectionGroup)
            {
                //SectionCount.Add($"{group.Count(n => n.IsRecorded)} / {group.Count()}");
            }
        }
    }
}
