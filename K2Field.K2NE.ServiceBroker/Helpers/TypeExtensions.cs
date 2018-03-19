using System;
using System.Linq;
using System.Reflection;

namespace K2Field.K2NE.ServiceBroker.Helpers
{
    /// <summary>
    /// Type Extension methods
    /// </summary>
    internal static class TypeExtensions
    {
        public static PropertyInfo GetPropertyInfo(this Type type, string propertyName)
        {
            PropertyInfo propertyInfo;
            do
            {
                propertyInfo = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                type = type.BaseType;
            }
            while (propertyInfo == null && type != null);

            return propertyInfo;
        }
    }
}