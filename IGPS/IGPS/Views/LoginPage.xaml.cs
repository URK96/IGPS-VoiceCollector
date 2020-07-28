using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Xamarin.Auth.OAuth2;
using IGPS.Models;
using IGPS.Services;
using System.Net;
using System.Net.Http;

namespace IGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            
        }

        private async void KakaoLoginButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (UserInfo.LoadUserInfo() != null)
                {
                    TestLabel.Text = "이미 사용자가 존재합니다.";

                    return;
                }

                var authService = new AuthService();

                await authService.LoginWithSNSAsync(SNSProvider.Kakao);
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