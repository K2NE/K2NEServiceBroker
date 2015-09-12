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
            switch (ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.ProcessInstanceManagement.GotoActivity:
                    GotoActivity();
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
            
            Method gotoActivity = Helper.CreateMethod(Constants.Methods.ProcessInstanceManagement.GotoActivity, "Move a process instance to a given activity.", MethodType.Execute);
            gotoActivity.InputProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivityName);
            gotoActivity.InputProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);
            gotoActivity.Validation.RequiredProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);
            gotoActivity.Validation.RequiredProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivityName);
            so.Methods.Add(gotoActivity);

            return new List<ServiceObject>() { so };
        }



        private void GotoActivity()
        {
            int processInstanceId = base.GetIntProperty(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);
            string activityName = base.GetStringProperty(Constants.SOProperties.ProcessInstanceManagement.ActivityName);
            
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
    }
}
