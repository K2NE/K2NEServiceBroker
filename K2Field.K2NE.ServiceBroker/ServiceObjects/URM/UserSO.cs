using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Threading;
using System.Threading.Tasks;
using SourceCode.Hosting.Server.Interfaces;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using K2Field.K2NE.ServiceBroker.Helpers;
using System.Data;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects.URM
{
    public class UserSO : ServiceObjectBase
    {
        public UserSO(K2NEServiceBroker api)
            : base(api)
        {
        }
        public override string ServiceFolder
        {
            get { return "URM"; }
        }

        public override List<ServiceObject> DescribeServiceObjects()
        {
            ServiceObject soGroup = Helper.CreateServiceObject("URMUser", "URMUser");
            soGroup.Properties.Create(Helper.CreateProperty(Constants.SOProperties.URM.FQN, SoType.Text, "Fully Qualified name of the User object"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.SOProperties.URM.UserName, SoType.Text, "User Name as provided by label provider"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.SOProperties.URM.Name, SoType.Text, "Name of the user"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.SOProperties.URM.Description, SoType.Text, "User Description"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.SOProperties.URM.Email, SoType.Text, "Email address"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.SOProperties.URM.Manager, SoType.Text, "User manager"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.SOProperties.URM.ObjectSid, SoType.Text, "User SID"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.SOProperties.URM.DisplayName, SoType.Text, "Display name of the User object"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.SOProperties.URM.Saml, SoType.Text, "sAMAccountName"));

            Method getUsers = Helper.CreateMethod(Constants.Methods.User.GetUsers, "Gets a List of groups", MethodType.List);
            getUsers.ReturnProperties.Add(Constants.SOProperties.URM.Name);
            getUsers.ReturnProperties.Add(Constants.SOProperties.URM.Description);
            getUsers.ReturnProperties.Add(Constants.SOProperties.URM.Email);
            getUsers.ReturnProperties.Add(Constants.SOProperties.URM.Manager);
            getUsers.ReturnProperties.Add(Constants.SOProperties.URM.DisplayName);
            getUsers.ReturnProperties.Add(Constants.SOProperties.URM.FQN);
            getUsers.ReturnProperties.Add(Constants.SOProperties.URM.UserName);
            getUsers.ReturnProperties.Add(Constants.SOProperties.URM.ObjectSid);
            getUsers.ReturnProperties.Add(Constants.SOProperties.URM.Saml);
            getUsers.InputProperties.Add(Constants.SOProperties.URM.Name);
            getUsers.InputProperties.Add(Constants.SOProperties.URM.Description);
            getUsers.InputProperties.Add(Constants.SOProperties.URM.Email);
            getUsers.InputProperties.Add(Constants.SOProperties.URM.DisplayName);
            getUsers.InputProperties.Add(Constants.SOProperties.URM.Saml);
            getUsers.MethodParameters.Create(Helper.CreateParameter(Constants.SOProperties.URM.Label, SoType.Text, true, "The label to use"));
            soGroup.Methods.Create(getUsers);

            return new List<ServiceObject>() { soGroup };
        }

        public override void Execute()
        {
            switch (ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.User.GetUsers:
                    GetUsers();
                    break;
            }
        }

        private void GetUsers()
        {
            string[] ldaps = LDAPPaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string[] netbioses = NetBiosNames.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string securityLabel = GetStringParameter(Constants.SOProperties.URM.Label, true);
            ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();

            if (string.Compare(securityLabel, "K2", true) == 0)
            {
                List<Thread> threads = new List<Thread>();
                string ldap, net;
                Parallel.For(0, ldaps.Length, i =>
                {
                    ldap = ldaps[i];
                    net = netbioses[i];
                    RunUMGetUsers(ldap, net);
                });
                //This line of code is needed so that the received items are not filtered again by K2 internal filtering of SMO.
                ServiceBroker.Service.ServiceObjects[0].Methods[0].Filter = null;
            }
            else
            {
                // The below is basically copy/pasted code from the URM service. We don't really have a better way of calling that service instance code.

                string fqn = GetStringProperty(Constants.SOProperties.URM.FQN);
                string name = GetStringProperty(Constants.SOProperties.URM.Name);
                string email = GetStringProperty(Constants.SOProperties.URM.Email);
                string description = GetStringProperty(Constants.SOProperties.URM.Description);
                string manager = GetStringProperty(Constants.SOProperties.URM.Manager);
                string displayName = GetStringProperty(Constants.SOProperties.URM.DisplayName);
                string userName = GetStringProperty(Constants.SOProperties.URM.UserName);
                string objectSid = GetStringProperty(Constants.SOProperties.URM.ObjectSid);
                string saml = GetStringProperty(Constants.SOProperties.URM.Saml);

                DataTable dtResults = ServiceBroker.ServicePackage.ResultTable;
                URMFilter urmFilter = new URMFilter(ServiceBroker.Service.ServiceObjects[0].Methods[0].Filter);

                foreach (var dictionary in urmFilter.GetFilterCollection().Values)
                {
                    foreach (var keyValuePair in dictionary)
                    {
                        switch (keyValuePair.Key)
                        {
                            case Constants.SOProperties.URM.FQN:
                                fqn = keyValuePair.Value.Replace("N'", "").Replace("'", "");
                                continue;
                            case Constants.SOProperties.URM.Name:
                                name = keyValuePair.Value.Replace("N'", "").Replace("'", "");
                                continue;
                            case Constants.SOProperties.URM.Email:
                                email = keyValuePair.Value.Replace("N'", "").Replace("'", "");
                                continue;
                            case Constants.SOProperties.URM.Description:
                                description = keyValuePair.Value.Replace("N'", "").Replace("'", "");
                                continue;
                            case Constants.SOProperties.URM.Manager:
                                manager = keyValuePair.Value.Replace("N'", "").Replace("'", "");
                                continue;
                            case Constants.SOProperties.URM.DisplayName:
                                displayName = keyValuePair.Value.Replace("N'", "").Replace("'", "");
                                continue;
                            case Constants.SOProperties.URM.UserName:
                                userName = keyValuePair.Value.Replace("N'", "").Replace("'", "");
                                continue;
                            case Constants.SOProperties.URM.ObjectSid:
                                objectSid = keyValuePair.Value.Replace("N'", "").Replace("'", "");
                                continue;
                            case Constants.SOProperties.URM.Saml:
                                saml = keyValuePair.Value.Replace("N'", "").Replace("'", "");
                                continue;
                            default:
                                continue;
                        }
                    }
                    var properties = new Dictionary<string, object>()
                    {
                        {Constants.SOProperties.URM.Name, name == String.Empty ? (object) (string) null : (object) name},
                        {
                            Constants.SOProperties.URM.Description,
                            description == String.Empty ? (object) (string) null : (object) description
                        },
                        {
                            Constants.SOProperties.URM.Email, email == String.Empty ? (object) (string) null : (object) email
                        },
                        {
                            Constants.SOProperties.URM.Manager,
                            manager == String.Empty ? (object) (string) null : (object) manager
                        },
                        {
                            Constants.SOProperties.URM.DisplayName,
                            displayName == String.Empty ? (object) (string) null : (object) displayName
                        }
                    };
                    Helper.AddNonStandardProperties(properties, base.ServiceBroker.IdentityService.QueryUserProperties(securityLabel));

                    if (!string.IsNullOrEmpty(securityLabel))
                        properties[Constants.SOProperties.URM.Label] = (object)securityLabel;

                    if (ADMaxResultSize != -1)
                    {
                        properties["RowCount"] = (object)(ADMaxResultSize);
                        properties["PageNumber"] = (object)1;
                    }
                    var collection = base.ServiceBroker.IdentityService.FindIdentities((IDictionary<string, object>)properties, IdentitySearchOptions.Users);

          
                    bool flag = properties.ContainsKey("RowCount");
                    int result = 0;
                    if (flag)
                    {
                        int.TryParse((string)properties["RowCount"], out result);
                    }
                    if (collection != null)
                    {
                        foreach (var cachedIdentity in collection)
                        {
                            if (cachedIdentity.Type == IdentityType.User)
                            {
                                DataRow dRow = dtResults.NewRow();
                                dRow[Constants.SOProperties.URM.FQN] = cachedIdentity.FullyQualifiedName.FQN;
                                if (cachedIdentity.Properties.ContainsKey("Name") && cachedIdentity.Properties["Name"] != null)
                                {
                                    dRow[Constants.SOProperties.URM.UserName] = cachedIdentity.Properties["Name"].ToString();
                                    dRow[Constants.SOProperties.URM.Name] = cachedIdentity.Properties["Name"].ToString();
                                    dRow[Constants.SOProperties.URM.Saml] = LdapHelper.GetSAMAccountName(cachedIdentity.Properties["Name"].ToString());
                                }
                                if (cachedIdentity.Properties.ContainsKey("Description") && cachedIdentity.Properties["Description"] != null)
                                {
                                    dRow[Constants.SOProperties.URM.Description] = cachedIdentity.Properties["Description"].ToString();
                                }
                                if (cachedIdentity.Properties.ContainsKey("Email") && !string.IsNullOrEmpty(cachedIdentity.Properties["Email"].ToString()))
                                {
                                    dRow[Constants.SOProperties.URM.Email] = cachedIdentity.Properties["Email"].ToString();
                                }
                                if (cachedIdentity.Properties.ContainsKey("Manager") && cachedIdentity.Properties["Manager"] != null)
                                {
                                    dRow[Constants.SOProperties.URM.Manager] = cachedIdentity.Properties["Manager"].ToString();
                                }
                                if (cachedIdentity.Properties.ContainsKey("ObjectSID") && cachedIdentity.Properties["ObjectSID"] != null)
                                {
                                    dRow[Constants.SOProperties.URM.ObjectSid] = cachedIdentity.Properties["ObjectSID"].ToString();
                                }
                                if (cachedIdentity.Properties.ContainsKey("DisplayName") && cachedIdentity.Properties["DisplayName"] != null)
                                {
                                    dRow[Constants.SOProperties.URM.DisplayName] = cachedIdentity.Properties["DisplayName"].ToString();
                                }
                                dtResults.Rows.Add(dRow);


                                if (flag && dtResults.Rows.Count == result)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void RunUMGetUsers(string ldap, string net)
        {
            Dictionary<string, string> inputProperties = new Dictionary<string, string>()
            {
                {Constants.SOProperties.URM.FQN, GetStringProperty(Constants.SOProperties.URM.FQN)},
                {Constants.SOProperties.URM.Name, GetStringProperty(Constants.SOProperties.URM.Name)},
                {Constants.SOProperties.URM.Description, GetStringProperty(Constants.SOProperties.URM.Description)},
                {Constants.SOProperties.URM.Email, GetStringProperty(Constants.SOProperties.URM.Email)},
                {Constants.SOProperties.URM.DisplayName, GetStringProperty(Constants.SOProperties.URM.DisplayName)},
                {Constants.SOProperties.URM.Saml, GetStringProperty(Constants.SOProperties.URM.Saml)}
            };

            string securityLabel = GetStringParameter(Constants.SOProperties.URM.Label, true);
            DirectorySearcher dSearcher = new DirectorySearcher(new DirectoryEntry(ldap));

            if (string.IsNullOrEmpty(securityLabel))
            {
                securityLabel = "K2";
            }

            dSearcher.Filter = LdapHelper.GetLdapQueryString(inputProperties, ServiceBroker.Service.ServiceObjects[0].Methods[0].Filter, IdentityType.User, ChangeContainsToStartWith);
            dSearcher.SizeLimit = ADMaxResultSize;

            dSearcher.PropertiesToLoad.Add(Constants.Properties.AdProperties.Name);
            dSearcher.PropertiesToLoad.Add(Constants.Properties.AdProperties.Email);
            dSearcher.PropertiesToLoad.Add(Constants.Properties.AdProperties.Description);
            dSearcher.PropertiesToLoad.Add(Constants.Properties.AdProperties.sAMAccountName);
            dSearcher.PropertiesToLoad.Add(Constants.Properties.AdProperties.DisplayName);
            dSearcher.PropertiesToLoad.Add(Constants.Properties.AdProperties.ObjectSID);

            SearchResultCollection col = dSearcher.FindAll();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;
            foreach (SearchResult res in col)
            {
                DataRow dr = results.NewRow();
                string saml = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, Constants.Properties.AdProperties.sAMAccountName);
                dr[Constants.SOProperties.URM.FQN] = string.Concat(securityLabel, ":", net, "\\", saml);
                dr[Constants.SOProperties.URM.Name] = string.Concat(net, "\\", saml);
                dr[Constants.SOProperties.URM.UserName] = string.Concat(net, "\\", saml);
                dr[Constants.SOProperties.URM.Description] = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, Constants.Properties.AdProperties.Description);
                dr[Constants.SOProperties.URM.Email] = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, Constants.Properties.AdProperties.Email);
                dr[Constants.SOProperties.URM.DisplayName] = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, Constants.Properties.AdProperties.DisplayName);
                dr[Constants.SOProperties.URM.ObjectSid] = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, Constants.Properties.AdProperties.ObjectSID);
                dr[Constants.SOProperties.URM.Manager] = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, Constants.Properties.AdProperties.Manager);
                dr[Constants.SOProperties.URM.Saml] = saml;
                lock (ServiceBroker.ServicePackage.ResultTable)
                {
                    results.Rows.Add(dr);
                }
            }
        }
    }
}