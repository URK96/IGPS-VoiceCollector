using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Essentials;

namespace IGPS.Droid
{
    [Activity(Label = "IGPS", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
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

            //Permissions.RequestAsync<Permissions.StorageRead>();
            //Permissions.RequestAsync<Permissions.StorageWrite>();
            //Permissions.RequestAsync<RecordAudioPermission>();

            RequestPermissions(new string[] { Android.Manifest.Permission.RecordAudio, Android.Manifest.Permission.ModifyAudioSettings }, 0);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}