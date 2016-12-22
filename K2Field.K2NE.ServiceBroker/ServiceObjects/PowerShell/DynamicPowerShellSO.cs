using K2Field.K2NE.ServiceBroker.Helpers.PowerShell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using K2Field.K2NE.ServiceBroker.Helpers;
using System.Data;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects.PowerShell
{
    public class DynamicPowerShellSO : ServiceObjectBase
    {
        public DynamicPowerShellSO(K2NEServiceBroker api)
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
        
        public override List<SourceCode.SmartObjects.Services.ServiceSDK.Objects.ServiceObject> DescribeServiceObjects()
        {
            List<ServiceObject> serviceObjects = new List<ServiceObject>();

            if(!String.IsNullOrEmpty(PowerShellSubdirectories))
            {
                //get all files from powershell directories 
                Dictionary<string, string> scriptFiles = PowerShellHelper.GetFilePathsFromDirectories(PowerShellSubdirectories);

                foreach (var scriptFile in scriptFiles)
                {
                    //exclude files with default powershell service object names
                    if (String.Compare(scriptFile.Key, "SimplePowershell") != 0 && String.Compare(scriptFile.Key, "PowershellVariables") != 0)
                    {
                        ServiceObject so = Helper.CreateServiceObject(scriptFile.Key, "ServiceObject for call script by \"" + scriptFile.Value + "\" path.");

                        so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.DynamicPowerShell.Variables, SoType.Memo, "A JSON serialized array of PowerShell variables."));
                        so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.DynamicPowerShell.ScriptOutput, SoType.Memo, "The full output of the script, as if it was executed on the console."));

                        //RunScript
                        Method mRunScript = Helper.CreateMethod(Constants.Methods.DynamicPowerShell.RunScript, "Runs the bit of PowerShell script provided in file. Returns the ScriptOutput and adds the variables that are needed.", MethodType.Read);
                        mRunScript.InputProperties.Add(Constants.SOProperties.DynamicPowerShell.Variables);
                        mRunScript.MetaData.AddServiceElement(Constants.SOProperties.DynamicPowerShell.MetaDataScriptPath, scriptFile.Value);

                        mRunScript.ReturnProperties.Add(Constants.SOProperties.DynamicPowerShell.ScriptOutput);
                        mRunScript.ReturnProperties.Add(Constants.SOProperties.DynamicPowerShell.Variables);

                        so.Methods.Add(mRunScript);

                        serviceObjects.Add(so);
                    }
                }
            }

            return serviceObjects;
        }

        public override void Execute()
        {
            switch (ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.DynamicPowerShell.RunScript:
                    RunScript();
                    break;

            }
        }

        private void RunScript()
        {
            string serializedVariables = GetStringProperty(Constants.SOProperties.SimplePowerShell.Variables, false);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            //get script path
            string metaDataScriptPath = serviceObject.Methods[0].MetaData.GetServiceElement<string>(Constants.SOProperties.DynamicPowerShell.MetaDataScriptPath);

            //deserialize variables
            List<PowerShellVariablesDC> variablesList = null;
            if (!String.IsNullOrEmpty(serializedVariables))
            {
                variablesList = PowerShellSerializationHelper.DeserializeArrayToList(serializedVariables);
            }
            else 
            {
                variablesList = new List<PowerShellVariablesDC>();
            }
            //run script from file
            string scriptOutput = PowerShellHelper.RunScript(PowerShellHelper.LoadScriptByPath(metaDataScriptPath), variablesList);

            DataRow dr = results.NewRow();
            dr[Constants.SOProperties.DynamicPowerShell.ScriptOutput] = scriptOutput;
            if (variablesList.Count != 0)
            {
                dr[Constants.SOProperties.DynamicPowerShell.Variables] = PowerShellSerializationHelper.SerializeList(variablesList);
            }
            else
            {
                dr[Constants.SOProperties.DynamicPowerShell.Variables] = String.Empty;
            }

            results.Rows.Add(dr);
        }
    }
}
