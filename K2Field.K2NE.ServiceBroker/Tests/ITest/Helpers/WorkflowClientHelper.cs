using SourceCode.SmartObjects.Services.Tests.Helpers;

namespace K2Field.K2NE.ServiceBroker.ITest.Helpers
{
    internal static class WorkflowClientHelper
    {
        public static SourceCode.Workflow.Client.ProcessInstance CreateProcessInstance(string processFullName, int version)
        {
            var connection = new SourceCode.Workflow.Client.Connection();
            connection.Open(
                ConnectionHelper.WorkflowConnectionStringBuilder.Host,
                ConnectionHelper.WorkflowConnectionStringBuilder.ConnectionString);

            using (connection)
            {
                var processInstance = connection.CreateProcessInstance(processFullName, version);
                processInstance.Folio = processFullName;
                connection.StartProcessInstance(processInstance);
                return processInstance;
            }
        }
    }
}