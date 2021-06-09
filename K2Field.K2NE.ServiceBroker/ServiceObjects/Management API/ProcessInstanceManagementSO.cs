﻿using K2Field.K2NE.ServiceBroker.Helpers;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using CLIENT=SourceCode.Workflow.Client;
using SourceCode.Workflow.Management;
using SourceCode.Workflow.Management.Criteria;
using System;
using System.Collections.Generic;
using System.Data;
using K2Field.K2NE.ServiceBroker.Properties;

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
                case Constants.Methods.ProcessInstanceManagement.GotoActivity2:
                    GotoActivity2();
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
                return Constants.ServiceFolders.ManagementAPI;
            }
        }
        public override List<ServiceObject> DescribeServiceObjects()
        {
            ServiceObject so = Helper.CreateServiceObject("ProcessInstanceManagement", "Exposes functionality to manage a process instances.");

            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceManagement.ActivityName, SoType.Text, "The name of the activity."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceManagement.ActivitySystemName, SoType.Text, "The system name of the activity."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId, SoType.Number, "The process instance ID."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceManagement.IgnoreProcessInstanceNotExists, SoType.YesNo, "When 'Yes' suppress an error if a process instance doesn't exists."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ProcessInstanceManagement.GoToActivityResult, SoType.YesNo, "Result of the Goto Activity operation"));
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
            gotoActivity.InputProperties.Add(Constants.SOProperties.ProcessInstanceManagement.IgnoreProcessInstanceNotExists);
            gotoActivity.Validation.RequiredProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);
            gotoActivity.Validation.RequiredProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivityName);

            so.Methods.Add(gotoActivity);

            Method gotoActivity2 = Helper.CreateMethod(Constants.Methods.ProcessInstanceManagement.GotoActivity2, "Move a process instance to a given activity and return results.", MethodType.Read);
            gotoActivity2.InputProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivityName);
            gotoActivity2.InputProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);
            gotoActivity2.InputProperties.Add(Constants.SOProperties.ProcessInstanceManagement.IgnoreProcessInstanceNotExists);
            gotoActivity2.Validation.RequiredProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);
            gotoActivity2.Validation.RequiredProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivityName);
            gotoActivity2.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivityName);
            gotoActivity2.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);
            gotoActivity2.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceManagement.GoToActivityResult);

            so.Methods.Add(gotoActivity2);

            Method listActivities = Helper.CreateMethod(Constants.Methods.ProcessInstanceManagement.ListActivities, "List all activities for a process instance.", MethodType.List);
            listActivities.InputProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);
            listActivities.InputProperties.Add(Constants.SOProperties.ProcessInstanceManagement.IncludeStartActivity);

            listActivities.Validation.RequiredProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);

            listActivities.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivityID);
            listActivities.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivityName);
            listActivities.ReturnProperties.Add(Constants.SOProperties.ProcessInstanceManagement.ActivitySystemName);
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
            _gotoActivity();
        }
        private void GotoActivity2()
        {
            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;
            _gotoActivity(results);
        }

        private void _gotoActivity(DataTable results = null)
        {
            int processInstanceId = base.GetIntProperty(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);
            bool ignoreProcessInstanceNotExists = base.GetBoolProperty(Constants.SOProperties.ProcessInstanceManagement.IgnoreProcessInstanceNotExists);
            string activityName = base.GetStringProperty(Constants.SOProperties.ProcessInstanceManagement.ActivityName);
            string activitySystemName = String.Empty;
            bool gotoActivityResult = false;

            WorkflowManagementServer mngServer = this.ServiceBroker.K2Connection.GetConnection<WorkflowManagementServer>();

            using (mngServer.Connection)
            {

                ProcessInstanceCriteriaFilter filter = new ProcessInstanceCriteriaFilter();
                filter.AddRegularFilter(ProcessInstanceFields.ProcInstID, Comparison.Equals, processInstanceId);
                ProcessInstances procInsts = mngServer.GetProcessInstancesAll(filter);
                if (!ignoreProcessInstanceNotExists & procInsts.Count == 0)
                {
                    throw new ApplicationException(String.Format(Resources.ProcessInstanceNotFound, processInstanceId));
                }

                if (procInsts.Count > 0)
                {
                    int procId = procInsts[0].ProcID;
                    processInstanceId = procInsts[0].ID;

                    Activities procActivities = mngServer.GetProcActivities(procId);

                    foreach (Activity act in procActivities)
                    {
                        if (act.DisplayName == activityName || act.Name == activityName)
                        {
                            activitySystemName = act.Name;
                        }
                    }

                    if (string.IsNullOrEmpty(activitySystemName))
                    {
                        throw new ApplicationException(String.Format(Resources.RequiredPropertyNotFound, activityName));
                    }

                    gotoActivityResult = mngServer.GotoActivity(processInstanceId, activitySystemName);
                }
            }
            if (results != null)
            {
                DataRow row = results.NewRow();
                row[Constants.SOProperties.ProcessInstanceManagement.ActivityName] = activityName + " (" + activitySystemName + ")";
                row[Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId] = processInstanceId;
                row[Constants.SOProperties.ProcessInstanceManagement.GoToActivityResult] = gotoActivityResult;
                results.Rows.Add(row);
            }
        }
        private void ListActivities()
        {
            int processInstanceId = base.GetIntProperty(Constants.SOProperties.ProcessInstanceManagement.ProcessInstanceId);
            bool includeStartActivity = base.GetBoolProperty(Constants.SOProperties.ProcessInstanceManagement.IncludeStartActivity);

            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = this.ServiceBroker.K2Connection.GetConnection<WorkflowManagementServer>();

            using (mngServer.Connection)
            {

                ProcessInstanceCriteriaFilter filter = new ProcessInstanceCriteriaFilter();
                filter.AddRegularFilter(ProcessInstanceFields.ProcInstID, Comparison.Equals, processInstanceId);
                ProcessInstances procInsts = mngServer.GetProcessInstancesAll(filter);
                if (procInsts.Count == 0)
                {
                    throw new ApplicationException(String.Format(Resources.ProcessInstanceNotFound, processInstanceId));
                }

                foreach (Activity actvt in mngServer.GetProcActivities(procInsts[0].ProcID))
                {
                    if (actvt.IsStart && !includeStartActivity)
                        continue;
                    DataRow row = results.NewRow();
                    row[Constants.SOProperties.ProcessInstanceManagement.ActivityID] = actvt.ID;
                    row[Constants.SOProperties.ProcessInstanceManagement.ActivityName] = actvt.DisplayName;
                    row[Constants.SOProperties.ProcessInstanceManagement.ActivitySystemName] = actvt.Name;
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
