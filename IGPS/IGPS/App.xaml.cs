using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IGPS.Services;
using IGPS.Views;

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
