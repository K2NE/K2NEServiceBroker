using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using K2Field.K2NE.ServiceBroker.Helpers;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects
{
    public class ActiveDirectorySO : ServiceObjectBase
    {
        public ActiveDirectorySO(K2NEServiceBroker api) : base(api) { }

        private class AdProperties
        {
            public const string SamlAccountName = "sAMAccountName";
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

        public override List<ServiceObject> DescribeServiceObjects()
        {
            string[] ldaps = base.LDAPPaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string[] netbios = base.NetBiosNames.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (ldaps.Length != netbios.Length)
            {
                throw new ArgumentException("The LDAP paths and NetBioses count do not match.");
            }

            ServiceObject soUser = Helper.CreateServiceObject("AD User", "Active Directory User.");
            soUser.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ActiveDirectory.UserFQN, SoType.Text, "The FQN username. Domain\\samlaccountname"));
            soUser.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ActiveDirectory.SubStringSearchInput, SoType.Text, "The string used to search for a specific value."));
            soUser.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ActiveDirectory.SamAccountName, SoType.Text, "The SAM Account name"));
            soUser.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ActiveDirectory.Name, SoType.Text, "The name of the user"));
            soUser.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ActiveDirectory.Description, SoType.Text, "Description"));
            soUser.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ActiveDirectory.Email, SoType.Text, "Email Address"));
            soUser.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ActiveDirectory.Manager, SoType.Text, "The Manager"));
            soUser.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ActiveDirectory.ObjectSID, SoType.Text, "An objectSID"));
            soUser.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ActiveDirectory.DisplayName, SoType.Text, "Display Name"));
            soUser.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ActiveDirectory.CommonName, SoType.Text, "The common name of the object."));
            soUser.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ActiveDirectory.GivenName, SoType.Text, "Given Name"));
            soUser.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ActiveDirectory.Initials, SoType.Text, "Initials"));
            soUser.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ActiveDirectory.Surname, SoType.Text, "Surname"));
            soUser.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ActiveDirectory.MaxSearchResultSize, SoType.Number, "Override the default max result size per LDAP directory."));
            soUser.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ActiveDirectory.OrganisationalUnit, SoType.Text, "OrganisationalUnit"));
            foreach (string prop in AdditionalADProps)
            {
                soUser.Properties.Add(Helper.CreateProperty(prop, SoType.Text, prop));
            }

            Method mGetUsers = Helper.CreateMethod(Constants.Methods.ActiveDirectory.GetUsers, "Get all users, filtered by exact matching of the input properties.", MethodType.List);
            mGetUsers.InputProperties.Add(Constants.SOProperties.ActiveDirectory.UserFQN);
            mGetUsers.InputProperties.Add(Constants.SOProperties.ActiveDirectory.DisplayName);
            mGetUsers.InputProperties.Add(Constants.SOProperties.ActiveDirectory.Email);
            mGetUsers.InputProperties.Add(Constants.SOProperties.ActiveDirectory.MaxSearchResultSize);
            mGetUsers.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.UserFQN);
            mGetUsers.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.SamAccountName);
            mGetUsers.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.DisplayName);
            mGetUsers.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.Email);
            foreach (string prop in AdditionalADProps)
            {
                mGetUsers.ReturnProperties.Add(prop);
            }
            soUser.Methods.Add(mGetUsers);

            Method mGetUserDetails = Helper.CreateMethod(Constants.Methods.ActiveDirectory.GetUserDetails, "Get all details for the users.", MethodType.Read);
            mGetUserDetails.InputProperties.Add(Constants.SOProperties.ActiveDirectory.UserFQN);
            mGetUserDetails.Validation.RequiredProperties.Add(Constants.SOProperties.ActiveDirectory.UserFQN);
            mGetUserDetails.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.SamAccountName);
            mGetUserDetails.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.DisplayName);
            mGetUserDetails.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.CommonName);
            mGetUserDetails.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.GivenName);
            mGetUserDetails.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.Initials);
            mGetUserDetails.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.Surname);
            mGetUserDetails.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.Email);
            mGetUserDetails.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.OrganisationalUnit);
            foreach (string prop in AdditionalADProps)
            {
                mGetUserDetails.ReturnProperties.Add(prop);
            }
            soUser.Methods.Add(mGetUserDetails);

            Method mSearchUser = Helper.CreateMethod(Constants.Methods.ActiveDirectory.SearchUsers, "Performs a StartWith query on DisplayName, SamlAccountName and E-mail.", MethodType.List);
            mSearchUser.InputProperties.Add(Constants.SOProperties.ActiveDirectory.SubStringSearchInput);
            mSearchUser.InputProperties.Add(Constants.SOProperties.ActiveDirectory.MaxSearchResultSize);
            mSearchUser.Validation.RequiredProperties.Add(Constants.SOProperties.ActiveDirectory.SubStringSearchInput);
            mSearchUser.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.UserFQN);
            mSearchUser.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.SamAccountName);
            mSearchUser.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.DisplayName);
            mSearchUser.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.Email);
            soUser.Methods.Add(mSearchUser);

            Method mUMGetUsers = Helper.CreateMethod(Constants.Methods.ActiveDirectory.UMGetUsers, "Performs a search using FILTERs provided to the SMO.", MethodType.List);
            mUMGetUsers.InputProperties.Add(Constants.SOProperties.ActiveDirectory.Name);
            mUMGetUsers.InputProperties.Add(Constants.SOProperties.ActiveDirectory.Description);
            mUMGetUsers.InputProperties.Add(Constants.SOProperties.ActiveDirectory.Email);
            mUMGetUsers.InputProperties.Add(Constants.SOProperties.ActiveDirectory.Manager);
            mUMGetUsers.InputProperties.Add(Constants.SOProperties.ActiveDirectory.DisplayName);
            mUMGetUsers.InputProperties.Add(Constants.SOProperties.ActiveDirectory.MaxSearchResultSize);
            mUMGetUsers.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.UserFQN);
            mUMGetUsers.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.Name);
            mUMGetUsers.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.Description);
            mUMGetUsers.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.DisplayName);
            mUMGetUsers.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.Email);
            mUMGetUsers.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.Manager);
            mUMGetUsers.ReturnProperties.Add(Constants.SOProperties.ActiveDirectory.ObjectSID);


            MethodParameter methParm = new MethodParameter();
            methParm.IsRequired = true;
            methParm.MetaData = new SourceCode.SmartObjects.Services.ServiceSDK.Objects.MetaData();
            methParm.MetaData.Description = Constants.SOProperties.ActiveDirectory.Label;
            methParm.MetaData.DisplayName = Constants.SOProperties.ActiveDirectory.Label;
            methParm.Name = Constants.SOProperties.ActiveDirectory.Label;
            methParm.SoType = SoType.Text;
            methParm.Type = MapHelper.GetTypeBySoType(methParm.SoType);

            mUMGetUsers.MethodParameters.Add(methParm);
            soUser.Methods.Add(mUMGetUsers);

            return new List<ServiceObject>() { soUser };
        }

        public override void Execute()
        {

            switch (ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.ActiveDirectory.GetUserDetails:
                    GetUserDetails();
                    break;
                case Constants.Methods.ActiveDirectory.GetUsers:
                    GetUsers();
                    break;
                case Constants.Methods.ActiveDirectory.SearchUsers:
                    SearchUsers();
                    break;
                case Constants.Methods.ActiveDirectory.UMGetUsers:
                    UMGetUsers();
                    break;
            }
        }

        private string EscapeSearchFilter(string filter)
        {
            string text = filter;

            text = text.TrimStart('*');
            text = text.TrimEnd('*');

            text = EscapeFilterChars(text);

            if (filter.StartsWith("*"))
            {
                text = "*" + text;
            }
            if (filter.EndsWith("*"))
            {
                text = text + "*";
            }

            return text;
        }

        public string EscapeFilterChars(string text)
        {
            text = text.Replace("\\", "\\5C");
            text = text.Replace("*", "\\2A");
            text = text.Replace("(", "\\28");
            text = text.Replace(")", "\\29");
            text = text.Replace("NUL", "\\00");
            return text;
        }

        private string GetOUFromDistinguishedName(string distinguisedName)
        {
            string OU = getADStringItem(distinguisedName, "OU", 0, ",");
            return OU;

        }

        /// <summary>
        /// Returns all values in a given AD string matching a given lookup e.g OU separted by a specified delimiter
        /// </summary>
        /// <param name="adString">e.g OU=Leavers,OU=Left,DC=denallix,DC=com</param>
        /// <param name="ADIndentifier">entry to get ie. OU</param>
        /// <param name="StartPos">position in the string to start-set to 0 to look through entire string</param>
        /// <param name="RequiredDelimiter"dlimiter to seprate values should there be multiple entries ie. a ,></param>
        /// <returns>string of given values delimited as specified</returns>
        string getADStringItem(string adString, string ADIndentifier, int StartPos, string RequiredDelimiter)
        {
            //This method enables the lookup of items from an ad string e.g OU=Home,DC=com
            string result = null;
            if ((string.IsNullOrEmpty(adString)) | (string.IsNullOrEmpty(adString))) { return result; }

            int sPos, ePos;
            sPos = adString.IndexOf(ADIndentifier + "=", StartPos);
            if (sPos > -1)
            {
                sPos += ADIndentifier.Length + 1;
                ePos = adString.IndexOf(",", sPos);
                if (ePos > sPos)
                {
                    //Found more parameters therefore look again
                    result = adString.Substring(sPos, ePos - sPos);
                    string nextResult = getADStringItem(adString.Substring(sPos), ADIndentifier, StartPos, RequiredDelimiter);
                    if (string.IsNullOrEmpty(nextResult) == false)
                    {
                        result = result + RequiredDelimiter + nextResult;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(result) == true)
                    {
                        result = adString.Substring(sPos);
                    }
                    else
                    {
                        result = result + RequiredDelimiter + adString.Substring(sPos);
                    }
                }
            }
            return result;
        }

        private static string GetObjectSid(System.DirectoryServices.ResultPropertyCollection props, string name)
        {
            if (!props.Contains(name))
            {
                return string.Empty;
            }

            ResultPropertyValueCollection pvc = props[name];
            if (pvc == null || pvc.Count == 0)
            {
                return string.Empty;
            }

            byte[] sidInBytes = (byte[])pvc[0];
            SecurityIdentifier sid = new SecurityIdentifier(sidInBytes, 0);
            return sid.ToString();
        }

        private static string GetSingleStringPropertyCollectionValue(System.DirectoryServices.ResultPropertyCollection props, string name)
        {
            if (!props.Contains(name))
            {
                return string.Empty;
            }

            ResultPropertyValueCollection pvc = props[name];
            if (pvc == null || pvc.Count == 0)
            {
                return string.Empty;
            }
            return pvc[0] as string;
        }

        private static string GetSingleStringPropertyCollectionValue(System.DirectoryServices.PropertyCollection props, string name)
        {
            if (!props.Contains(name))
            {
                return string.Empty;
            }

            PropertyValueCollection pvc = props[name];
            if (pvc == null || pvc.Count == 0)
            {
                return string.Empty;
            }
            return pvc[0] as string;
        }

        private DirectoryEntry GetDirectoryEntry(string ldap)
        {
            return new DirectoryEntry(ldap);
        }

        #region PickUser

        //.. Example SqlFilter strings
        //.. ((sAMAccountName like '%bob%' or displayName like '%bob%') or mail like '%bob%')
        //.. ((sAMAccountName like '%(bob)%' or displayName like '%(bob)%') or mail like '%(bob)%')
        public Dictionary<string, string> GetFilters()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>();
            if (base.ServiceBroker.Service.ServiceObjects[0].Methods[0].SqlFilter == null)
            {
                return ret;
            }

            string[] sepAndOr = new string[2] { " and ", " or " };
            string[] sepLikeEqual = new string[2] { " like ", "=" };

            string sqlFilter = base.ServiceBroker.Service.ServiceObjects[0].Methods[0].SqlFilter.ToString();

            foreach (string str in sqlFilter.Split(sepAndOr, StringSplitOptions.RemoveEmptyEntries))
            {
                string[] strArray = str.Split(sepLikeEqual, StringSplitOptions.RemoveEmptyEntries);

                string TmpValue = strArray[1].Replace("%", "*");
                int firstQuoteIndex = TmpValue.IndexOf("'");
                int LastQuoteIndex = TmpValue.LastIndexOf("'");
                TmpValue = TmpValue.Substring(firstQuoteIndex + 1, LastQuoteIndex - firstQuoteIndex - 1).Trim();

                //This is for performance reason, yes - we simply ignore the leading item for performance as the OOF picker always does a contains which is just damn slow on AD.
                TmpValue = TmpValue.TrimStart('*');

                string propName = strArray[0].Replace("(", "").Replace(")", "").Trim();
                ret.Add(propName, TmpValue);
            }

            return ret;

        }

        public string getADUserLookUpStringWithPickerFilters()
        {
            StringBuilder ADFiltersLogicalOr = new StringBuilder();


            StringBuilder searchFilter = new StringBuilder();
            searchFilter.Append("(&");
            searchFilter.Append("(objectcategory=person)(objectclass=user)");

            string displayName = base.GetStringProperty(Constants.SOProperties.ActiveDirectory.DisplayName, false);
            string email = base.GetStringProperty(Constants.SOProperties.ActiveDirectory.Email, false);
            string userfqn = base.GetStringProperty(Constants.SOProperties.ActiveDirectory.UserFQN, false);
            string name = base.GetStringProperty(Constants.SOProperties.ActiveDirectory.Name, false);

            if (!string.IsNullOrEmpty(displayName))
            {
                searchFilter.AppendFormat("({0}={1})", AdProperties.DisplayName, displayName);
            }

            if (!string.IsNullOrEmpty(email))
            {
                searchFilter.AppendFormat("({0}={1})", AdProperties.Email, email);
            }

            string saml = string.Empty;
            if (!string.IsNullOrEmpty(userfqn))
            {
                saml = userfqn.Substring(userfqn.IndexOf('\\') + 1);
            }
            if (!string.IsNullOrEmpty(name))
            {
                saml = name.Substring(name.IndexOf('\\') + 1);
            }
            if (!string.IsNullOrEmpty(saml))
            {
                searchFilter.AppendFormat("({0}={1})", AdProperties.SamlAccountName, saml);
            }

            Dictionary<string, string> filters = GetFilters();
            if (filters.Count() > 0)
            {
                foreach (KeyValuePair<string, string> Filter in filters)
                {
                    ADFiltersLogicalOr.AppendFormat("({0}={1})", Filter.Key, Filter.Value);
                }

                if (filters.Count() > 1)
                {
                    ADFiltersLogicalOr.Insert(0, "(|");
                    ADFiltersLogicalOr.Append(")");
                }
                searchFilter.Append(ADFiltersLogicalOr.ToString());

            }
            searchFilter.Append(")");

            return searchFilter.ToString();
        }

        private void UMGetUsers()
        {
            string[] ldaps = base.LDAPPaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string[] netbioses = base.NetBiosNames.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            int maxResultSet = base.GetIntProperty(Constants.SOProperties.ActiveDirectory.MaxSearchResultSize, false);

            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();

            List<Thread> threads = new List<Thread>();

            string ldap, net;
            Parallel.For(0, ldaps.Length, i =>
            {
                ldap = ldaps[i];
                net = netbioses[i];
                RunUMGetUsers(ldap, net, maxResultSet);
            });
            //This line of code is needed so that the received items are not filtered again by K2 internal filtering of SMO.
            ServiceBroker.Service.ServiceObjects[0].Methods[0].Filter = null;
        }

        private void RunUMGetUsers(string ldap, string netbios, int maxResultSet)
        {
            try
            {
                DirectorySearcher searcher = new DirectorySearcher(GetDirectoryEntry(ldap));

                string label = base.GetStringParameter(Constants.SOProperties.ActiveDirectory.Label, true);

                if (string.IsNullOrEmpty(label))
                {
                    label = "K2";
                }

                searcher.Filter = getADUserLookUpStringWithPickerFilters();

                if (maxResultSet == 0)
                {
                    searcher.PageSize = base.ADMaxResultSize;
                }
                else
                {
                    searcher.PageSize = maxResultSet;
                }

                searcher.PropertiesToLoad.Add(AdProperties.SamlAccountName);
                searcher.PropertiesToLoad.Add(AdProperties.Name);
                searcher.PropertiesToLoad.Add(AdProperties.DisplayName);
                searcher.PropertiesToLoad.Add(AdProperties.Email);
                searcher.PropertiesToLoad.Add(AdProperties.Description);
                searcher.PropertiesToLoad.Add(AdProperties.Manager);
                searcher.PropertiesToLoad.Add(AdProperties.ObjectSID);

                DataRow dr;
                string saml;
                SearchResultCollection col = searcher.FindAll();
                DataTable results = base.ServiceBroker.ServicePackage.ResultTable;
                foreach (SearchResult res in col)
                {
                    dr = results.NewRow();
                    saml = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.SamlAccountName);
                    dr[Constants.SOProperties.ActiveDirectory.UserFQN] = string.Concat(label, ":", netbios, "\\", saml);
                    dr[Constants.SOProperties.ActiveDirectory.Name] = string.Concat(netbios, "\\", saml);
                    dr[Constants.SOProperties.ActiveDirectory.Description] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.Description);
                    dr[Constants.SOProperties.ActiveDirectory.DisplayName] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.DisplayName);
                    dr[Constants.SOProperties.ActiveDirectory.Email] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.Email);
                    dr[Constants.SOProperties.ActiveDirectory.Manager] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.Manager);
                    dr[Constants.SOProperties.ActiveDirectory.ObjectSID] = GetObjectSid(res.Properties, AdProperties.ObjectSID);

                    lock (base.ServiceBroker.ServicePackage.ResultTable)
                    {
                        results.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Failed to query {0}.", ldap), ex);
            }
        }

        #endregion PickUser

        #region SearchUsers

        private void SearchUsers()
        {
            string[] ldaps = base.LDAPPaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string[] netbioses = base.NetBiosNames.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            int maxResultSet = base.GetIntProperty(Constants.SOProperties.ActiveDirectory.MaxSearchResultSize, false);

            string searchval = base.GetStringProperty(Constants.SOProperties.ActiveDirectory.SubStringSearchInput, true);
            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();

            Parallel.For(0, ldaps.Length, i =>
            {
                string ldap = ldaps[i];
                string net = netbioses[i];
                RunSearchUser(ldap, net, maxResultSet, searchval);
            });
        }

        private void RunSearchUser(string ldap, string netbios, int maxResultSet, string searchval)
        {
            try
            {
                DirectorySearcher searcher = new DirectorySearcher(GetDirectoryEntry(ldap));

                searcher.Filter = string.Format("(&(objectcategory=person)(objectclass=user)(|(sAMAccountName={0}*)(displayName={0}*)(mail={0}*)))", searchval);
                if (maxResultSet == 0)
                {
                    searcher.SizeLimit = base.ADMaxResultSize;
                }
                else
                {
                    searcher.SizeLimit = maxResultSet;
                }
                searcher.PropertiesToLoad.Add(AdProperties.SamlAccountName);
                searcher.PropertiesToLoad.Add(AdProperties.DisplayName);
                searcher.PropertiesToLoad.Add(AdProperties.Email);
                foreach (string prop in AdditionalADProps)
                {
                    searcher.PropertiesToLoad.Add(prop);
                }

                SearchResultCollection col = searcher.FindAll();

                DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

                DataRow dr;
                string saml;
                foreach (SearchResult res in col)
                {
                    dr = results.NewRow();

                    saml = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.SamlAccountName);
                    dr[Constants.SOProperties.ActiveDirectory.UserFQN] = string.Concat(netbios, "\\", saml);
                    dr[Constants.SOProperties.ActiveDirectory.SamAccountName] = saml;
                    dr[Constants.SOProperties.ActiveDirectory.DisplayName] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.DisplayName);
                    dr[Constants.SOProperties.ActiveDirectory.Email] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.Email);
                    foreach (string prop in AdditionalADProps)
                    {
                        dr[prop] = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, prop);
                    }
                    lock (base.ServiceBroker.ServicePackage.ResultTable)
                    {
                        results.Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Failed to query {0}", ldap), ex);
            }
        }
        #endregion SearchUsers

        #region GetUsers

        private void GetUsers()
        {
            string[] ldaps = base.LDAPPaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            string[] netbioses = base.NetBiosNames.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            int maxResultSet = base.GetIntProperty(Constants.SOProperties.ActiveDirectory.MaxSearchResultSize, false);

            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();


            Parallel.For(0, ldaps.Length, i =>
            {
                string ldap = ldaps[i];
                string net = netbioses[i];
                RunGetUsers(ldap, net, maxResultSet);
            });
        }

        private void RunGetUsers(string ldap, string netbios, int maxResultSet)
        {
            try
            {
                {
                    DirectorySearcher searcher = new DirectorySearcher(GetDirectoryEntry(ldap));

                    StringBuilder searchFilter = new StringBuilder();
                    searchFilter.Append("(&");
                    searchFilter.Append("(objectcategory=person)(objectclass=user)");

                    string displayName = base.GetStringProperty(Constants.SOProperties.ActiveDirectory.DisplayName, false);
                    string email = base.GetStringProperty(Constants.SOProperties.ActiveDirectory.Email, false);
                    string userfqn = base.GetStringProperty(Constants.SOProperties.ActiveDirectory.UserFQN, false);
                    if (!string.IsNullOrEmpty(displayName)) { searchFilter.AppendFormat("({0}={1})", AdProperties.DisplayName, displayName); }
                    if (!string.IsNullOrEmpty(email)) { searchFilter.AppendFormat("({0}={1})", AdProperties.Email, email); }
                    if (!string.IsNullOrEmpty(userfqn)) { searchFilter.AppendFormat("({0}={1})", AdProperties.SamlAccountName, userfqn.Substring(userfqn.IndexOf('\\') + 1)); }

                    searchFilter.Append(")");
                    searcher.Filter = searchFilter.ToString();

                    if (maxResultSet == 0)
                    {
                        searcher.SizeLimit = base.ADMaxResultSize;
                    }
                    else
                    {
                        searcher.SizeLimit = maxResultSet;
                    }

                    searcher.PropertiesToLoad.Add(AdProperties.SamlAccountName);
                    searcher.PropertiesToLoad.Add(AdProperties.DisplayName);
                    searcher.PropertiesToLoad.Add(AdProperties.Email);
                    foreach (string prop in AdditionalADProps)
                    {
                        searcher.PropertiesToLoad.Add(prop);
                    }

                    DataRow dr;
                    string saml;
                    SearchResultCollection col = searcher.FindAll();
                    DataTable results = base.ServiceBroker.ServicePackage.ResultTable;
                    foreach (SearchResult res in col)
                    {
                        dr = results.NewRow();
                        saml = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.SamlAccountName);
                        dr[Constants.SOProperties.ActiveDirectory.UserFQN] = string.Concat(netbios, "\\", saml);
                        dr[Constants.SOProperties.ActiveDirectory.SamAccountName] = saml;
                        dr[Constants.SOProperties.ActiveDirectory.DisplayName] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.DisplayName);
                        dr[Constants.SOProperties.ActiveDirectory.Email] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.Email);
                        foreach (string prop in AdditionalADProps)
                        {
                            dr[prop] = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, prop);
                        }
                        lock (base.ServiceBroker.ServicePackage.ResultTable)
                        {
                            results.Rows.Add(dr);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format("Failed to query {0}", ldap), ex);
            }
        }

        #endregion GetUsers

        #region GetUserDetails

        private void GetUserDetails()
        {
            string userfqn = base.GetStringProperty(Constants.SOProperties.ActiveDirectory.UserFQN, true);
            string samlaccountname = userfqn.Substring(userfqn.IndexOf('\\') + 1);

            string[] ldaps = base.LDAPPaths.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string ldap in ldaps)
            {
                DirectorySearcher searcher = new DirectorySearcher(GetDirectoryEntry(ldap));
                searcher.Filter = string.Format("(&(objectcategory=person)(objectclass=user)(sAMAccountName={0}))", EscapeSearchFilter(samlaccountname));
                searcher.PageSize = base.ADMaxResultSize;

                SearchResult res = searcher.FindOne();
                if (res != null)
                {
                    ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
                    serviceObject.Properties.InitResultTable();
                    DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

                    DataRow dr = results.NewRow();

                    dr[Constants.SOProperties.ActiveDirectory.SamAccountName] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.SamlAccountName);
                    dr[Constants.SOProperties.ActiveDirectory.DisplayName] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.DisplayName);
                    dr[Constants.SOProperties.ActiveDirectory.CommonName] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.CommonName);
                    dr[Constants.SOProperties.ActiveDirectory.GivenName] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.GivenName);
                    dr[Constants.SOProperties.ActiveDirectory.Initials] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.Initials);
                    dr[Constants.SOProperties.ActiveDirectory.Surname] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.Surname);
                    dr[Constants.SOProperties.ActiveDirectory.Email] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.Email);
                    dr[Constants.SOProperties.ActiveDirectory.OrganisationalUnit] = GetOUFromDistinguishedName(GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.DistinguishedName));
                    foreach (string prop in AdditionalADProps)
                    {
                        dr[prop] = LdapHelper.GetSingleStringPropertyCollectionValue(res.Properties, prop);
                    }

                    results.Rows.Add(dr);
                    break; // there can be only one as this is a read method.
                }
            }


        #endregion GetUserDetails

        }
    }
}
