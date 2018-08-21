using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Database;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using XMRN.Common.Data;

namespace XMRN.Android.Common.Cursor
{

    public class CursorDataReader : IDataReader, IDisposable
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
                if (disposedValue) throw new ObjectDisposedException(nameof(Executor));
                return _executor;
            }
        }

        public CursorQuery Query
        {
            get
            {
                if (disposedValue) throw new ObjectDisposedException(nameof(Query));
                return _query;
            }
        }

        public ICursor Cursor
        {
            get
            {
                if (disposedValue) throw new ObjectDisposedException(nameof(Cursor));

                if (_cursor == null)
                    _cursor = Executor.Execute(Query);

                return _cursor;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_cursor != null)
                        _cursor.Dispose();
                }

                _executor = null;
                _query = null;
                _cursor = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        #region IDataReader Support

        public object this[int i] => GetValue(i);

        public object this[string name] => GetValue(name);

        public int Depth => 0;

        public bool IsClosed => Cursor.IsClosed;

        public int RecordsAffected => Cursor.Position + 1;

        public int FieldCount => Query.Fields.Length;

        public bool NextResult()
        {
            return false;
        }

        public bool Read()
        {
            bool r;
            if (Cursor.IsBeforeFirst)
                r = Cursor.MoveToFirst();
            else
                r = Cursor.MoveToNext();
            return r;
        }

        public void Close()
        {
            Cursor.Close();
        }

        public DataTable GetSchemaTable()
        {
            var b = new DataTableBuilder();
            for (int i = 0; i < FieldCount; i++)
                b.AddColumn(c =>
                     c.SetName(GetName(i))
                     .SetType(GetFieldType(i)));

            var t = b.Build();
            var r = t.CreateDataReader();
            var schemaTable = r.GetSchemaTable();

            return schemaTable;
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

        public Type GetFieldType(int i)
        {
            var f = GetField(i);
            if (f.Type == CursorFieldType.Object)
                return typeof(object);
            else if (f.Type == CursorFieldType.Blob)
                return typeof(byte[]);
            else throw new NotSupportedException(f.Type.ToString());
        }

        public string GetName(int i)
        {
            var field = GetField(i);
            return field.Name;
        }

        public int GetOrdinal(string name)
        {
            var index = Array.FindIndex(Query.Fields, f => f.Name == name);
            if (index < 0) throw new IndexOutOfRangeException(name);
            return index;
        }

        public object GetValue(int i)
        {
            var f = GetField(i);
            if (f.Type == CursorFieldType.Object)
                return GetString(i);
            else if (f.Type == CursorFieldType.Blob)
                return GetBlob(i);
            else throw new NotSupportedException(f.Type.ToString());
        }

        public int GetValues(object[] values)
        {
            for (int i = 0; i < FieldCount; i++)
                values[i] = GetValue(i);

            return FieldCount;
        }

        public object GetValue(string name)
        {
            return GetValue(GetOrdinal(name));
        }

        public byte[] GetBlob(int i)
        {
            return Cursor.GetBlob(i);
        }

        public string GetString(int i)
        {
            return Cursor.GetString(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }



        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        #region NotSupported

        public bool GetBoolean(int i)
        {
            throw new NotSupportedException();
        }

        public byte GetByte(int i)
        {
            throw new NotSupportedException();
        }

        #endregion

        #endregion
    }
}