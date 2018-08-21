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
using Android;
using System.Data;
using System.IO;
using System.Text;
using Android.Support.V4.App;

namespace FunnyMoney.Droid
{
    [Activity(Label = "FunnyMoney", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity,
        IPermissiableActivity
    {
        public Activity Activity => this;

        public event EventHandler<RequestPermissionResultEventArgs> RequestPermissionResult;

        public PermissionsContext PermissionsContext;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            if (PermissionsContext != null)
                PermissionsContext.Dispose();
            PermissionsContext = new PermissionsContext(this, 1);
            TestSms();

            LoadApplication(new App());
        }

        private void TestSms()
        {
            if (PermissionsContext.CheckPermissions(Manifest.Permission.ReadSms).Result)
            {
                var tt = ContentResolver.Query(CursorUri.SMS_INBOX, null, null, null, null, null);

                if (tt.MoveToFirst())
                {
                    var columns = tt.GetColumnNames();
                    var dt = new DataTable("t");

                    foreach (var c in columns)
                        dt.Columns.Add(c);

                    do
                    {
                        var r = dt.NewRow();
                        for (int i = 0; i < tt.ColumnCount; i++)
                        {
                            r[i] = tt.GetString(i);
                        }

                        dt.Rows.Add(r);
                    }
                    while (tt.MoveToNext());


                    using (var ms = new MemoryStream())
                    {
                        dt.WriteXml(ms, XmlWriteMode.WriteSchema);

                        var txt = Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
                tt.Dispose();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
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
    }
}

