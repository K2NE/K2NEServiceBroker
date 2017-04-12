using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SourceCode.SmartObjects.Services.Tests.Helpers;
using SourceCode.Workflow.Management;

namespace K2Field.K2NE.ServiceBroker.ITest.Helpers
{
    internal static class WorkflowManagementHelper
    {
        public static void DeleteProcessDefinitions(string processSetFolder)
        {
            var server = ConnectionHelper.GetServer<WorkflowManagementServer>();
            using (server.Connection)
            {
                foreach (SourceCode.Workflow.Management.ProcessSet processSet in server.GetProcSets())
                {
                    if (processSetFolder.Equals(processSet.Folder, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        server.DeleteProcessDefinition(processSet.FullName, 0, true);
                    }
                }
            }
        }

        public static int GetProcessSetIDWithWorkflowManagementServer(string ProcessFullName)
        {
            var workflowManagent = ConnectionHelper.GetServer<WorkflowManagementServer>();
            using (workflowManagent.Connection)
            {
                var processSet = workflowManagent.GetProcSet(ProcessFullName);
                Assert.IsNotNull(processSet, string.Format("Process does not exist. {0}", ProcessFullName));

                return processSet.ProcSetID;
            }
        }

        public static ProcessSet GetProcessSet(string processFullName)
        {
            var workflowManagent = ConnectionHelper.GetServer<WorkflowManagementServer>();
            using (workflowManagent.Connection)
            {
                return workflowManagent.GetProcSet(processFullName);
            }
        }

        public static ProcessInstance GetProcessInstance(string processFullName)
        {
            var workflowManagent = ConnectionHelper.GetServer<WorkflowManagementServer>();
            using (workflowManagent.Connection)
            {
                var processSet = workflowManagent.GetProcSet(processFullName);

                return workflowManagent.GetProcessInstances(processSet.ProcID)[0];
            }
        }
    }
}