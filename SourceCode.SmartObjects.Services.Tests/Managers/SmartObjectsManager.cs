using SourceCode.SmartObjects.Management;
using SourceCode.SmartObjects.Services.Tests.Extensions;
using SourceCode.SmartObjects.Services.Tests.Helpers;

namespace SourceCode.SmartObjects.Services.Tests.Managers
{
    public class SmartObjectsManager
    {
        private readonly ServiceInstanceSettings _serviceInstanceSettings;

        public SmartObjectsManager(ServiceInstanceSettings serviceInstanceSettings)
        {
            _serviceInstanceSettings = serviceInstanceSettings;
        }

        public ServiceInstanceSettings ServiceInstanceSettings
        {
            get { return _serviceInstanceSettings; }
        }

        public void Delete()
        {
            var smartObjectManagementServer = ConnectionHelper.GetServer<SmartObjectManagementServer>();
            using (smartObjectManagementServer.Connection)
            {
                foreach (SmartObjectInfo smartObject in smartObjectManagementServer.GetSmartObjects(_serviceInstanceSettings.Guid).SmartObjects)
                {
                    smartObjectManagementServer.DeleteSmartObject(smartObject.Name);
                }
            }
        }

        public void Register()
        {
            var managementServer = ConnectionHelper.GetServer<SmartObjectManagementServer>();
            using (managementServer.Connection)
            {
                managementServer.GenerateSmartObjects(_serviceInstanceSettings.Guid, true, true, true);
            }
        }
    }
}