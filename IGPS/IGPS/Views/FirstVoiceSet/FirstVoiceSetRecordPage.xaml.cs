using IGPS.Services.Server;
using IGPS.ViewModels;

using Plugin.AudioRecorder;

using System;
using System.IO;
using System.Timers;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGPS.Views.FirstVoiceSet
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirstVoiceSetRecordPage : ContentPage
    {
        private string recordFileName;
        private readonly string recordPath;
        private string recordFilePath;

        private bool isSuddenStop = false;
        private bool isRecorded = false;
        private bool isUploaded = true;

        private int secCount = 0;

        private AudioRecorderService recorder;

        private FirstVoiceSetRecordViewModel context;

        private Timer timeTimer;

        public FirstVoiceSetRecordPage()
        {
            InitializeComponent();

            BindingContext = context = new FirstVoiceSetRecordViewModel();

            recordFileName = $"F{context.Stage}_{context.Count}.mp3";
            recordPath = Path.Combine(AppEnvironment.appDataPath, AppEnvironment.authService.AuthenticatedUser.GetUserString());
            recordFilePath = Path.Combine(recordPath, recordFileName);

            timeTimer = new Timer(1000);
            timeTimer.AutoReset = true;
            timeTimer.Elapsed += delegate
            {
                RecordCountProgressBar.Progress = secCount++ / 10.0;

                if (secCount > 10)
                {
                    timeTimer.Stop();
                    MainThread.BeginInvokeOnMainThread(() => { recorder.StopRecording(); });
                }
            };
        }

        protected override void OnDisappearing()
        {
            if (recorder?.IsRecording != null)
            {
                recorder.StopRecording();

                isSuddenStop = true;
            }

            base.OnDisappearing();            
        }

        private void UpdateButtonStatus()
        {
            RecordButton.IsEnabled = false;
            UploadButton.IsEnabled = false;
            FirstVoiceSetRecordNextButton.BackgroundColor = Color.OrangeRed;
            FirstVoiceSetRecordNextButton.IsEnabled = true;
        }

        private void UpdatePath()
        {
            recordFileName = $"F{context.Stage}_{context.Count}.mp3";
            recordFilePath = Path.Combine(recordPath, recordFileName);
            recorder.FilePath = recordFilePath;
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
                RecordCountProgressBar.IsVisible = true;
                isRecorded = false;

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
                DependencyService.Get<IToast>().Show(ex.ToString());
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

            if ((context.Count == 3) && isRecorded)
            {
                UploadFile();
            }
            else if (isRecorded)
            {
                context.Count += 1;
                RecordCountLabel.Text = $"{context.Count} / 3";

                UpdatePath();
            }
        }

        private void UploadFile()
        {
            try
            {
                string serverDirPath = AppEnvironment.dataService.ServerUserDataDirPath;

                UploadButton.IsEnabled = false;
                UploadButton.Text = AppResources.Uploading;

                if (!FTPService.CheckDirExist(serverDirPath))
                {
                    FTPService.CreateDir(serverDirPath);
                }

                isUploaded = true;

                for (int i = 1; (i <= context.Count) && isUploaded; ++i)
                {
                    string fileName = $"F{context.Stage}_{i}.mp3";
                    string filePath = Path.Combine(recordPath, fileName);

                    isUploaded = FTPService.UploadFile(filePath, Path.Combine(serverDirPath, Path.GetFileName(fileName)));
                }

                if (!isUploaded)
                {
                    throw new Exception("Cannot upload file");
                }

                UpdateButtonStatus();
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
            }
        }

        private void UploadButton_Clicked(object sender, EventArgs e)
        {
            UploadFile();
        }

        private void FirstVoiceSetRecordNextButton_Clicked(object sender, EventArgs e)
        {
            if (context.Stage == 3)
            {
                Application.Current.MainPage = new MainPage();
            }
            else
            {
                switch (context.Stage)
                {
                    case 1:
                        context.Stage = 2;
                        context.Count = 1;
                        FirstVoiceSetRecordText.Text = AppResources.VoiceFirstSet_VoiceText_2;
                        break;
                    case 2:
                        context.Stage = 3;
                        context.Count = 1;
                        FirstVoiceSetRecordText.Text = AppResources.VoiceFirstSet_VoiceText_3;
                        FirstVoiceSetRecordNextButton.Text = AppResources.VoiceFirstSet_NextButton_Finish;
                        break;
                }

                RecordCountLabel.Text = $"{context.Count} / 3";
                FirstVoiceSetRecordNextButton.BackgroundColor = Color.Default;
                FirstVoiceSetRecordNextButton.IsEnabled = false;
                RecordButton.IsEnabled = true;

                UpdatePath();
            }
        }
    }
}