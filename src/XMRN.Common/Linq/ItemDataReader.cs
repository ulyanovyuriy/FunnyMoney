using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XMRN.Common.Data;

namespace XMRN.Common.Linq
{
    public class ItemDataReader<T> : FlatDataReader
    {
        private IEnumerator<T> _iterator;

        private IDictionary<string, MemberInfo> _map;

        public ItemDataReader(IEnumerable<T> items
            , IDictionary<string, MemberInfo> map)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (map == null) throw new ArgumentNullException(nameof(map));

            _iterator = items.GetEnumerator();
            _map = map;
        }

        private MemberInfo GetMember(int i)
        {
            return _map.ElementAt(i).Value;
        }

        #region FlatDataReader Support

        public override int FieldCount => _map.Count;

        public override Type GetFieldType(int i)
        {
            var member = GetMember(i);
            var prop = member as PropertyInfo;
            if (prop != null)
                return prop.PropertyType;
            var field = member as FieldInfo;
            if (field != null)
                return field.FieldType;

            throw new NotSupportedException(member.MemberType.ToString());
        }

        public override string GetName(int i)
        {
            return _map.ElementAt(i).Key;
        }

        public override int GetOrdinal(string name)
        {

        }

        public override object GetValue(int i)
        {
            throw new NotImplementedException();
        }

        public override bool ReadCore()
        {
            var result = _iterator.MoveNext();
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _iterator.Dispose();
            _iterator = null;

            base.Dispose(disposing);
        }

        #endregion
    }
}
