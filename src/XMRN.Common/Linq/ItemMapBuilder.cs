using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace XMRN.Common.Linq
{
    /// <summary>
    /// Построитель списка соответствий для преобразования в IDataReader
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ItemMapBuilder<T>
    {
        /// <summary>
        /// список соответствий
        /// </summary>
        private List<ItemMap> _maps = new List<ItemMap>();

        /// <summary>
        /// Добавить член
        /// </summary>
        /// <param name="name"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public ItemMapBuilder<T> AddMember(string name, MemberInfo member)
        {
            var map = new ItemMap(name, member);
            _maps.Add(map);
            return this;
        }

        /// <summary>
        /// Добавить свойство
        /// </summary>
        /// <param name="name"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        public ItemMapBuilder<T> AddProperty(string name, PropertyInfo property)
        {
            var map = new ItemMap(name, property);
            _maps.Add(map);
            return this;
        }

        /// <summary>
        /// Добавить поле
        /// </summary>
        /// <param name="name"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public ItemMapBuilder<T> AddField(string name, FieldInfo field)
        {
            var map = new ItemMap(name, field);
            _maps.Add(map);
            return this;
        }

        /// <summary>
        /// Добавить член
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public ItemMapBuilder<T> AddMember(MemberInfo member)
        {
            if (member == null) throw new ArgumentNullException("member");
            return AddMember(member.Name, member);
        }

        /// <summary>
        /// Добавить член
        /// </summary>
        /// <param name="name"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public ItemMapBuilder<T> AddMember(string name, Expression<Func<T, object>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            var member = expression.GetMember();
            if (member == null) throw new NullReferenceException("member");

            return AddMember(name, member);
        }

        /// <summary>
        /// Добавить член
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public ItemMapBuilder<T> AddMember(Expression<Func<T, object>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            var member = expression.GetMember();
            return AddMember(member);
        }

        /// <summary>
        /// Добавить свойство
        /// </summary>
        /// <param name="name"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public ItemMapBuilder<T> AddProperty(string name, string propertyName)
        {
            return AddProperty(name, typeof(T).GetProperty(propertyName));
        }

        /// <summary>
        /// Добавить свойство
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ItemMapBuilder<T> AddProperty(string name)
        {
            return AddProperty(name, name);
        }

        /// <summary>
        /// Добавить поле
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public ItemMapBuilder<T> AddField(string name, string fieldName)
        {
            return AddField(name, typeof(T).GetField(fieldName));
        }

        /// <summary>
        /// Добавить поле
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ItemMapBuilder<T> AddField(string name)
        {
            return AddField(name, name);
        }

        /// <summary>
        /// Добавить все свойства
        /// </summary>
        /// <returns></returns>
        public ItemMapBuilder<T> AddAllProperties()
        {
            typeof(T).GetProperties()
                .Where(p => p.GetIndexParameters().None())
                .ToList()
                .ForEach(p => AddProperty(p.Name, p));

            return this;
        }

        /// <summary>
        /// Добавить все поля
        /// </summary>
        /// <returns></returns>
        public ItemMapBuilder<T> AddAllFields()
        {
            typeof(T).GetFields()
                .ToList()
                .ForEach(f => AddField(f.Name, f));

            return this;
        }

        /// <summary>
        /// Добавить все члены
        /// </summary>
        /// <returns></returns>
        public ItemMapBuilder<T> AddAllMembers()
        {
            AddAllProperties();
            AddAllFields();

            return this;
        }

        /// <summary>
        /// Построить
        /// </summary>
        /// <returns></returns>
        public ItemMap[] Build()
        {
            return _maps.ToArray();
        }
    }
}
