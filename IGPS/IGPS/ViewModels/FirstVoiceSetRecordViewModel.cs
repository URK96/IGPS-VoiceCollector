using IGPS.Models;

using System;

namespace IGPS.ViewModels
{
    class FirstVoiceSetRecordViewModel : BaseViewModel
    {
        public VoiceListItem Item { get; set; }

        public FirstVoiceSetRecordViewModel(VoiceListItem item)
        {
            Item = item;

            Title = $"{Item.Section}-{item.Chapter} No. {Item.Number}";
        }

        public void UpdateItemInfo(bool isRecorded, bool isUploaded)
        {
            int statusCode = 0;

            statusCode += isRecorded ? 1 : 0;
            statusCode += isUploaded ? 2 : 0;

            AppEnvironment.dataService.firstVoiceSetStatusData[Item.Index] = statusCode;

            AppEnvironment.dataService.SaveFirstVoiceStatus();
            AppEnvironment.dataService.UploadFirstVoiceStatus();
        }

        public bool CheckRecordedFile()
        {
            try
            {
                return AppEnvironment.dataService.firstVoiceSetStatusData[Item.Index] != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
