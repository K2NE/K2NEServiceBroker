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
    public class RoleManagementTests
    {
        [TestMethod]
        [TestCategory("RoleManagement")]
        public void Execute_RoleManagement_ListRoles()
        {
            DataTable dataTable;
            var clientServer = ConnectionHelper.GetServer<SmartObjectClientServer>();
            using (clientServer.Connection)
            {
                var smartObject = SmartObjectHelper.GetSmartObject(clientServer, "RoleManagement", K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance);
                smartObject.MethodToExecute = "ListRoles";
                dataTable = SmartObjectHelper.ExecuteListDataTable(clientServer, smartObject);
            }
            Assert.IsTrue(dataTable.Rows.Count > 0);
            foreach (DataRow row in dataTable.Rows)
            {
                row.AssertHasValue<String>("Role Name");
                row.AssertHasValue<String>("Description");
                row.AssertHasValue<Boolean>("Is Dynamic");
            }
        }
    }
}