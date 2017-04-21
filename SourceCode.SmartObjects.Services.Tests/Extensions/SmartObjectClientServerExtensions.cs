using System;
using System.Data;
using SourceCode.SmartObjects.Client;
using SourceCode.SmartObjects.Services.Tests.Helpers;
using SourceCode.SmartObjects.Services.Tests.Managers;

namespace SourceCode.SmartObjects.Services.Tests.Extensions
{
    public static class SmartObjectClientServerExtensions
    {
        /// <summary>
        /// Deserializes <paramref name="serializedString"/> to a SmartObject
        /// </summary>
        /// <param name="clientServer"></param>
        /// <param name="serviceObjectName"></param>
        /// <param name="serializedString">The serialized string version of the object</param>
        /// <returns>The SmartObject containing the return properties of the Deserialize call</returns>
        public static SmartObject Deserialize(this SmartObjectClientServer clientServer, string serviceObjectName,
            ServiceInstanceSettings serviceInstanceSettings, String serializedString)
        {
            var smartObject = SmartObjectHelper.GetSmartObject(clientServer, serviceObjectName, serviceInstanceSettings);

            smartObject.MethodToExecute = "Deserialize";
            smartObject.SetInputPropertyValue("Serialized_Item__String_", serializedString);

            SmartObjectHelper.ExecuteScalar(clientServer, smartObject);

            return smartObject;
        }

        /// <summary>
        /// Deserializes <paramref name="serializedString"/> to a DataTable
        /// </summary>
        /// <param name="clientServer"></param>
        /// <param name="serviceObjectName"></param>
        /// <param name="serializedString">The string to deserializd to a datatable</param>
        /// <returns>A DataTable containing the deserialized data for the object</returns>
        public static DataTable DeserializeTypedArray(this SmartObjectClientServer clientServer, string serviceObjectName,
            ServiceInstanceSettings serviceInstanceSettings, String serializedString
            )
        {
            var smartObject = SmartObjectHelper.GetSmartObject(clientServer, serviceObjectName, serviceInstanceSettings);

            smartObject.MethodToExecute = "DeserializeTypedArray";
            smartObject.SetInputPropertyValue("Serialized_Array", serializedString);

            var dataTable = SmartObjectHelper.ExecuteListDataTable(clientServer, smartObject);

            return dataTable;
        }

        /// <summary>
        /// Executes the Serialize method of the <paramref name="serviceObjectName"/>
        /// </summary>
        /// <param name="clientServer"></param>
        /// <param name="serviceObjectName"></param>
        /// <param name="actions">
        ///		One or more custom actions to perform on the SmartObject associated with the specified
        ///		<paramref name="serviceObjectName"/>. This will most commonly take the form of setting
        ///		input values. See the accompanying example.
        /// </param>
        /// <returns>
        ///		The serialized string representation of the specified <paramref name="serviceObjectName"/>
        /// </returns>
        ///	<example>
        ///		<code>
        ///		clientServer.Serialize("Person",
        ///			so => so.SetInputPropertyValue("FirstName", firstName),
        ///			so => so.SetInputPropertyValue("LastName", lastName));
        ///		</code>
        ///	</example>
        public static string Serialize(this SmartObjectClientServer clientServer, string serviceObjectName,
            ServiceInstanceSettings serviceInstanceSettings, params Action<SmartObject>[] actions)
        {
            var smartObject = SmartObjectHelper.GetSmartObject(clientServer, serviceObjectName, serviceInstanceSettings);
            smartObject.MethodToExecute = "Serialize";

            foreach (var action in actions)
            {
                action(smartObject);
            }

            var serialized = SmartObjectHelper.ExecuteScalar(clientServer, smartObject);
            return serialized.GetReturnPropertyValue("Serialized_Item__String_");
        }

        /// <summary>
        /// Executes the SerializeAddItemToArray method on the <paramref name="serviceObjectName"/>
        /// </summary>
        /// <param name="clientServer"></param>
        /// <param name="serviceObjectName"></param>
        /// <param name="existingSerializedArray">
        ///		An existing serialized array to add the item to
        /// </param>
        ///		One or more custom actions to perform on the SmartObject associated with the specified
        ///		<paramref name="serviceObjectName"/>. This will most commonly take the form of setting
        ///		input values. See the accompanying example.
        /// <returns>
        ///		A string representing the serialized form of the <paramref name="existingSerializedArray"/> with the item created by the actions used to set the input properties
        /// </returns>
        public static string SerializeAddItemToArray(this SmartObjectClientServer clientServer, string serviceObjectName, string existingSerializedArray,
            ServiceInstanceSettings serviceInstanceSettings, params Action<SmartObject>[] actions)
        {
            var smartObject = SmartObjectHelper.GetSmartObject(clientServer, serviceObjectName, serviceInstanceSettings);
            smartObject.MethodToExecute = "SerializeAddItemToArray";
            smartObject.SetInputPropertyValue("Serialized_Array", existingSerializedArray);

            foreach (var action in actions)
            {
                action(smartObject);
            }

            SmartObjectHelper.ExecuteScalar(clientServer, smartObject);

            return smartObject.Properties["Serialized_Array"].Value;
        }

        /// <summary>
        /// Executes the SerializeItemToArray method on the <paramref name="serviceObjectName"/>
        /// </summary>
        /// <param name="clientServer"></param>
        /// <param name="serviceObjectName"></param>
        /// <param name="existingSerializedArray">
        ///		An existing serialized array to add the item to
        /// </param>
        ///		One or more custom actions to perform on the SmartObject associated with the specified
        ///		<paramref name="serviceObjectName"/>. This will most commonly take the form of setting
        ///		input values. See the accompanying example.
        /// <returns>
        ///		A string representing the serialized array containing the item created by the actions used to set the input properties
        /// </returns>
        public static string SerializeItemToArray(this SmartObjectClientServer clientServer, string serviceObjectName,
            ServiceInstanceSettings serviceInstanceSettings, params Action<SmartObject>[] actions)
        {
            var smartObject = SmartObjectHelper.GetSmartObject(clientServer, serviceObjectName, serviceInstanceSettings);
            smartObject.MethodToExecute = "SerializeItemToArray";

            foreach (var action in actions)
            {
                action(smartObject);
            }

            SmartObjectHelper.ExecuteScalar(clientServer, smartObject);

            return smartObject.Properties["Serialized_Array"].Value;
        }
    }
}