using K2Field.K2NE.ServiceBroker.Helpers;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using CLIENT=SourceCode.Workflow.Client;
using SourceCode.Workflow.Management;
using SourceCode.Workflow.Management.Criteria;
using System;
using System.Collections.Generic;
using System.Data;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects
{
    public class ProcessInstanceManagementSO : ServiceObjectBase
    {

        public ProcessInstanceManagementSO(K2NEServiceBroker api) : base(api) { }


        public override void Execute()
        {
            switch (base.ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.ProcessInstanceManagement.GotoActivity:
                    GotoActivity();
                    break;

                case Constants.Methods.ProcessInstanceManagement.StartProcessInstance:
                    StartProcessInstance();
                    break;
            }
        }


        public override string ServiceFolder
        {
            get
            {
                return "Management API";
            }
        }
        public override List<ServiceObject> DescribeServiceObjects()
        {
         
            ServiceObject so = Helper.CreateServiceObject("ProcessInstanceManagement", "Exposes functionality to manage a process instances.");

            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceManagement.ActivityName, SoType.Text, "The name of the activity."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId, SoType.Number, "The process instance ID."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceManagement.ProcessFolio, SoType.Text, "The folio to use for the process."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceManagement.ProcessName, SoType.Text, "The full name of the process.")); 
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceManagement.ProcessVersion, SoType.Number, "The full name of the process."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceManagement.StartSync, SoType.YesNo, "Start the process synchronously or not."));

            Method gotoActivity = Helper.CreateMethod(Constants.Methods.ProcessInstanceManagement.GotoActivity, "Move a process instance to a given activity.", MethodType.Execute);
            gotoActivity.InputProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivityName);
            gotoActivity.InputProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);
            gotoActivity.Validation.RequiredProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);
            gotoActivity.Validation.RequiredProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivityName);
            so.Methods.Add(gotoActivity);

            Method startProcessInstance = Helper.CreateMethod(Constants.Methods.ProcessInstanceManagement.StartProcessInstance, "Start a new process instance", MethodType.Create);
            startProcessInstance.InputProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessName);
            startProcessInstance.InputProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessFolio);
            startProcessInstance.InputProperties.Add(Constants.SOProperties.ProcessInstanceManagement.StartSync);
            startProcessInstance.InputProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessVersion);
            startProcessInstance.Validation.RequiredProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessName);
            startProcessInstance.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);
            startProcessInstance.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessFolio);
            so.Methods.Add(startProcessInstance);

            return new List<ServiceObject>() { so };
        }



        private void GotoActivity()
        {
            int processInstanceId = base.GetIntProperty(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);
            string activityName = base.GetStringProperty(Constants.SOProperties.ProcessInstanceManagement.ActivityName);
            
            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = new WorkflowManagementServer();

            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);

                ProcessInstanceCriteriaFilter filter = new ProcessInstanceCriteriaFilter();
                filter.AddRegularFilter(ProcessInstanceFields.ProcInstID, Comparison.Equals, processInstanceId);
                ProcessInstances procInsts = mngServer.GetProcessInstancesAll(filter);
                if (procInsts.Count == 0)
                {
                    throw new ApplicationException(String.Format("Sorry, process instance with id {0} not found.", processInstanceId));
                }

                mngServer.GotoActivity(procInsts[0].ID, activityName);
            
            }
        }



        private void StartProcessInstance()
        {
            string processName = base.GetStringProperty(Constants.SOProperties.ProcessInstanceManagement.ProcessName, true);
            int processVersion = base.GetIntProperty(Constants.SOProperties.ProcessInstanceManagement.ProcessVersion, false);

            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;


            using (CLIENT.Connection k2Con = new CLIENT.Connection())
            {
                k2Con.Open(base.K2ClientConnectionSetup);
                
                CLIENT.ProcessInstance pi;

                if (processVersion > 0)
                {
                    pi = k2Con.CreateProcessInstance(processName, processVersion);
                }
                else
                {
                    pi = k2Con.CreateProcessInstance(processName);
                }

                string folio = base.GetStringProperty(Constants.SOProperties.ProcessInstanceManagement.ProcessFolio);
                if (string.IsNullOrEmpty(folio))
                {
                    pi.Folio = folio;
                }

                k2Con.StartProcessInstance(pi, base.GetBoolProperty(Constants.SOProperties.ProcessInstanceManagement.StartSync));

                DataRow dr = results.NewRow();
                dr[Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId] = pi.ID;
                dr[Constants.SOProperties.ProcessInstanceManagement.ProcessFolio] = pi.Folio;

                results.Rows.Add(dr);

            }

        }


     
    }
}
