using System;
using System.Data;
using K2Field.K2NE.ServiceBroker.ITest.Helpers;
using K2Field.K2NE.ServiceBroker.ITest.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SourceCode.SmartObjects.Client;
using SourceCode.SmartObjects.Services.Tests.Extensions;
using SourceCode.SmartObjects.Services.Tests.Helpers;

namespace K2Field.K2NE.ServiceBroker.ITest
{
    [TestClass]
    public class ProcessInstanceManagementTests
    {
        [TestMethod]
        [TestCategory("Identity")]
        public void Execute_ProcessInstanceManagement_ListActivities()
        {
            DataTable dataTable;
            var clientServer = ConnectionHelper.GetServer<SmartObjectClientServer>();
            using (clientServer.Connection)
            {
                var smartObject = SmartObjectHelper.GetSmartObject(clientServer, "ProcessInstanceManagement", K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance);
                smartObject.MethodToExecute = "ListActivities";
                smartObject.SetInputPropertyValue("ProcessInstanceId", WorkflowManagementHelper.GetProcessInstance(Constants.Process.TestProcess1).ID);
                dataTable = SmartObjectHelper.ExecuteListDataTable(clientServer, smartObject);
            }

            Assert.IsTrue(dataTable.Rows.Count > 0);

            foreach (DataRow row in dataTable.Rows)
            {
                row.AssertHasValue<Int64>("Activity ID");
                row.AssertHasValue<String>("Activity Name");
                row.AssertHasValue<Int64>("Activity Expected Duration");
                row.AssertHasValue<Int64>("Activity Priority");
                row.AssertHasValue<Boolean>("Is Start Activity");

                // Empty values
                //row.AssertHasValue<String>("Activity Description");
                //row.AssertHasValue<String>("Activity Meta Data");
            }
        }
    }
}