﻿using System;
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

        public string AllowPowershellScript
        {
            get
            {
                if (!ServiceBroker.Service.ServiceConfiguration.Contains(Constants.ConfigurationProperties.AllowPowershellScript))
                {
                    throw new ApplicationException(string.Format(Constants.ErrorMessages.ConfigOptionNotFound, Constants.ConfigurationProperties.AllowPowershellScript));
                }
                return Convert.ToString(ServiceBroker.Service.ServiceConfiguration[Constants.ConfigurationProperties.AllowPowershellScript]);                
            }
        }

        public override List<ServiceObject> DescribeServiceObjects()
        {
            if (AllowPowershellScript != "true")
            {
                return new List<ServiceObject> { };
            }

            ServiceObject so = Helper.CreateServiceObject("SimplePowershell", "An easy and simple way to execute some PowerShell code.");
            
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.SimplePowerShell.PowerShellScript, SoType.Memo, "The PowerShell script to execute. This is a string containing the script. Not a file location."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.SimplePowerShell.Variables, SoType.Memo, "A JSON serialized array of PowerShell variables."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.SimplePowerShell.ScriptOutput, SoType.Memo, "The full output of the script, as if it was executed on the console."));

            //RunScript
            Method mRunScript = Helper.CreateMethod(Constants.Methods.SimplePowerShell.RunScript, "Runs the bit of PowerShell script provided in PowerShellScript. Returns the ScriptOutput and adds the variables that are needed.", MethodType.Read);
            mRunScript.InputProperties.Add(Constants.SOProperties.SimplePowerShell.PowerShellScript);
            mRunScript.Validation.RequiredProperties.Add(Constants.SOProperties.SimplePowerShell.PowerShellScript);
            mRunScript.InputProperties.Add(Constants.SOProperties.SimplePowerShell.Variables);
            
            mRunScript.ReturnProperties.Add(Constants.SOProperties.SimplePowerShell.ScriptOutput);
            mRunScript.ReturnProperties.Add(Constants.SOProperties.SimplePowerShell.Variables);

            so.Methods.Add(mRunScript);

            return new List<ServiceObject> { so };
        }

        public override void Execute()
        {
            switch (ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.SimplePowerShell.RunScript:
                    RunScript();
                    break;
               
            }
        }

        private void RunScript()
        {
            string powerShellScript = GetStringProperty(Constants.SOProperties.SimplePowerShell.PowerShellScript, true);
            string serializedVariables = GetStringProperty(Constants.SOProperties.SimplePowerShell.Variables, false);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            //deserialize variables
            List<PowerShellVariablesDC> variablesList = !String.IsNullOrEmpty(serializedVariables)? PowerShellSerializationHelper.DeserializeArrayToList(serializedVariables) : 
                new List<PowerShellVariablesDC>();
            //run script
            string scriptOutput = PowerShellHelper.RunScript(powerShellScript, variablesList);

            DataRow dr = results.NewRow();
            dr[Constants.SOProperties.SimplePowerShell.ScriptOutput] = scriptOutput;
            dr[Constants.SOProperties.SimplePowerShell.Variables] = variablesList.Count != 0 ? PowerShellSerializationHelper.SerializeList(variablesList) : String.Empty;

            results.Rows.Add(dr);
        }
    }
}
