using IGPS.Services.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace IGPS
{
    public static class AppEnvironment
    {
        public static IAuthService authService;

        public static void ToggleLoadingIndicator(ActivityIndicator indicator, bool isEnable)
        {
            indicator.IsRunning = isEnable;
            indicator.BackgroundColor = isEnable ? Color.Gray : Color.Default;
        }
    }
}
