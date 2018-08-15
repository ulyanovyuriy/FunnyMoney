using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XMRN.Android.Common.Cursor
{
    public class CursorExecutor : IDisposable
    {
        private ContentResolver _context;

        public CursorExecutor(ContentResolver context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public ContentResolver Context
        {
            get
            {
                if (disposedValue) throw new ObjectDisposedException(nameof(Context));
                return _context;
            }
        }

        public ICursor Execute(CursorQuery query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));
            query.Validate();

            if (query.Args != null)
                return Context.Query(query.Uri, query.Projection, query.Args, query.CancelationSignal);
            else
                return Context.Query(query.Uri, query.Projection, query.Selection, query.SelectionArgs, query.SortOrder, query.CancelationSignal);
        }

        public ICursor Execute(global::Android.Net.Uri uri
            , CursorField[] fields
            , string selection = null
            , string[] selectionArgs = null
            , string sortOrder = null
            , CancellationSignal cancellationSignal = null)
        {
            return Execute(new CursorQuery()
            {
                Uri = uri,
                Fields = fields,
                Selection = selection,
                SelectionArgs = selectionArgs,
                SortOrder = sortOrder,
                CancelationSignal = cancellationSignal
            });
        }

        public ICursor Execute(global::Android.Net.Uri uri
            , CursorField[] fields
            , Bundle args
            , CancellationSignal cancellationSignal = null)
        {
            return Execute(new CursorQuery()
            {
                Uri = uri,
                Fields = fields,
                Args = args,
                CancelationSignal = cancellationSignal
            });
        }


        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }
                _context = null;
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