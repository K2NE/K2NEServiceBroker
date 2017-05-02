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
    public class ErrorLogTests
    {
        [TestMethod]
        [TestCategory("ErrorLog")]
        public void Execute_ErrorLog_GetErrors()
        {
            DataTable dataTable;
            var clientServer = ConnectionHelper.GetServer<SmartObjectClientServer>();
            using (clientServer.Connection)
            {
                var smartObject = SmartObjectHelper.GetSmartObject(clientServer, "ErrorLog", K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance);
                smartObject.MethodToExecute = "GetErrors";
                dataTable = SmartObjectHelper.ExecuteListDataTable(clientServer, smartObject);
            }
            Assert.IsTrue(dataTable.Rows.Count > 0);
            foreach (DataRow row in dataTable.Rows)
            {
                row.AssertHasValue<Int64>("Process Instance Id");
                row.AssertHasValue<String>("Process Definition Name");
                row.AssertHasValue<String>("Folio");
                row.AssertHasValue<String>("Error Description");
                row.AssertHasValue<String>("Error Item");
                row.AssertHasValue<DateTime>("Error Date");
                row.AssertHasValue<Int64>("Error Id");
                row.AssertHasValue<String>("Type Description");
                row.AssertHasValue<String>("Stack Trace");
                row.AssertHasValue<Int64>("Executing Proc Id");
            }
        }
    }
}