using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K2Field.K2NE.ServiceBroker.Constants
{
    public static class ConfigurationProperties
    {
        public const string EnvironmentToUse = "Environment to use (empty for default)";
        public const string WorkflowManagmentPort = "Workflow Management Port";
        public const string WorkflowClientPort = "Workflow Client Port";
        public const string DefaultCulture = "Default Culture";
        public const string Platform = "Platform to use";
        public const string LDAPPaths = "AD LDAP Path";
        public const string NetbiosNames = "AD Netbios Name";
        public const string AdMaxResultSize = "AD Maximum Resultsize";
        public const string ChangeContainsToStartsWith = "Change Contains operator to StartsWith for AD";
        public const string AdditionalADProps = "Additional AD properties delimited by ;";
        public const string ADOQueries = "Data queries delimited by ;";
    }

}
