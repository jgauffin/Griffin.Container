using System;

namespace Griffin.Container
{
    /// <summary>
    /// Extension methods for Type
    /// </summary>
    public static class TypeExtensionMethods
    {
        /// <summary>
        /// Displays generic in the same ways as they are declared.
        /// </summary>
        /// <param name="type">Type to print</param>
        /// <returns>Code formatted string</returns>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// var type = typeof(IEnumerable<User>);
        /// Console.WriteLine(type.ToBetterString()); // prints "System.Collections.Generic.IEnumerable<Your.Namespace.User>" instead of "System.Collections.Generic.IEnumerable`1"
        /// ]]>
        /// </code>
        /// </example>
        public static string ToBetterString(this Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (!type.IsGenericType)
                return type.ToString();

            var value = type.FullName.Substring(0, type.FullName.IndexOf('`')) + "<";
            var genericArgs = type.GetGenericArguments();

            for (var i = 0; i < genericArgs.Length; i++)
            {
                value += genericArgs[i] + ", ";
            }

            value = value.Remove(value.Length - 2, 2);
            value += ">";
            return value;
        }
    }
}