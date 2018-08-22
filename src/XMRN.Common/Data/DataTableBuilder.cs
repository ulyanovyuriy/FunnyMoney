using System;
using System.Collections.Generic;
using System.Data;

namespace XMRN.Common.Data
{
    public class DataTableBuilder
    {
        private string _tableName;

        private string _nameSpace;

        private List<DataColumn> _columns = new List<DataColumn>();

        public DataTableBuilder UseName(string tableName)
        {
            _tableName = tableName;
            return this;
        }

        public DataTableBuilder UseNameSpace(string nameSpace)
        {
            _nameSpace = nameSpace;
            return this;
        }

        public DataTableBuilder AddColumn(Action<DataColumnBuilder> builder)
        {
            var b = new DataColumnBuilder();
            builder?.Invoke(b);
            var c = b.Build();
            _columns.Add(c);

            return this;
        }

        public DataTable Build()
        {
            DataTable table;
            if (_tableName != null)
            {
                if (_nameSpace != null)
                    table = new DataTable(_tableName, _nameSpace);
                else
                    table = new DataTable(_tableName);
            }
            else
                table = new DataTable();

            foreach (var column in _columns)
            {
                table.Columns.Add(column);
            }

            return table;
        }
    }
}
