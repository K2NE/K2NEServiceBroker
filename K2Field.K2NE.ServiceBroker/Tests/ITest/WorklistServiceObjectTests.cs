using System;
using System.Data;
using K2Field.K2NE.ServiceBroker.ITest.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SourceCode.SmartObjects.Client;
using SourceCode.SmartObjects.Services.Tests.Extensions;
using SourceCode.SmartObjects.Services.Tests.Helpers;

namespace K2Field.K2NE.ServiceBroker.ITest
{
    [TestClass]
    public class WorklistServiceObjectTests
    {
        [TestMethod]
        [TestCategory(Constants.TestAttribute.Worklist)]
        public void Execute_Worklist_GetWorklist()
        {
            DataTable dataTable;

            var clientServer = ConnectionHelper.GetServer<SmartObjectClientServer>();
            using (clientServer.Connection)
            {
                var smartObject = SmartObjectHelper.GetSmartObject(clientServer, "Worklist", K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance);
                smartObject.MethodToExecute = "GetWorklist";

                dataTable = SmartObjectHelper.ExecuteListDataTable(clientServer, smartObject);
            }

            Assert.IsTrue(dataTable.Rows.Count > 0);

            //var generatedCode = dataTable.GenerateGetAssertHasValue();

            foreach (DataRow row in dataTable.Rows)
            {
                row.AssertHasValue<String>("Process Name");
                row.AssertHasValue<String>("Folder");
                row.AssertHasValue<String>("Process Fullname");
                row.AssertHasValue<String>("Process Status");
                row.AssertHasValue<Int64>("Process Priority");
                row.AssertHasValue<DateTime>("Process Start Date");
                row.AssertHasValue<Int64>("Process Expected Duration");
                row.AssertHasValue<Int64>("Process Id");
                row.AssertHasValue<String>("Activity Name");
                row.AssertHasValue<String>("Activity Priority");
                row.AssertHasValue<DateTime>("Activity Start Date");
                row.AssertHasValue<Int64>("Activity Expected Duration");
                row.AssertHasValue<String>("Event Name");
                row.AssertHasValue<String>("Event Priority");
                row.AssertHasValue<DateTime>("Event Start Date");
                row.AssertHasValue<String>("Event Expected Duration");
                row.AssertHasValue<String>("Status");
                row.AssertHasValue<String>("Folio");
                row.AssertHasValue<String>("Serial Number");
                row.AssertHasValue<Guid>("Process Guid");
                row.AssertHasValue<Int64>("Activity Id");
                row.AssertHasValue<String>("Event Id");
                row.AssertHasValue<String>("Original Destination");

                // Empty values
                //row.AssertHasValue<String>("Process Description");
                //row.AssertHasValue<String>("Pocess Metadata");
                //row.AssertHasValue<String>("Activity Description");
                //row.AssertHasValue<String>("Activity Metadata");
                //row.AssertHasValue<String>("Event Metadata");
                //row.AssertHasValue<String>("Event Description");
                //row.AssertHasValue<String>("Data");
            }
        }
    }
}