using System;
using SourceCode.SmartObjects.Services.Tests.Helpers;

namespace K2Field.K2NE.ServiceBroker.ITest.Helpers
{
    internal static class WorkflowManagementHelper
    {
        public static void DeleteProcessDefinitions(string processSetFolder)
        {
            var server = ConnectionHelper.GetServer<SourceCode.Workflow.Management.WorkflowManagementServer>();
            using (server.Connection)
            {
                foreach (SourceCode.Workflow.Management.ProcessSet processSet in server.GetProcSets())
                {
                    if (processSetFolder?.Equals(processSet.Folder, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        server.DeleteProcessDefinition(processSet.FullName, 0, true);
                    }
                }
            }
        }
    }
}