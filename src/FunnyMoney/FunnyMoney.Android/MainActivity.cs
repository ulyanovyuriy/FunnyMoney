using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using XMRN.Android.Common.Cursor;
using XMRN.Common.Data;

namespace FunnyMoney.Droid
{
    [Activity(Label = "FunnyMoney", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            Test();

            LoadApplication(new App());
        }

        private void Test()
        {
            var q = new CursorQuery();

            using (var e = new CursorExecutor(this.ContentResolver))
            using (var r = new CursorDataReader(e, q))
            {
                var dt = r.AsDataTable();
            }
        }
    }
}

