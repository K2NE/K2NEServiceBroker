using System;
using SourceCode.SmartObjects.Services.Management;

namespace SourceCode.SmartObjects.Services.Tests.Extensions
{
    public static class ServiceManagementServerExtensions
    {
        public static void DeleteServiceInstance(this ServiceManagementServer server, Guid guid)
        {
            if (!string.IsNullOrEmpty(server.GetServiceInstanceCompact(guid)))
            {
                server.DeleteServiceInstance(guid, false);
            }
        }
    }
}