using IGPS.Models;
using IGPS.Services.Server;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

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

        internal async Task UpdateStatus()
        {
            await Task.Delay(100);

            string serverDirPath = AppEnvironment.dataService.ServerUserDataDirPath + "/";

            foreach (var item in ListItems)
            {
                string serverFilePath = Path.Combine(serverDirPath, $"{item.Section}_{item.Chapter}_{item.Number}.mp3");

                if (FTPService.CheckFileExist(serverFilePath))
                {
                    item.IsUploaded = true;
                    item.CompletedText = AppResources.Successed;
                    item.UploadTextColor = Color.Green;
                }
                else
                {
                    item.IsUploaded = false;
                    item.CompletedText = AppResources.NonSuccessed;
                    item.UploadTextColor = Color.Red;
                }
            }
        }

        internal async Task UpdateStatus(VoiceListItem item)
        {
            await Task.Delay(100);

            string serverDirPath = AppEnvironment.dataService.ServerUserDataDirPath + "/";

            string serverFilePath = Path.Combine(serverDirPath, $"{item.Section}_{item.Chapter}_{item.Number}.mp3");

            if (FTPService.CheckFileExist(serverFilePath))
            {
                item.IsUploaded = true;
                item.CompletedText = AppResources.Successed;
                item.UploadTextColor = Color.Green;
            }
            else
            {
                item.IsUploaded = false;
                item.CompletedText = AppResources.NonSuccessed;
                item.UploadTextColor = Color.Red;
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
                        item.CompletedText = AppResources.NonSuccessed;
                        item.UploadTextColor = Color.Red;
                        break;
                    case 1:
                        item.IsRecorded = true;
                        item.IsUploaded = false;
                        item.CompletedText = AppResources.NonSuccessed;
                        item.UploadTextColor = Color.Red;
                        break;
                    case 2:
                        item.IsRecorded = false;
                        item.IsUploaded = true;
                        item.CompletedText = AppResources.Successed;
                        item.UploadTextColor = Color.Green;
                        break;
                    case 3:
                        item.IsRecorded = true;
                        item.IsUploaded = true;
                        item.CompletedText = AppResources.Successed;
                        item.UploadTextColor = Color.Green;
                        break;
                }
            }
        }
    }
}
