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

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDQyOTk1QDMxMzkyZTMxMmUzMGlWZ3Z3UHZqMTZvVzc5TEVwUzFvR2lxZHBGNVhWekVFYjE0ZW9zSFg5UVE9");

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
