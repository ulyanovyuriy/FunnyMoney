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

namespace XMRN.Android.Common.Security
{
    public interface IPermissiableActivity
    {
        event EventHandler<RequestPermissionResultEventArgs> RequestPermissionResult;

        Activity Activity { get; }
    }
}