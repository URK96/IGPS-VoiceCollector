using IGPS.Models;

using System.Collections.Generic;

namespace IGPS.ViewModels
{
    class MainVoiceListViewModel : BaseViewModel
    {
        public List<VoiceListItem> ListItems { get; set; }

        private ChapterItem currentChapterItem;

        public MainVoiceListViewModel(ChapterItem item)
        {
            Title = AppResources.Main_MenuPage_RecordList;

            currentChapterItem = item;

            CreateItems();
        }

        private void CreateItems()
        {
            ListItems = new List<VoiceListItem>();

            for (int i = 0; i < currentChapterItem.Count; ++i)
            {
                var item = new VoiceListItem()
                {
                    Section = currentChapterItem.Section,
                    Chapter = currentChapterItem.Chapter,
                    Index = currentChapterItem.Range.start + i,
                    Number = i + 1
                };

                item.VoiceText = AppEnvironment.dataService.voiceTextData[item.Section][item.Index];

                ListItems.Add(item);
            }
        }

        internal void SetStatus()
        {
            for (int i = 0; i < ListItems.Count; ++i)
            {
                var item = ListItems[i];

                int statusCode = AppEnvironment.dataService.voiceStatusData[item.Section][item.Index];

                switch (statusCode)
                {
                    case 0:
                        item.IsRecorded = false;
                        item.IsUploaded = false;
                        break;
                    case 1:
                        item.IsRecorded = true;
                        item.IsUploaded = false;
                        break;
                    case 2:
                        item.IsRecorded = false;
                        item.IsUploaded = true;
                        break;
                    case 3:
                        item.IsRecorded = true;
                        item.IsUploaded = true;
                        break;
                }
            }
        }
    }
}
