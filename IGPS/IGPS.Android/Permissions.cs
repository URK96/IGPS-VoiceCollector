using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace IGPS.Droid
{
    public class RecordAudioPermission : Xamarin.Essentials.Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new List<(string, bool)>
        {
            (Android.Manifest.Permission.RecordAudio, true),
            (Android.Manifest.Permission.ModifyAudioSettings, true)
        }.ToArray();
    }
}