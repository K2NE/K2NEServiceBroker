using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using CLIENT=SourceCode.Workflow.Client;
using SourceCode.Workflow.Management;
using SourceCode.Workflow.Management.Criteria;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace K2Field.K2NE.ServiceBroker
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

            so.Properties.Create(Helper.CreateProperty(Constants.Properties.ProcessInstanceManagement.ActivityName, SoType.Text, "The name of the activity."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.ProcessInstanceManagement.ProcessInstanceId, SoType.Number, "The process instance ID."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.ProcessInstanceManagement.ProcessFolio, SoType.Text, "The folio to use for the process."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.ProcessInstanceManagement.ProcessName, SoType.Text, "The full name of the process.")); 
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.ProcessInstanceManagement.ProcessVersion, SoType.Number, "The full name of the process."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.ProcessInstanceManagement.StartSync, SoType.YesNo, "Start the process synchronously or not."));

            Method gotoActivity = Helper.CreateMethod(Constants.Methods.ProcessInstanceManagement.GotoActivity, "Move a process instance to a given activity.", MethodType.Execute);
            gotoActivity.InputProperties.Add(Constants.Properties.ProcessInstanceManagement.ActivityName);
            gotoActivity.InputProperties.Add(Constants.Properties.ProcessInstanceManagement.ProcessInstanceId);
            gotoActivity.Validation.RequiredProperties.Add(Constants.Properties.ProcessInstanceManagement.ProcessInstanceId);
            gotoActivity.Validation.RequiredProperties.Add(Constants.Properties.ProcessInstanceManagement.ActivityName);
            so.Methods.Create(gotoActivity);

            Method startProcessInstance = Helper.CreateMethod(Constants.Methods.ProcessInstanceManagement.StartProcessInstance, "Start a new process instance", MethodType.Create);
            startProcessInstance.InputProperties.Add(Constants.Properties.ProcessInstanceManagement.ProcessName);
            startProcessInstance.InputProperties.Add(Constants.Properties.ProcessInstanceManagement.ProcessFolio);
            startProcessInstance.InputProperties.Add(Constants.Properties.ProcessInstanceManagement.StartSync);
            startProcessInstance.InputProperties.Add(Constants.Properties.ProcessInstanceManagement.ProcessVersion);
            startProcessInstance.Validation.RequiredProperties.Add(Constants.Properties.ProcessInstanceManagement.ProcessName);
            startProcessInstance.ReturnProperties.Add(Constants.Properties.ProcessInstanceManagement.ProcessInstanceId);
            startProcessInstance.ReturnProperties.Add(Constants.Properties.ProcessInstanceManagement.ProcessFolio);
            so.Methods.Create(startProcessInstance);

            return new List<ServiceObject>() { so };
        }



        private void GotoActivity()
        {
            int processInstanceId = base.GetIntProperty(Constants.Properties.ProcessInstanceManagement.ProcessInstanceId);
            string activityName = base.GetStringProperty(Constants.Properties.ProcessInstanceManagement.ActivityName);
            
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
            string processName = base.GetStringProperty(Constants.Properties.ProcessInstanceManagement.ProcessName, true);
            int processVersion = base.GetIntProperty(Constants.Properties.ProcessInstanceManagement.ProcessVersion, false);

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

                string folio = base.GetStringProperty(Constants.Properties.ProcessInstanceManagement.ProcessFolio);
                if (string.IsNullOrEmpty(folio))
                {
                    pi.Folio = folio;
                }

                k2Con.StartProcessInstance(pi, base.GetBoolProperty(Constants.Properties.ProcessInstanceManagement.StartSync));

                DataRow dr = results.NewRow();
                dr[Constants.Properties.ProcessInstanceManagement.ProcessInstanceId] = pi.ID;
                dr[Constants.Properties.ProcessInstanceManagement.ProcessFolio] = pi.Folio;

                results.Rows.Add(dr);

            }

        }


     
    }
}
