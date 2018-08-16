using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using XMRN.Android.Common.Cursor;
using XMRN.Common.Data;
using XMRN.Android.Common.Security;

namespace FunnyMoney.Droid
{
    [Activity(Label = "FunnyMoney", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
        , IPermissiableActivity
    {
        public Activity Activity => this;

        public event EventHandler<RequestPermissionResultEventArgs> RequestPermissionResult;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            Test();

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode
            , string[] permissions
            , [GeneratedEnum] Permission[] grantResults)
        {
            bool handled = false;
            if (RequestPermissionResult != null)
            {
                var arg = new RequestPermissionResultEventArgs(requestCode, permissions, grantResults);
                RequestPermissionResult(this, arg);
                handled = arg.Handled;
            }

            if (handled == false)
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void Test()
        {
            var q = new CursorQuery();
            q.Uri = CursorUri.SMS_INBOX;
            q.Fields = new CursorField[]
            {
                new CursorField(){ Name = "_id" }
            };

            using (var e = new CursorExecutor(this.ContentResolver))
            using (var r = new CursorDataReader(e, q))
            {
                var dt = r.AsDataTable();
            }
        }
    }
}

