using IGPS.Models;
using IGPS.Services.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Windows.Input;

namespace IGPS.ViewModels
{
    class LoginViewModel : BaseViewModel
    {
        private IAuthService authService;

        public ICommand SNSSignInCommand;

        public LoginViewModel()
        {
            SNSSignInCommand = new Command<SNSProvider?>(async (provider) => { await SNSSignIn(provider); });

            
        }

        public async Task SNSSignIn(SNSProvider? provider)
        {
            IsBusy = true;

            try
            {
                if (provider.HasValue)
                {
                    await authService.LoginWithSNSAsync(provider.Value);
                }
            }
            catch (Exception ex) when ((ex is WebException) || (ex is HttpRequestException))
            {
                
            }
            catch (Exception ex)
            {
                //Debug.WriteLine($"Error in: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
