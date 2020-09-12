using AiForms.Dialogs;
using AiForms.Dialogs.Abstractions;

using IGPS.Services.Server;
using IGPS.Views.Dialogs;

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;

namespace IGPS.ViewModels
{
    public class SettingViewModel : BaseViewModel
    {
        public ICommand UploadCommand { get; }

        public SettingViewModel()
        {
            Title = AppResources.SettingPage_Title;
            UploadCommand = new Command(async () =>
            {
                bool result = await Dialog.Instance.ShowAsync<SimpleDialogView>(new SimpleDialogViewModel()
                {
                    Title = AppResources.SettingPage_UploadAllVoice_Dialog_Title,
                    Message = AppResources.SettingPage_UploadAllVoice_Dialog_Message
                });

                if (result)
                {
                    await UploadAllVoices();
                }
            });
        }

        private async Task UploadAllVoices()
        {
            try
            {
                string localRecordPath = Path.Combine(AppEnvironment.appDataPath, AppEnvironment.authService.AuthenticatedUser.GetUserString());
                string serverDirPath = AppEnvironment.dataService.ServerUserDataDirPath + "/";
                string[] recordedFiles = Directory.GetFiles(localRecordPath);

                if (recordedFiles.Length < 1)
                {
                    DependencyService.Get<IToast>().Show(AppResources.SettingPage_UploadAllVoice_NoRecordedFiles);

                    return;
                }

                Configurations.LoadingConfig = new LoadingConfig
                {
                    IndicatorColor = Color.AliceBlue,
                    OverlayColor = Color.Gray,
                    Opacity = 0.6,
                    DefaultMessage = AppResources.Uploading
                };

                await Loading.Instance.StartAsync(async (progress) =>
                {
                    using var wc = new WebClient();

                    int count = 0;

                    wc.UploadFileCompleted += (sender, e) =>
                    {
                        progress.Report((count + 1) / (double)recordedFiles.Length);
                    };

                    for (count = 0; count < recordedFiles.Length; ++count)
                    {
                        string fileName = Path.GetFileName(recordedFiles[count]);

                        //Loading.Instance.SetMessage($"{AppResources.Uploading}({count + 1}/{recordedFiles.Length})");

                        await FTPService.UploadFileAsync(Path.Combine(localRecordPath, fileName), Path.Combine(serverDirPath, fileName), wc);
                        await Task.Delay(100);
                    }

                    await Task.Delay(1000);
                });
            }
            catch (Exception ex)
            {
#if DEBUG
                DependencyService.Get<IToast>().Show(ex.ToString());
#endif
                DependencyService.Get<IToast>().Show(AppResources.UploadFail);
            }
            finally
            {
                Loading.Instance.Hide();
            }
        }
    }
}