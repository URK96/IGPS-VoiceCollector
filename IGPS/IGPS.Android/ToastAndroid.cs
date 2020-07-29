using Android.App;
using Android.Widget;

using IGPS.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(ToastAndroid))]
namespace IGPS.Droid
{
    class ToastAndroid : IToast
    {
        public void Show(string message)
        {
            Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }
    }
}