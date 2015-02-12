using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using System.Data;
using SourceCode.Workflow.Management;
using SourceCode.Workflow.Management.Criteria;

namespace K2Field.K2NE.ServiceObjects
{
    public class ErrorLogSO : ServiceObjectBase
    {

        public ErrorLogSO(K2NEServiceBroker api) : base(api) { }

        public override List<ServiceObject> DescribeServiceObjects()
{
            ServiceObject so = Helper.CreateServiceObject("ErrorLog", "Service Object that exposes the ErrorLog of the K2 server.");
 	        
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ErrorLog.Profile, SoType.Text, "The error profile to return, default 'All'"));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ErrorLog.ProcessInstanceId, SoType.Number, "The errored process id."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ErrorLog.ProcessName, SoType.Text, "The errored process name."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ErrorLog.Folio, SoType.Text, "The folio of the errored process."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ErrorLog.ErrorDescription, SoType.Text, "The description/exception of the error."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ErrorLog.ErrorItem, SoType.Text, "The item that has errored (event)."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ErrorLog.ErrorId, SoType.Number, "The Identified for the error log"));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ErrorLog.ErrorDate, SoType.DateTime, "Date when the error occured"));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.ErrorLog.TryNewVersion, SoType.YesNo, "retry the error in new version."));


            Method getErrors = Helper.CreateMethod(Constants.Methods.ErrorLog.GetErrors, "Retrieve the K2 error log", MethodType.List);
            getErrors.InputProperties.Add(so.Properties[Constants.Properties.ErrorLog.Profile]);
            getErrors.ReturnProperties.Add(so.Properties[Constants.Properties.ErrorLog.ProcessInstanceId]);
            getErrors.ReturnProperties.Add(so.Properties[Constants.Properties.ErrorLog.ProcessName]);
            getErrors.ReturnProperties.Add(so.Properties[Constants.Properties.ErrorLog.Folio]);
            getErrors.ReturnProperties.Add(so.Properties[Constants.Properties.ErrorLog.ErrorDescription]);
            getErrors.ReturnProperties.Add(so.Properties[Constants.Properties.ErrorLog.ErrorItem]);
            getErrors.ReturnProperties.Add(so.Properties[Constants.Properties.ErrorLog.ErrorDate]);
            getErrors.ReturnProperties.Add(so.Properties[Constants.Properties.ErrorLog.ErrorId]);
            so.Methods.Add(getErrors);

            Method retryProcess = Helper.CreateMethod(Constants.Methods.ErrorLog.RetryProcess, "Retry a process instance", MethodType.Execute);
            retryProcess.InputProperties.Add(so.Properties[Constants.Properties.ErrorLog.ProcessInstanceId]);
            retryProcess.InputProperties.Add(so.Properties[Constants.Properties.ErrorLog.TryNewVersion]);
            so.Methods.Add(retryProcess);

            return new List<ServiceObject> { so };
        }

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
                case Constants.Methods.ErrorLog.GetErrors:
                    GetErrors();
                    break;
                case Constants.Methods.ErrorLog.RetryProcess:
                    RetryProcess();
                    break;
            }

        }

        private void RetryProcess()
        {
            bool newVersion = base.GetBoolProperty(Constants.Properties.ErrorLog.TryNewVersion);
            int procInstId = base.GetIntProperty(Constants.Properties.ErrorLog.ProcessInstanceId, true);

            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = new WorkflowManagementServer();
            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);

                ErrorProfile all = mngServer.GetErrorProfiles()[0];
                ErrorLogCriteriaFilter errorfilter = new ErrorLogCriteriaFilter();
                errorfilter.AddRegularFilter(ErrorLogFields.ProcInstID, Comparison.Equals, procInstId);
                ErrorLogs errors = mngServer.GetErrorLogs(all.ID, errorfilter);

                if (errors.Count != 1)
                {
                    throw new ApplicationException(string.Format("Could not retrieve process (with id: {0}). Got {1} results.", procInstId, errors.Count));
                }

                int errorId = errors[0].ID;

                if (newVersion)
                {
                    int newVersionNumber = 0;
                    ProcessInstanceCriteriaFilter procFilter = new ProcessInstanceCriteriaFilter();
                    procFilter.AddRegularFilter(ProcessInstanceFields.ProcInstID, Comparison.Equals, procInstId);
                    ProcessInstances procs = mngServer.GetProcessInstancesAll(procFilter);
                    Processes procesVersions = mngServer.GetProcessVersions(procs[0].ProcSetID);
                    foreach (Process proc in procesVersions)
                    {
                        if (proc.VersionNumber > newVersionNumber)
                            newVersionNumber = proc.VersionNumber;
                    }
                    mngServer.SetProcessInstanceVersion(procInstId, newVersionNumber);
                }
                mngServer.RetryError(procInstId, errorId, string.Format("Process Retry using {0}", base.ServiceBroker.Service.ServiceObjects[0].Name));
            }
        }

        private void GetErrors()
        {
            string profile = base.GetStringProperty(Constants.Properties.ErrorLog.Profile);
            if (string.IsNullOrEmpty(profile))
            {
                profile = "All";
            }

            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = new WorkflowManagementServer();
            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);

                //TODO: catch exception on this?
                ErrorProfile prof = mngServer.GetErrorProfile(profile);
                ErrorLogs errors = mngServer.GetErrorLogs(prof.ID);

                foreach (ErrorLog e in errors)
                {
                    DataRow r = results.NewRow();
                    r[Constants.Properties.ErrorLog.ProcessInstanceId] = e.ProcInstID;
                    r[Constants.Properties.ErrorLog.ProcessName] = e.ProcessName;
                    r[Constants.Properties.ErrorLog.Folio] = e.Folio;
                    r[Constants.Properties.ErrorLog.ErrorDescription] = e.Description;
                    r[Constants.Properties.ErrorLog.ErrorItem] = e.ErrorItemName;
                    r[Constants.Properties.ErrorLog.ErrorDate] = e.ErrorDate;
                    r[Constants.Properties.ErrorLog.ErrorId] = e.ID;
                    results.Rows.Add(r);
                }
            }


        }
    }
}
