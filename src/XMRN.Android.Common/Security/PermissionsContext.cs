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
    public class PermissionsContext : IDisposable
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

            var permissionObjects = GetPermissions(permissions);

            List<string> requestPermission = new List<string>();
            foreach (var permission in permissions)
            {
                var po = permissionObjects[permission];
                if (ContextCompat.CheckSelfPermission(Context, permission)
                    == global::Android.Content.PM.Permission.Granted)
                {
                    po.Completion.TrySetResult(true);
                }
                else
                {
                    requestPermission.Add(permission);
                }
            }

            if (requestPermission.Any())
            {
                ActivityCompat.RequestPermissions(Activity, requestPermission.ToArray(), RequestCode);
            }

            var all = await Task.WhenAll(permissionObjects.Select(x => x.Value.Completion.Task));
            return all.All(f => f);
        }

        private void _activity_RequestPermissionResult(object sender, RequestPermissionResultEventArgs e)
        {
            if (e.RequestCode == RequestCode
                && e.Permissions.Any())
            {
                var permissionObjects = GetPermissions(e.Permissions);

                foreach (var permission in e.Permissions)
                {
                    var po = permissionObjects[permission];
                    var granted = e.IsGranted(permission);
                    po.Completion.TrySetResult(granted);
                }
            }
        }

        private Dictionary<string, PermissionObject> GetPermissions(string[] permissions)
        {
            if (permissions == null) throw new ArgumentNullException(nameof(permissions));

            var permissionObjects = new Dictionary<string, PermissionObject>();
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
                permissionObjects.Add(permission, po);
            }
            return permissionObjects;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    PermissiableActivity.RequestPermissionResult -= _activity_RequestPermissionResult;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }


        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~PermissionsContext() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}