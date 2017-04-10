using System;
using System.Collections.Generic;
using SourceCode.SmartObjects.Services.Management;
using SourceCode.SmartObjects.Services.Tests.Managers;

namespace K2Field.K2NE.ServiceBroker.ITest.Settings
{
    public class K2NEServiceBrokerIntegrationTestsServiceInstanceSettings : ServiceInstanceSettings
    {
        private static readonly Lazy<K2NEServiceBrokerIntegrationTestsServiceInstanceSettings> lazy =
            new Lazy<K2NEServiceBrokerIntegrationTestsServiceInstanceSettings>(() => new K2NEServiceBrokerIntegrationTestsServiceInstanceSettings());

        private Dictionary<string, string> _configurationSettings;

        private K2NEServiceBrokerIntegrationTestsServiceInstanceSettings()
        {
        }

        public static K2NEServiceBrokerIntegrationTestsServiceInstanceSettings Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public override IDictionary<string, string> ConfigurationSettings
        {
            get
            {
                if (_configurationSettings == null)
                {
                    _configurationSettings = new Dictionary<string, string>();
                    _configurationSettings.Add("Workflow Management Port", "5555");
                    _configurationSettings.Add("AD Netbios Name", "Denallix");
                    _configurationSettings.Add("Workflow Client Port", "5252");
                    _configurationSettings.Add("Environment to use (empty for default)", "");
                    _configurationSettings.Add("AD LDAP Path", "LDAP://DC=denallix,DC=COM");
                    _configurationSettings.Add("AD Maximum Resultsize", "1000");
                    _configurationSettings.Add("Change Contains operator to StartsWith for AD", "true");
                    _configurationSettings.Add("Default Culture", "EN-us");
                    _configurationSettings.Add("SMO data queries file path", "");
                    _configurationSettings.Add("PowerShell Subdirectories", "PowerShellScripts");
                    _configurationSettings.Add("Additional AD properties delimited by ;", "");
                    _configurationSettings.Add("SMO data queries delimited by ;", "");
                    _configurationSettings.Add("Allow Simple PowerShell Scripts", "true");
                    _configurationSettings.Add("Platform to use", "ASP");
                }
                return _configurationSettings;
            }
        }

        public override string DisplayName
        {
            get
            {
                return "K2NEServiceBroker Integration Tests";
            }
        }

        public override string Description
        {
            get
            {
                return "A Service Broker that provides various functional service objects that aid the implementation of a K2 project.";
            }
        }

        public override Guid Guid
        {
            get
            {
                return new Guid("281821dd-4795-48ca-833e-f132264f8408");
            }
        }

        public override string Name
        {
            get
            {
                return "K2NEServiceBrokerITest";
            }
        }

        public override ServiceAuthenticationInfo ServiceAuthentication
        {
            get
            {
                return new ServiceAuthenticationInfo
                {
                    EnforceImpersonation = false,
                    Extra = "",
                    Impersonate = true,
                    OAuthResourceAudience = "",
                    OAuthResourceId = "",
                    OAuthToken = "",
                    Password = "",
                    SecurityProvider = "",
                    UseOAuth = false,
                    UserName = "",
                };
            }
        }
    }
}