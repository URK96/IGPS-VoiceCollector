using IGPS.Services;
using IGPS.Views;

using Xamarin.Forms;

namespace IGPS
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            AppEnvironment.authService = new AuthService();

            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
