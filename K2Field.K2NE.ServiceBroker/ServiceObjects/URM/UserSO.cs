using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Threading;
using System.Threading.Tasks;
using SourceCode.Hosting.Server.Interfaces;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using K2Field.K2NE.ServiceBroker.Helpers;

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
            var soGroup = Helper.CreateServiceObject("URMUser", "URMUser");
            soGroup.Properties.Create(Helper.CreateProperty(Constants.Properties.URM.FQN, SoType.Text,
                "Fully Qualified name of the User object"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.Properties.URM.UserName, SoType.Text,
                "User Name as provided by label provider"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.Properties.URM.Name, SoType.Text,
                "Name of the user"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.Properties.URM.Description, SoType.Text,
                "User Description"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.Properties.URM.Email, SoType.Text,
                "Email address"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.Properties.URM.Manager, SoType.Text,
                "User manager"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.Properties.URM.ObjectSid, SoType.Text,
                "User SID"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.Properties.URM.DisplayName, SoType.Text,
                "Display name of the User object"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.Properties.URM.Saml, SoType.Text,
                "sAMAccountName"));

            var getUsers = Helper.CreateMethod(Constants.Methods.User.GetUsers,
                "Gets a List of groups", MethodType.List);
            getUsers.ReturnProperties.Add(Constants.Properties.URM.Name);
            getUsers.ReturnProperties.Add(Constants.Properties.URM.Description);
            getUsers.ReturnProperties.Add(Constants.Properties.URM.Email);
            getUsers.ReturnProperties.Add(Constants.Properties.URM.Manager);
            getUsers.ReturnProperties.Add(Constants.Properties.URM.DisplayName);
            getUsers.ReturnProperties.Add(Constants.Properties.URM.FQN);
            getUsers.ReturnProperties.Add(Constants.Properties.URM.UserName);
            getUsers.ReturnProperties.Add(Constants.Properties.URM.ObjectSid);
            getUsers.ReturnProperties.Add(Constants.Properties.URM.Saml);
            getUsers.InputProperties.Add(Constants.Properties.URM.Name);
            getUsers.InputProperties.Add(Constants.Properties.URM.Description);
            getUsers.InputProperties.Add(Constants.Properties.URM.Email);
            getUsers.InputProperties.Add(Constants.Properties.URM.DisplayName);
            getUsers.InputProperties.Add(Constants.Properties.URM.Saml);
            getUsers.MethodParameters.Create(Helper.CreateParameter(Constants.Properties.URM.Label, SoType.Text, true, "Label"));
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
            var ldaps = LDAPPaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var netbioses = NetBiosNames.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var lbl = GetStringParameter(Constants.Properties.URM.Label, true);
            ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            if (lbl == "K2")
            {
                var threads = new List<Thread>();
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
                var fqn = GetStringProperty(Constants.Properties.URM.FQN);
                var name = GetStringProperty(Constants.Properties.URM.Name);
                var email = GetStringProperty(Constants.Properties.URM.Email);
                var description = GetStringProperty(Constants.Properties.URM.Description);
                var manager = GetStringProperty(Constants.Properties.URM.Manager);
                var displayName = GetStringProperty(Constants.Properties.URM.DisplayName);
                var userName = GetStringProperty(Constants.Properties.URM.UserName);
                var objectSid = GetStringProperty(Constants.Properties.URM.ObjectSid);
                var saml = GetStringProperty(Constants.Properties.URM.Saml);

                var dtResults = ServiceBroker.ServicePackage.ResultTable;
                var urmFilter = new URMFilter(ServiceBroker.Service.ServiceObjects[0].Methods[0].Filter);

                foreach (var dictionary1 in urmFilter.GetFilterCollection().Values)
                {
                    foreach (var keyValuePair in dictionary1)
                    {
                        switch (keyValuePair.Key)
                        {
                            case Constants.Properties.URM.FQN:
                                fqn = keyValuePair.Value.Replace("'", "");
                                continue;
                            case Constants.Properties.URM.Name:
                                name = keyValuePair.Value.Replace("'", "");
                                continue;
                            case Constants.Properties.URM.Email:
                                email = keyValuePair.Value.Replace("'", "");
                                continue;
                            case Constants.Properties.URM.Description:
                                description = keyValuePair.Value.Replace("'", "");
                                continue;
                            case Constants.Properties.URM.Manager:
                                manager = keyValuePair.Value.Replace("'", "");
                                continue;
                            case Constants.Properties.URM.DisplayName:
                                displayName = keyValuePair.Value.Replace("'", "");
                                continue;
                            case Constants.Properties.URM.UserName:
                                userName = keyValuePair.Value.Replace("'", "");
                                continue;
                            case Constants.Properties.URM.ObjectSid:
                                objectSid= keyValuePair.Value.Replace("'", "");
                                continue;
                            case Constants.Properties.URM.Saml:
                                saml = keyValuePair.Value.Replace("'", "");
                                continue;
                            default:
                                continue;
                        }
                    }
                    var dictionary2 = new Dictionary<string, object>()
                    {
                        {Constants.Properties.URM.FQN, fqn},
                        {Constants.Properties.URM.Name, name},
                        {Constants.Properties.URM.Description, description},
                        {Constants.Properties.URM.Email, email},
                        {Constants.Properties.URM.Manager, manager},
                        {Constants.Properties.URM.DisplayName, displayName},
                        {Constants.Properties.URM.UserName, userName},
                        {Constants.Properties.URM.ObjectSid, objectSid},
                        {Constants.Properties.URM.Saml, saml}

                    };
                    if (!string.IsNullOrEmpty(lbl))
                        dictionary2["Label"] = (object)lbl;
                    var identities = K2NEServiceBroker.IdentityService.FindIdentities(dictionary2,
                        IdentitySearchOptions.Users);
                    if (identities == null) return;

                    foreach (var cachedIdentity in identities)
                    {
                        if (cachedIdentity.Type == IdentityType.Group)
                        {
                            var dRow = dtResults.NewRow();
                            dRow[Constants.Properties.URM.FQN] = cachedIdentity.FullyQualifiedName.FQN;
                            dRow[Constants.Properties.URM.GroupName] = cachedIdentity.FullyQualifiedName.FullName;
                            dRow[Constants.Properties.URM.Saml] = cachedIdentity.FullyQualifiedName.FullName;
                            if (cachedIdentity.Properties.ContainsKey("Name") &&
                                cachedIdentity.Properties["Name"] != null)
                                dRow[Constants.Properties.URM.Name] = cachedIdentity.Properties["Name"].ToString();
                            if (cachedIdentity.Properties.ContainsKey("Description") &&
                                cachedIdentity.Properties["Description"] != null)
                                dRow[Constants.Properties.URM.Description] =
                                    cachedIdentity.Properties["Description"].ToString();
                            if (cachedIdentity.Properties.ContainsKey("Email") &&
                                !string.IsNullOrEmpty(cachedIdentity.Properties["Email"].ToString()))
                                dRow[Constants.Properties.URM.Email] = cachedIdentity.Properties["Email"].ToString();
                            dtResults.Rows.Add(dRow);
                        }
                    }
                }
            }
            ServiceBroker.Service.ServiceObjects[0].Methods[0].Filter = null;
        }
        private void RunUMGetUsers(string ldap, string net)
        {
            var inputProperties = new Dictionary<string, string>()
            {
                {Constants.Properties.URM.FQN, GetStringProperty(Constants.Properties.URM.FQN)},
                {Constants.Properties.URM.Name, GetStringProperty(Constants.Properties.URM.Name)},
                {Constants.Properties.URM.Description, GetStringProperty(Constants.Properties.URM.Description)},
                {Constants.Properties.URM.Email, GetStringProperty(Constants.Properties.URM.Email)},
                {Constants.Properties.URM.DisplayName, GetStringProperty(Constants.Properties.URM.DisplayName)},
                {Constants.Properties.URM.Saml, GetStringProperty(Constants.Properties.URM.Saml)}
            };
            
            var lbl = GetStringParameter(Constants.Properties.URM.Label, true);
            var dSearcher = new DirectorySearcher(new DirectoryEntry(ldap));

            if (string.IsNullOrEmpty(lbl))
            {
                lbl = "K2";
            }
            var urmFilter = new URMFilter(ServiceBroker.Service.ServiceObjects[0].Methods[0].Filter);
            var filterCollection = urmFilter.GetFilterCollection();

            dSearcher.Filter = LdapHelper.GetLdapFilters(inputProperties, filterCollection, IdentityType.User );
            dSearcher.PageSize = ADMaxResultSize;

            dSearcher.PropertiesToLoad.Add(LdapHelper.AdProperties.Name);
            dSearcher.PropertiesToLoad.Add(LdapHelper.AdProperties.Email);
            dSearcher.PropertiesToLoad.Add(LdapHelper.AdProperties.Description);
            dSearcher.PropertiesToLoad.Add(LdapHelper.AdProperties.sAMAccountName);
            dSearcher.PropertiesToLoad.Add(LdapHelper.AdProperties.DisplayName);
            dSearcher.PropertiesToLoad.Add(LdapHelper.AdProperties.ObjectSID);

            var col = dSearcher.FindAll();
            var results = ServiceBroker.ServicePackage.ResultTable;
            foreach (SearchResult res in col)
            {
                var dr = results.NewRow();
                var saml = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, LdapHelper.AdProperties.sAMAccountName);
                dr[Constants.Properties.URM.FQN] = string.Concat(lbl, ":", net, "\\", saml);
                dr[Constants.Properties.URM.Name] = string.Concat(net, "\\", saml);
                dr[Constants.Properties.URM.UserName] = string.Concat(net, "\\", saml);
                dr[Constants.Properties.URM.Description] = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, LdapHelper.AdProperties.Description);
                dr[Constants.Properties.URM.Email] = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, LdapHelper.AdProperties.Email);
                dr[Constants.Properties.URM.DisplayName] = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, LdapHelper.AdProperties.DisplayName);
                dr[Constants.Properties.URM.ObjectSid] = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, LdapHelper.AdProperties.ObjectSID);
                dr[Constants.Properties.URM.Manager] = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, LdapHelper.AdProperties.Manager);
                dr[Constants.Properties.URM.Saml] = saml;
                lock (ServiceBroker.ServicePackage.ResultTable)
                {
                    results.Rows.Add(dr);
                }
            }
        }
    }
}