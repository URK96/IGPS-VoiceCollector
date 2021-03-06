﻿using IGPS.Models;

using System;

namespace IGPS.ViewModels
{
    class VoiceRecordDetailViewModel : BaseViewModel
    {
        public VoiceListItem Item { get; set; }

        public VoiceRecordDetailViewModel(VoiceListItem item)
        {
            Item = item;

            Title = $"{Item.Section}-{item.Chapter} No. {Item.Number}";
        }

        public void UpdateItemInfo(bool isRecorded, bool isUploaded)
        {
            int statusCode = 0;

            statusCode += isRecorded ? 1 : 0;
            statusCode += isUploaded ? 2 : 0;

            AppEnvironment.dataService.voiceStatusData[Item.Section][Item.Index] = statusCode;

            AppEnvironment.dataService.SaveVoiceStatus();
            AppEnvironment.dataService.UploadVoiceStatus();
        }

        public bool CheckRecordedFile()
        {
            try
            {
                return AppEnvironment.dataService.voiceStatusData[Item.Section][Item.Index] != 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
