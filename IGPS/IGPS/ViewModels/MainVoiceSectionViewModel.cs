using IGPS.Models;

using System.Collections.Generic;
using System.Linq;

namespace IGPS.ViewModels
{
    class MainVoiceSectionViewModel : BaseViewModel
    {
        public List<SectionItem> SetionItems { get; set; }

        public MainVoiceSectionViewModel()
        {
            Title = AppResources.Main_MenuPage_RecordSection;

            CreateItems();
        }

        private void CreateItems()
        {
            SetionItems = new List<SectionItem>();

            for (int i = 0; i < AppEnvironment.dataService.voiceTextData.Keys.Count; ++i)
            {
                SetionItems.Add(new SectionItem(i + 1));
            }
        }

        internal void CalcProgress()
        {
            for (int i = 0; i < SetionItems.Count; ++i)
            {
                var section = SetionItems[i];

                section.Progress = AppEnvironment.dataService.voiceStatusData[section.Section].Count(n => (n == 3)) / (float)section.Count * 100;
            }
        }
    }
}
