using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management.Automation;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Json;


namespace K2Field.K2NE.ServiceBroker.Helpers.PowerShell
{
    public static class PowerShellSerializationHelper
    {
        #region Serialization
        public static string SerializeItem(string name, string value)
        {
            PowerShellVariablesDC powerShellVariable = new PowerShellVariablesDC(name, value);

            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(PowerShellVariablesDC));

            jsonSerializer.WriteObject(stream, powerShellVariable);

            stream.Position = 0;
            StreamReader streamReader = new StreamReader(stream);

            return streamReader.ReadToEnd();
        }

        public static string SerializeItemToArray(string name, string value)
        {
            PowerShellVariablesDC powerShellVariable = new PowerShellVariablesDC(name, value);

            List<PowerShellVariablesDC> powerShellVariableList = new List<PowerShellVariablesDC>
            {
                powerShellVariable
            };

            return SerializeList(powerShellVariableList);
        }

        public static string AddSerializedItemToArray(string serializedArray, string serializedItem)
        {
            PowerShellVariablesDC powerShellVariable = Deserialize(serializedItem);

            List<PowerShellVariablesDC> powerShellVariableList = DeserializeArrayToList(serializedArray);

            powerShellVariableList.Add(powerShellVariable);

            return SerializeList(powerShellVariableList);
        }

        public static string SerializeAddItemToArray(string name, string value, string serializedArray)
        {
            PowerShellVariablesDC powerShellVariable = new PowerShellVariablesDC(name, value);

            List<PowerShellVariablesDC> powerShellVariableList = DeserializeArrayToList(serializedArray);

            powerShellVariableList.Add(powerShellVariable);

            return SerializeList(powerShellVariableList);
        }

        public static string SerializeList(List<PowerShellVariablesDC> list)
        {
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(list.GetType());

            jsonSerializer.WriteObject(stream, list);

            stream.Position = 0;
            StreamReader streamReader = new StreamReader(stream);

            return streamReader.ReadToEnd();
        }

        #endregion

        #region Deserialization

        public static PowerShellVariablesDC Deserialize(string serializeItem)
        {
            MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(serializeItem));
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(PowerShellVariablesDC));

            stream.Position = 0;
            return (PowerShellVariablesDC)jsonSerializer.ReadObject(stream);
        }

        public static PowerShellVariablesDC DeserializeItemFromArray(string serializedArray, string name)
        {
            MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(serializedArray));
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<PowerShellVariablesDC>));

            stream.Position = 0;
            return (((List<PowerShellVariablesDC>)jsonSerializer.ReadObject(stream)).Where(x => x.Name == name)).First();
        }

        public static List<PowerShellVariablesDC> DeserializeArrayToList(string serializedArray)
        {
            MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(serializedArray));
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<PowerShellVariablesDC>));

            stream.Position = 0;
            return (List<PowerShellVariablesDC>)jsonSerializer.ReadObject(stream);
        }

        #endregion
    }
}
