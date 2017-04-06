using K2Field.K2NE.ServiceBroker.ITest.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SourceCode.SmartObjects.Services.Tests.Managers;

namespace K2Field.K2NE.ServiceBroker.ITest
{
    [TestClass]
    public class ServiceInstanceTests
    {
#if DEBUG

        [TestMethod()]
        [TestCategory("Initialize Service Instance")]
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
        }
    }
}