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
                case Constants.Methods.ProcessInstanceManagement.ListActivities:
                    ListActivities();
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
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceManagement.IncludeStartActivity, SoType.YesNo, "Include the Start activity."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceManagement.ActivityDescription, SoType.Text, "The description of the activity"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceManagement.ActivityExpectedDuration, SoType.Number, "The expected duration of the activity."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceManagement.ActivityID, SoType.Number, "The ID of the activity."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceManagement.IsStartActivity, SoType.YesNo, "Indicates if the activity is the Start."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceManagement.ActivityMetaData, SoType.Text, "The metadata of the activity."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceManagement.ActivityPriority, SoType.Number, "The priority of the activity."));

            Method gotoActivity = Helper.CreateMethod(Constants.Methods.ProcessInstanceManagement.GotoActivity, "Move a process instance to a given activity.", MethodType.Execute);
            gotoActivity.InputProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivityName);
            gotoActivity.InputProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);
            gotoActivity.Validation.RequiredProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);
            gotoActivity.Validation.RequiredProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivityName);
            so.Methods.Add(gotoActivity);

            Method listActivities = Helper.CreateMethod(Constants.Methods.ProcessInstanceManagement.ListActivities, "List all activities for a process instance.", MethodType.List);
            listActivities.InputProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);
            listActivities.InputProperties.Add(Constants.SOProperties.ProcessInstanceManagement.IncludeStartActivity);

            listActivities.Validation.RequiredProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);

            listActivities.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivityID);
            listActivities.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivityName);
            listActivities.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivityDescription);
            listActivities.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivityExpectedDuration);
            listActivities.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivityMetaData);
            listActivities.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivityPriority);
            listActivities.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceManagement.IsStartActivity);

            so.Methods.Add(listActivities);

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
        private void ListActivities()
        {
            int processInstanceId = base.GetIntProperty(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);
            bool includeStartActivity = base.GetBoolProperty(Constants.SOProperties.ProcessInstanceManagement.IncludeStartActivity);

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

                foreach (Activity actvt in mngServer.GetProcInstActivities(procInsts[0].ID))
                {
                    if (actvt.IsStart && !includeStartActivity)
                        continue;
                    DataRow row = results.NewRow();
                    row[Constants.SOProperties.ProcessInstanceManagement.ActivityID] = actvt.ID;
                    row[Constants.SOProperties.ProcessInstanceManagement.ActivityName] = actvt.Name;
                    row[Constants.SOProperties.ProcessInstanceManagement.ActivityDescription] = actvt.Description;
                    row[Constants.SOProperties.ProcessInstanceManagement.ActivityExpectedDuration] = actvt.ExpectedDuration;
                    row[Constants.SOProperties.ProcessInstanceManagement.IsStartActivity] = actvt.IsStart;
                    row[Constants.SOProperties.ProcessInstanceManagement.ActivityMetaData] = actvt.MetaData;
                    row[Constants.SOProperties.ProcessInstanceManagement.ActivityPriority] = actvt.Priority;
                    results.Rows.Add(row);
                }
            }
        }
    }
}
