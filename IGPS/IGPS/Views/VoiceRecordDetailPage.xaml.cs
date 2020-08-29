using IGPS.Models;
using IGPS.Services.Server;
using IGPS.ViewModels;

using Plugin.AudioRecorder;
using Plugin.SimpleAudioPlayer;

using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;

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
        private ISimpleAudioPlayer audioPlayer;

        private FileStream voiceFileStream;

        public VoiceRecordDetailPage(VoiceListItem item)
        {
            InitializeComponent();

            recordFileName = $"{item.Section}_{item.Number}.mp3";
            recordFilePath = Path.Combine(AppEnvironment.appDataPath, AppEnvironment.authService.AuthenticatedUser.GetUserString(), recordFileName);

            BindingContext = new VoiceRecordDetailViewModel(item);

            UpdateButtonStatus();
        }

        private void UpdateButtonStatus()
        {
            bool hasRecordedFile = (BindingContext as VoiceRecordDetailViewModel).CheckRecordedFile();

            PlayButton.IsEnabled = hasRecordedFile;
            RecordButton.Text = hasRecordedFile ? AppResources.Re_Record : AppResources.Record;
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
                    TotalAudioTimeout = TimeSpan.FromMinutes(Preferences.Get(AppSettingKeys.RecordTimeOut, 5)),
                };

                recorder.AudioInputReceived += Recorder_AudioInputReceived;
            }

            if (recorder.IsRecording)
            {
                recorder.StopRecording();

                RecordStatusLabel.IsVisible = false;
            }
            else
            {
                RecordButton.Text = AppResources.StopRecord;

                recorder.StartRecording();

                RecordStatusLabel.IsVisible = true;
            }
        }

        protected override void OnDisappearing()
        {
            if ((recorder != null) && recorder.IsRecording)
            {
                recorder.StopRecording();

                isSuddenStop = true;
            }

            base.OnDisappearing();
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

                DependencyService.Get<IToast>().Show(AppResources.RecordSuccess);
            }
            catch (Exception ex)
            {
#if DEBUG
                DependencyService.Get<IToast>().Show(ex.ToString());
#endif
                DependencyService.Get<IToast>().Show(AppResources.RecordFail_Rerecord);

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
#if DEBUG
                DependencyService.Get<IToast>().Show(ex.ToString());
#endif
                DependencyService.Get<IToast>().Show(AppResources.UploadFail_ReUpload);

                UploadButton.IsEnabled = true;
            }
            finally
            {
                UploadButton.Text = AppResources.Upload;
                RecordButton.IsEnabled = true;
                RecordButton.Text = File.Exists(recordFilePath) ? AppResources.Re_Record : AppResources.Record;
            }

            try
            {
                (BindingContext as VoiceRecordDetailViewModel).UpdateItemInfo(isRecorded, isUploaded);
            }
            catch (Exception)
            {
                DependencyService.Get<IToast>().Show("Cannot update info");
            }
            finally
            {
                UploadButton.IsEnabled = true;
                UploadButton.Text = isUploaded ? AppResources.ReUpload : AppResources.Upload;

                UpdateButtonStatus();

                recorder = null;
            }
        }

        private void UploadButton_Clicked(object sender, EventArgs e)
        {
            _ = UploadFile();
        }

        private void PlayButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (audioPlayer == null)
                {
                    audioPlayer = CrossSimpleAudioPlayer.Current;
                }

                if (audioPlayer.IsPlaying)
                {
                    audioPlayer.Stop();

                    RecordButton.IsEnabled = true;
                    UploadButton.IsEnabled = true;

                    PlayButton.Text = AppResources.Play;

                    voiceFileStream?.Close();
                }
                else
                {
                    if (!File.Exists(recordFilePath))
                    {
                        if (!FTPService.DownloadFile(AppEnvironment.dataService.ServerUserDataDirPath, recordFilePath))
                        {
                            throw new Exception("Cannot download record file");
                        }
                    }

                    voiceFileStream?.Close();
                    voiceFileStream = new FileStream(recordFilePath, FileMode.Open, FileAccess.Read);

                    audioPlayer.Load(voiceFileStream);
                    audioPlayer.Play();
                    audioPlayer.PlaybackEnded += delegate
                    {
                        RecordButton.IsEnabled = true;
                        UploadButton.IsEnabled = true;

                        PlayButton.Text = AppResources.Play;

                        voiceFileStream?.Close();
                    };

                    RecordButton.IsEnabled = false;
                    UploadButton.IsEnabled = false;

                    PlayButton.Text = AppResources.Stop;
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().Show(ex.ToString());
            }
        }
    }
}