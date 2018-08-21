using System;
using System.Data;

namespace XMRN.Common.Data
{
    public class DataColumnBuilder
    {
        private string _columnName;

        private Type _dataType;

        public DataColumnBuilder SetName(string columnName)
        {
            _columnName = columnName;
            return this;
        }

        public DataColumnBuilder SetType(Type dataType)
        {
            _dataType = dataType;
            return this;
        }

        public DataColumn Build()
        {
            var dc = new DataColumn();

            if (_columnName != null)
                dc.ColumnName = _columnName;

            if (_dataType != null)
                dc.DataType = _dataType;

            return dc;
        }
    }
}
