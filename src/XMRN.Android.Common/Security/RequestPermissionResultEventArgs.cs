using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XMRN.Android.Common.Security
{
    public class RequestPermissionResultEventArgs : EventArgs
    {
        public RequestPermissionResultEventArgs(int requestCode
            , string[] permissions
            , Permission[] grantResults)
        {
            RequestCode = requestCode;
            Permissions = permissions;
            GrantResults = grantResults;
        }
        public bool Handled { get; set; } = false;

        public int RequestCode { get; }

        public string[] Permissions { get; }

        public Permission[] GrantResults { get; }

        public Permission this[string name] { get { return GetResult(name); } }

        public Permission GetResult(string permission)
        {
            var i = Array.IndexOf(Permissions, permission);
            if (i < 0)
                throw new IndexOutOfRangeException(permission);

            var result = GrantResults[i];
            return result;
        }

        public bool IsGranted(string permission)
        {
            return GetResult(permission) == Permission.Granted;
        }

        public bool IsDenied(string permission)
        {
            return IsGranted(permission) == false;
        }
    }
}