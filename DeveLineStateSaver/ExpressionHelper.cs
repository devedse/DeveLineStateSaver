using System;
using System.Linq.Expressions;

namespace DeveLineStateSaver
{
    internal static class ExpressionHelper
    {
        public static object GetValue(MemberExpression member)
        {
            var objectMember = Expression.Convert(member, typeof(object));
            var getterLambda = Expression.Lambda<Func<object>>(objectMember);
            var getter = getterLambda.Compile();
            return getter();
        }
    }
}
