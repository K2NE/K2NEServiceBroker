using System;
using System.ComponentModel;

namespace K2Field.K2NE.ServiceBroker.Helpers
{
    /// <summary>
    /// Object Extension methods
    /// </summary>
    internal static class ObjectExtensions
    {
        /// <summary>
        /// Gets internal, protected or private property value.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns></returns>
        internal static object GetPropertyValue(this object obj, string fieldName)
        {
            obj.ThrowIfNull("obj");

            Type objType = obj.GetType();
            var propertyValue = objType.GetPropertyInfo(fieldName);

            propertyValue.ThrowIfNull("propertyValue");

            return propertyValue.GetValue(obj, null);
        }

        /// <summary>
        /// Tries to parse the object value to the generic type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectValue">The object value.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        internal static bool TryParse<T>(this object objectValue, out T value)
        {
            var type = typeof(T);

            if (type.IsValueType &&
                Nullable.GetUnderlyingType(type) == null)
            {
                value = (T)System.Convert.ChangeType(objectValue, type);
                return true;
            }
            else
            {
                var converter = TypeDescriptor.GetConverter(type);
                if (converter != null)
                {
                    value = (T)converter.ConvertFrom(objectValue);
                    return true;
                }
            }

            value = default(T);
            return false;
        }
    }
}