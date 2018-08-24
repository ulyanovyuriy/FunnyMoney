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

        private Dictionary<int, IItemMap> _map;

        private Dictionary<string, int> _indexMap;

        public ItemDataReader(IEnumerable<T> items
            , IEnumerable<IItemMap> map)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            if (map == null) throw new ArgumentNullException(nameof(map));

            _iterator = items.GetEnumerator();
            var lookup = map.Select((x, i) => new { Index = i, Map = x }).ToArray();
            _map = lookup.ToDictionary(x => x.Index, x => x.Map);
            _indexMap = lookup.ToDictionary(x => x.Map.Name, x => x.Index);
        }

        public IEnumerator<T> Iterator
        {
            get
            {
                CheckDisposed();
                return _iterator;
            }
        }

        public Dictionary<int, IItemMap> Map
        {
            get
            {
                CheckDisposed();
                return _map;
            }
        }

        public Dictionary<string, int> IndexMap
        {
            get
            {
                CheckDisposed();
                return _indexMap;
            }
        }

        #region FlatDataReader Support

        public override int FieldCount => Map.Count;

        public override Type GetFieldType(int i)
        {
            var map = Map[i];
            return map.GetFieldType();
        }

        public override string GetName(int i)
        {
            return Map[i].Name;
        }

        public override int GetOrdinal(string name)
        {
            return IndexMap[name];
        }

        public override object GetValue(int i)
        {
            return Map[i].GetValue(Iterator.Current);
        }

        public override bool ReadCore()
        {
            var result = Iterator.MoveNext();
            return result;
        }

        protected override void Disposing()
        {
            _iterator.Dispose();

            _iterator = null;
            _map = null;
            _indexMap = null;
        }

        #endregion
    }

    public interface IItemMap
    {
        string Name { get; }

        Type GetFieldType();

        object GetValue(object instance);
    }

    public class ItemMap : IItemMap
    {
        public ItemMap(string name, MemberInfo member)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            var prop = member as PropertyInfo;
            if (prop != null)
                Property = prop;
            var field = member as FieldInfo;
            if (field != null)
                Field = field;

            if (Field == null && Property == null) throw new ArgumentException($"{nameof(member)}: {member.MemberType}");
        }

        public ItemMap(string name, PropertyInfo property)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Property = property ?? throw new ArgumentNullException(nameof(property));
        }

        public ItemMap(string name, FieldInfo field)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Field = field ?? throw new ArgumentNullException(nameof(field));
        }

        public PropertyInfo Property { get; }

        public FieldInfo Field { get; }

        public string Name { get; }

        public Type GetFieldType()
        {
            if (Property != null)
                return Property.PropertyType;
            if (Field != null)
                return Field.FieldType;

            throw new NotSupportedException();
        }

        public object GetValue(object instance)
        {
            if (Property != null)
                return Property.GetValue(instance, null);
            if (Field != null)
                return Field.GetValue(instance);

            throw new NotSupportedException();
        }
    }
}
