using K2Field.K2NE.ServiceBroker.Helpers;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using CLIENT = SourceCode.Workflow.Client;
using SourceCode.Workflow.Management;
using System;
using System.Collections.Generic;
using System.Data;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects.Client_API
{
    public class ProcessInstanceClientSO : ServiceObjectBase
    {
        public ProcessInstanceClientSO(K2NEServiceBroker api) : base(api) { }
        public override string ServiceFolder
        {
            get
            {
                return "Client API";
            }
        }
        public override List<ServiceObject> DescribeServiceObjects()
        {
            ServiceObject so = Helper.CreateServiceObject("ProcessInstanceClient", "Exposes functionality to start the workflow.");

            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceClient.ProcessFolio, SoType.Text, "The folio to use for the process."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceClient.ProcessName, SoType.Text, "The full name of the process."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceClient.StartSync, SoType.YesNo, "Start the process synchronously or not."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceClient.ProcessInstanceId, SoType.Number, "The process instance ID."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceClient.ProcessVersion, SoType.Number, "The version number to start. Leave empty for default."));

            Method startProcessInstance = Helper.CreateMethod(Constants.Methods.ProcessInstanceClient.StartProcessInstance, "Start a new process instance", MethodType.Create);
            startProcessInstance.InputProperties.Add(Constants.SOProperties.ProcessInstanceClient.ProcessName);
            startProcessInstance.InputProperties.Add(Constants.SOProperties.ProcessInstanceClient.ProcessFolio);
            startProcessInstance.InputProperties.Add(Constants.SOProperties.ProcessInstanceClient.ProcessVersion);
            startProcessInstance.InputProperties.Add(Constants.SOProperties.ProcessInstanceClient.StartSync);
            startProcessInstance.Validation.RequiredProperties.Add(Constants.SOProperties.ProcessInstanceClient.ProcessName);
            startProcessInstance.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceClient.ProcessInstanceId);
            startProcessInstance.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceClient.ProcessFolio);
            so.Methods.Add(startProcessInstance);

            //Adding a separate StartWF method for each workflow, exposing DataFields as Parameters
            WorkflowManagementServer mngServer = new WorkflowManagementServer();
            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);

                ProcessSets pSets = mngServer.GetProcSets();
                foreach (ProcessSet pSet in pSets)
                {
                    string displayName = Constants.Methods.ProcessInstanceClient.StartProcess + "_" + pSet.FullName;
                    string description = "Starts " + pSet.FullName;
                    Method m = new Method
                    {
                        Name = pSet.FullName,
                        Type = MethodType.Create,
                        MetaData = new MetaData(displayName, description)
                    };
                    m.InputProperties.Add(Constants.SOProperties.ProcessInstanceClient.ProcessFolio);
                    m.InputProperties.Add(Constants.SOProperties.ProcessInstanceClient.StartSync);
                    m.InputProperties.Add(Constants.SOProperties.ProcessInstanceClient.ProcessVersion);
                    m.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceClient.ProcessInstanceId);
                    m.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceClient.ProcessFolio);

                    foreach (ProcessDataField pDataField in mngServer.GetProcessDataFields(pSet.ProcID))
                    {
                        m.MethodParameters.Add(Helper.CreateParameter(pDataField.Name, GetDataFieldType(pDataField.Type), false, pDataField.Name));
                    }
                    so.Methods.Add(m);
                }
            }
            return new List<ServiceObject>() { so };
        }
        public override void Execute()
        {
            switch (ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.ProcessInstanceClient.StartProcessInstance:
                    StartProcessInstance();
                    break;
                default:
                    StartProcess();
                    break;
            }
        }
        private void StartProcessInstance()
        {
            string processName = GetStringProperty(Constants.SOProperties.ProcessInstanceClient.ProcessName, true);
            int processVersion = GetIntProperty(Constants.SOProperties.ProcessInstanceClient.ProcessVersion);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;


            using (CLIENT.Connection k2Con = new CLIENT.Connection())
            {
                k2Con.Open(K2ClientConnectionSetup);

                CLIENT.ProcessInstance pi;

                if (processVersion > 0)
                {
                    pi = k2Con.CreateProcessInstance(processName, processVersion);
                }
                else
                {
                    pi = k2Con.CreateProcessInstance(processName);
                }

                string folio = GetStringProperty(Constants.SOProperties.ProcessInstanceClient.ProcessFolio);
                if (string.IsNullOrEmpty(folio))
                {
                    pi.Folio = folio;
                }

                k2Con.StartProcessInstance(pi, GetBoolProperty(Constants.SOProperties.ProcessInstanceClient.StartSync));

                DataRow dr = results.NewRow();
                dr[Constants.SOProperties.ProcessInstanceClient.ProcessInstanceId] = pi.ID;
                dr[Constants.SOProperties.ProcessInstanceClient.ProcessFolio] = pi.Folio;

                results.Rows.Add(dr);
            }
        }
        private void StartProcess()
        {
            bool startSync = GetBoolProperty(Constants.SOProperties.ProcessInstanceClient.StartSync);
            string folio = GetStringProperty(Constants.SOProperties.ProcessInstanceClient.ProcessFolio);
            string procName = ServiceBroker.Service.ServiceObjects[0].Methods[0].Name;
            MethodParameters mParams = ServiceBroker.Service.ServiceObjects[0].Methods[0].MethodParameters;

            ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            using (CLIENT.Connection k2Con = new CLIENT.Connection())
            {
                k2Con.Open(K2ClientConnectionSetup);

                CLIENT.ProcessInstance pi = k2Con.CreateProcessInstance(procName);
                pi.Folio = folio;
                foreach (CLIENT.DataField df in pi.DataFields)
                {
                    df.Value = GetDataFieldValue(mParams[df.Name].Value, df.ValueType);
                }
                k2Con.StartProcessInstance(pi, startSync);
                
                DataRow dr = results.NewRow();
                dr[Constants.SOProperties.ProcessInstanceClient.ProcessInstanceId] = pi.ID;
                dr[Constants.SOProperties.ProcessInstanceClient.ProcessFolio] = pi.Folio;
                results.Rows.Add(dr);
            }
        }
        /// <summary>
        /// Used to map Workflow DataField types to SoType of SMOs.
        /// </summary>
        /// <param name="type">Type of the datafield</param>
        /// <returns></returns>
        private SoType GetDataFieldType(int type)
        {
            switch (type)
            {
                case 1: //Boolean
                    return SoType.YesNo;
                case 2: //DateTime
                    return SoType.DateTime;
                case 3: //Decimal
                    return SoType.Decimal;
                case 4: //Double
                    return SoType.Decimal;
                case 5: //Integer
                    return SoType.Number;
                case 6: //Long
                    return SoType.Number;
                case 7: //String
                    return SoType.Text;
                case 8: //Binary
                    return SoType.Memo;
                default:
                    return SoType.Memo;
            }
        }
        /// <summary>
        /// Used to convert parameter value types to datafield ones.
        /// </summary>
        /// <param name="val">Value of the parameter</param>
        /// <param name="dType">DataType of the DataField</param>
        /// <returns></returns>
        private object GetDataFieldValue(object val, CLIENT.DataType dType)
        {
            switch (dType)
            {
                case CLIENT.DataType.TypeBinary:
                    return Convert.FromBase64String(Convert.ToString(val));
                default:
                    return val;
            }
        }
    }
}
