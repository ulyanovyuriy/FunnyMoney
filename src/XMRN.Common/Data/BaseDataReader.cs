using System;
using System.Data;
using XMRN.Common.System;

namespace XMRN.Common.Data
{
    public abstract class BaseDataRecord : DisposableObject, IDataRecord
    {
        #region IDataRecord Support

        public abstract int FieldCount { get; }

        public abstract long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length);

        public abstract long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length);

        public abstract IDataReader GetData(int i);

        public abstract Type GetFieldType(int i);

        public abstract string GetName(int i);

        public abstract int GetOrdinal(string name);

        public abstract object GetValue(int i);

        public virtual object this[int i] => GetValue(i);

        public virtual object this[string name] => GetValue(GetOrdinal(name));

        public virtual bool GetBoolean(int i)
        {
            return (bool)GetValue(i);
        }

        public virtual byte GetByte(int i)
        {
            return (byte)GetValue(i);
        }

        public virtual char GetChar(int i)
        {
            return (char)GetValue(i);
        }

        public virtual string GetDataTypeName(int i)
        {
            return GetFieldType(i).Name;
        }

        public virtual DateTime GetDateTime(int i)
        {
            return (DateTime)GetValue(i);
        }

        public virtual decimal GetDecimal(int i)
        {
            return (decimal)GetValue(i);
        }

        public virtual double GetDouble(int i)
        {
            return (double)GetValue(i);
        }

        public virtual float GetFloat(int i)
        {
            return (float)GetValue(i);
        }

        public virtual Guid GetGuid(int i)
        {
            return (Guid)GetValue(i);
        }

        public virtual short GetInt16(int i)
        {
            return (short)GetValue(i);
        }

        public virtual int GetInt32(int i)
        {
            return (int)GetValue(i);
        }

        public virtual long GetInt64(int i)
        {
            return (long)GetValue(i);
        }

        public virtual string GetString(int i)
        {
            return (string)GetValue(i);
        }

        public virtual int GetValues(object[] values)
        {
            for (int i = 0; i < FieldCount; i++)
            {
                values[i] = GetValue(i);
            }

            return FieldCount;
        }

        public virtual bool IsDBNull(int i)
        {
            var v = GetValue(i);
            return DBNull.Value == v || v == null;
        }

        #endregion      
    }

    public abstract class BaseDataReader : BaseDataRecord, IDataReader
    {
        private int _recordsAffected;

        public abstract bool ReadCore();

        #region IDataReader Support        

        public abstract int Depth { get; }

        public abstract bool NextResult();

        public virtual int RecordsAffected => _recordsAffected;

        public virtual bool IsClosed => Disposed;

        public virtual bool Read()
        {
            var result = ReadCore();
            if (result)
                _recordsAffected++;

            return result;
        }

        public virtual void Close()
        {
            Dispose(true);
        }

        public virtual DataTable GetSchemaTable()
        {
            var tb = new DataTableBuilder();
            for (int i = 0; i < FieldCount; i++)
            {
                tb.AddColumn(cb => cb.SetName(GetName(i)).SetType(GetFieldType(i)));
            }

            using (var t = tb.Build())
            using (var r = t.CreateDataReader())
            {
                var schema = r.GetSchemaTable();
                return schema;
            }
        }

        #endregion
    }

    public abstract class FlatDataReader : BaseDataReader, IDataReader
    {
        #region BaseDataReader Support        

        public override int Depth => 0;

        public override bool NextResult()
        {
            return false;
        }

        #region Not Support

        public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException();
        }

        public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotSupportedException();
        }

        public override IDataReader GetData(int i)
        {
            throw new NotSupportedException();
        }

        #endregion

        #endregion

    }
}
