using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Widget;

using System.Threading.Tasks;
using System.Timers;

using Xamarin.Essentials;

namespace IGPS.Droid
{
    [Activity(Label = "IGPS", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private Timer exitTimer;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Platform.Init(this, savedInstanceState);
            Xamarin.Forms.Forms.SetFlags(new string[] { "RadioButton_Experimental", "Shapes_Experimental" });
            Xamarin.Forms.Forms.Init(this, savedInstanceState);  
            Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, savedInstanceState);
            LoadApplication(new App());

            _ = RequestPermissions();

            exitTimer = new Timer(1500);
            exitTimer.Elapsed += delegate { exitTimer.Stop(); };
            exitTimer.AutoReset = true;
        }

        private async Task RequestPermissions()
        {
            await Permissions.RequestAsync<Permissions.StorageRead>();
            await Permissions.RequestAsync<Permissions.StorageWrite>();
            await Permissions.RequestAsync<RecordAudioPermission>();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            /*if (exitTimer.Enabled)
            {
                base.OnBackPressed();
            }
            else
            {
                Toast.MakeText(this, "앱을 종료하려면 뒤로가기를 다시 눌러주세요.", ToastLength.Short).Show();

                exitTimer.Start();
            }*/
        }
    }
}