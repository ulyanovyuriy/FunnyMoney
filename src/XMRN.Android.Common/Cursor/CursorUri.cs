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

namespace XMRN.Android.Common.Cursor
{
    public static class CursorUri
    {
        public static global::Android.Net.Uri SMS_INBOX = global::Android.Net.Uri.Parse("content://sms/inbox");
    }
}