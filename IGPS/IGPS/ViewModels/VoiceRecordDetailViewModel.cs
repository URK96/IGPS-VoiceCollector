using System;
using System.Collections.Generic;
using System.Text;

using IGPS.Models;

namespace IGPS.ViewModels
{
    class VoiceRecordDetailViewModel : BaseViewModel
    {
        public VoiceDataItem Item { get; set; }

        public VoiceRecordDetailViewModel(VoiceDataItem item)
        {
            Item = item;

            Title = $"Section {Item.Section} - No. {Item.Number}";
        }
    }
}
