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
    public class GroupSO : ServiceObjectBase
    {
        public GroupSO(K2NEServiceBroker api)
            : base(api)
        {
        }
        private static class AdProperties
        {
            public const string Name = "name";
            public const string sAMAccountName  = "sAMAccountName";
            public const string Email = "mail";
            public const string Description = "description";
        }

        public override string ServiceFolder
        {
            get { return "URM"; }
        }

        public override List<ServiceObject> DescribeServiceObjects()
        {
            var soGroup = Helper.CreateServiceObject("URMGroup", "URMGroup");
            soGroup.Properties.Create(Helper.CreateProperty(Constants.Properties.URM.FQN, SoType.Text,
                "Fully Qualified name of the Group object"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.Properties.URM.GroupName, SoType.Text,
                "Formatted name of Group Name for a label"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.Properties.URM.Name, SoType.Text,
                "Name of Group"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.Properties.URM.Description, SoType.Text,
                "Description of group"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.Properties.URM.Email, SoType.Text,
                "Email of group"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.Properties.URM.Saml, SoType.Text,
                "sAMAccountName"));

            var getGroupDetails = Helper.CreateMethod(Constants.Methods.Group.GetGroupDetails,
                "Get Details for a specific group", MethodType.Read);
            getGroupDetails.ReturnProperties.Add(Constants.Properties.URM.FQN);
            getGroupDetails.ReturnProperties.Add(Constants.Properties.URM.GroupName);
            getGroupDetails.ReturnProperties.Add(Constants.Properties.URM.Name);
            getGroupDetails.ReturnProperties.Add(Constants.Properties.URM.Description);
            getGroupDetails.ReturnProperties.Add(Constants.Properties.URM.Email);
            getGroupDetails.InputProperties.Add(Constants.Properties.URM.FQN);
            getGroupDetails.Validation.RequiredProperties.Add(Constants.Properties.URM.FQN);
            soGroup.Methods.Create(getGroupDetails);

            var getGroups = Helper.CreateMethod(Constants.Methods.Group.GetGroups,
                "Gets a List of groups", MethodType.List);
            getGroups.ReturnProperties.Add(Constants.Properties.URM.FQN);
            getGroups.ReturnProperties.Add(Constants.Properties.URM.GroupName);
            getGroups.ReturnProperties.Add(Constants.Properties.URM.Name);
            getGroups.ReturnProperties.Add(Constants.Properties.URM.Description);
            getGroups.ReturnProperties.Add(Constants.Properties.URM.Email);
            getGroups.ReturnProperties.Add(Constants.Properties.URM.Saml);
            getGroups.InputProperties.Add(Constants.Properties.URM.FQN);
            getGroups.InputProperties.Add(Constants.Properties.URM.Name);
            getGroups.InputProperties.Add(Constants.Properties.URM.Saml);
            getGroups.MethodParameters.Create(Helper.CreateParameter(Constants.Properties.URM.Label, SoType.Text, true, "Label"));
            soGroup.Methods.Create(getGroups);

            return new List<ServiceObject>() { soGroup };
        }

        public override void Execute()
        {
            switch (ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.Group.GetGroupDetails:
                    GetGroupDetails();
                    break;
                case Constants.Methods.Group.GetGroups:
                    GetGroups();
                    break;
            }
        }

        private void GetGroupDetails()
        {
            var fqn = GetStringProperty(Constants.Properties.URM.FQN, true);

            ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            var dtResults = ServiceBroker.ServicePackage.ResultTable;

            var identityFromName = K2NEServiceBroker.IdentityService.GetIdentityFromName(K2NEServiceBroker.SecurityManager.GetFullyQualifiedName(fqn), IdentityType.Group, (string)null);
            if (identityFromName == null) return;

            var dRow = dtResults.NewRow();
            dRow[Constants.Properties.URM.FQN] = identityFromName.FullyQualifiedName.FQN;
            dRow[Constants.Properties.URM.GroupName] = identityFromName.FullyQualifiedName.FullName;
            if (identityFromName.Properties.ContainsKey("Name") && identityFromName.Properties["Name"] != null)
                dRow[Constants.Properties.URM.Name] = identityFromName.Properties["Name"].ToString();
            if (identityFromName.Properties.ContainsKey("Description") && identityFromName.Properties["Description"] != null)
                dRow[Constants.Properties.URM.Description] = identityFromName.Properties["Description"].ToString();
            if (identityFromName.Properties.ContainsKey("Email") && !string.IsNullOrEmpty(identityFromName.Properties["Email"].ToString()))
                dRow[Constants.Properties.URM.Email] = identityFromName.Properties["Email"].ToString();

            dtResults.Rows.Add(dRow);
        }
        private void GetGroups()
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
                    RunUMGetGroups(ldap, net);
                });
                //This line of code is needed so that the received items are not filtered again by K2 internal filtering of SMO.
                ServiceBroker.Service.ServiceObjects[0].Methods[0].Filter = null;
            }
            else
            {
                var fqn = GetStringProperty(Constants.Properties.URM.FQN);
                var name = GetStringProperty(Constants.Properties.URM.Name);
                var groupName = GetStringProperty(Constants.Properties.URM.GroupName);
                var email = GetStringProperty(Constants.Properties.URM.Email);
                var description = GetStringProperty(Constants.Properties.URM.Description);
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
                            case Constants.Properties.URM.GroupName:
                                groupName = keyValuePair.Value.Replace("'", "");
                                continue;
                            case Constants.Properties.URM.Description:
                                description = keyValuePair.Value.Replace("'", "");
                                continue;
                            case Constants.Properties.URM.Email:
                                email = keyValuePair.Value.Replace("'", "");
                                continue;
                            default:
                                continue;
                        }
                    }
                    var dictionary2 = new Dictionary<string, object>()
                    {
                        {"FQN", fqn},
                        {"Name", name},
                        {"GroupName", groupName},
                        {"Description", description},
                        {"Email", email}
                    };
                    if (!string.IsNullOrEmpty(lbl))
                        dictionary2["Label"] = (object) lbl;
                    var identities = K2NEServiceBroker.IdentityService.FindIdentities(dictionary2,
                        IdentitySearchOptions.Groups);
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
        }
        private void RunUMGetGroups(string ldap, string net)
        {
            var inputProperties = new Dictionary<string, string>()
            {
                {Constants.Properties.URM.FQN, GetStringProperty(Constants.Properties.URM.FQN)},
                {Constants.Properties.URM.Name, GetStringProperty(Constants.Properties.URM.Name)},
                {Constants.Properties.URM.Description, GetStringProperty(Constants.Properties.URM.Description)},
                {Constants.Properties.URM.Email, GetStringProperty(Constants.Properties.URM.Email)}
            };
            
            var lbl = GetStringParameter(Constants.Properties.URM.Label, true);
            var dSearcher = new DirectorySearcher(new DirectoryEntry(ldap));

            if (string.IsNullOrEmpty(lbl))
            {
                lbl = "K2";
            }
            var urmFilter = new URMFilter(ServiceBroker.Service.ServiceObjects[0].Methods[0].Filter);
            var filterCollection = urmFilter.GetFilterCollection();

            dSearcher.Filter = LdapHelper.GetLdapFilters(inputProperties, filterCollection, IdentityType.Group );
            dSearcher.PageSize = ADMaxResultSize;

            dSearcher.PropertiesToLoad.Add(AdProperties.sAMAccountName);
            dSearcher.PropertiesToLoad.Add(AdProperties.Name);
            dSearcher.PropertiesToLoad.Add(AdProperties.Email);
            dSearcher.PropertiesToLoad.Add(AdProperties.Description);

            var col = dSearcher.FindAll();
            var results = ServiceBroker.ServicePackage.ResultTable;
            foreach (SearchResult res in col)
            {
                var dr = results.NewRow();
                var saml = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.sAMAccountName);
                dr[Constants.Properties.URM.FQN] = string.Concat(lbl, ":", net, "\\", saml);
                dr[Constants.Properties.URM.Name] = string.Concat(net, "\\", saml);
                dr[Constants.Properties.URM.GroupName] = string.Concat(net, "\\", saml);
                dr[Constants.Properties.URM.Description] = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.Description);
                dr[Constants.Properties.URM.Email] = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.Email);
                dr[Constants.Properties.URM.Saml] = saml;

                lock (ServiceBroker.ServicePackage.ResultTable)
                {
                    results.Rows.Add(dr);
                }
            }
        }
    }
}