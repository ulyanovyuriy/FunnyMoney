﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using XMRN.Common.Threading;

namespace XMRN.Android.Common.Security
{
    public class PermissionsContext : BaseContextScope<PermissionsContext>
    {
        private IPermissiableActivity _activity;

        public PermissionsContext(IPermissiableActivity activity)
        {
            _activity = activity ?? throw new ArgumentNullException(nameof(activity));
        }

        public Activity Activity => _activity.Activity;

        public Context Context => Activity;

        public void Execute<T>(Action action
            , params string[] permissions)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (permissions == null) throw new ArgumentNullException(nameof(permissions));

            if (permissions.None())
            {
                action();
                return;
            }

            List<string> requestPermission = new List<string>();
            foreach (var permission in permissions)
                if (ContextCompat.CheckSelfPermission(Context, permission)
                    != global::Android.Content.PM.Permission.Granted)
                    requestPermission.Add(permission);

            if (requestPermission.None())
            {
                action();
                return;
            }

            var rp = requestPermission
                .Select(p => new
                {
                    Permission = p,
                    ShouldShowRequestPermissionRationale = ActivityCompat.ShouldShowRequestPermissionRationale(Activity, p)
                })
                .Where(p => p.ShouldShowRequestPermissionRationale)
                .ToArray();

            if (rp.Any())
            {
                throw new NotSupportedException(string.Join(";", rp.Select(p => p.ToString()).ToArray()));
            }

            ActivityCompat.re

            return default(T);
        }
    }
}