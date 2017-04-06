using System;
using System.ComponentModel;
using System.Globalization;
using SourceCode.SmartObjects.Client;

namespace SourceCode.SmartObjects.Services.Tests.Helpers
{
    public static class ValueHelper
    {
        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static object GetDefaultValue(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            object result;
            if (type.IsValueType &&
                type != typeof(DateTime))
            {
                result = Activator.CreateInstance(type);
            }
            else
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Gets the type of the dot net.
        /// </summary>
        /// <param name="propertyType">Type of the so.</param>
        /// <returns></returns>
        public static Type GetDotNetType(PropertyType propertyType)
        {
            switch (propertyType)
            {
                case PropertyType.AutoGuid:
                case PropertyType.Guid:
                    return typeof(Guid);

                case PropertyType.Autonumber:
                case PropertyType.Number:
                    return typeof(long);

                case PropertyType.DateTime:
                    return typeof(DateTime);

                case PropertyType.Decimal:
                    return typeof(decimal);

                case PropertyType.Default:
                    return typeof(string);

                case PropertyType.File:
                    return typeof(string);

                case PropertyType.HyperLink:
                    return typeof(string);

                case PropertyType.Image:
                    return typeof(string);

                case PropertyType.MultiValue:
                    return typeof(string);

                case PropertyType.Memo:
                case PropertyType.Xml:
                case PropertyType.Text:
                    return typeof(string);

                case PropertyType.YesNo:
                    return typeof(bool);

                case PropertyType.Time:
                    return typeof(TimeSpan);

                default:
                    return typeof(string);
            }
        }

        /// <summary>
        /// Returns the property's value converted to T. Used for when a different type is needed
        /// that the property has, for example a DateTime? when the property is of type DataTime.
        /// Properties can't have a type of DateTime? as Nullable types aren't supported in DataTables.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="smartProperty">The property.</param>
        /// <returns>The propertys' value converted to T.</returns>
        public static T GetValue<T>(SmartProperty smartProperty)
        {
            if (smartProperty == null) throw new ArgumentNullException("smartProperty");

            object result;
            TryConvert(typeof(T), smartProperty.Value, out result);
            return (T)result;
        }

        /// <summary>
        /// Tries the convert.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">property</exception>
        public static bool TryConvert(Type type, object value, out object result)
        {
            if (type == null) throw new ArgumentNullException("type");

            if (value == null ||
                value == DBNull.Value)
            {
                result = GetDefaultValue(type);
                return false;
            }

            if (type.Equals(value.GetType()))
            {
                result = value;
                return true;
            }

            string typeName = type.Name;

            try
            {
                try
                {
                    var typeConverter = TypeDescriptor.GetConverter(type);
                    result = typeConverter.ConvertFrom(value);
                    return true;
                }
                catch
                {
                    if (type == typeof(decimal))
                    {
                        decimal returnValue;
                        if (decimal.TryParse(Convert.ToString(value, CultureInfo.InvariantCulture), NumberStyles.Any, CultureInfo.InvariantCulture, out returnValue))
                        {
                            result = returnValue;
                            return true;
                        }
                    }
                    else
                    {
                        result = Convert.ChangeType(value, type, CultureInfo.CurrentCulture);
                        return true;
                    }
                }
            }
            catch
            {
                result = GetDefaultValue(type);
                return false;
            }

            result = GetDefaultValue(type);
            return false;
        }
    }
}