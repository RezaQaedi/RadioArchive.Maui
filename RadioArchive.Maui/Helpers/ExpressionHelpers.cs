using System.Linq.Expressions;
using System.Reflection;

namespace RadioArchive.Maui
{
    /// <summary>
    /// a helper for expressions
    /// </summary>
    public static class ExpressionHelpers
    {
        /// <summary>
        /// compiles a expression and gets the functions return value
        /// </summary>
        /// <typeparam name="T">the type of return value</typeparam>
        /// <param name="lamda">the Expression to compile</param>
        /// <returns></returns>
        public static T GetPropertyValue<T>(this Expression<Func<T>> lamda)
        {
            return lamda.Compile().Invoke();
        }
                
        /// <summary>
        /// compiles a expression and gets the functions return value
        /// </summary>
        /// <typeparam name="T">the type of return value</typeparam>
        /// <param name="lamda">the Expression to compile</param>
        /// <typeparam name="In">The input to the expression</typeparam>
        /// <returns></returns>
        public static T GetPropertyValue<In, T>(this Expression<Func<In, T>> lamda, In input)
        {
            return lamda.Compile().Invoke(input);
        }

        /// <summary>
        /// sets the underlying value to given value 
        /// from a Expression that contains property
        /// </summary>
        /// <typeparam name="T">the type of value to set</typeparam>
        /// <param name="lamda">the Expression</param>
        /// <param name="value">the value we set in the property</param>
        public static void SetPropertyValue<T>(this Expression<Func<T>> lamda, T value)
        {
            //converts the lambda() => property to property
            var expression = (lamda as LambdaExpression).Body as MemberExpression;

            //gets the property info so we can set it
            var propertyInfo = (PropertyInfo)expression.Member;
            var target = Expression.Lambda(expression.Expression).Compile().DynamicInvoke();

            //sets the property value
            propertyInfo.SetValue(target, value);
        }

        /// <summary>
        /// sets the underlying value to given value 
        /// from a Expression that contains property
        /// </summary>
        /// <typeparam name="T">the type of value to set</typeparam>
        /// <param name="lamda">the Expression</param>
        /// <typeparam name="In">The type of input</typeparam>
        /// <param name="value">the value we set in the property</param>
        public static void SetPropertyValue<In, T>(this Expression<Func<In, T>> lamda, T value, In Input)
        {
            //converts the lambda() => property to property
            var expression = (lamda as LambdaExpression).Body as MemberExpression;

            //gets the property info so we can set it
            var propertyInfo = (PropertyInfo)expression.Member;
            //sets the property value
            propertyInfo.SetValue(Input, value);
        }
    }
}
