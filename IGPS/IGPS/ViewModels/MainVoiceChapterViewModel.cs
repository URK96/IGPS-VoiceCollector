using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using IGPS.Models;

namespace IGPS.ViewModels
{
    class MainVoiceChapterViewModel : BaseViewModel
    {
        public List<ChapterItem> ChapterItems { get; set; }

        private int section;

        public MainVoiceChapterViewModel(int key)
        {
            Title = AppResources.Main_MenuPage_RecordChapter;

            section = key;

            CreateItems();
        }

        private void CreateItems()
        {
            for (int i = 0; i < 5; ++i)
            {
                (int start, int end) = (i * 50, (i + 1) * 50 - 1);

                ChapterItems.Add(new ChapterItem(section, i + 1, start, end));
            }

            for (int i = 0; i < 10; ++i)
            {
                (int start, int end) = (i * 100 + 250, (i + 1) * 100 - 1 + 250);

                ChapterItems.Add(new ChapterItem(section, i + 6, start, end));
            }
        }

        internal void CalcProgress()
        {
            for (int i = 0; i < ChapterItems.Count; ++i)
            {
                var item = ChapterItems[i];

                item.Progress = AppEnvironment.dataService.voiceStatusData[item.Section].Skip(item.Range.start).Take(item.Count).Count(n => (n == 3)) / (float)item.Count;
            }
        }
    }
}
