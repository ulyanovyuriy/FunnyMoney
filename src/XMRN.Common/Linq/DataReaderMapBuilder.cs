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
    public class DataReaderMapBuilder<T>
    {
        /// <summary>
        /// список соответствий
        /// </summary>
        private Dictionary<string, MemberInfo> _maps = new Dictionary<string, MemberInfo>();

        /// <summary>
        /// Добавить член
        /// </summary>
        /// <param name="key"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public DataReaderMapBuilder<T> AddMember(string key, MemberInfo member)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (member == null) throw new ArgumentNullException("member");
            _maps.Add(key, member);
            return this;
        }

        /// <summary>
        /// Добавить член
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        public DataReaderMapBuilder<T> AddMember(MemberInfo member)
        {
            if (member == null) throw new ArgumentNullException("member");
            return AddMember(member.Name, member);
        }

        /// <summary>
        /// Добавить член
        /// </summary>
        /// <param name="key"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public DataReaderMapBuilder<T> AddMember(string key, Expression<Func<T, object>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            var member = expression.GetMember();
            if (member == null) throw new NullReferenceException("member");

            return AddMember(key, member);
        }

        /// <summary>
        /// Добавить член
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public DataReaderMapBuilder<T> AddMember(Expression<Func<T, object>> expression)
        {
            if (expression == null) throw new ArgumentNullException("expression");
            var member = expression.GetMember();
            if (member == null) throw new NullReferenceException("member");

            return AddMember(member);
        }

        /// <summary>
        /// Добавить свойство
        /// </summary>
        /// <param name="key"></param>
        /// <param name="prop"></param>
        /// <returns></returns>
        public DataReaderMapBuilder<T> AddProperty(string key, PropertyInfo prop)
        {
            return AddMember(key, prop);
        }

        /// <summary>
        /// Добавить свойство
        /// </summary>
        /// <param name="key"></param>
        /// <param name="propName"></param>
        /// <returns></returns>
        public DataReaderMapBuilder<T> AddProperty(string key, string propName)
        {
            return AddProperty(key, typeof(T).GetProperty(propName));
        }

        /// <summary>
        /// Добавить свойство
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataReaderMapBuilder<T> AddProperty(string name)
        {
            return AddProperty(name, name);
        }

        /// <summary>
        /// Добавить поле
        /// </summary>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        public DataReaderMapBuilder<T> AddField(string key, FieldInfo field)
        {
            return AddMember(key, field);
        }

        /// <summary>
        /// Добавить поле
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public DataReaderMapBuilder<T> AddField(string key, string fieldName)
        {
            return AddField(key, typeof(T).GetField(fieldName));
        }

        /// <summary>
        /// Добавить поле
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public DataReaderMapBuilder<T> AddField(string name)
        {
            return AddField(name, name);
        }

        /// <summary>
        /// Добавить все свойства
        /// </summary>
        /// <returns></returns>
        public DataReaderMapBuilder<T> AddAllProperties()
        {
            typeof(T).GetProperties()
                .ToList()
                .ForEach(p => AddProperty(p.Name, p));

            return this;
        }

        /// <summary>
        /// Добавить все поля
        /// </summary>
        /// <returns></returns>
        public DataReaderMapBuilder<T> AddAllFields()
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
        public DataReaderMapBuilder<T> AddAllMembers()
        {
            AddAllProperties();
            AddAllFields();

            return this;
        }

        /// <summary>
        /// Построить
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, MemberInfo> Build()
        {
            return _maps;
        }
    }
}
