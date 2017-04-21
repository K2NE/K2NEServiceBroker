using System;
using System.Collections.Generic;
using System.Linq;
using SourceCode.SmartObjects.Client;
using SourceCode.SmartObjects.Client.Filters;
using SourceCode.SmartObjects.Services.Tests.Helpers;

namespace SourceCode.SmartObjects.Services.Tests.Extensions
{
    public static class SmartObjectExtensions
    {
        public static void AddFirstPropertyOrderBy(this SmartObject smartObject)
        {
            var listMethod = smartObject.ListMethods[smartObject.MethodToExecute] as SmartListMethod;
            listMethod.OrderBy.Add(listMethod.ReturnProperties[0], OrderByDirection.ASC);
        }

        public static void AddPropertyOrderBy(this SmartObject smartObject, string propertyName)
        {
            var listMethod = smartObject.ListMethods[smartObject.MethodToExecute] as SmartListMethod;
            listMethod.OrderBy.Add(listMethod.ReturnProperties[propertyName], OrderByDirection.ASC);
        }

        public static SmartMethodBase GetExecutingMethod(this SmartObject smartObject)
        {
            var method = smartObject.AllMethods.FirstOrDefault(i => i.Name == smartObject.MethodToExecute);
            return method;
        }

        public static T GetPropertyValue<T>(this SmartObject smartObject, string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentNullException("propertyName");
            var prop = smartObject.Properties.OfType<SmartProperty>()
                 .Where(i => i.Name.StartsWith(propertyName, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();
            if (prop == null) throw new Exception(string.Concat("Could not find property ", propertyName, " on smart object ", smartObject.Name));
            string propValueString = prop.Value;
            if (string.IsNullOrWhiteSpace(propValueString))
                return default(T);
            object changedType = Convert.ChangeType(propValueString, typeof(T));
            if (changedType == null || !(changedType is T))
            {
                throw new Exception(string.Concat("Failed to convert property ", propertyName, " to ", typeof(T).Name));
            }
            return (T)changedType;
        }

        public static PropertyReferenceCollection GetReturnProperties(this SmartObject smartObject)
        {
            if (smartObject == null) throw new ArgumentNullException("smartObject");

            var method = smartObject.GetExecutingMethod();
            if (method == null)
            {
                throw new NullReferenceException("method");
            }

            var propertyReferenceCollectionType = typeof(PropertyReferenceCollection);
            var ctor = propertyReferenceCollectionType.GetConstructors(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).First();

            var propertyCollection = (PropertyReferenceCollection)ctor.Invoke(new object[] { });

            var addMethod = propertyReferenceCollectionType.GetMethod("Add", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

            foreach (SmartProperty returnProperty in method.ReturnProperties)
            {
                var property = smartObject.Properties[returnProperty.Name];
                addMethod.Invoke(propertyCollection, new object[] { property });
            }

            return propertyCollection;
        }

        public static SmartProperty GetReturnProperty(this SmartObject smartObject, string propertyName)
        {
            var returnProperties = smartObject.GetReturnProperties();
            if (returnProperties.Count == 0)
            {
                throw new Exception(string.Concat(smartObject.Name, " SmartObject did not have any return properties."));
            }

            if (string.IsNullOrEmpty(propertyName))
            {
                return returnProperties[0];
            }
            else
            {
                var returnProperty = returnProperties.OfType<SmartProperty>()
                    .Where(i => i.Name.StartsWith(propertyName, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();

                if (returnProperty == null)
                {
                    throw new Exception(string.Concat("Could not find return property ", propertyName, " on SmartObject ", smartObject.Name));
                }

                return returnProperty;
            }
        }

        public static string GetReturnPropertyValue(this SmartObject smartObject, string propertyName = null)
        {
            var smartProperty = GetReturnProperty(smartObject, propertyName);
            return smartProperty.Value;
        }

        public static T GetReturnPropertyValue<T>(this SmartObject smartObject, string propertyName = null)
        {
            var property = GetReturnProperty(smartObject, propertyName);

            return ValueHelper.GetValue<T>(property);
        }

        public static void SetFilter(this SmartObject smartObject, LogicalFilter filter)
        {
            var listMethod = smartObject.GetExecutingMethod() as SmartListMethod;
            listMethod.Filter = filter;
        }

        public static void SetInputPropertyValue(this SmartObject smartObject, string propertyName, object value)
        {
            if (smartObject == null) throw new ArgumentNullException("smartObject");
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentNullException("propertyName");
            if (string.IsNullOrEmpty(smartObject.MethodToExecute)) throw new NullReferenceException("smartObject.MethodToExecute");

            var method = smartObject.AllMethods.Where(i => i.Name == smartObject.MethodToExecute).FirstOrDefault();
            if (method == null)
            {
                if (method == null) throw new NullReferenceException("method");
            }

            var inputProperty = method.InputProperties.OfType<SmartProperty>()
                .Where(i => i.Name.StartsWith(propertyName, StringComparison.InvariantCultureIgnoreCase))
                .FirstOrDefault();

            if (inputProperty == null)
            {
                throw new NullReferenceException(string.Format("Property '{0}' not found in '{1}'", propertyName, smartObject.Name));
            }

            inputProperty.Value = value != null ? Convert.ToString(value) : null;
        }

        public static void SetNewMethod(this SmartObject smartObject, string methodName, bool ignoreCase = true, bool clearPropertyValues = true)
        {
            var stringComparison = StringComparison.InvariantCultureIgnoreCase;
            if (!ignoreCase)
            {
                stringComparison = StringComparison.InvariantCulture;
            }

            if (clearPropertyValues)
            {
                smartObject.ClearPropertyValues();
            }

            foreach (var method in smartObject.AllMethods)
            {
                if (method.Name.Equals(methodName, stringComparison))
                {
                    smartObject.MethodToExecute = method.Name;
                }

                if (clearPropertyValues)
                {
                    foreach (SmartParameter parameter in method.Parameters)
                    {
                        parameter.Value = null;
                    }
                }
            }
        }

        public static IEnumerable<SmartObject> ToList(this SmartObjectList smartObjectList, string methodToExecute)
        {
            var list = new List<SmartObject>();

            foreach (SmartObject smartObject in smartObjectList.SmartObjectsList)
            {
                if (!string.IsNullOrWhiteSpace(methodToExecute))
                {
                    smartObject.MethodToExecute = methodToExecute;
                }

                list.Add(smartObject);
            }

            return list;
        }
    }
}