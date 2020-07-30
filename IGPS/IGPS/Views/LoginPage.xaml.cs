
using IGPS.Models;
using IGPS.Views.FirstSet;
using System.Threading.Tasks;
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

            MessagingCenter.Subscribe<UserInfo, bool>(this, MessengerKeys.AuthenticationRequested, CheckLogIn);
        }

        private void CheckLogIn(UserInfo user, bool isLogin)
        {
            if (isLogin || UserInfo.LoadUserInfo() != null)
            {
                DependencyService.Get<IToast>().Show(AppResources.SNSLogin_LoginSuccess);

                if (!AppEnvironment.authService.AuthenticatedUser.FirstSetCompleted)
                {
                    Application.Current.MainPage = new NavigationPage();
                    Application.Current.MainPage.Navigation.PushAsync(new NamePage(), true);
                }
                else
                {
                    Application.Current.MainPage = new MainPage();
                }
            }
            else
            {
                SNSLoginMethodLayout.IsVisible = true;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            AppEnvironment.ToggleLoadingIndicator(LoginIndicator, true);

            await Task.Delay(1000);

            CheckLogIn(null, false);

            AppEnvironment.ToggleLoadingIndicator(LoginIndicator, false);
        }
    }
}