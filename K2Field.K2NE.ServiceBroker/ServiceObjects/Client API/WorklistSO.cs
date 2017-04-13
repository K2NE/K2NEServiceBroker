using System.Collections.Generic;
using K2Field.K2NE.ServiceBroker.Helpers;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.Workflow.Client;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using System.Data;
using System;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects.Client_API
{
    public class WorklistSO : ServiceObjectBase
    {
        public WorklistSO(K2NEServiceBroker worklistAPI) : base(worklistAPI) { }

        public override string ServiceFolder
        {
            get
            {
                return Constants.ServiceFolders.ClientAPI;
            }
        }

        public override List<ServiceObject> DescribeServiceObjects()
        {
            ServiceObject worklistSO = Helper.CreateServiceObject("Worklist", "ServiceObject that exposes the users worklist.");

            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ProcessName, SoType.Text, "The name of the process."));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ProcessFolder, SoType.Text, "The folder in which the process resides."));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ProcessFullname, SoType.Text, "The full name of the process (folder + \\ + name)."));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ProcessDescription, SoType.Text, "A description of the process."));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ProcessMetadata, SoType.Text, "Metadata defined in the process."));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ProcessStatus, SoType.Text, "The current status of the process instance."));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ProcessPriority, SoType.Number, "The current priority of the process instance."));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ProcessStartdate, SoType.DateTime, "The start date of the process instance."));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ProcessExpectedDuration, SoType.Number, "The expected duration for this process instance in seconds."));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ProcessGuid, SoType.Guid, "The unique guid of the process instance."));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ProcessId, SoType.Number, "The unique id of the process instance."));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ActivityId, SoType.Number, "The unique id of the activity."));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ActivityName, SoType.Text, "The name of the activity."));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ActivityPriority, SoType.Text, "The current priority of the activity."));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ActivityDescription, SoType.Text, "The description of the activity."));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ActivityMetadata, SoType.Text, "The metadata defined on the activity."));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ActivityStartdate, SoType.DateTime, "The start date of the activity instance."));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ActivityExpectedDuration, SoType.Number, "The expected duration of the activity."));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.EventId, SoType.Text, "EventId"));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.EventName, SoType.Text, "EventName"));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.EventMetadata, SoType.Text, "EventMetadata"));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.EventDescription, SoType.Text, "EventDescription"));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.EventPriority, SoType.Text, "EventPriority"));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.EventStartDate, SoType.DateTime, "EventStartDate"));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.EventExpectedDuration, SoType.Text, "EventExpectedDuration"));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.WorklistItemStatus, SoType.Text, "WorklistItemStatus"));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.OriginalDestination, SoType.Text, "OriginalDestination"));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.Folio, SoType.Text, "Folio"));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.Data, SoType.Text, "Data"));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.SerialNumber, SoType.Text, "SerialNumber"));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.IncludeShared, SoType.YesNo, "Include Shared Tasks"));
            worklistSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ExcludeAllocated, SoType.YesNo, "Exclude Allocated Tasks"));


            Method getWorkload = Helper.CreateMethod(Constants.Methods.ClientWorklist.GetWorklist, "Provides a client's view of the user workload.", MethodType.List);
            // Input properties, will be used for an excact match in search, combined with 'AND'. Please note that the list is NOT the same as the ReturnProperties because not every field is filterable via API.
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.IncludeShared);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.ExcludeAllocated);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.ProcessName);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.ProcessFolder);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.ProcessFullname);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.ProcessDescription);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.ProcessMetadata);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.ProcessStatus);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.ProcessPriority);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.ProcessStartdate);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.ProcessExpectedDuration);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.ProcessId);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.ActivityName);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.ActivityPriority);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.ActivityDescription);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.ActivityMetadata);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.ActivityStartdate);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.ActivityExpectedDuration);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.EventName);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.EventMetadata);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.EventDescription);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.EventPriority);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.EventStartDate);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.EventExpectedDuration);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.WorklistItemStatus);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.Folio);
            getWorkload.InputProperties.Add(Constants.SOProperties.ClientWorklist.SerialNumber);

            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.ProcessName);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.ProcessFolder);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.ProcessFullname);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.ProcessDescription);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.ProcessMetadata);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.ProcessStatus);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.ProcessPriority);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.ProcessStartdate);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.ProcessExpectedDuration);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.ProcessGuid);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.ProcessId);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.ActivityId);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.ActivityName);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.ActivityPriority);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.ActivityDescription);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.ActivityMetadata);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.ActivityStartdate);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.ActivityExpectedDuration);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.EventId);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.EventName);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.EventMetadata);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.EventDescription);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.EventPriority);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.EventStartDate);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.EventExpectedDuration);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.WorklistItemStatus);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.OriginalDestination);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.Folio);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.Data);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ClientWorklist.SerialNumber);
            worklistSO.Methods.Add(getWorkload);




            ServiceObject worklistItemSO = Helper.CreateServiceObject("WorklistItem", "Exposes functionality for a single worklistitem");
            worklistItemSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.SerialNumber, SoType.Text, "SerialNumber"));
            worklistItemSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.FQN, SoType.Text, "Fully Qualified User Name"));
            worklistItemSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ActionName, SoType.Text, "The name of the action"));
            worklistItemSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ProcessId, SoType.Number, "The unique id of the process instance."));
            worklistItemSO.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ClientWorklist.ActivityName, SoType.Text, "The name of the activity."));

            Method releaseWorklistItem = Helper.CreateMethod(Constants.Methods.ClientWorklist.ReleaseWorklistItem, "Release a worklistitem.", MethodType.Execute);
            releaseWorklistItem.InputProperties.Add(Constants.SOProperties.ClientWorklist.SerialNumber);
            releaseWorklistItem.Validation.RequiredProperties.Add(Constants.SOProperties.ClientWorklist.SerialNumber);
            worklistItemSO.Methods.Add(releaseWorklistItem);

            Method actionWorklistItem = Helper.CreateMethod(Constants.Methods.ClientWorklist.ActionWorklistItem, "Action a worklistitem", MethodType.Execute);
            actionWorklistItem.InputProperties.Add(Constants.SOProperties.ClientWorklist.ProcessId);
            actionWorklistItem.InputProperties.Add(Constants.SOProperties.ClientWorklist.ActivityName);
            actionWorklistItem.InputProperties.Add(Constants.SOProperties.ClientWorklist.ActionName);
            actionWorklistItem.Validation.RequiredProperties.Add(Constants.SOProperties.ClientWorklist.ProcessId);
            actionWorklistItem.Validation.RequiredProperties.Add(Constants.SOProperties.ClientWorklist.ActivityName);
            actionWorklistItem.Validation.RequiredProperties.Add(Constants.SOProperties.ClientWorklist.ActionName);
            worklistItemSO.Methods.Add(actionWorklistItem);



            Method redirectWorklistItem = Helper.CreateMethod(Constants.Methods.ClientWorklist.RedirectWorklistItem, "Redirect a single worklistitem", MethodType.Execute);
            redirectWorklistItem.InputProperties.Add(Constants.SOProperties.ClientWorklist.SerialNumber);
            redirectWorklistItem.InputProperties.Add(Constants.SOProperties.ClientWorklist.FQN);
            redirectWorklistItem.Validation.RequiredProperties.Add(Constants.SOProperties.ClientWorklist.SerialNumber);
            redirectWorklistItem.Validation.RequiredProperties.Add(Constants.SOProperties.ClientWorklist.FQN);
            worklistItemSO.Methods.Add(redirectWorklistItem);

            return new List<ServiceObject>() { worklistSO, worklistItemSO };

        }


        public override void Execute()
        {
            switch (base.ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.ClientWorklist.GetWorklist:
                    GetWorklist();
                    break;
                case Constants.Methods.ClientWorklist.ReleaseWorklistItem:
                    ReleaseWorklistItem();
                    break;
                case Constants.Methods.ClientWorklist.RedirectWorklistItem:
                    RedirectWorklistItem();
                    break;
                case Constants.Methods.ClientWorklist.ActionWorklistItem:
                    ActionWorklistitem();
                    break;
            }
        }

        private void ActionWorklistitem()
        {
            string processInstanceId = base.GetStringProperty(Constants.SOProperties.ClientWorklist.ProcessId, true);
            string activityName = base.GetStringProperty(Constants.SOProperties.ClientWorklist.ActivityName, true);
            string actionName = base.GetStringProperty(Constants.SOProperties.ClientWorklist.ActionName, true);


            
            using (Connection k2Con = new Connection())
            {
                k2Con.Open(base.K2ClientConnectionSetup);

                WorklistCriteria wc = new WorklistCriteria();
                wc.Platform = base.Platform;
                wc.AddFilterField(WCField.ProcessID, WCCompare.Equal, processInstanceId);
                wc.AddFilterField(WCLogical.And, WCField.ActivityName, WCCompare.Equal, activityName);
                Worklist wl = k2Con.OpenWorklist(wc);

                if (wl.TotalCount == 0)
                {
                    throw new ApplicationException("No worklist item found with those criteria.");
                }

                if (wl.TotalCount > 1)
                {
                    throw new ApplicationException("More than one worklist item found with those criteria.");
                }

                
                foreach (SourceCode.Workflow.Client.Action a in wl[0].Actions)
                {
                    if (string.Compare(a.Name, actionName, true) == 0)
                    {
                        a.Execute();
                        k2Con.Close();
                        return;
                    }
                }
                k2Con.Close();

                throw new ApplicationException("Failed to find the action specified. Worklist item was found.");
            }
        }

        private void RedirectWorklistItem()
        {
            string sn = base.GetStringProperty(Constants.SOProperties.ClientWorklist.SerialNumber, true);
            string fqn = base.GetStringProperty(Constants.SOProperties.ClientWorklist.FQN, true);

            using (Connection k2Con = new Connection())
            {
                k2Con.Open(base.K2ClientConnectionSetup);

                WorklistItem wli = k2Con.OpenWorklistItem(sn);
                wli.Redirect(fqn);

                k2Con.Close();
            }
        }


        private void ReleaseWorklistItem()
        {
            string sn = base.GetStringProperty(Constants.SOProperties.ClientWorklist.SerialNumber, true);
            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            using (Connection k2Con = new Connection())
            {
                k2Con.Open(base.K2ClientConnectionSetup);

                WorklistItem wli = k2Con.OpenWorklistItem(sn);
                wli.Release();

                k2Con.Close();
            }
        }


        private void GetWorklist()
        {
            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            using (Connection k2Con = new Connection())
            {
                k2Con.Open(base.K2ClientConnectionSetup);
                
                WorklistCriteria wc = new WorklistCriteria();
                wc.Platform = base.Platform;
                AddFieldFilters(wc);
                if (base.GetBoolProperty(Constants.SOProperties.ClientWorklist.IncludeShared) == true)
                {
                    wc.AddFilterField(WCLogical.Or, WCField.WorklistItemOwner, "Me", WCCompare.Equal, WCWorklistItemOwner.Me); 
                    wc.AddFilterField(WCLogical.Or, WCField.WorklistItemOwner, "Other", WCCompare.Equal, WCWorklistItemOwner.Other);
                }
                if (base.GetBoolProperty(Constants.SOProperties.ClientWorklist.ExcludeAllocated) == true)
                {
                    wc.AddFilterField(WCLogical.And, WCField.WorklistItemStatus, WCCompare.NotEqual, WorklistStatus.Allocated);
                }
                Worklist wl = k2Con.OpenWorklist(wc);

                foreach (WorklistItem wli in wl)
                {
                    AddRowToDataTable(results, wli);
                }

                k2Con.Close();
            }
        }

        private void AddFieldFilters(WorklistCriteria wc)
        {
            foreach (KeyValuePair<WCField, string> filterField in PropertiesToWCFields)
            {
                Property p = base.ServiceBroker.Service.ServiceObjects[0].Properties[filterField.Value];
                if (p != null && p.Value != null)
                {
                    wc.AddFilterField(filterField.Key, WCCompare.Equal, p.Value);
                }
            }
        }

        private void AddRowToDataTable(DataTable table, WorklistItem wli)
        {
            DataRow dr = table.NewRow();
            dr[Constants.SOProperties.ClientWorklist.ProcessName] = wli.ProcessInstance.Name;
            dr[Constants.SOProperties.ClientWorklist.ProcessFolder] = wli.ProcessInstance.Folder;
            dr[Constants.SOProperties.ClientWorklist.ProcessFullname] = wli.ProcessInstance.FullName;
            dr[Constants.SOProperties.ClientWorklist.ProcessDescription] = wli.ProcessInstance.Description;
            dr[Constants.SOProperties.ClientWorklist.ProcessMetadata] = wli.ProcessInstance.MetaData;
            dr[Constants.SOProperties.ClientWorklist.ProcessStatus] = wli.ProcessInstance.Status1;
            dr[Constants.SOProperties.ClientWorklist.ProcessPriority] = wli.ProcessInstance.Priority;
            dr[Constants.SOProperties.ClientWorklist.ProcessStartdate] = wli.ProcessInstance.StartDate;
            dr[Constants.SOProperties.ClientWorklist.ProcessExpectedDuration] = wli.ProcessInstance.ExpectedDuration;
            dr[Constants.SOProperties.ClientWorklist.ProcessGuid] = wli.ProcessInstance.Guid;
            dr[Constants.SOProperties.ClientWorklist.ProcessId] = wli.ProcessInstance.ID;
            dr[Constants.SOProperties.ClientWorklist.ActivityId] = wli.ActivityInstanceDestination.ID;
            dr[Constants.SOProperties.ClientWorklist.ActivityName] = wli.ActivityInstanceDestination.Name;
            dr[Constants.SOProperties.ClientWorklist.ActivityPriority] = wli.ActivityInstanceDestination.Priority;
            dr[Constants.SOProperties.ClientWorklist.ActivityDescription] = wli.ActivityInstanceDestination.Description;
            dr[Constants.SOProperties.ClientWorklist.ActivityMetadata] = wli.ActivityInstanceDestination.MetaData;
            dr[Constants.SOProperties.ClientWorklist.ActivityStartdate] = wli.ActivityInstanceDestination.StartDate;
            dr[Constants.SOProperties.ClientWorklist.ActivityExpectedDuration] = wli.ActivityInstanceDestination.ExpectedDuration;
            dr[Constants.SOProperties.ClientWorklist.EventId] = wli.EventInstance.ID;
            dr[Constants.SOProperties.ClientWorklist.EventName] = wli.EventInstance.Name;
            dr[Constants.SOProperties.ClientWorklist.EventMetadata] = wli.EventInstance.MetaData;
            dr[Constants.SOProperties.ClientWorklist.EventDescription] = wli.EventInstance.Description;
            dr[Constants.SOProperties.ClientWorklist.EventPriority] = wli.EventInstance.Priority;
            dr[Constants.SOProperties.ClientWorklist.EventStartDate] = wli.EventInstance.StartDate;
            dr[Constants.SOProperties.ClientWorklist.EventExpectedDuration] = wli.EventInstance.ExpectedDuration;
            dr[Constants.SOProperties.ClientWorklist.WorklistItemStatus] = wli.Status;
            dr[Constants.SOProperties.ClientWorklist.Folio] = wli.ProcessInstance.Folio;
            dr[Constants.SOProperties.ClientWorklist.Data] = wli.Data;
            dr[Constants.SOProperties.ClientWorklist.SerialNumber] = wli.SerialNumber;
            dr[Constants.SOProperties.ClientWorklist.OriginalDestination] = wli.AllocatedUser;
            table.Rows.Add(dr);
        }



        public static Dictionary<WCField, string> PropertiesToWCFields = new Dictionary<WCField, string>() {
                {WCField.ProcessName, Constants.SOProperties.ClientWorklist.ProcessName},
                {WCField.ProcessFolder, Constants.SOProperties.ClientWorklist.ProcessFolder},
                {WCField.ProcessFullName, Constants.SOProperties.ClientWorklist.ProcessFullname},
                {WCField.ProcessDescription, Constants.SOProperties.ClientWorklist.ProcessDescription},
                {WCField.ProcessMetaData, Constants.SOProperties.ClientWorklist.ProcessMetadata},
                {WCField.ProcessStatus, Constants.SOProperties.ClientWorklist.ProcessStatus},
                {WCField.ProcessPriority, Constants.SOProperties.ClientWorklist.ProcessPriority},
                {WCField.ProcessStartDate, Constants.SOProperties.ClientWorklist.ProcessStartdate},
                {WCField.ProcessExpectedDuration, Constants.SOProperties.ClientWorklist.ProcessExpectedDuration},
                {WCField.ProcessID, Constants.SOProperties.ClientWorklist.ProcessId},
                {WCField.ActivityName, Constants.SOProperties.ClientWorklist.ActivityName},
                {WCField.ActivityPriority, Constants.SOProperties.ClientWorklist.ActivityPriority},
                {WCField.ActivityDescription, Constants.SOProperties.ClientWorklist.ActivityDescription},
                {WCField.ActivityMetaData, Constants.SOProperties.ClientWorklist.ActivityMetadata},
                {WCField.ActivityStartDate, Constants.SOProperties.ClientWorklist.ActivityStartdate},
                {WCField.ActivityExpectedDuration, Constants.SOProperties.ClientWorklist.ActivityExpectedDuration},
                {WCField.EventName, Constants.SOProperties.ClientWorklist.EventName},
                {WCField.EventMetaData, Constants.SOProperties.ClientWorklist.EventMetadata},
                {WCField.EventDescription, Constants.SOProperties.ClientWorklist.EventDescription},
                {WCField.EventPriority, Constants.SOProperties.ClientWorklist.EventPriority},
                {WCField.EventStartDate, Constants.SOProperties.ClientWorklist.EventStartDate},
                {WCField.EventExpectedDuration, Constants.SOProperties.ClientWorklist.EventExpectedDuration},
                {WCField.WorklistItemStatus, Constants.SOProperties.ClientWorklist.WorklistItemStatus},
                {WCField.ProcessFolio, Constants.SOProperties.ClientWorklist.Folio},
                {WCField.SerialNumber, Constants.SOProperties.ClientWorklist.SerialNumber}
            };

    }
}
