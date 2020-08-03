using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IGPS.ViewModels;
using IGPS.Models;
using Plugin.AudioRecorder;
using Xamarin.Essentials;
using System.IO;
using IGPS.Services.Server;

namespace IGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VoiceRecordDetailPage : ContentPage
    {
        private readonly string recordFileName;
        private readonly string recordFilePath;

        private AudioRecorderService recorder;

        public VoiceRecordDetailPage(VoiceDataItem item)
        {
            InitializeComponent();

            recordFileName = $"{item.Section}_{item.Number}.mp3";
            recordFilePath = Path.Combine(AppEnvironment.appDataPath, AppEnvironment.authService.AuthenticatedUser.GetUserString(), recordFileName);

            BindingContext = new VoiceRecordDetailViewModel(item);
        }

        private void RecordButton_Clicked(object sender, EventArgs e)
        {
            if (recorder == null)
            {
                recorder = new AudioRecorderService()
                {
                    FilePath = recordFilePath,
                    StopRecordingOnSilence = false,
                    StopRecordingAfterTimeout = true,
                    TotalAudioTimeout = TimeSpan.FromMinutes(Preferences.Get(AppSettingKeys.RecordTimeOut, 5))
                };

                recorder.AudioInputReceived += Recorder_AudioInputReceived;
            }

            if (recorder.IsRecording)
            {
                recorder.StopRecording();
            }
            else
            {
                RecordButton.Text = AppResources.StopRecord;

                recorder.StartRecording();
            }
        }

        private void Recorder_AudioInputReceived(object sender, string audioFile)
        {
            bool isSuccess = false;

            try
            {
                RecordButton.IsEnabled = false;
                RecordButton.Text = AppResources.Saving;

                if (string.IsNullOrEmpty(audioFile))
                {
                    throw new Exception("No recorded file");
                }

                isSuccess = true;
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().Show(ex.ToString());
            }

            _ = UploadFile(isSuccess);
        }

        private async Task UploadFile(bool isRecordSuccess)
        {
            string serverDirPath = AppEnvironment.dataService.ServerUserDataDirPath;
            bool isSuccess = false;

            await Task.Delay(10);

            try
            {
                UploadButton.IsEnabled = false;
                UploadButton.Text = AppResources.Uploading;

                if (!FTPService.CheckDirExist(serverDirPath))
                {
                    FTPService.CreateDir(serverDirPath);
                }

                isSuccess = FTPService.UploadFile(recordFilePath, Path.Combine(serverDirPath, Path.GetFileName(recordFilePath)));

                if (!isSuccess)
                {
                    throw new Exception("Cannot upload file");
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().Show(ex.ToString());
            }
            finally
            {
                UploadButton.IsEnabled = true;
                UploadButton.Text = isSuccess ? AppResources.ReUpload : AppResources.Upload;

                RecordButton.IsEnabled = true;
                RecordButton.Text = File.Exists(recordFilePath) ? AppResources.Re_Record : AppResources.Record;

                (BindingContext as VoiceRecordDetailViewModel).UpdateItemInfo(isRecordSuccess, isSuccess);
            }
        }
    }
}