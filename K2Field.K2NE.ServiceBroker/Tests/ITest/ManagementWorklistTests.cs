using System;
using System.Data;
using System.Threading;
using K2Field.K2NE.ServiceBroker.ITest.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SourceCode.SmartObjects.Client;
using SourceCode.SmartObjects.Services.Tests.Extensions;
using SourceCode.SmartObjects.Services.Tests.Helpers;

namespace K2Field.K2NE.ServiceBroker.ITest
{
    [TestClass]
    public class ManagementWorklistTests
    {
        [TestMethod]
        [TestCategory("ManagementWorklist")]
        public void Execute_ManagementWorklist_GetWorklist()
        {
            // Wait for Worklist to be assigned
            Thread.Sleep(2000);

            DataTable dataTable;
            var clientServer = ConnectionHelper.GetServer<SmartObjectClientServer>();
            using (clientServer.Connection)
            {
                var smartObject = SmartObjectHelper.GetSmartObject(clientServer, "ManagementWorklist", K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance);
                smartObject.MethodToExecute = "GetWorklist";
                dataTable = SmartObjectHelper.ExecuteListDataTable(clientServer, smartObject);
            }

            Assert.IsTrue(dataTable.Rows.Count > 0);
            foreach (DataRow row in dataTable.Rows)
            {
                row.AssertHasValue<Int64>("Activity Id");
                row.AssertHasValue<Int64>("Activity Instance Destination Id");
                row.AssertHasValue<Int64>("Activity Instance Id");
                row.AssertHasValue<String>("Activity Name");
                row.AssertHasValue<String>("Destination");
                row.AssertHasValue<Int64>("Event Id");
                row.AssertHasValue<String>("Event Name");
                row.AssertHasValue<String>("Folio");
                row.AssertHasValue<Int64>("Process Instance Id");
                row.AssertHasValue<String>("Process Name");
                row.AssertHasValue<String>("Process Status");
                row.AssertHasValue<DateTime>("Start Date");
                row.AssertHasValue<String>("Status");
                row.AssertHasValue<Int64>("Id");

                // Empty values
                //row.AssertHasValue<String>("Destination Type");
            }
        }
    }
}