using IGPS.Models;

using System.Collections.Generic;
using System.Linq;

namespace IGPS.ViewModels
{
    class MainVoiceSectionViewModel : BaseViewModel
    {
        public List<SectionItem> SectionItems { get; set; }

        public MainVoiceSectionViewModel()
        {
            Title = AppResources.Main_MenuPage_RecordSection;

            CreateItems();
        }

        private void CreateItems()
        {
            SectionItems = new List<SectionItem>();

            SectionItems.Add(new SectionItem(0, 9)); // First Voice Set Section

            for (int i = 1; i <= AppEnvironment.dataService.voiceTextData.Keys.Count; ++i)
            {
                SectionItems.Add(new SectionItem(i));
            }
        }

        internal void CalcProgress()
        {
            // First Voice Section Progress
            SectionItems[0].Progress = AppEnvironment.dataService.firstVoiceSetStatusData.Count(n => (n == 3)) / (float)SectionItems[0].Count * 100;

            for (int i = 1; i < SectionItems.Count; ++i)
            {
                var section = SectionItems[i];

                section.Progress = AppEnvironment.dataService.voiceStatusData[section.Section].Count(n => (n == 3)) / (float)section.Count * 100;
            }
        }
    }
}
