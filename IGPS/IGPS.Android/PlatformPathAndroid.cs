using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using IGPS.Droid;
using System.IO;

using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(PlatformPathAndroid))]
namespace IGPS.Droid
{
    class PlatformPathAndroid : IPlatformPath
    {
        public string GetAppDataPath()
        {
            return Path.Combine(Environment.ExternalStorageDirectory.ToString(), "Android", "data", "com.igps.voicecollector");
        }
    }
}