using IGPS.Services;
using IGPS.Services.Auth;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.IO;

namespace IGPS
{
    public static class AppEnvironment
    {
        public static IAuthService authService;
        public static DataService dataService;

        public static string appDataPath = FileSystem.AppDataDirectory;//DependencyService.Get<IPlatformPath>().GetAppDataPath();
        public static string appCachePath = FileSystem.CacheDirectory;
        public static string serverRootPath = @"http://chlwlsgur96.ipdisk.co.kr/publist/HDD1/Data/Project/IGPS/";
        public static string ftpRootPath = @"ftp://chlwlsgur96.ipdisk.co.kr/HDD1/Data/Project/IGPS/";

        public static void ToggleLoadingIndicator(ActivityIndicator indicator, bool isEnable)
        {
            indicator.IsRunning = isEnable;
            indicator.BackgroundColor = isEnable ? Color.Gray : Color.Default;
        }
    }
}
