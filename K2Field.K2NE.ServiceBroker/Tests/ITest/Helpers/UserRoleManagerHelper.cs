using SourceCode.Hosting.Server.Interfaces;
using SourceCode.Security.UserRoleManager.Management;
using SourceCode.SmartObjects.Services.Tests.Helpers;

namespace K2Field.K2NE.ServiceBroker.ITest.Helpers
{
    /// <summary>
    /// UserRoleManagerHelper
    /// </summary>
    internal static class UserRoleManagerHelper
    {
        internal static void ResolveIdentities(string[] users, string[] groups)
        {
            var server = ConnectionHelper.GetServer<UserRoleManager>();
            using (server.Connection)
            {
                foreach (var user in users)
                {
                    var fqn = new FQName(user);
                    fqn.Label = "K2";
                    server.ResolveIdentity(fqn, IdentityType.User, IdentitySection.Identity | IdentitySection.Containers);
                }

                foreach (var group in groups)
                {
                    var fqn = new FQName(group);
                    fqn.Label = "K2";
                    server.ResolveIdentity(fqn, IdentityType.Group, IdentitySection.Members | IdentitySection.Identity | IdentitySection.Containers);
                }
            }
        }

        internal static void ResolveIdentitiesForTestUsers()
        {
            var users = new string[] {
                    Constants.Security.User1Name,
                    Constants.Security.User2Name,
                    Constants.Security.User3Name,
                    Constants.Security.User4Name,
                };

            var groups = new string[] {
                    Constants.Security.User3GroupName,
                };

            ResolveIdentities(users, groups);
        }
    }
}