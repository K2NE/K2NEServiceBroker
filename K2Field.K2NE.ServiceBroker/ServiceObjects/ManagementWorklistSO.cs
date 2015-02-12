using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using SourceCode.Hosting.Client.BaseAPI;
using SourceCode.SmartObjects.Services.ServiceSDK;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using SourceCode.Workflow.Management;
using SourceCode.Workflow.Management.Criteria;

namespace K2Field.K2NE.ServiceObjects
{
    /// <summary>
    /// Implementation of the Management Service Object
    /// </summary>
    public class ManagementWorklistSO : ServiceObjectBase
    {

        public ManagementWorklistSO(K2NEServiceBroker api) : base(api) { }


        public override string ServiceFolder
        {
            get
            {
                return "Management API";
            }
        }

        public override void Execute()
        {
            switch (base.ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.ManagementWorklist.GetWorklist:
                    GetWorklist();
                    break;
            }
        }

        public override List<ServiceObject> DescribeServiceObjects()
        {
            ServiceObject so = Helper.CreateServiceObject("ManagementWorklist", "Exposes the management worklist.");

            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ManagementWorklist.ActivityId, SoType.Number, "Activity Id"));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ManagementWorklist.ActivityInstanceDestinationId, SoType.Number, "Activity Instance Destination Id"));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ManagementWorklist.ActivityInstanceId, SoType.Number, "Activity Instance Id"));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ManagementWorklist.ActivityName, SoType.Text, "The name of the activity."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ManagementWorklist.Destination, SoType.Text, "The destination of the worklist item."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ManagementWorklist.DestinationType, SoType.Text, "The type of the destination (user, group, role)"));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ManagementWorklist.EventId, SoType.Number, "Event Id"));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ManagementWorklist.EventName, SoType.Text, "The name of the event."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ManagementWorklist.Folio, SoType.Text, "The process folio."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ManagementWorklist.ProcessInstanceId, SoType.Number, "The process instance ID."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ManagementWorklist.ProcessName, SoType.Text, "The name of the process."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ManagementWorklist.ProcessStatus, SoType.Text, "The status of the process."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ManagementWorklist.StartDate, SoType.DateTime, "The date when the worklist item was created."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ManagementWorklist.WorklistItemStatus, SoType.Text, "The current status of the worklist item."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ManagementWorklist.WorklistItemId, SoType.Number, "The ID of the worklistitem."));

            Method getWorkload = Helper.CreateMethod(Constants.Methods.ManagementWorklist.GetWorklist, "Provides a management view of the user workload.", MethodType.List);
            getWorkload.ReturnProperties.Add(Constants.Properties.ManagementWorklist.ActivityId);
            getWorkload.ReturnProperties.Add(Constants.Properties.ManagementWorklist.ActivityInstanceDestinationId);
            getWorkload.ReturnProperties.Add(Constants.Properties.ManagementWorklist.ActivityInstanceId);
            getWorkload.ReturnProperties.Add(Constants.Properties.ManagementWorklist.ActivityName);
            getWorkload.ReturnProperties.Add(Constants.Properties.ManagementWorklist.Destination);
            getWorkload.ReturnProperties.Add(Constants.Properties.ManagementWorklist.DestinationType);
            getWorkload.ReturnProperties.Add(Constants.Properties.ManagementWorklist.EventId);
            getWorkload.ReturnProperties.Add(Constants.Properties.ManagementWorklist.EventName);
            getWorkload.ReturnProperties.Add(Constants.Properties.ManagementWorklist.Folio);
            getWorkload.ReturnProperties.Add(Constants.Properties.ManagementWorklist.ProcessInstanceId);
            getWorkload.ReturnProperties.Add(Constants.Properties.ManagementWorklist.ProcessName);
            getWorkload.ReturnProperties.Add(Constants.Properties.ManagementWorklist.ProcessStatus);
            getWorkload.ReturnProperties.Add(Constants.Properties.ManagementWorklist.StartDate);
            getWorkload.ReturnProperties.Add(Constants.Properties.ManagementWorklist.WorklistItemStatus);
            getWorkload.ReturnProperties.Add(Constants.Properties.ManagementWorklist.WorklistItemId);
            so.Methods.Add(getWorkload);

            return new List<ServiceObject> { so };
        }



        private void GetWorklist()
        {
            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = new WorkflowManagementServer();

            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);

                WorklistCriteriaFilter filter = new WorklistCriteriaFilter();
                WorklistItems wlItems = mngServer.GetWorklistItems(filter);

                foreach (WorklistItem wlItem in wlItems)
                {
                    DataRow row = CreateWorklistItemRow(results, wlItem);
                    results.Rows.Add(row);
                }
            }
        }

        private DataRow CreateWorklistItemRow(DataTable table, WorklistItem wlItem)
        {
            DataRow r = table.NewRow();
            r[Constants.Properties.ManagementWorklist.ActivityId] = wlItem.ActID;
            r[Constants.Properties.ManagementWorklist.ActivityInstanceDestinationId] = wlItem.ActInstDestID;
            r[Constants.Properties.ManagementWorklist.ActivityInstanceId] = wlItem.ActInstID;
            r[Constants.Properties.ManagementWorklist.ActivityName] = wlItem.ActivityName;
            r[Constants.Properties.ManagementWorklist.Destination] = wlItem.Destination;
            //TODO: r[CoProperties.ManagementWorklistProperty.DestinationType] = wlItem.;
            r[Constants.Properties.ManagementWorklist.EventId] = wlItem.EventID;
            r[Constants.Properties.ManagementWorklist.EventName] = wlItem.EventName;
            r[Constants.Properties.ManagementWorklist.Folio] = wlItem.Folio;
            r[Constants.Properties.ManagementWorklist.ProcessInstanceId] = wlItem.ProcInstID;
            r[Constants.Properties.ManagementWorklist.ProcessStatus] = wlItem.ProcessInstanceStatus.ToString();
            r[Constants.Properties.ManagementWorklist.ProcessName] = wlItem.ProcName;
            r[Constants.Properties.ManagementWorklist.StartDate] = wlItem.StartDate;
            r[Constants.Properties.ManagementWorklist.WorklistItemStatus] = wlItem.Status.ToString();
            r[Constants.Properties.ManagementWorklist.WorklistItemId] = wlItem.ID;
            return r;
        }
    }
}