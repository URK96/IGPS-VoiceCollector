using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using IGPS.Models;

using Xamarin.Forms;

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

        public void UpdateItemInfo(bool isRecorded, bool isUploaded)
        {
            const string RecordedIndex = "IsRecorded";
            const string UploadedIndex = "IsUploaded";

            try
            {
                int rowIndex = AppEnvironment.dataService.voiceDataTable.Rows.IndexOf(AppEnvironment.dataService.FindDataRow(Item.Section, Item.Number));

                AppEnvironment.dataService.voiceDataTable.Rows[rowIndex][RecordedIndex] = isRecorded;
                AppEnvironment.dataService.voiceDataTable.Rows[rowIndex][UploadedIndex] = isUploaded;

                AppEnvironment.dataService.voiceDataTable.AcceptChanges();
                AppEnvironment.dataService.voiceDataTable.WriteXml(AppEnvironment.dataService.LocalDataFilePath, System.Data.XmlWriteMode.WriteSchema);
                AppEnvironment.dataService.UpdateItem();

                if (!AppEnvironment.dataService.UploadTable())
                {
                    throw new Exception("Cannot upload table");
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().Show(ex.ToString());
            }
        }
    }
}
