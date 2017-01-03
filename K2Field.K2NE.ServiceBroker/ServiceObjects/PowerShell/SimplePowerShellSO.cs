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
    public class SimplePowerShellSO : ServiceObjectBase
    {
        public SimplePowerShellSO(K2NEServiceBroker api)
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
            if (!AllowPowershellScript)
            {
                return new List<ServiceObject> { };
            }

            ServiceObject so = Helper.CreateServiceObject("SimplePowershell", "An easy and simple way to execute some PowerShell code.");
            
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.SimplePowerShell.PowerShellScript, SoType.Memo, "The PowerShell script to execute. This is a string containing the script. Not a file location."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.SimplePowerShell.Variables, SoType.Memo, "A JSON serialized array of PowerShell variables."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.SimplePowerShell.ScriptOutput, SoType.Memo, "The full output of the script, as if it was executed on the console."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.SimplePowerShell.PowerShellFilePath, SoType.Memo, "The path to PowerShell script file."));

            //RunScript
            Method mRunScriptCode = Helper.CreateMethod(Constants.Methods.SimplePowerShell.RunScriptCode, "Runs the bit of PowerShell script provided in PowerShellScript. Returns the ScriptOutput and adds the variables that are needed.", MethodType.Read);
            mRunScriptCode.InputProperties.Add(Constants.SOProperties.SimplePowerShell.PowerShellScript);
            mRunScriptCode.Validation.RequiredProperties.Add(Constants.SOProperties.SimplePowerShell.PowerShellScript);
            mRunScriptCode.InputProperties.Add(Constants.SOProperties.SimplePowerShell.Variables);
            mRunScriptCode.ReturnProperties.Add(Constants.SOProperties.SimplePowerShell.ScriptOutput);
            mRunScriptCode.ReturnProperties.Add(Constants.SOProperties.SimplePowerShell.Variables);
            so.Methods.Add(mRunScriptCode);

            //RunScriptByFilePath
            Method mRunScriptByFilePath = Helper.CreateMethod(Constants.Methods.SimplePowerShell.RunScriptByFilePath, "Runs the bit of PowerShell script provided from file by path. Returns the ScriptOutput and adds the variables that are needed.", MethodType.Read);
            mRunScriptByFilePath.InputProperties.Add(Constants.SOProperties.SimplePowerShell.PowerShellFilePath);
            mRunScriptByFilePath.Validation.RequiredProperties.Add(Constants.SOProperties.SimplePowerShell.PowerShellFilePath);
            mRunScriptByFilePath.InputProperties.Add(Constants.SOProperties.SimplePowerShell.Variables);
            mRunScriptByFilePath.ReturnProperties.Add(Constants.SOProperties.SimplePowerShell.ScriptOutput);
            mRunScriptByFilePath.ReturnProperties.Add(Constants.SOProperties.SimplePowerShell.Variables);
            so.Methods.Add(mRunScriptByFilePath);

            return new List<ServiceObject> { so };
        }

        public override void Execute()
        {
            switch (ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.SimplePowerShell.RunScriptCode:
                    RunScriptCode();
                    break;
                case Constants.Methods.SimplePowerShell.RunScriptByFilePath:
                    RunScriptByFilePath();
                    break; 
            }
        }

        private void RunScriptCode()
        {
            string powerShellScript = GetStringProperty(Constants.SOProperties.SimplePowerShell.PowerShellScript, true);
            string serializedVariables = GetStringProperty(Constants.SOProperties.SimplePowerShell.Variables, false);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            //deserialize variables
            List<PowerShellVariablesDC> variablesList = new List<PowerShellVariablesDC>();
            if (!String.IsNullOrEmpty(serializedVariables))
            {
                variablesList = PowerShellSerializationHelper.DeserializeArrayToList(serializedVariables);
            }
  
            //run script
            string scriptOutput = PowerShellHelper.RunScriptCode(powerShellScript, variablesList);

            DataRow dr = results.NewRow();
            dr[Constants.SOProperties.SimplePowerShell.ScriptOutput] = scriptOutput;
            if (variablesList.Count != 0)
            {
                dr[Constants.SOProperties.SimplePowerShell.Variables] = PowerShellSerializationHelper.SerializeList(variablesList);
            }
            else
            {
                dr[Constants.SOProperties.SimplePowerShell.Variables] = String.Empty;
            }

            results.Rows.Add(dr);
        }

        private void RunScriptByFilePath()
        {
            string powerShellFilePath = GetStringProperty(Constants.SOProperties.SimplePowerShell.PowerShellFilePath, true);
            string serializedVariables = GetStringProperty(Constants.SOProperties.SimplePowerShell.Variables, false);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            //deserialize variables
            List<PowerShellVariablesDC> variablesList = new List<PowerShellVariablesDC>();
            if (!String.IsNullOrEmpty(serializedVariables))
            {
                variablesList = PowerShellSerializationHelper.DeserializeArrayToList(serializedVariables);
            }
      
            //run script from file
            string scriptOutput = PowerShellHelper.RunScriptFile(powerShellFilePath, variablesList);

            DataRow dr = results.NewRow();
            dr[Constants.SOProperties.SimplePowerShell.ScriptOutput] = scriptOutput;
            if (variablesList.Count != 0)
            {
                dr[Constants.SOProperties.SimplePowerShell.Variables] = PowerShellSerializationHelper.SerializeList(variablesList);
            }
            else
            {
                dr[Constants.SOProperties.SimplePowerShell.Variables] = String.Empty;
            }

            results.Rows.Add(dr);
        }
    }
}
