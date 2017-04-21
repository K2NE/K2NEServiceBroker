using System;
using System.Collections.Generic;
using SourceCode.SmartObjects.Services.Management;

namespace SourceCode.SmartObjects.Services.Tests.Managers
{
    public abstract class ServiceInstanceSettings
    {
        public abstract IDictionary<string, string> ConfigurationSettings
        {
            get;
        }

        public abstract string Description
        {
            get;
        }

        public abstract string DisplayName
        {
            get;
        }

        public abstract Guid Guid
        {
            get;
        }

        public abstract string Name
        {
            get;
        }

        public abstract ServiceAuthenticationInfo ServiceAuthentication
        {
            get;
        }
    }
}