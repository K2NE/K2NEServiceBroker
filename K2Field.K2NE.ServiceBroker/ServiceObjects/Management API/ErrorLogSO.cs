using System;
using System.Collections.Generic;
using K2Field.K2NE.ServiceBroker.Helpers;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using System.Data;
using SourceCode.Workflow.Management;
using SourceCode.Workflow.Management.Criteria;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects
{
    public class ErrorLogSO : ServiceObjectBase
    {

        public ErrorLogSO(K2NEServiceBroker api) : base(api) { }

        public override List<ServiceObject> DescribeServiceObjects()
{
            ServiceObject so = Helper.CreateServiceObject("ErrorLog", "Service Object that exposes the ErrorLog of the K2 server.");
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ErrorLog.Profile, SoType.Text, "The error profile to return, default 'All'"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ErrorLog.ProcessInstanceId, SoType.Number, "The errored process id."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ErrorLog.ProcessName, SoType.Text, "The errored process name."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ErrorLog.Folio, SoType.Text, "The folio of the errored process."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ErrorLog.ErrorDescription, SoType.Text, "The description/exception of the error."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ErrorLog.ErrorItem, SoType.Text, "The item that has errored (event)."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ErrorLog.ErrorId, SoType.Number, "The Identified for the error log"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ErrorLog.ErrorDate, SoType.DateTime, "Date when the error occured"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ErrorLog.TryNewVersion, SoType.YesNo, "retry the error in new version."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ErrorLog.TypeDescription, SoType.Text, "A short description/name of the type of K2 workflow element that is causing the error."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ErrorLog.StackTrace, SoType.Memo, "The stacktrace of the error."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ErrorLog.ExecutingProcId, SoType.Number, "The executing process ID."));



            Method getErrors = Helper.CreateMethod(Constants.Methods.ErrorLog.GetErrors, "Retrieve the K2 error log", MethodType.List);
            getErrors.InputProperties.Add(Constants.SOProperties.ErrorLog.Profile);
            getErrors.ReturnProperties.Add(Constants.SOProperties.ErrorLog.ProcessInstanceId);
            getErrors.ReturnProperties.Add(Constants.SOProperties.ErrorLog.ProcessName);
            getErrors.ReturnProperties.Add(Constants.SOProperties.ErrorLog.Folio);
            getErrors.ReturnProperties.Add(Constants.SOProperties.ErrorLog.ErrorDescription);
            getErrors.ReturnProperties.Add(Constants.SOProperties.ErrorLog.ErrorItem);
            getErrors.ReturnProperties.Add(Constants.SOProperties.ErrorLog.ErrorDate);
            getErrors.ReturnProperties.Add(Constants.SOProperties.ErrorLog.ErrorId);
            getErrors.ReturnProperties.Add(Constants.SOProperties.ErrorLog.TypeDescription);
            getErrors.ReturnProperties.Add(Constants.SOProperties.ErrorLog.StackTrace);
            getErrors.ReturnProperties.Add(Constants.SOProperties.ErrorLog.ExecutingProcId);

            so.Methods.Add(getErrors);

            Method retryProcess = Helper.CreateMethod(Constants.Methods.ErrorLog.RetryProcess, "Retry a process instance", MethodType.Execute);
            retryProcess.InputProperties.Add(Constants.SOProperties.ErrorLog.ProcessInstanceId);
            retryProcess.Validation.RequiredProperties.Add(Constants.SOProperties.ErrorLog.ProcessInstanceId);
            retryProcess.InputProperties.Add(Constants.SOProperties.ErrorLog.TryNewVersion);
            so.Methods.Add(retryProcess);

            return new List<ServiceObject> { so };
        }

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
            bool newVersion = base.GetBoolProperty(Constants.SOProperties.ErrorLog.TryNewVersion);
            int procInstId = base.GetIntProperty(Constants.SOProperties.ErrorLog.ProcessInstanceId, true);

            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = this.ServiceBroker.K2Connection.GetConnection<WorkflowManagementServer>();

            using (mngServer.Connection)
            {

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
            string profile = base.GetStringProperty(Constants.SOProperties.ErrorLog.Profile);
            if (string.IsNullOrEmpty(profile))
            {
                profile = "All";
            }

            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = this.ServiceBroker.K2Connection.GetConnection<WorkflowManagementServer>();

            using (mngServer.Connection)
            {
                ErrorProfile prof = mngServer.GetErrorProfile(profile);
                if (prof == null)
                    throw new Exception(string.Format("Profile with name \"{0}\" was not found.", profile));

                ErrorLogs errors = mngServer.GetErrorLogs(prof.ID);

                foreach (ErrorLog e in errors)
                {
                    DataRow r = results.NewRow();
                    r[Constants.SOProperties.ErrorLog.ProcessInstanceId] = e.ProcInstID;
                    r[Constants.SOProperties.ErrorLog.ProcessName] = e.ProcessName;
                    r[Constants.SOProperties.ErrorLog.Folio] = e.Folio;
                    r[Constants.SOProperties.ErrorLog.ErrorDescription] = e.Description;
                    r[Constants.SOProperties.ErrorLog.ErrorItem] = e.ErrorItemName;
                    r[Constants.SOProperties.ErrorLog.ErrorDate] = e.ErrorDate;
                    r[Constants.SOProperties.ErrorLog.ErrorId] = e.ID;
                    r[Constants.SOProperties.ErrorLog.TypeDescription] = e.TypeDescription;
                    r[Constants.SOProperties.ErrorLog.ExecutingProcId] = e.ExecutingProcID;
                    r[Constants.SOProperties.ErrorLog.StackTrace] = e.StackTrace;
                    results.Rows.Add(r);
                }
            }


        }
    }
}
