using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XMRN.Common.Threading;

namespace XMRN.Android.Common.Security
{
    public class PermissionsContext : BaseContextScope<PermissionsContext>
    {
        private readonly Dictionary<string, PermissionObject> _cache
            = new Dictionary<string, PermissionObject>();

        public PermissionsContext(
            IPermissiableActivity activity
            , int requestCode)
        {
            PermissiableActivity = activity ?? throw new ArgumentNullException(nameof(activity));
            RequestCode = requestCode;

            PermissiableActivity.RequestPermissionResult += _activity_RequestPermissionResult;
        }

        public IPermissiableActivity PermissiableActivity { get; }

        public int RequestCode { get; }

        public Activity Activity => PermissiableActivity.Activity;

        public Context Context => Activity;

        public async Task<bool> CheckPermissions(params string[] permissions)
        {
            if (permissions == null) throw new ArgumentNullException(nameof(permissions));

            if (permissions.None())
                return true;

            var current = new Dictionary<string, PermissionObject>();
            foreach (var permission in permissions)
            {
                PermissionObject po;
                lock (_cache)
                {
                    if (_cache.TryGetValue(permission, out po) == false)
                    {
                        po = new PermissionObject(permission);
                        _cache.Add(permission, po);
                    }
                }
                current.Add(permission, po);
            }

            List<string> requestPermission = new List<string>();
            foreach (var permission in permissions)
            {
                var po = current[permission];
                if (ContextCompat.CheckSelfPermission(Context, permission)
                    == global::Android.Content.PM.Permission.Granted)
                {
                    po.Completion.SetResult(true);
                }
                else
                {
                    requestPermission.Add(permission);
                }
            }

            if (requestPermission.None())
            {
                var all = await Task.WhenAll(current.Select(x => x.Value.Completion.Task));
                return all.All(f => f);
            }
        }

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                PermissiableActivity.RequestPermissionResult -= _activity_RequestPermissionResult;
            }

            base.Dispose(disposing);
        }

        private void _activity_RequestPermissionResult(object sender, RequestPermissionResultEventArgs e)
        {
            if (e.RequestCode == RequestCode)
            {
                e.
            }
        }
    }
}