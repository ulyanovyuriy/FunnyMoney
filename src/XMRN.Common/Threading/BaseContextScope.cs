using System;
using System.Threading;

namespace XMRN.Common.Threading
{
    /// <summary>
    /// Базовое окружение некоторого контекста
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseContextScope<TContext> : IDisposable
        where TContext : BaseContextScope<TContext>, new()
    {
        /// <summary>
        /// текущее окружение
        /// </summary>
        static AsyncLocal<TContext> _current = new AsyncLocal<TContext>();

        /// <summary>
        /// сохраненный контекст
        /// </summary>
        private TContext _saved;

        /// <summary>
        /// Конструктор
        /// </summary>
        protected BaseContextScope()
        {
            PushScope();
        }

        /// <summary>
        /// Текущее окружение
        /// </summary>
        public static TContext Current
        {
            get
            {
                return _current.Value;
            }
            private set
            {
                _current.Value = value;
            }
        }

        /// <summary>
        /// Исполнение в контексте
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exec"></param>
        /// <returns></returns>
        public static T UseContextIfNotExists<T>(Func<TContext, T> exec)
        {
            TContext context = Current;
            bool newContext = context == null;
            if (newContext)
                context = new TContext();
            try
            {
                var result = exec(context);
                return result;
            }
            finally
            {
                if (newContext)
                    context.Dispose();
            }
        }

        /// <summary>
        /// Исполнение в контексте
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exec"></param>
        /// <returns></returns>
        public static void UseContextIfNotExists(Action<TContext> exec)
        {
            TContext context = Current;
            bool newContext = context == null;
            if (newContext)
                context = new TContext();
            try
            {
                exec(context);
            }
            finally
            {
                if (newContext)
                    context.Dispose();
            }
        }

        /// <summary>
        /// Положить окружение
        /// </summary>
        private void PushScope()
        {
            _saved = Current;
            Current = (TContext)this;
        }

        /// <summary>
        /// Извлечь окружение
        /// </summary>
        private void PopScope()
        {
            Current = _saved;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    PopScope();
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
