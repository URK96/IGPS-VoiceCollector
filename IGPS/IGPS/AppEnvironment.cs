using IGPS.Services;
using IGPS.Services.Auth;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace IGPS
{
    public static class AppEnvironment
    {
        public static IAuthService authService;
        public static DataService dataService;

        public static string appDataPath = FileSystem.AppDataDirectory; //DependencyService.Get<IPlatformPath>().GetAppDataPath();
        public static string appCachePath = FileSystem.CacheDirectory;
        public static string serverRootPath = @"http://chlwlsgur96.ipdisk.co.kr/publist/HDD1/Data/Project/IGPS/";
        public static string ftpRootPath = @"ftp://bise.diskstation.me/igps/"; //@"ftp://chlwlsgur96.ipdisk.co.kr/HDD1/Data/Project/IGPS/";

        public static void ToggleLoadingIndicator(ActivityIndicator indicator, bool isEnable)
        {
            indicator.IsRunning = isEnable;
            indicator.BackgroundColor = isEnable ? Color.Gray : Color.Default;
        }

        public static void ShowErrorMessage(string message)
        {
#if DEBUG
            DependencyService.Get<IToast>().Show(message);
#endif
        }
    }
}
