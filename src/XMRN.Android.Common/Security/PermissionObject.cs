using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XMRN.Android.Common.Security
{
    internal class PermissionObject : IDisposable
    {
        private TaskCompletionSource<bool> _completion = new TaskCompletionSource<bool>();

        private string _permission;

        public PermissionObject(string permission)
        {
            _permission = permission ?? throw new ArgumentNullException(nameof(permission));
        }

        public TaskCompletionSource<bool> Completion
        {
            get
            {
                if (disposedValue) throw new ObjectDisposedException(nameof(Completion));
                return _completion;
            }
        }

        public string Permission => _permission;

        public bool Check(TimeSpan timeOut)
        {
            var tsk = CheckAsync();
            if (tsk.Wait(timeOut) == false) throw new TimeoutException();

            return tsk.Result;
        }

        public Task<bool> CheckAsync()
        {
            return Completion.Task;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _completion = null;
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}