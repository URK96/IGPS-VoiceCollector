using IGPS.Models;
using IGPS.Services.Server;
using IGPS.ViewModels;

using Plugin.AudioRecorder;

using System;
using System.IO;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VoiceRecordDetailPage : ContentPage
    {
        private readonly string recordFileName;
        private readonly string recordFilePath;

        private bool isRecorded = false;
        private bool isUploaded = false;
        private bool isSuddenStop = false;

        private AudioRecorderService recorder;

        public VoiceRecordDetailPage(VoiceListItem item)
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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (recorder.IsRecording)
            {
                recorder.StopRecording();

                isSuddenStop = true;
            }
        }

        private void Recorder_AudioInputReceived(object sender, string audioFile)
        {
            if (isSuddenStop)
            {
                return;
            }

            try
            {
                RecordButton.IsEnabled = false;
                RecordButton.Text = AppResources.Saving;

                if (string.IsNullOrEmpty(audioFile))
                {
                    throw new Exception("No recorded file");
                }

                isRecorded = true;
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().Show(ex.ToString());

                isRecorded = false;
            }

            _ = UploadFile();
        }

        private async Task UploadFile()
        {
            string serverDirPath = AppEnvironment.dataService.ServerUserDataDirPath;

            await Task.Delay(10);

            try
            {
                UploadButton.IsEnabled = false;
                UploadButton.Text = AppResources.Uploading;

                if (!FTPService.CheckDirExist(serverDirPath))
                {
                    FTPService.CreateDir(serverDirPath);
                }

                isUploaded = FTPService.UploadFile(recordFilePath, Path.Combine(serverDirPath, Path.GetFileName(recordFilePath)));

                if (!isUploaded)
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
                RecordButton.IsEnabled = true;
                RecordButton.Text = File.Exists(recordFilePath) ? AppResources.Re_Record : AppResources.Record;
            }

            try
            {
                (BindingContext as VoiceRecordDetailViewModel).UpdateItemInfo(isRecorded, isUploaded);
            }
            catch (Exception)
            {

            }
            finally
            {
                UploadButton.IsEnabled = true;
                UploadButton.Text = isUploaded ? AppResources.ReUpload : AppResources.Upload;
            }
        }
    }
}