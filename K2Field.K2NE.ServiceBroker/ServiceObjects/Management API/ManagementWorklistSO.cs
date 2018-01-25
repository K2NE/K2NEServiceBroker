using System.Collections.Generic;
using System.Data;
using K2Field.K2NE.ServiceBroker.Helpers;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using SourceCode.Workflow.Management;
using SourceCode.Workflow.Management.Criteria;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects
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
                return Constants.ServiceFolders.ManagementAPI;
            }
        }

        public override void Execute()
        {
            switch (base.ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.ManagementWorklist.GetWorklist:
                    GetWorklist();
                    break;
                case Constants.Methods.ManagementWorklist.RedirectWorklistItem:
                    RedirectWorklistItem();
                    break;

                case Constants.Methods.ManagementWorklist.ReleaseWorklistItem:
                    ReleaseWorklistItem();
                    break;
            }
        }

        public override List<ServiceObject> DescribeServiceObjects()
        {
            ServiceObject so = Helper.CreateServiceObject("ManagementWorklist", "Exposes the management worklist.");

            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ManagementWorklist.ActivityId, SoType.Number, "Activity Id"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ManagementWorklist.ActivityInstanceDestinationId, SoType.Number, "Activity Instance Destination Id"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ManagementWorklist.ActivityInstanceId, SoType.Number, "Activity Instance Id"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ManagementWorklist.ActivityName, SoType.Text, "The name of the activity."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ManagementWorklist.Destination, SoType.Text, "The destination of the worklist item."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ManagementWorklist.DestinationType, SoType.Text, "The type of the destination (user, group, role)"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ManagementWorklist.EventId, SoType.Number, "Event Id"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ManagementWorklist.EventName, SoType.Text, "The name of the event."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ManagementWorklist.Folio, SoType.Text, "The process folio."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ManagementWorklist.ProcessInstanceId, SoType.Number, "The process instance ID."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ManagementWorklist.ProcessName, SoType.Text, "The name of the process."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ManagementWorklist.ProcessStatus, SoType.Text, "The status of the process."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ManagementWorklist.StartDate, SoType.DateTime, "The date when the worklist item was created."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ManagementWorklist.WorklistItemStatus, SoType.Text, "The current status of the worklist item."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ManagementWorklist.WorklistItemId, SoType.Number, "The ID of the worklistitem."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ManagementWorklist.FromUser, SoType.Text, "The FQN of the user to redirect/delegate from."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ManagementWorklist.ToUser, SoType.Text, "The FQN of the user to redirect/delegate to."));



            Method getWorkload = Helper.CreateMethod(Constants.Methods.ManagementWorklist.GetWorklist, "Provides a management view of the user workload.", MethodType.List);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ManagementWorklist.ActivityId);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ManagementWorklist.ActivityInstanceDestinationId);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ManagementWorklist.ActivityInstanceId);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ManagementWorklist.ActivityName);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ManagementWorklist.Destination);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ManagementWorklist.DestinationType);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ManagementWorklist.EventId);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ManagementWorklist.EventName);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ManagementWorklist.Folio);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ManagementWorklist.ProcessInstanceId);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ManagementWorklist.ProcessName);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ManagementWorklist.ProcessStatus);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ManagementWorklist.StartDate);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ManagementWorklist.WorklistItemStatus);
            getWorkload.ReturnProperties.Add(Constants.SOProperties.ManagementWorklist.WorklistItemId);
            so.Methods.Add(getWorkload);



            Method mRedirectWorklistItem = Helper.CreateMethod(Constants.Methods.ManagementWorklist.RedirectWorklistItem, "Redirect the worklistitem to another user", SourceCode.SmartObjects.Services.ServiceSDK.Types.MethodType.Execute);
            mRedirectWorklistItem.InputProperties.Add(Constants.SOProperties.ManagementWorklist.ProcessInstanceId);
            mRedirectWorklistItem.InputProperties.Add(Constants.SOProperties.ManagementWorklist.WorklistItemId);
            mRedirectWorklistItem.InputProperties.Add(Constants.SOProperties.ManagementWorklist.ActivityInstanceDestinationId);
            mRedirectWorklistItem.InputProperties.Add(Constants.SOProperties.ManagementWorklist.FromUser);
            mRedirectWorklistItem.InputProperties.Add(Constants.SOProperties.ManagementWorklist.ToUser); 
            mRedirectWorklistItem.Validation.RequiredProperties.Add(Constants.SOProperties.ManagementWorklist.ProcessInstanceId);
            mRedirectWorklistItem.Validation.RequiredProperties.Add(Constants.SOProperties.ManagementWorklist.WorklistItemId);
            mRedirectWorklistItem.Validation.RequiredProperties.Add(Constants.SOProperties.ManagementWorklist.ActivityInstanceDestinationId);
            mRedirectWorklistItem.Validation.RequiredProperties.Add(Constants.SOProperties.ManagementWorklist.ToUser);
            
            so.Methods.Add(mRedirectWorklistItem);

            Method mReleaseWorklistItem = Helper.CreateMethod(Constants.Methods.ManagementWorklist.ReleaseWorklistItem, "Release the worklistitem slot back into the wild", SourceCode.SmartObjects.Services.ServiceSDK.Types.MethodType.Execute);
            mReleaseWorklistItem.InputProperties.Add(Constants.SOProperties.ManagementWorklist.WorklistItemId); 
            mReleaseWorklistItem.Validation.RequiredProperties.Add(Constants.SOProperties.ManagementWorklist.WorklistItemId);
            so.Methods.Add(mReleaseWorklistItem);

            return new List<ServiceObject> { so };
        }



        private void GetWorklist()
        {
            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = this.ServiceBroker.K2Connection.GetConnection<WorkflowManagementServer>();

            using (mngServer.Connection)
            {
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
            r[Constants.SOProperties.ManagementWorklist.ActivityId] = wlItem.ActID;
            r[Constants.SOProperties.ManagementWorklist.ActivityInstanceDestinationId] = wlItem.ActInstDestID;
            r[Constants.SOProperties.ManagementWorklist.ActivityInstanceId] = wlItem.ActInstID;
            r[Constants.SOProperties.ManagementWorklist.ActivityName] = wlItem.ActivityName;
            r[Constants.SOProperties.ManagementWorklist.Destination] = wlItem.Destination;
            //TODO: r[CoProperties.ManagementWorklistProperty.DestinationType] = wlItem.;
            r[Constants.SOProperties.ManagementWorklist.EventId] = wlItem.EventID;
            r[Constants.SOProperties.ManagementWorklist.EventName] = wlItem.EventName;
            r[Constants.SOProperties.ManagementWorklist.Folio] = wlItem.Folio;
            r[Constants.SOProperties.ManagementWorklist.ProcessInstanceId] = wlItem.ProcInstID;
            r[Constants.SOProperties.ManagementWorklist.ProcessStatus] = wlItem.ProcessInstanceStatus.ToString();
            r[Constants.SOProperties.ManagementWorklist.ProcessName] = wlItem.ProcName;
            r[Constants.SOProperties.ManagementWorklist.StartDate] = wlItem.StartDate;
            r[Constants.SOProperties.ManagementWorklist.WorklistItemStatus] = wlItem.Status.ToString();
            r[Constants.SOProperties.ManagementWorklist.WorklistItemId] = wlItem.ID;
            return r;
        }




        private void ReleaseWorklistItem()
        {
            int worklistItemId = base.GetIntProperty(Constants.SOProperties.ManagementWorklist.WorklistItemId, true);

            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = this.ServiceBroker.K2Connection.GetConnection<WorkflowManagementServer>();

            using (mngServer.Connection)
            {
                mngServer.ReleaseWorklistItem(worklistItemId);
            }
        }

        private void RedirectWorklistItem()
        {
            string fromUser = base.GetStringProperty(Constants.SOProperties.ManagementWorklist.FromUser, false);
            string toUser = base.GetStringProperty(Constants.SOProperties.ManagementWorklist.ToUser, true);
            int procInstId = base.GetIntProperty(Constants.SOProperties.ManagementWorklist.ProcessInstanceId, true);
            int actInstId = base.GetIntProperty(Constants.SOProperties.ManagementWorklist.ActivityInstanceDestinationId, true);
            int worklistItemId = base.GetIntProperty(Constants.SOProperties.ManagementWorklist.WorklistItemId, true);


            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = this.ServiceBroker.K2Connection.GetConnection<WorkflowManagementServer>();

            using (mngServer.Connection)
            {
                mngServer.RedirectWorklistItem(fromUser, toUser, procInstId, actInstId, worklistItemId);
            }
        }
    }
}
