using IGPS.Models;
using IGPS.Services.Server;
using IGPS.ViewModels;

using Plugin.AudioRecorder;
using Plugin.SimpleAudioPlayer;

using System;
using System.IO;
using System.Timers;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.SimpleAudioRecorder;

namespace IGPS.Views.FirstVoiceSet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirstVoiceSetRecordPage : ContentPage
    {
        private readonly string recordFileName;
        private readonly string recordPath;
        private readonly string recordFilePath;

        private bool isRecorded = false;
        private bool isUploaded = true;
        private bool isSuddenStop = false;
        
        private int secCount = 0;

        private AudioRecorderService recorder;
        private ISimpleAudioPlayer audioPlayer;

        private FileStream voiceFileStream;

        private Timer timeTimer;

        public FirstVoiceSetRecordPage(VoiceListItem item)
        {
            InitializeComponent();

            recordFileName = $"{item.Chapter}_{item.Number}.mp3";
            recordPath = Path.Combine(AppEnvironment.appDataPath, AppEnvironment.authService.AuthenticatedUser.GetUserString());
            recordFilePath = Path.Combine(recordPath, recordFileName);

            BindingContext = new FirstVoiceSetRecordViewModel(item);

            timeTimer = new Timer(1000)
            {
                AutoReset = true
            };
            timeTimer.Elapsed += delegate
            {
                RecordCountProgressBar.Progress = secCount++ / 10.0;

                if (secCount > 10)
                {
                    timeTimer.Stop();
                    MainThread.BeginInvokeOnMainThread(() => { recorder.StopRecording(); });
                }
            };

            UpdateButtonStatus();
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

        private void UpdateButtonStatus()
        {
            bool hasRecordedFile = (BindingContext as FirstVoiceSetRecordViewModel).CheckRecordedFile();

            PlayButton.IsEnabled = true;
            RecordButton.Text = hasRecordedFile ? AppResources.Re_Record : AppResources.Record;
            UploadButton.IsEnabled = File.Exists(recordFilePath);
        }

        private void RecordButton_Clicked(object sender, EventArgs e)
        {
            if (recorder == null)
            {
                recorder = new AudioRecorderService()
                {
                    FilePath = recordFilePath,
                    StopRecordingOnSilence = false
                };

                recorder.AudioInputReceived += Recorder_AudioInputReceived;
            }

            if (recorder.IsRecording)
            {
                recorder.StopRecording();
            }
            else
            {
                recorder.StartRecording();

                RecordButton.Text = $"{AppResources.Recording}";
                RecordButton.IsEnabled = false;
                RecordStatusLabel.IsVisible = true;
                RecordCountProgressBar.Progress = 0;
                RecordCountProgressBar.IsVisible = true;

                timeTimer.Start();
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
                RecordCountProgressBar.IsVisible = false;
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
#if DEBUG
                DependencyService.Get<IToast>().Show(ex.ToString());
#endif
                DependencyService.Get<IToast>().Show(AppResources.RecordFail_Rerecord);

                isRecorded = false;
            }
            finally
            {
                RecordButton.IsEnabled = true;
                RecordButton.Text = AppResources.Record;
                RecordStatusLabel.IsVisible = false;

                secCount = 0;
            }

            UploadFile();
        }

        private void UploadFile()
        {
            try
            {
                string serverDirPath = AppEnvironment.dataService.ServerUserDataDirPath + "/";

                UploadButton.IsEnabled = false;
                UploadButton.Text = AppResources.Uploading;

                if (!FTPService.CheckDirExist(serverDirPath))
                {
                    FTPService.CreateDir(serverDirPath);
                }

                isUploaded = true;

                isUploaded = FTPService.UploadFile(recordFilePath, Path.Combine(serverDirPath, Path.GetFileName(recordFileName)));

                if (!isUploaded)
                {
                    throw new Exception("Cannot upload file");
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().Show(ex.ToString());
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
                (BindingContext as FirstVoiceSetRecordViewModel).UpdateItemInfo(isRecorded, isUploaded);
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
            UploadFile();
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