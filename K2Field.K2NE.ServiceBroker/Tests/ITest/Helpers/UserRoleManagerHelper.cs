using SourceCode.Security.UserRoleManager.Client;
using SourceCode.SmartObjects.Services.Tests.Helpers;

namespace K2Field.K2NE.ServiceBroker.ITest.Helpers
{
    internal static class UserRoleManagerHelper
    {
        public static string ConvertUserNameToFQN(string userName)
        {
            return $"{GetDefaultLabelName()}:{userName}";
        }

        public static string GetDefaultLabelName()
        {
            var server = ConnectionHelper.GetServer<UserRoleManagerServer>();
            using (server.Connection)
            {
                return server.GetDefaultLabelName();
            }
        }

        public static string GetCurrentUserFQN()
        {
            var userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            return ConvertUserNameToFQN(userName);
        }
    }
}