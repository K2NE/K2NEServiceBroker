using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;
using System.Text;
using K2Field.K2NE.ServiceBroker.ServiceObjects.URM;
using SourceCode.Hosting.Server.Interfaces;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;

namespace K2Field.K2NE.ServiceBroker.Helpers
{
    public static class LdapHelper
    {
        public static class AdProperties
        {
            public const string sAMAccountName = "sAMAccountName";
            public const string DisplayName = "displayName";
            public const string CommonName = "cn";
            public const string GivenName = "givenName";
            public const string Initials = "initials";
            public const string Surname = "sn";
            public const string Email = "mail";
            public const string DistinguishedName = "distinguishedName";
            public const string Description = "description";
            public const string ObjectSID = "objectSid";
            public const string Manager = "manager";
            public const string Name = "name";
        }

        public static string GetSingleStringPropertyCollectionValue(ResultPropertyCollection props, string name)
        {
            if (!props.Contains(name))
            {
                return string.Empty;
            }
            var pvc = props[name];
            if (pvc == null || pvc.Count == 0)
            {
                return string.Empty;
            }
            switch (name)
            {
                case AdProperties.ObjectSID:
                    var sidInBytes = (byte[])pvc[0];
                    var sid = new SecurityIdentifier(sidInBytes, 0);
                    return Convert.ToString(sid);
                default:
                    return pvc[0] as string;
            }
        }

        public static string GetLdapFilters(Dictionary<string, string> inputProp,
            Dictionary<string, Dictionary<string, string>> filters, IdentityType iType)
        {
            var searchFilter = new StringBuilder();
            searchFilter.Append("(&");
            //Adding the type of AD object we are looking for.
            switch (iType)
            {
                case IdentityType.Group:
                    searchFilter.Append("(objectcategory=group)(objectclass=group)");
                    break;
                case IdentityType.User:
                    searchFilter.Append("(objectcategory=person)(objectclass=user)");
                    break;
            }
            //Adding input parameters filtering
            foreach (var item in inputProp)
            {
                if (!String.IsNullOrEmpty(item.Value))
                {
                    switch (item.Key)
                    {
                        case Constants.Properties.URM.Description:
                            searchFilter.AppendFormat("({0}={1})", AdProperties.Description, item.Value);
                            continue;
                        case Constants.Properties.URM.DisplayName:
                            searchFilter.AppendFormat("({0}={1})", AdProperties.DisplayName, item.Value);
                            continue;
                        case Constants.Properties.URM.Email:
                            searchFilter.AppendFormat("({0}={1})", AdProperties.Email, item.Value);
                            continue;
                        case Constants.Properties.URM.FQN:
                            var fqn = item.Value.Substring(item.Value.IndexOf('\\') + 1);
                            searchFilter.AppendFormat("({0}={1})", AdProperties.sAMAccountName, fqn);
                            continue;
                        case Constants.Properties.URM.GroupName:
                            var groupName = item.Value.Substring(item.Value.IndexOf('\\') + 1);
                            searchFilter.AppendFormat("({0}={1})", AdProperties.sAMAccountName, groupName);
                            continue;
                        case Constants.Properties.URM.Name:
                            var name = item.Value.Substring(item.Value.IndexOf('\\') + 1);
                            searchFilter.AppendFormat("({0}={1})", AdProperties.sAMAccountName, name);
                            continue;
                        case Constants.Properties.URM.ObjectSid:
                            searchFilter.AppendFormat("({0}={1})", AdProperties.ObjectSID, item.Value);
                            continue;
                        case Constants.Properties.URM.Saml:
                            searchFilter.AppendFormat("({0}={1})", AdProperties.sAMAccountName, item.Value);
                            continue;
                        default:
                            continue;
                    }
                }
            }
            //TODO: Conversion of SQL filters to LDAP filters
            //Adding filters - adding them only as OR
            foreach (var dict in filters.Values)
            {
                if (dict.Values.Any())
                {
                    searchFilter.Append("(|");
                    foreach (var item in dict)
                    {
                        switch (item.Key)
                        {
                            case Constants.Properties.URM.Description:
                                var description = item.Value.Replace("'", "");
                                description = description.TrimStart('*');
                                searchFilter.AppendFormat("({0}={1})", AdProperties.Description, description);
                                continue;
                            case Constants.Properties.URM.DisplayName:
                                var displayName = item.Value.Replace("'", "");
                                displayName = displayName.TrimStart('*');
                                searchFilter.AppendFormat("({0}={1})", AdProperties.DisplayName, displayName);
                                continue;
                            case Constants.Properties.URM.Email:
                                var email = item.Value.Replace("'", "");
                                email = email.TrimStart('*');
                                searchFilter.AppendFormat("({0}={1})", AdProperties.Email, email);
                                continue;
                            case Constants.Properties.URM.FQN:
                                var fqn = item.Value.Replace("'", "");
                                fqn = fqn.Substring(fqn.IndexOf('\\') + 1);
                                fqn = fqn.TrimStart('*');
                                searchFilter.AppendFormat("({0}={1})", AdProperties.sAMAccountName, fqn);
                                continue;
                            case Constants.Properties.URM.GroupName:
                                var groupName = item.Value.Replace("'", "");
                                groupName = groupName.Substring(groupName.IndexOf('\\') + 1);
                                groupName = groupName.TrimStart('*');
                                searchFilter.AppendFormat("({0}={1})", AdProperties.sAMAccountName, groupName);
                                continue;
                            case Constants.Properties.URM.Name:
                                var name = item.Value.Replace("'", "");
                                name = name.Substring(name.IndexOf('\\') + 1);
                                name = name.TrimStart('*');
                                searchFilter.AppendFormat("({0}={1})", AdProperties.sAMAccountName, name);
                                continue;
                            case Constants.Properties.URM.ObjectSid:
                                var objectSid = item.Value.Replace("'", "");
                                objectSid = objectSid.TrimStart('*');
                                searchFilter.AppendFormat("({0}={1})", AdProperties.ObjectSID, objectSid);
                                continue;
                            case Constants.Properties.URM.Saml:
                                var saml = item.Value.Replace("'", "");
                                saml = saml.TrimStart('*');
                                searchFilter.AppendFormat("({0}={1})", AdProperties.sAMAccountName, saml);
                                continue;
                            default:
                                continue;
                        }
                    }
                    searchFilter.Append(")");
                }
            }
            searchFilter.Append(")");
            return searchFilter.ToString();
        }
    }
}
