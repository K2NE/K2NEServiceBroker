using System;
using System.Collections.Generic;
using System.Linq;
using K2Field.K2NE.ServiceBroker.Properties;

namespace K2Field.K2NE.ServiceBroker.Helpers
{
    /// <summary>
    /// Validation Extensions
    /// </summary>
    internal static class ValidationExtensions
    {
        /// <summary>
        /// Checks if the object is null
        /// </summary>
        /// <typeparam name="T">Type of the object being checked</typeparam>
        /// <param name="param"></param>
        /// <param name="name"></param>
        internal static void ThrowIfNull<T>(this T param, string name) where T : class
        {
            if (param == null)
            {
                throw new ArgumentException(Resources.ErrorRequiredEmpty, name);
            }
        }

        /// <summary>
        /// Checks if the enumberable object is null or empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param">The parameter.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentException"></exception>
        internal static void ThrowIfNullOrEmpty<T>(this IEnumerable<T> param, string name)
        {
            if (param == null || !param.Any())
            {
                throw new ArgumentException(Resources.ErrorRequiredEmpty, name);
            }
        }

        /// <summary>
        /// Checks if a string is null or whitespace
        /// </summary>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        internal static void ThrowIfNullOrWhiteSpace(this string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(Resources.ErrorRequiredEmpty, name);
            }
        }
    }
}