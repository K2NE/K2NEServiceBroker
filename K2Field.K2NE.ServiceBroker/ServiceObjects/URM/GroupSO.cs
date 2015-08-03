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
            ServiceObject soGroup = Helper.CreateServiceObject("URMGroup", "URMGroup");
            soGroup.Properties.Create(Helper.CreateProperty(Constants.SOProperties.URM.FQN, SoType.Text, "Fully Qualified name of the Group object"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.SOProperties.URM.GroupName, SoType.Text, "Formatted name of Group Name for a label"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.SOProperties.URM.Name, SoType.Text, "Name of Group"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.SOProperties.URM.Description, SoType.Text, "Description of group"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.SOProperties.URM.Email, SoType.Text, "Email of group"));
            soGroup.Properties.Create(Helper.CreateProperty(Constants.SOProperties.URM.Saml, SoType.Text, "sAMAccountName"));

            Method getGroupDetails = Helper.CreateMethod(Constants.Methods.Group.GetGroupDetails, "Get Details for a specific group", MethodType.Read);
            getGroupDetails.ReturnProperties.Add(Constants.SOProperties.URM.FQN);
            getGroupDetails.ReturnProperties.Add(Constants.SOProperties.URM.GroupName);
            getGroupDetails.ReturnProperties.Add(Constants.SOProperties.URM.Name);
            getGroupDetails.ReturnProperties.Add(Constants.SOProperties.URM.Description);
            getGroupDetails.ReturnProperties.Add(Constants.SOProperties.URM.Email);
            getGroupDetails.InputProperties.Add(Constants.SOProperties.URM.FQN);
            getGroupDetails.Validation.RequiredProperties.Add(Constants.SOProperties.URM.FQN);
            soGroup.Methods.Create(getGroupDetails);

            Method getGroups = Helper.CreateMethod(Constants.Methods.Group.GetGroups, "Gets a List of groups", MethodType.List);
            getGroups.ReturnProperties.Add(Constants.SOProperties.URM.FQN);
            getGroups.ReturnProperties.Add(Constants.SOProperties.URM.GroupName);
            getGroups.ReturnProperties.Add(Constants.SOProperties.URM.Name);
            getGroups.ReturnProperties.Add(Constants.SOProperties.URM.Description);
            getGroups.ReturnProperties.Add(Constants.SOProperties.URM.Email);
            getGroups.ReturnProperties.Add(Constants.SOProperties.URM.Saml);
            getGroups.InputProperties.Add(Constants.SOProperties.URM.Name);
            getGroups.InputProperties.Add(Constants.SOProperties.URM.Description);
            getGroups.InputProperties.Add(Constants.SOProperties.URM.Saml);
            getGroups.MethodParameters.Create(Helper.CreateParameter(Constants.SOProperties.URM.Label, SoType.Text, true, "Label"));
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
            string fqn = GetStringProperty(Constants.SOProperties.URM.FQN, true);

            ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable dtResults = ServiceBroker.ServicePackage.ResultTable;

            var identityFromName = base.ServiceBroker.IdentityService.GetIdentityFromName(base.ServiceBroker.SecurityManager.GetFullyQualifiedName(fqn), IdentityType.Group, (string)null);
            if (identityFromName == null)
            {
                return;
            }

            DataRow dRow = dtResults.NewRow();
            dRow[Constants.SOProperties.URM.FQN] = identityFromName.FullyQualifiedName.FQN;
            dRow[Constants.SOProperties.URM.GroupName] = identityFromName.FullyQualifiedName.FullName;
            if (identityFromName.Properties.ContainsKey("Name") && identityFromName.Properties["Name"] != null)
            {
                dRow[Constants.SOProperties.URM.Name] = identityFromName.Properties["Name"].ToString();
            }
            if (identityFromName.Properties.ContainsKey("Description") && identityFromName.Properties["Description"] != null)
            {
                dRow[Constants.SOProperties.URM.Description] = identityFromName.Properties["Description"].ToString();
            }
            if (identityFromName.Properties.ContainsKey("Email") && !string.IsNullOrEmpty(identityFromName.Properties["Email"].ToString()))
            {
                dRow[Constants.SOProperties.URM.Email] = identityFromName.Properties["Email"].ToString();
            }

            dtResults.Rows.Add(dRow);
        }
        private void GetGroups()
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
                    RunUMGetGroups(ldap, net);
                });

                //This line of code is needed so that the received items are not filtered again by K2 internal filtering of SMO.
                ServiceBroker.Service.ServiceObjects[0].Methods[0].Filter = null;
            }
            else
            {

                // The below code is a copy/paste from reflected code with some modifications as we can't do anything else.
                string name = GetStringProperty(Constants.SOProperties.URM.Name);
                string email = GetStringProperty(Constants.SOProperties.URM.Email);
                string description = GetStringProperty(Constants.SOProperties.URM.Description);
                DataTable dtResults = ServiceBroker.ServicePackage.ResultTable;
                URMFilter urmFilter = new URMFilter(ServiceBroker.Service.ServiceObjects[0].Methods[0].Filter);

                foreach (var filterCollectionValues in urmFilter.GetFilterCollection().Values)
                {
                    foreach (var keyValuePair in filterCollectionValues)
                    {
                        switch (keyValuePair.Key)
                        {
                            case Constants.SOProperties.URM.Name:
                                name = keyValuePair.Value.Replace("'", "");
                                continue;
                            case Constants.SOProperties.URM.Description:
                                description = keyValuePair.Value.Replace("'", "");
                                continue;
                            case Constants.SOProperties.URM.Email:
                                email = keyValuePair.Value.Replace("'", "");
                                continue;
                            default:
                                continue;
                        }
                    }
                    Dictionary<string, object> dictionary2 = new Dictionary<string, object>()
                    {
                        {"Name", name},
                        {"Description", description},
                        {"Email", email}
                    };
                    if (!string.IsNullOrEmpty(securityLabel))
                    {
                        dictionary2["Label"] = securityLabel as object;
                    }
                    var identities = base.ServiceBroker.IdentityService.FindIdentities(dictionary2, IdentitySearchOptions.Groups);
                    if (identities == null)
                    {
                        return;
                    }

                    foreach (var cachedIdentity in identities)
                    {
                        if (cachedIdentity.Type == IdentityType.Group)
                        {
                            DataRow dRow = dtResults.NewRow();
                            dRow[Constants.SOProperties.URM.FQN] = cachedIdentity.FullyQualifiedName.FQN;
                            dRow[Constants.SOProperties.URM.GroupName] = cachedIdentity.FullyQualifiedName.FullName;
                            dRow[Constants.SOProperties.URM.Saml] = cachedIdentity.FullyQualifiedName.FullName;

                            if (cachedIdentity.Properties.ContainsKey("Name") && cachedIdentity.Properties["Name"] != null)
                            {
                                dRow[Constants.SOProperties.URM.Name] = cachedIdentity.Properties["Name"].ToString();
                            }
                            if (cachedIdentity.Properties.ContainsKey("Description") && cachedIdentity.Properties["Description"] != null)
                            {
                                dRow[Constants.SOProperties.URM.Description] = cachedIdentity.Properties["Description"].ToString();
                            }
                            if (cachedIdentity.Properties.ContainsKey("Email") && !string.IsNullOrEmpty(cachedIdentity.Properties["Email"].ToString()))
                            {
                                dRow[Constants.SOProperties.URM.Email] = cachedIdentity.Properties["Email"].ToString();
                            }
                            dtResults.Rows.Add(dRow);
                        }
                    }
                }
            }
        }

        private void RunUMGetGroups(string ldap, string net)
        {
            Dictionary<string, string> inputProperties = new Dictionary<string, string>()
            {
                {Constants.SOProperties.URM.FQN, GetStringProperty(Constants.SOProperties.URM.FQN)},
                {Constants.SOProperties.URM.Name, GetStringProperty(Constants.SOProperties.URM.Name)},
                {Constants.SOProperties.URM.Description, GetStringProperty(Constants.SOProperties.URM.Description)},
                {Constants.SOProperties.URM.Email, GetStringProperty(Constants.SOProperties.URM.Email)}
            };
            
            string securityLabel = GetStringParameter(Constants.SOProperties.URM.Label, true);
            var dSearcher = new DirectorySearcher(new DirectoryEntry(ldap));

            if (string.IsNullOrEmpty(securityLabel))
            {
                securityLabel = "K2";
            }
            
            dSearcher.Filter = LdapHelper.GetLdapQueryString(inputProperties, ServiceBroker.Service.ServiceObjects[0].Methods[0].Filter, IdentityType.Group, ChangeContainsToStartWith);
            dSearcher.PageSize = ADMaxResultSize;

            dSearcher.PropertiesToLoad.Add(AdProperties.sAMAccountName);
            dSearcher.PropertiesToLoad.Add(AdProperties.Name);
            dSearcher.PropertiesToLoad.Add(AdProperties.Email);
            dSearcher.PropertiesToLoad.Add(AdProperties.Description);

            SearchResultCollection col = dSearcher.FindAll();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;
            foreach (SearchResult res in col)
            {
                DataRow dr = results.NewRow();
                string saml = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.sAMAccountName);
                dr[Constants.SOProperties.URM.FQN] = string.Concat(securityLabel, ":", net, "\\", saml);
                dr[Constants.SOProperties.URM.Name] = string.Concat(net, "\\", saml);
                dr[Constants.SOProperties.URM.GroupName] = string.Concat(net, "\\", saml);
                dr[Constants.SOProperties.URM.Description] = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.Description);
                dr[Constants.SOProperties.URM.Email] = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.Email);
                dr[Constants.SOProperties.URM.Saml] = saml;

                lock (ServiceBroker.ServicePackage.ResultTable)
                {
                    results.Rows.Add(dr);
                }
            }
        }
    }
}