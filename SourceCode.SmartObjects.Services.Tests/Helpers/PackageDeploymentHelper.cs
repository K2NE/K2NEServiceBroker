using System;
using System.IO;
using System.Linq;
using System.Reflection;
using SourceCode.Deployment.Management;

namespace SourceCode.SmartObjects.Services.Tests.Helpers
{
    public static class PackageDeploymentHelper
    {
        private const string _sessionName = "SmartObjectsServicesTestsHelpers";

        public static void DeployPackage(byte[] package)
        {
            var packageDeploymentManager = ConnectionHelper.GetServer<PackageDeploymentManager>();
            using (packageDeploymentManager.Connection)
            {
                using (var fileStream = new MemoryStream(package))
                using (var session = packageDeploymentManager.CreateSession(_sessionName, fileStream))
                {
                    session.Deploy();
                }
            }
        }

        public static void DeployPackages(Assembly assembly)
        {
            var packageDeploymentManager = ConnectionHelper.GetServer<PackageDeploymentManager>();
            using (packageDeploymentManager.Connection)
            {
                // Get the KSPX package from the embeded resources of the assembly
                foreach (var resource in assembly.GetManifestResourceNames().Where(i => i.IndexOf(".kspx", StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    using (var streamKspx = assembly.GetManifestResourceStream(resource))
                    using (var session = packageDeploymentManager.CreateSession(resource))
                    {
                        session.Load(streamKspx);
                        session.Deploy();
                    }
                }
            }
        }
    }
}