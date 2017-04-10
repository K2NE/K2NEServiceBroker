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
    public class AD_UserTests
    {
        [TestMethod]
        [TestCategory("AD_User")]
        public void Execute_AD_User_GetUsers()
        {
            DataTable dataTable;
            var clientServer = ConnectionHelper.GetServer<SmartObjectClientServer>();
            using (clientServer.Connection)
            {
                var smartObject = SmartObjectHelper.GetSmartObject(clientServer, "AD_User", K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance);
                smartObject.MethodToExecute = "GetUsers";
                dataTable = SmartObjectHelper.ExecuteListDataTable(clientServer, smartObject);
            }
            Assert.IsTrue(dataTable.Rows.Count > 0);

            bool hasDisplayName = false;
            bool hasEmail = false;
            foreach (DataRow dataRow in dataTable.Rows)
            {
                dataRow.AssertHasValue<String>("User FQN");
                dataRow.AssertHasValue<String>("sAM Acount Name");

                hasDisplayName = !string.IsNullOrEmpty(dataRow.Field<string>("Display Name")) || hasDisplayName;
                hasEmail = !string.IsNullOrEmpty(dataRow.Field<string>("Email")) || hasEmail;
            }

            Assert.IsTrue(hasDisplayName, "None of the rows returned a Display Name");
            Assert.IsTrue(hasEmail, "None of the rows returned an Email");
        }
    }
}