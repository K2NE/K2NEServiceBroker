using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;

namespace K2Field.K2NE.ServiceBroker.Helpers
{
    internal static class MetaDataExtensions
    {
        public static void AddServiceElement(this MetaData metaData, string elementName, object elementValue)
        {
            if (metaData == null)
                throw new ArgumentNullException("metaData");

            metaData.ServiceProperties.Add(elementName, elementValue);
        }

        public static T GetServiceElement<T>(this MetaData metaData, string elementName)
        {
            if (metaData == null)
                throw new ArgumentNullException("metaData");

            if (string.IsNullOrEmpty(elementName))
                return default(T);

            var value = metaData.ServiceProperties[elementName];

            if (!string.IsNullOrEmpty(value.ToString()))
                return (T)Convert.ChangeType(value, typeof(T));

            return default(T);
        }

    }
}
