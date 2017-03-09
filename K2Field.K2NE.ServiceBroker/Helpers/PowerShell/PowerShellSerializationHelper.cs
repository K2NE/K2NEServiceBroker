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

            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(PowerShellVariablesDC));

                    jsonSerializer.WriteObject(stream, powerShellVariable);

                    stream.Position = 0;
                    using (StreamReader streamReader = new StreamReader(stream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("Failed to serialize property: {0}", e.Message), e);
            }
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

            List<PowerShellVariablesDC> powerShellVariableList = new List<PowerShellVariablesDC>();
            if (!string.IsNullOrEmpty(serializedArray))
            {
                powerShellVariableList = DeserializeArrayToList(serializedArray);
            }

            powerShellVariableList.Add(powerShellVariable);

            return SerializeList(powerShellVariableList);
        }

        public static string SerializeList(List<PowerShellVariablesDC> list)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(list.GetType());

                    jsonSerializer.WriteObject(stream, list);

                    stream.Position = 0;
                    using (StreamReader streamReader = new StreamReader(stream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch(Exception e)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Array serialization went wrong: ");
                stringBuilder.AppendLine(e.Message);
                stringBuilder.AppendLine("\n");
                throw new Exception(stringBuilder.ToString(), e);
            }
        }

        #endregion

        #region Deserialization

        public static PowerShellVariablesDC Deserialize(string serializeItem)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(serializeItem)))
                {
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(PowerShellVariablesDC));

                    stream.Position = 0;
                    return (PowerShellVariablesDC)jsonSerializer.ReadObject(stream);
                }
            }
            catch(Exception e)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Item deserialization went wrong: ");
                stringBuilder.AppendLine(e.Message);
                stringBuilder.AppendLine("\n");
                throw new Exception(stringBuilder.ToString(), e);
            }
        }

        public static PowerShellVariablesDC DeserializeItemFromArray(string serializedArray, string name)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(serializedArray)))
                {
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<PowerShellVariablesDC>));

                    stream.Position = 0;
                    return (((List<PowerShellVariablesDC>)jsonSerializer.ReadObject(stream)).Where(x => x.Name == name)).First();
                }
            }
            catch(Exception e)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Item from array deserialization went wrong: ");
                stringBuilder.AppendLine(e.Message);
                stringBuilder.AppendLine("\n");
                throw new Exception(stringBuilder.ToString(), e);
            }
        }

        public static List<PowerShellVariablesDC> DeserializeArrayToList(string serializedArray)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(serializedArray)))
                {
                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(List<PowerShellVariablesDC>));

                    stream.Position = 0;
                    return (List<PowerShellVariablesDC>)jsonSerializer.ReadObject(stream);
                }
            }
            catch(Exception e)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("Array deserialization went wrong: ");
                stringBuilder.AppendLine(e.Message);
                stringBuilder.AppendLine("\n");
                throw new Exception(stringBuilder.ToString(), e);
            }
        }

        #endregion
    }
}
