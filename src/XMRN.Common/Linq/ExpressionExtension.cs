using System;
using System.Linq.Expressions;
using System.Reflection;

namespace XMRN.Common.Linq
{
    /// <summary>
    /// Расширение для работы с деревьями выражений
    /// </summary>
    public static class ExpressionExtension
    {
        /// <summary>
        /// Получить член по выражению
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MemberInfo GetMember(this Expression expression)
        {
            if (expression.NodeType == ExpressionType.Lambda)
                expression = ((LambdaExpression)expression).Body;

            if (expression.NodeType == ExpressionType.Convert)
                expression = ((UnaryExpression)expression).Operand;

            var memEx = expression as MemberExpression;
            if (memEx == null) throw new NotSupportedException("member: " + expression.ToString());

            return memEx.Member;
        }
    }
}
