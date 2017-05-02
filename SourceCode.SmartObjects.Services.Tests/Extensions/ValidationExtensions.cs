using System;
using SourceCode.SmartObjects.Services.Tests.Properties;

namespace SourceCode.SmartObjects.Services.Tests.Extensions
{
    /// <summary>
    /// ValidationExtensions
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Checks if the object is null
        /// </summary>
        /// <typeparam name="T">Type of the object being checked</typeparam>
        /// <param name="param"></param>
        /// <param name="name"></param>
        public static void ThrowIfNull<T>(this T param, string name) where T : class
        {
            if (param == null)
                throw new ArgumentException(Resources.ErrorRequiredEmpty, name);
        }

        /// <summary>
        /// Checks if a string is null or whitespace
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static void ThrowIfNullOrWhiteSpace(this string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(Resources.ErrorRequiredEmpty, name);
        }
    }
}