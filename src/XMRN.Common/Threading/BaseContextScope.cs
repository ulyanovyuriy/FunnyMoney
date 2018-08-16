using System;
using System.Threading;

namespace XMRN.Common.Threading
{
    /// <summary>
    /// Базовое окружение некоторого контекста
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class BaseContextScope<TContext> : IDisposable
        where TContext : BaseContextScope<TContext>
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
        /// Флаг очистки
        /// </summary>
        private bool _disposed;

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

        /// <summary>
        /// Очистка
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;

            PopScope();

            _disposed = true;
        }
    }
}
