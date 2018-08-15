using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace XMRN.Android.Common.Cursor
{
    public class CursorQuery
    {
        private string[] _projection;
        private CursorField[] _fields;

        public global::Android.Net.Uri Uri { get; set; }

        public CursorField[] Fields
        {
            get { return _fields; }
            set
            {
                _fields = value;
                _projection = null;
            }
        }

        public string[] Projection
        {
            get
            {
                if (_projection == null)
                {
                    if (_fields != null)
                    {
                        _projection = _fields.Select(f => f.Name).ToArray();
                    }
                }
                return _projection;
            }
        }

        public string Selection { get; set; }
        public string[] SelectionArgs { get; set; }
        public string SortOrder { get; set; }

        public Bundle Args { get; set; }

        public CancellationSignal CancelationSignal { get; set; }

        public void Validate()
        {
            if (Uri == null) throw new NullReferenceException(nameof(Uri));
            if (Projection == null) throw new NullReferenceException(nameof(Projection));
        }
    }
}