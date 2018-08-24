using Android.Database;
using System;
using XMRN.Common.Data;

namespace XMRN.Android.Common.Cursor
{

    public class CursorDataReader : FlatDataReader
    {
        private CursorExecutor _executor;

        private CursorQuery _query;

        private ICursor _cursor;

        public CursorDataReader(CursorExecutor executor
            , CursorQuery query)
        {
            _executor = executor ?? throw new ArgumentNullException(nameof(executor));
            _query = query ?? throw new ArgumentNullException(nameof(query));
        }

        public CursorExecutor Executor
        {
            get
            {
                CheckDisposed();
                return _executor;
            }
        }

        public CursorQuery Query
        {
            get
            {
                CheckDisposed();
                return _query;
            }
        }

        public ICursor Cursor
        {
            get
            {
                CheckDisposed();

                if (_cursor == null)
                    _cursor = Executor.Execute(Query);

                return _cursor;
            }
        }

        public CursorField GetField(int i)
        {
            var field = Query.Fields[i];
            return field;
        }

        public CursorField GetField(string name)
        {
            return GetField(GetOrdinal(name));
        }

        #region FlatDataReader Support

        public override int FieldCount => Query.Fields.Length;

        public override Type GetFieldType(int i)
        {
            var f = GetField(i);
            if (f.Type == CursorFieldType.Object)
                return typeof(object);
            else if (f.Type == CursorFieldType.Blob)
                return typeof(byte[]);
            else throw new NotSupportedException(f.Type.ToString());
        }

        public override string GetName(int i)
        {
            var field = GetField(i);
            return field.Name;
        }

        public override int GetOrdinal(string name)
        {
            var index = Array.FindIndex(Query.Fields, f => f.Name == name);
            if (index < 0) throw new IndexOutOfRangeException(name);
            return index;
        }

        public override object GetValue(int i)
        {
            var f = GetField(i);
            if (f.Type == CursorFieldType.Object)
                return Cursor.GetString(i);
            else if (f.Type == CursorFieldType.Blob)
                return Cursor.GetBlob(1);
            else throw new NotSupportedException(f.Type.ToString());
        }

        public override bool ReadCore()
        {
            bool r;
            if (Cursor.IsBeforeFirst)
                r = Cursor.MoveToFirst();
            else
                r = Cursor.MoveToNext();
            return r;
        }

        protected override void Disposing()
        {
            Cursor.Dispose();

            _cursor = null;
            _executor = null;
            _query = null;
        }

        #endregion
    }
}