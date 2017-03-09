using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using K2Field.K2NE.ServiceBroker.Helpers;
using K2Field.K2NE.ServiceBroker.Helpers.PowerShell;
using System.Data;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects.PowerShell
{
    public class PowerShellVariablesSO : ServiceObjectBase
    {
        public PowerShellVariablesSO(K2NEServiceBroker api)
            : base(api)
        {
        }

        public override string ServiceFolder
        {
            get
            {
                return Constants.ServiceFolders.PowerShell;
            }
        }
        
        public override List<ServiceObject> DescribeServiceObjects()
        {
            ServiceObject so = Helper.CreateServiceObject("PowershellVariables", "A serialize/deserialize object for powershell variables.");
            
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.PowershellVariables.Name, SoType.Text, "The name of the variable."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.PowershellVariables.Value, SoType.Memo, "The value of the variable."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.PowershellVariables.SerializedArray, SoType.Memo, "JSON of a Serialized array of variables."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.PowershellVariables.SerializedItem, SoType.Memo, "JSON Serialized variable."));

            //SerializeItem
            Method mSerializeItem = Helper.CreateMethod(Constants.Methods.PowershellVariables.SerializeItem, "Serialize a single variable and return the json.", MethodType.Read);
            mSerializeItem.InputProperties.Add(Constants.SOProperties.PowershellVariables.Name);
            mSerializeItem.Validation.RequiredProperties.Add(Constants.SOProperties.PowershellVariables.Name);
            mSerializeItem.InputProperties.Add(Constants.SOProperties.PowershellVariables.Value);
            mSerializeItem.Validation.RequiredProperties.Add(Constants.SOProperties.PowershellVariables.Value);
            mSerializeItem.ReturnProperties.Add(Constants.SOProperties.PowershellVariables.SerializedItem);
            so.Methods.Add(mSerializeItem);

            //SerializeItemToArray
            Method mSerializeItemToArray = Helper.CreateMethod(Constants.Methods.PowershellVariables.SerializeItemToArray, "Serialize a single variable into an array and return the json.", MethodType.Read);
            mSerializeItemToArray.InputProperties.Add(Constants.SOProperties.PowershellVariables.Name);
            mSerializeItemToArray.Validation.RequiredProperties.Add(Constants.SOProperties.PowershellVariables.Name);
            mSerializeItemToArray.InputProperties.Add(Constants.SOProperties.PowershellVariables.Value);
            mSerializeItemToArray.Validation.RequiredProperties.Add(Constants.SOProperties.PowershellVariables.Value);
            mSerializeItemToArray.ReturnProperties.Add(Constants.SOProperties.PowershellVariables.SerializedArray);
            so.Methods.Add(mSerializeItemToArray);

            //AddSerializedItemToArray
            Method mAddSerializedItemToArray = Helper.CreateMethod(Constants.Methods.PowershellVariables.AddSerializedItemToArray, "Add the serialized item to the array and return it.", MethodType.Read);
            mAddSerializedItemToArray.InputProperties.Add(Constants.SOProperties.PowershellVariables.SerializedArray);
            mAddSerializedItemToArray.Validation.RequiredProperties.Add(Constants.SOProperties.PowershellVariables.SerializedArray);
            mAddSerializedItemToArray.InputProperties.Add(Constants.SOProperties.PowershellVariables.SerializedItem);
            mAddSerializedItemToArray.Validation.RequiredProperties.Add(Constants.SOProperties.PowershellVariables.SerializedItem);
            mAddSerializedItemToArray.ReturnProperties.Add(Constants.SOProperties.PowershellVariables.SerializedArray);
            so.Methods.Add(mAddSerializedItemToArray);

            //SerializeAddItemToArray
            Method mSerializeAddItemToArray = Helper.CreateMethod(Constants.Methods.PowershellVariables.SerializeAddItemToArray, "Serialize a single item and add it to the existing array.", MethodType.Read);
            mSerializeAddItemToArray.InputProperties.Add(Constants.SOProperties.PowershellVariables.Name);
            mSerializeAddItemToArray.Validation.RequiredProperties.Add(Constants.SOProperties.PowershellVariables.Name);
            mSerializeAddItemToArray.InputProperties.Add(Constants.SOProperties.PowershellVariables.Value);
            mSerializeAddItemToArray.Validation.RequiredProperties.Add(Constants.SOProperties.PowershellVariables.Value);
            mSerializeAddItemToArray.InputProperties.Add(Constants.SOProperties.PowershellVariables.SerializedArray);
            mSerializeAddItemToArray.ReturnProperties.Add(Constants.SOProperties.PowershellVariables.SerializedArray);
            so.Methods.Add(mSerializeAddItemToArray);

            //Deserialize
            Method mDeserialize = Helper.CreateMethod(Constants.Methods.PowershellVariables.Deserialize, "Deserialize the provided array and return the name & value.", MethodType.Read);
            mDeserialize.InputProperties.Add(Constants.SOProperties.PowershellVariables.SerializedItem);
            mDeserialize.Validation.RequiredProperties.Add(Constants.SOProperties.PowershellVariables.SerializedItem);
            mDeserialize.ReturnProperties.Add(Constants.SOProperties.PowershellVariables.Name);
            mDeserialize.ReturnProperties.Add(Constants.SOProperties.PowershellVariables.Value);
            so.Methods.Add(mDeserialize);

            //DeserializeItemFromArray
            Method mDeserializeItemFromArray = Helper.CreateMethod(Constants.Methods.PowershellVariables.DeserializeItemFromArray, "Deserialize a single value (name) from the given array.", MethodType.Read);
            mDeserializeItemFromArray.InputProperties.Add(Constants.SOProperties.PowershellVariables.SerializedArray);
            mDeserializeItemFromArray.Validation.RequiredProperties.Add(Constants.SOProperties.PowershellVariables.SerializedArray);
            mDeserializeItemFromArray.InputProperties.Add(Constants.SOProperties.PowershellVariables.Name);
            mDeserializeItemFromArray.Validation.RequiredProperties.Add(Constants.SOProperties.PowershellVariables.Name);
            mDeserializeItemFromArray.ReturnProperties.Add(Constants.SOProperties.PowershellVariables.Name);
            mDeserializeItemFromArray.ReturnProperties.Add(Constants.SOProperties.PowershellVariables.Value);
            so.Methods.Add(mDeserializeItemFromArray);

            //DeserializeArrayToList
            Method mDeserializeArrayToList = Helper.CreateMethod(Constants.Methods.PowershellVariables.DeserializeArrayToList, "Deserialize an array into name/value list.", MethodType.List);
            mDeserializeArrayToList.InputProperties.Add(Constants.SOProperties.PowershellVariables.SerializedArray);
            mDeserializeArrayToList.Validation.RequiredProperties.Add(Constants.SOProperties.PowershellVariables.SerializedArray);
            mDeserializeArrayToList.ReturnProperties.Add(Constants.SOProperties.PowershellVariables.Name);
            mDeserializeArrayToList.ReturnProperties.Add(Constants.SOProperties.PowershellVariables.Value);
            so.Methods.Add(mDeserializeArrayToList);

            return new List<ServiceObject> { so };
        }

        public override void Execute()
        {
            switch (ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.PowershellVariables.AddSerializedItemToArray:
                    AddSerializedItemToArray();
                    break;
                case Constants.Methods.PowershellVariables.Deserialize:
                    Deserialize();
                    break;
                case Constants.Methods.PowershellVariables.DeserializeArrayToList:
                    DeserializeArrayToList();
                    break;
                case Constants.Methods.PowershellVariables.DeserializeItemFromArray:
                    DeserializeItemFromArray();
                    break;
                case Constants.Methods.PowershellVariables.SerializeAddItemToArray:
                    SerializeAddItemToArray();
                    break;
                case Constants.Methods.PowershellVariables.SerializeItem:
                    SerializeItem();
                    break;
                case Constants.Methods.PowershellVariables.SerializeItemToArray:
                    SerializeItemToArray();
                    break;
            }
        }

        private void SerializeItem()
        {
            string name = GetStringProperty(Constants.SOProperties.PowershellVariables.Name, true);
            string value = GetStringProperty(Constants.SOProperties.PowershellVariables.Value, true);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            string serializedItem = PowerShellSerializationHelper.SerializeItem(name, value);

            DataRow dr = results.NewRow();
            dr[Constants.SOProperties.PowershellVariables.SerializedItem] = serializedItem;

            results.Rows.Add(dr);
        }

        private void SerializeItemToArray()
        {
            string name = GetStringProperty(Constants.SOProperties.PowershellVariables.Name, true);
            string value = GetStringProperty(Constants.SOProperties.PowershellVariables.Value, true);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            string serializedArray = PowerShellSerializationHelper.SerializeItemToArray(name, value);

            DataRow dr = results.NewRow();
            dr[Constants.SOProperties.PowershellVariables.SerializedArray] = serializedArray;

            results.Rows.Add(dr);
        }

        private void AddSerializedItemToArray()
        {
            string serializedArray = GetStringProperty(Constants.SOProperties.PowershellVariables.SerializedArray, true);
            string serializedItem = GetStringProperty(Constants.SOProperties.PowershellVariables.SerializedItem, true);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            serializedArray = PowerShellSerializationHelper.AddSerializedItemToArray(serializedArray, serializedItem);

            DataRow dr = results.NewRow();
            dr[Constants.SOProperties.PowershellVariables.SerializedArray] = serializedArray;

            results.Rows.Add(dr);
        }

        private void SerializeAddItemToArray()
        {
            string name = GetStringProperty(Constants.SOProperties.PowershellVariables.Name, true);
            string value = GetStringProperty(Constants.SOProperties.PowershellVariables.Value, true);
            string serializedArray = GetStringProperty(Constants.SOProperties.PowershellVariables.SerializedArray, false);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            serializedArray = PowerShellSerializationHelper.SerializeAddItemToArray(name, value, serializedArray);

            DataRow dr = results.NewRow();
            dr[Constants.SOProperties.PowershellVariables.SerializedArray] = serializedArray;

            results.Rows.Add(dr);
        }

        private void Deserialize()
        {
            string serializedItem = GetStringProperty(Constants.SOProperties.PowershellVariables.SerializedItem, true);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            PowerShellVariablesDC powerShellVariables = PowerShellSerializationHelper.Deserialize(serializedItem);

            DataRow dr = results.NewRow();
            dr[Constants.SOProperties.PowershellVariables.Name] = powerShellVariables.Name;
            dr[Constants.SOProperties.PowershellVariables.Value] = powerShellVariables.Value;

            results.Rows.Add(dr);
        }

        private void DeserializeItemFromArray()
        {
            string name = GetStringProperty(Constants.SOProperties.PowershellVariables.Name, true);
            string serializedArray = GetStringProperty(Constants.SOProperties.PowershellVariables.SerializedArray, true);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            PowerShellVariablesDC powerShellVariables = PowerShellSerializationHelper.DeserializeItemFromArray(serializedArray, name);

            DataRow dr = results.NewRow();
            dr[Constants.SOProperties.PowershellVariables.Name] = powerShellVariables.Name;
            dr[Constants.SOProperties.PowershellVariables.Value] = powerShellVariables.Value;

            results.Rows.Add(dr);
        }

        private void DeserializeArrayToList()
        {
            string serializedArray = GetStringProperty(Constants.SOProperties.PowershellVariables.SerializedArray, true);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            List<PowerShellVariablesDC> powerShellVariablesList = PowerShellSerializationHelper.DeserializeArrayToList(serializedArray);

            foreach (PowerShellVariablesDC item in powerShellVariablesList)
            {
                DataRow dr = results.NewRow();
                dr[Constants.SOProperties.PowershellVariables.Name] = item.Name;
                dr[Constants.SOProperties.PowershellVariables.Value] = item.Value;
                results.Rows.Add(dr);
            }                        
        }
    }
}
