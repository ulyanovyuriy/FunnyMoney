using System;

namespace XMRN.Common.System
{
    public abstract class DisposableObject : IDisposable
    {
        public bool Disposed { get; private set; }

        protected virtual void CheckDisposed(string name = null)
        {
            if (Disposed) throw new ObjectDisposedException(name ?? GetType().Name);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                    Disposing();

                Disposed = true;
            }
        }

        protected abstract void Disposing();

        #region IDisposable Support

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
