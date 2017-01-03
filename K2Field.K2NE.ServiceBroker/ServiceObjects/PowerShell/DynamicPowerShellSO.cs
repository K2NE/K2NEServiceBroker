using K2Field.K2NE.ServiceBroker.Helpers.PowerShell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using K2Field.K2NE.ServiceBroker.Helpers;
using System.Data;
using System.Management.Automation.Language;

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

            if (!String.IsNullOrEmpty(PowerShellSubdirectories))
            {
                //get all files from powershell directories 

                Dictionary<string, string> scriptFiles;
                try
                {
                    scriptFiles = PowerShellHelper.GetFilePathsFromDirectories(PowerShellSubdirectories);
                }
                catch (Exception ex)
                {
                    base.ServiceBroker.HostServiceLogger.LogError("Failed to retrieve PowerShell files.");
                    base.ServiceBroker.HostServiceLogger.LogException(ex);
                    return serviceObjects;
                }

                foreach (KeyValuePair<string, string> scriptFile in scriptFiles)
                {

                    if (String.Compare(scriptFile.Key, "SimplePowershell", true) == 0 || String.Compare(scriptFile.Key, "PowershellVariables", true) == 0)
                    {
                        base.ServiceBroker.HostServiceLogger.LogError(string.Format("Not creating service object for PowerShell script '{0}'. This is a system service object already.", scriptFile.Key));
                        continue;
                    }


                    ServiceObject so = Helper.CreateServiceObject(scriptFile.Key, string.Format("ServiceObject for call script '{0}'", scriptFile.Value));

                    so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.DynamicPowerShell.Variables, SoType.Memo, "A JSON serialized array of PowerShell variables."));
                    so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.DynamicPowerShell.ScriptOutput, SoType.Memo, "The full output of the script, as if it was executed on the console."));

                    //RunScript
                    Method mRunScript = Helper.CreateMethod(Constants.Methods.DynamicPowerShell.RunScript, "Runs the bit of PowerShell script provided in file. Returns the ScriptOutput and adds the variables that are needed.", MethodType.Read);
                    mRunScript.InputProperties.Add(Constants.SOProperties.DynamicPowerShell.Variables);
                    mRunScript.MetaData.AddServiceElement(Constants.SOProperties.DynamicPowerShell.MetaDataScriptPath, scriptFile.Value);
                    mRunScript.ReturnProperties.Add(Constants.SOProperties.DynamicPowerShell.ScriptOutput);
                    mRunScript.ReturnProperties.Add(Constants.SOProperties.DynamicPowerShell.Variables);
                    so.Methods.Add(mRunScript);

                    //parsing internal functions
                    List<FunctionDefinitionAst> internalFunctions = PowerShellHelper.GetInternalFunctionsFromScriptByPath(scriptFile.Value);

                    foreach (FunctionDefinitionAst internalFunction in internalFunctions)
                    {
                        if (string.Compare(internalFunction.Name, "RunScript", true) == 0)
                        {
                            base.ServiceBroker.HostServiceLogger.LogError(string.Format("Not creating Service Object Method for PowerShell function '{0}' in script '{1}'. This is a system method.", internalFunction.Name, scriptFile.Key));
                            continue;
                        }

                        Method mFunction = Helper.CreateMethod(internalFunction.Name, "Internal function", MethodType.Read);


                        if (internalFunction.Parameters != null)
                        {
                            foreach (ParameterAst parameter in internalFunction.Parameters)
                            {
                                so.Properties.Add(Helper.CreateProperty(parameter.Name.ToString(), SoType.Memo, "Function parameter"));
                                mFunction.InputProperties.Add(parameter.Name.ToString());
                            }
                        }


                        mFunction.MetaData.AddServiceElement(Constants.SOProperties.DynamicPowerShell.MetaDataPSFunctionName, internalFunction.Name);
                        mFunction.MetaData.AddServiceElement(Constants.SOProperties.DynamicPowerShell.MetaDataScriptPath, scriptFile.Value);
                        mFunction.ReturnProperties.Add(Constants.SOProperties.DynamicPowerShell.ScriptOutput);
                        so.Methods.Add(mFunction);
                    }

                    serviceObjects.Add(so);

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
                default:
                    RunFunction();
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
            List<PowerShellVariablesDC> variablesList = new List<PowerShellVariablesDC>();
            if (!String.IsNullOrEmpty(serializedVariables))
            {
                variablesList = PowerShellSerializationHelper.DeserializeArrayToList(serializedVariables);
            }
    
            //run script from file
            string scriptOutput = PowerShellHelper.RunScriptFile(metaDataScriptPath, variablesList);

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

        private void RunFunction()
        {
            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;


            string metaDataPSFunctionName = serviceObject.Methods[0].MetaData.GetServiceElement<string>(Constants.SOProperties.DynamicPowerShell.MetaDataPSFunctionName);
            string metaDataScriptPath = serviceObject.Methods[0].MetaData.GetServiceElement<string>(Constants.SOProperties.DynamicPowerShell.MetaDataScriptPath);

            //get list of functions from powershell script and get current function by name
            List<FunctionDefinitionAst> powerShellInternalFunctions = PowerShellHelper.GetInternalFunctionsFromScriptByPath(metaDataScriptPath);
            FunctionDefinitionAst currentFunctionMetaData = powerShellInternalFunctions.Where(s => String.Compare(s.Name, metaDataPSFunctionName) == 0).FirstOrDefault();

            //getting input parameters
            Dictionary<string, string> functionInputParameters = new Dictionary<string, string>();

            if (currentFunctionMetaData.Parameters != null)
            {
                foreach (ParameterAst parameter in currentFunctionMetaData.Parameters)
                {
                    string parameterValue = GetStringProperty(parameter.Name.ToString(), false);

                    if (!String.IsNullOrEmpty(parameterValue))
                    {
                        functionInputParameters.Add(parameter.Name.ToString(), parameterValue);
                    }
                    else
                    {
                        functionInputParameters.Add(parameter.Name.ToString(), String.Empty);
                    }
                }
            }

            //script based on functions
            string scriptBasedOnFunctions = PowerShellHelper.BuildScriptBasedOnFunctions(powerShellInternalFunctions);

            //run script
            string scriptOutput = PowerShellHelper.RunFunction(currentFunctionMetaData, functionInputParameters, scriptBasedOnFunctions);

            DataRow dr = results.NewRow();
            dr[Constants.SOProperties.DynamicPowerShell.ScriptOutput] = scriptOutput;

            results.Rows.Add(dr);
        }
    }
}
