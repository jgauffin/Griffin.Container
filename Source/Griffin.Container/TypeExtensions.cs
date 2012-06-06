using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Griffin.Container
{
    /// <summary>
    /// Extension methods for <c>Type</c>.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Check if generic types matches
        /// </summary>
        /// <param name="serviceType">Service/interface</param>
        /// <param name="concreteType">Concrete/class</param>
        /// <returns><c>true</c> if the concrete implements the service; otherwise <c>false</c></returns>
        public static bool IsAssignableFromGeneric(this Type serviceType, Type concreteType)
        {
            var interfaceTypes = concreteType.GetInterfaces();
            if (interfaceTypes.Any(it => it.IsGenericType && it.GetGenericTypeDefinition() == serviceType))
                return true;

            var baseType = concreteType.BaseType;
            if (baseType == null) 
                return false;

            return baseType.IsGenericType &&
                baseType.GetGenericTypeDefinition() == serviceType ||
                IsAssignableFromGeneric(serviceType, baseType);
        }

    }
}
