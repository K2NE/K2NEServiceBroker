using K2Field.K2NE.ServiceBroker.ITest.Helpers;
using K2Field.K2NE.ServiceBroker.ITest.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SourceCode.SmartObjects.Services.Tests.Helpers;
using SourceCode.SmartObjects.Services.Tests.Managers;

namespace K2Field.K2NE.ServiceBroker.ITest
{
    [TestClass]
    public class ServiceInstanceTests
    {
#if DEBUG

        [TestMethod()]
        [Priority(1)]
        public void Create_K2NEServiceBrokerIntegrationTests_SmartObjects()
#else

        [AssemblyInitialize()]
        public static void K2NEServiceBroker_AssemblyInitialize(TestContext context)
#endif
        {
            // ServiceType
            var serviceTypeManager = new ServiceTypeManager(K2NEServiceBrokerServiceTypeSettings.Instance);
            // ServiceInstance
            var serviceInstanceSettings = K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance;
            var serviceInstanceManager = new ServiceInstanceManager(serviceTypeManager, serviceInstanceSettings);
            // SmartObjects
            var smartObjectsManager = new SmartObjectsManager(serviceInstanceSettings);
            serviceTypeManager.Register();
            serviceInstanceManager.Register();
            smartObjectsManager.Register();

#if DEBUG
        }

        [TestMethod()]
        [Priority(1)]
        public void Deploy_K2NEServiceBroker_Packages()
        {
#endif
            // Delete ProcessDefinitions
            WorkflowManagementHelper.DeleteProcessDefinitions(Constants.ProcessSetFolder.K2NEServiceBroker);
            WorkflowManagementHelper.DeleteProcessDefinitions(Constants.Category.CategoryK2NEServiceBrokerName);

            // Deploy Twice for Multiple Versions
            PackageDeploymentHelper.DeployPackage(Properties.Resources.K2NEServiceBroker);
            PackageDeploymentHelper.DeployPackage(Properties.Resources.K2NEServiceBroker);

            // Start 1 version 1 ProcessInstance
            WorkflowClientHelper.CreateProcessInstance(Constants.Process.TestProcess1, 1);

            // Start 2 version 2 ProcessInstance of TestProcess1
            WorkflowClientHelper.CreateProcessInstance(Constants.Process.TestProcess1, 2);
            WorkflowClientHelper.CreateProcessInstance(Constants.Process.TestProcess1, 2);

            // Start 1 version 1 ProcessInstance of TestProcess2
            WorkflowClientHelper.CreateProcessInstance(Constants.Process.TestProcess2, 1);
        }
    }
}