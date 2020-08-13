using IGPS.Models;

using System;
using System.Net;
using System.Net.Http;
using System.Windows.Input;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace IGPS.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        public ICommand SNSSignInCommand { private set; get; }

        public LoginViewModel()
        {
            Permissions.RequestAsync<Permissions.StorageRead>();
            Permissions.RequestAsync<Permissions.StorageWrite>();

            SNSSignInCommand = new Command<SNSProvider?>(
                execute: (provider) => { SNSSignIn(provider); });
        }

        public void SNSSignIn(SNSProvider? provider)
        {
            IsBusy = true;

            try
            {
                if (provider.HasValue)
                {
                    AppEnvironment.authService.LoginWithSNS(provider.Value);
                }
            }
            catch (Exception ex) when ((ex is WebException) || (ex is HttpRequestException))
            {
                DependencyService.Get<IToast>().Show(AppResources.SNSLogin_NetworkError);
            }
            catch (Exception)
            {
                DependencyService.Get<IToast>().Show(AppResources.SNSLogin_UnknownError);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
