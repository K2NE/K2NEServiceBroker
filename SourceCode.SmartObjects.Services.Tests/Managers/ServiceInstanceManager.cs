using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SourceCode.SmartObjects.Authoring;
using SourceCode.SmartObjects.Management;
using SourceCode.SmartObjects.Services.Management;
using SourceCode.SmartObjects.Services.Tests.Extensions;
using SourceCode.SmartObjects.Services.Tests.Helpers;

namespace SourceCode.SmartObjects.Services.Tests.Managers
{
    public class ServiceInstanceManager
    {
        private readonly ServiceInstanceSettings _serviceInstanceSettings;

        private readonly ServiceTypeManager _serviceTypeCreator;

        public ServiceInstanceManager(ServiceTypeManager serviceTypeCreator, ServiceInstanceSettings serviceInstanceSettings)
        {
            _serviceTypeCreator = serviceTypeCreator;
            _serviceInstanceSettings = serviceInstanceSettings;
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

            var serviceManagementServer = ConnectionHelper.GetServer<ServiceManagementServer>();
            using (smartObjectManagementServer.Connection)
            {
                if (!string.IsNullOrEmpty(serviceManagementServer.GetServiceInstanceCompact(_serviceInstanceSettings.Guid)))
                {
                    serviceManagementServer.DeleteServiceInstance(_serviceInstanceSettings.Guid, false);
                }
            }
        }

        public void Register(bool refreshOnly = false)
        {
            var server = ConnectionHelper.GetServer<ServiceManagementServer>();
            using (server.Connection)
            {
                var serviceConfig = this.GetServiceConfigInfo(server);

                var serviceInstancesCompactXml = server.GetServiceInstancesCompact(_serviceTypeCreator.Guid);
                var serviceInstances = ServiceInstanceInfoList.Create(serviceInstancesCompactXml);

                var smartObjectManagementServer = ConnectionHelper.GetServer<SmartObjectManagementServer>();
                using (smartObjectManagementServer.Connection)
                {
                    // Remove ServiceInstance with the same name
                    foreach (var instanceInfo in serviceInstances)
                    {
                        if (instanceInfo.Name == _serviceInstanceSettings.Name &&
                            instanceInfo.Guid != _serviceInstanceSettings.Guid)
                        {
                            smartObjectManagementServer.DeleteSmartObjects(instanceInfo.Guid);
                            server.DeleteServiceInstance(instanceInfo.Guid);
                        }
                    }
                }

                // Refresh if exists
                var existingServiceInstance = serviceInstances.FirstOrDefault(i => i.Guid == _serviceInstanceSettings.Guid);

                if (existingServiceInstance != null)
                {
                    if (refreshOnly)
                    {
                        server.RefreshServiceInstance(_serviceInstanceSettings.Guid);
                    }
                    else
                    {
                        server.RefreshServiceInstance(
                          _serviceInstanceSettings.Guid,
                          serviceConfig.WriteXml());
                    }
                }
                else
                {
                    server.RegisterServiceInstance(
                          _serviceTypeCreator.Guid,
                          _serviceInstanceSettings.Guid,
                          _serviceInstanceSettings.Name,
                          _serviceInstanceSettings.DisplayName,
                          _serviceInstanceSettings.Description ?? _serviceInstanceSettings.DisplayName,
                          serviceConfig.WriteXml());
                }

                // Output all the ServiceObjects and methods
                var serviceInstance = ServiceInstance.Create(server.GetServiceInstance(_serviceInstanceSettings.Guid));
                serviceInstance.ServiceObjects.Sort();

                foreach (ServiceObject serviceObject in serviceInstance.ServiceObjects)
                {
                    Debug.WriteLine(serviceObject.Name);
                    foreach (ServiceObjectMethod method in serviceObject.Methods)
                    {
                        Debug.WriteLine("\t" + method.Name);
                    }
                }
            }
        }

        public void Update(IDictionary<string, string> configurationSettings)
        {
            var server = ConnectionHelper.GetServer<ServiceManagementServer>();
            using (server.Connection)
            {
                var serviceConfig = this.GetServiceConfigInfo(server);

                var serviceInstancesCompactXml = server.GetServiceInstancesCompact(_serviceTypeCreator.Guid);
                var serviceInstances = ServiceInstanceInfoList.Create(serviceInstancesCompactXml);

                var smartObjectManagementServer = ConnectionHelper.GetServer<SmartObjectManagementServer>();

                foreach (var configSetting in configurationSettings)
                {
                    serviceConfig.ConfigSettings[configSetting.Key].Value = configSetting.Value;
                }

                server.RefreshServiceInstance(
                  _serviceInstanceSettings.Guid,
                  serviceConfig.WriteXml());
            }
        }

        private ServiceConfigInfo GetServiceConfigInfo(ServiceManagementServer server)
        {
            // ServiceType's Service Config
            var serviceInstanceConfig = server.GetServiceInstanceConfig(_serviceTypeCreator.Guid);
            var serviceConfig = ServiceConfigInfo.Create(serviceInstanceConfig);

            // Authentication
            serviceConfig.ServiceAuthentication.EnforceImpersonation = _serviceInstanceSettings.ServiceAuthentication.EnforceImpersonation;
            serviceConfig.ServiceAuthentication.Extra = _serviceInstanceSettings.ServiceAuthentication.Extra;
            serviceConfig.ServiceAuthentication.Impersonate = _serviceInstanceSettings.ServiceAuthentication.Impersonate;
            serviceConfig.ServiceAuthentication.OAuthToken = _serviceInstanceSettings.ServiceAuthentication.OAuthToken;
            serviceConfig.ServiceAuthentication.Password = _serviceInstanceSettings.ServiceAuthentication.Password;
            serviceConfig.ServiceAuthentication.SecurityProvider = _serviceInstanceSettings.ServiceAuthentication.SecurityProvider;
            serviceConfig.ServiceAuthentication.UseOAuth = _serviceInstanceSettings.ServiceAuthentication.UseOAuth;
            serviceConfig.ServiceAuthentication.UserName = _serviceInstanceSettings.ServiceAuthentication.UserName;

            // Config Settings
            foreach (var configSetting in serviceConfig.ConfigSettings)
            {
                if (_serviceInstanceSettings.ConfigurationSettings.ContainsKey(configSetting.Name))
                {
                    configSetting.Value = _serviceInstanceSettings.ConfigurationSettings[configSetting.Name];
                }
            }

            return serviceConfig;
        }
    }
}