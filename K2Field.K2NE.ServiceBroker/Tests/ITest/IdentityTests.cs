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
    public class IdentityTests
    {
        [TestMethod]
        [TestCategory("Identity")]
        public void Execute_Identity_GetEmailFromIdentity()
        {
            DataTable dataTable;
            var clientServer = ConnectionHelper.GetServer<SmartObjectClientServer>();
            using (clientServer.Connection)
            {
                var smartObject = SmartObjectHelper.GetSmartObject(clientServer, "Identity", K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance);
                smartObject.MethodToExecute = "GetEmailFromIdentity";
                smartObject.SetInputPropertyValue("FQN", UserRoleManagerHelper.GetCurrentUserFQN());
                dataTable = SmartObjectHelper.ExecuteListDataTable(clientServer, smartObject);
            }

            Assert.IsTrue(dataTable.Rows.Count > 0);

            foreach (DataRow row in dataTable.Rows)
            {
                row.AssertHasValue<String>("FQN");
                row.AssertHasValue<String>("User Email");
                row.AssertHasValue<String>("User Display Name");
            }
        }
    }
}