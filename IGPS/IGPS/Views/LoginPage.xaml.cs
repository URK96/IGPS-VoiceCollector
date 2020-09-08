
using IGPS.Models;
using IGPS.Views.FirstSet;
using IGPS.Views.FirstVoiceSet;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IGPS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            //MessagingCenter.Subscribe<UserInfo, bool>(this, MessengerKeys.AuthenticationRequested, CheckLogIn);
        }

        private void CheckLogIn()
        {
            if (UserInfo.LoadUserInfo() != null)
            {
                DependencyService.Get<IToast>().Show(AppResources.SNSLogin_LoginSuccess);

                if (!AppEnvironment.authService.AuthenticatedUser.FirstSetCompleted)
                {
                    EnterFirstSetPage();
                }
                else
                {
                    Application.Current.MainPage = new MainPage();
                }
            }
            else
            {
                LoginNextButton.IsVisible = true;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            AppEnvironment.ToggleLoadingIndicator(LoginIndicator, true);

            CheckLogIn();

            AppEnvironment.ToggleLoadingIndicator(LoginIndicator, false);
        }

        private void LoginNextButton_Clicked(object sender, System.EventArgs e)
        {
            AppEnvironment.authService.CreateUser();

            EnterFirstSetPage();
        }

        private void EnterFirstSetPage()
        {
            Application.Current.MainPage = new NavigationPage();
            Application.Current.MainPage.Navigation.PushAsync(new InitialPage(), true);
        }
    }
}