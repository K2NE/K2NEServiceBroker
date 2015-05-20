using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.Workflow.Client;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using System.Data;

namespace K2Field.K2NE.ServiceBroker
{
    public class WorklistSO : ServiceObjectBase
    {
        public WorklistSO(K2NEServiceBroker worklistAPI) : base(worklistAPI) { }

        public override string ServiceFolder
        {
            get
            {
                return "Client API";
            }
        }

        public override List<ServiceObject> DescribeServiceObjects()
        {
            ServiceObject worklistSO = Helper.CreateServiceObject("Worklist", "ServiceObject that exposes the users worklist.");

            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ProcessName, SoType.Text, "The name of the process."));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ProcessFolder, SoType.Text, "The folder in which the process resides."));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ProcessFullname, SoType.Text, "The full name of the process (folder + \\ + name)."));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ProcessDescription, SoType.Text, "A description of the process."));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ProcessMetadata, SoType.Text, "Metadata defined in the process."));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ProcessStatus, SoType.Text, "The current status of the process instance."));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ProcessPriority, SoType.Number, "The current priority of the process instance."));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ProcessStartdate, SoType.DateTime, "The start date of the process instance."));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ProcessExpectedDuration, SoType.Number, "The expected duration for this process instance in seconds."));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ProcessGuid, SoType.Guid, "The unique guid of the process instance."));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ProcessId, SoType.Number, "The unique id of the process instance."));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ActivityId, SoType.Number, "The unique id of the activity."));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ActivityName, SoType.Text, "The name of the activity."));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ActivityPriority, SoType.Text, "The current priority of the activity."));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ActivityDescription, SoType.Text, "The description of the activity."));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ActivityMetadata, SoType.Text, "The metadata defined on the activity."));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ActivityStartdate, SoType.DateTime, "The start date of the activity instance."));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ActivityExpectedDuration, SoType.Number, "The expected duration of the activity."));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.EventId, SoType.Text, "EventId"));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.EventName, SoType.Text, "EventName"));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.EventMetadata, SoType.Text, "EventMetadata"));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.EventDescription, SoType.Text, "EventDescription"));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.EventPriority, SoType.Text, "EventPriority"));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.EventStartDate, SoType.Text, "EventStartDate"));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.EventExpectedDuration, SoType.Text, "EventExpectedDuration"));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.WorklistItemStatus, SoType.Text, "WorklistItemStatus"));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.OriginalDestination, SoType.Text, "OriginalDestination"));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.Folio, SoType.Text, "Folio"));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.Data, SoType.Text, "Data"));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.SerialNumber, SoType.Text, "SerialNumber"));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.IncludeShared, SoType.YesNo, "Include Shared Tasks"));
            worklistSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.ExcludeAllocated, SoType.YesNo, "Exclude Allocated Tasks"));

            Method getWorkload = Helper.CreateMethod(Constants.Methods.ClientWorklist.GetWorklist, "Provides a client's view of the user workload.", MethodType.List);
            // Input properties, will be used for an excact match in search, combined with 'AND'. Please note that the list is NOT the same as the ReturnProperties because not every field is filterable via API.
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.IncludeShared);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.ExcludeAllocated);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.ProcessName);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.ProcessFolder);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.ProcessFullname);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.ProcessDescription);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.ProcessMetadata);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.ProcessStatus);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.ProcessPriority);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.ProcessStartdate);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.ProcessExpectedDuration);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.ProcessId);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.ActivityName);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.ActivityPriority);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.ActivityDescription);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.ActivityMetadata);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.ActivityStartdate);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.ActivityExpectedDuration);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.EventName);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.EventMetadata);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.EventDescription);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.EventPriority);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.EventStartDate);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.EventExpectedDuration);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.WorklistItemStatus);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.Folio);
            getWorkload.InputProperties.Add(Constants.Properties.ClientWorklist.SerialNumber);

            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.ProcessName);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.ProcessFolder);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.ProcessFullname);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.ProcessDescription);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.ProcessMetadata);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.ProcessStatus);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.ProcessPriority);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.ProcessStartdate);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.ProcessExpectedDuration);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.ProcessGuid);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.ProcessId);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.ActivityId);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.ActivityName);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.ActivityPriority);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.ActivityDescription);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.ActivityMetadata);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.ActivityStartdate);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.ActivityExpectedDuration);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.EventId);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.EventName);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.EventMetadata);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.EventDescription);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.EventPriority);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.EventStartDate);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.EventExpectedDuration);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.WorklistItemStatus);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.OriginalDestination);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.Folio);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.Data);
            getWorkload.ReturnProperties.Add(Constants.Properties.ClientWorklist.SerialNumber);
            worklistSO.Methods.Create(getWorkload);




            ServiceObject worklistItemSO = Helper.CreateServiceObject("WorklistItem", "Exposes functionality for a single worklistitem");


            worklistItemSO.Properties.Create(Helper.CreateProperty(Constants.Properties.ClientWorklist.SerialNumber, SoType.Text, "SerialNumber"));

            Method releaseWorklistItem = Helper.CreateMethod(Constants.Methods.ClientWorklist.ReleaseWorklistItem, "Release a worklistitem.", MethodType.Execute);
            releaseWorklistItem.InputProperties.Add(Constants.Properties.ClientWorklist.SerialNumber);
            releaseWorklistItem.Validation.RequiredProperties.Add(Constants.Properties.ClientWorklist.SerialNumber);
            worklistItemSO.Methods.Create(releaseWorklistItem);



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
            }
        }


        private void ReleaseWorklistItem()
        {
            string sn = base.GetStringProperty(Constants.Properties.ClientWorklist.SerialNumber, true);
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
                if (base.GetBoolProperty(Constants.Properties.ClientWorklist.IncludeShared) == true)
                {
                    wc.AddFilterField(WCLogical.Or, WCField.WorklistItemOwner, "Me", WCCompare.Equal, WCWorklistItemOwner.Me); 
                    wc.AddFilterField(WCLogical.Or, WCField.WorklistItemOwner, "Other", WCCompare.Equal, WCWorklistItemOwner.Other);
                }
                if (base.GetBoolProperty(Constants.Properties.ClientWorklist.ExcludeAllocated) == true)
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
            dr[Constants.Properties.ClientWorklist.ProcessName] = wli.ProcessInstance.Name;
            dr[Constants.Properties.ClientWorklist.ProcessFolder] = wli.ProcessInstance.Folder;
            dr[Constants.Properties.ClientWorklist.ProcessFullname] = wli.ProcessInstance.FullName;
            dr[Constants.Properties.ClientWorklist.ProcessDescription] = wli.ProcessInstance.Description;
            dr[Constants.Properties.ClientWorklist.ProcessMetadata] = wli.ProcessInstance.MetaData;
            dr[Constants.Properties.ClientWorklist.ProcessStatus] = wli.ProcessInstance.Status1;
            dr[Constants.Properties.ClientWorklist.ProcessPriority] = wli.ProcessInstance.Priority;
            dr[Constants.Properties.ClientWorklist.ProcessStartdate] = wli.ProcessInstance.StartDate;
            dr[Constants.Properties.ClientWorklist.ProcessExpectedDuration] = wli.ProcessInstance.ExpectedDuration;
            dr[Constants.Properties.ClientWorklist.ProcessGuid] = wli.ProcessInstance.Guid;
            dr[Constants.Properties.ClientWorklist.ProcessId] = wli.ProcessInstance.ID;
            dr[Constants.Properties.ClientWorklist.ActivityId] = wli.ActivityInstanceDestination.ID;
            dr[Constants.Properties.ClientWorklist.ActivityName] = wli.ActivityInstanceDestination.Name;
            dr[Constants.Properties.ClientWorklist.ActivityPriority] = wli.ActivityInstanceDestination.Priority;
            dr[Constants.Properties.ClientWorklist.ActivityDescription] = wli.ActivityInstanceDestination.Description;
            dr[Constants.Properties.ClientWorklist.ActivityMetadata] = wli.ActivityInstanceDestination.MetaData;
            dr[Constants.Properties.ClientWorklist.ActivityStartdate] = wli.ActivityInstanceDestination.StartDate;
            dr[Constants.Properties.ClientWorklist.ActivityExpectedDuration] = wli.ActivityInstanceDestination.ExpectedDuration;
            dr[Constants.Properties.ClientWorklist.EventId] = wli.EventInstance.ID;
            dr[Constants.Properties.ClientWorklist.EventName] = wli.EventInstance.Name;
            dr[Constants.Properties.ClientWorklist.EventMetadata] = wli.EventInstance.MetaData;
            dr[Constants.Properties.ClientWorklist.EventDescription] = wli.EventInstance.Description;
            dr[Constants.Properties.ClientWorklist.EventPriority] = wli.EventInstance.Priority;
            dr[Constants.Properties.ClientWorklist.EventStartDate] = wli.EventInstance.StartDate;
            dr[Constants.Properties.ClientWorklist.EventExpectedDuration] = wli.EventInstance.ExpectedDuration;
            dr[Constants.Properties.ClientWorklist.WorklistItemStatus] = wli.Status;
            dr[Constants.Properties.ClientWorklist.Folio] = wli.ProcessInstance.Folio;
            dr[Constants.Properties.ClientWorklist.Data] = wli.Data;
            dr[Constants.Properties.ClientWorklist.SerialNumber] = wli.SerialNumber;
            dr[Constants.Properties.ClientWorklist.OriginalDestination] = wli.AllocatedUser;
            table.Rows.Add(dr);
        }



        public static Dictionary<WCField, string> PropertiesToWCFields = new Dictionary<WCField, string>() {
                {WCField.ProcessName, Constants.Properties.ClientWorklist.ProcessName},
                {WCField.ProcessFolder, Constants.Properties.ClientWorklist.ProcessFolder},
                {WCField.ProcessFullName, Constants.Properties.ClientWorklist.ProcessFullname},
                {WCField.ProcessDescription, Constants.Properties.ClientWorklist.ProcessDescription},
                {WCField.ProcessMetaData, Constants.Properties.ClientWorklist.ProcessMetadata},
                {WCField.ProcessStatus, Constants.Properties.ClientWorklist.ProcessStatus},
                {WCField.ProcessPriority, Constants.Properties.ClientWorklist.ProcessPriority},
                {WCField.ProcessStartDate, Constants.Properties.ClientWorklist.ProcessStartdate},
                {WCField.ProcessExpectedDuration, Constants.Properties.ClientWorklist.ProcessExpectedDuration},
                {WCField.ProcessID, Constants.Properties.ClientWorklist.ProcessId},
                {WCField.ActivityName, Constants.Properties.ClientWorklist.ActivityName},
                {WCField.ActivityPriority, Constants.Properties.ClientWorklist.ActivityPriority},
                {WCField.ActivityDescription, Constants.Properties.ClientWorklist.ActivityDescription},
                {WCField.ActivityMetaData, Constants.Properties.ClientWorklist.ActivityMetadata},
                {WCField.ActivityStartDate, Constants.Properties.ClientWorklist.ActivityStartdate},
                {WCField.ActivityExpectedDuration, Constants.Properties.ClientWorklist.ActivityExpectedDuration},
                {WCField.EventName, Constants.Properties.ClientWorklist.EventName},
                {WCField.EventMetaData, Constants.Properties.ClientWorklist.EventMetadata},
                {WCField.EventDescription, Constants.Properties.ClientWorklist.EventDescription},
                {WCField.EventPriority, Constants.Properties.ClientWorklist.EventPriority},
                {WCField.EventStartDate, Constants.Properties.ClientWorklist.EventStartDate},
                {WCField.EventExpectedDuration, Constants.Properties.ClientWorklist.EventExpectedDuration},
                {WCField.WorklistItemStatus, Constants.Properties.ClientWorklist.WorklistItemStatus},
                {WCField.ProcessFolio, Constants.Properties.ClientWorklist.Folio},
                {WCField.SerialNumber, Constants.Properties.ClientWorklist.SerialNumber}
            };

    }
}
