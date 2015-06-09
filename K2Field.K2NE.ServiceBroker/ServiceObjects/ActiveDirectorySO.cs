using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading;

namespace K2Field.K2NE.ServiceBroker
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
            public static string DistinguishedName = "DistinguishedName";
        }

        public override List<SourceCode.SmartObjects.Services.ServiceSDK.Objects.ServiceObject> DescribeServiceObjects()
        {
            ServiceObject soUser = Helper.CreateServiceObject("AD User", "Active Directory User.");

            soUser.Properties.Add(Helper.CreateProperty(Constants.Properties.ActiveDirectory.SubStringSearchInput, SoType.Text, "The string used to search for a specific value."));
            soUser.Properties.Add(Helper.CreateProperty(Constants.Properties.ActiveDirectory.UserFQN, SoType.Text, "The FQN username. Domain\\samlaccountname"));
            soUser.Properties.Add(Helper.CreateProperty(Constants.Properties.ActiveDirectory.SamAccountName, SoType.Text, "The SAM Account name"));
            soUser.Properties.Add(Helper.CreateProperty(Constants.Properties.ActiveDirectory.DisplayName, SoType.Text, "Display Name"));
            soUser.Properties.Add(Helper.CreateProperty(Constants.Properties.ActiveDirectory.CommonName, SoType.Text, "The common name of the object."));
            soUser.Properties.Add(Helper.CreateProperty(Constants.Properties.ActiveDirectory.GivenName, SoType.Text, "Given Name"));
            soUser.Properties.Add(Helper.CreateProperty(Constants.Properties.ActiveDirectory.Initials, SoType.Text, "Initials"));
            soUser.Properties.Add(Helper.CreateProperty(Constants.Properties.ActiveDirectory.Surname, SoType.Text, "Surname"));
            soUser.Properties.Add(Helper.CreateProperty(Constants.Properties.ActiveDirectory.Email, SoType.Text, "Email Address"));
            soUser.Properties.Add(Helper.CreateProperty(Constants.Properties.ActiveDirectory.MaxSearchResultSize, SoType.Number, "Override the default max result size per LDAP directory."));
            soUser.Properties.Add(Helper.CreateProperty(Constants.Properties.ActiveDirectory.OrganisationalUnit, SoType.Text, "OrganisationalUnit"));



            Method mGetUsers = Helper.CreateMethod(Constants.Methods.ActiveDirectory.GetUsers, "Get all users, filtered by exact matching of the input properties.", MethodType.List);
            mGetUsers.InputProperties.Add(Constants.Properties.ActiveDirectory.UserFQN);
            mGetUsers.InputProperties.Add(Constants.Properties.ActiveDirectory.DisplayName);
            mGetUsers.InputProperties.Add(Constants.Properties.ActiveDirectory.Email);
            mGetUsers.InputProperties.Add(Constants.Properties.ActiveDirectory.MaxSearchResultSize);
            mGetUsers.ReturnProperties.Add(Constants.Properties.ActiveDirectory.UserFQN);
            mGetUsers.ReturnProperties.Add(Constants.Properties.ActiveDirectory.SamAccountName);
            mGetUsers.ReturnProperties.Add(Constants.Properties.ActiveDirectory.DisplayName);
            mGetUsers.ReturnProperties.Add(Constants.Properties.ActiveDirectory.Email);
            soUser.Methods.Add(mGetUsers);


            Method mGetUserDetails = Helper.CreateMethod(Constants.Methods.ActiveDirectory.GetUserDetails, "Get all details for the users.", MethodType.Read);
            mGetUserDetails.InputProperties.Add(Constants.Properties.ActiveDirectory.UserFQN);
            mGetUserDetails.Validation.RequiredProperties.Add(Constants.Properties.ActiveDirectory.UserFQN);
            mGetUserDetails.ReturnProperties.Add(Constants.Properties.ActiveDirectory.SamAccountName);
            mGetUserDetails.ReturnProperties.Add(Constants.Properties.ActiveDirectory.DisplayName);
            mGetUserDetails.ReturnProperties.Add(Constants.Properties.ActiveDirectory.CommonName);
            mGetUserDetails.ReturnProperties.Add(Constants.Properties.ActiveDirectory.GivenName);
            mGetUserDetails.ReturnProperties.Add(Constants.Properties.ActiveDirectory.Initials);
            mGetUserDetails.ReturnProperties.Add(Constants.Properties.ActiveDirectory.Surname);
            mGetUserDetails.ReturnProperties.Add(Constants.Properties.ActiveDirectory.Email);
            mGetUserDetails.ReturnProperties.Add(Constants.Properties.ActiveDirectory.OrganisationalUnit);
            soUser.Methods.Add(mGetUserDetails);


            Method mSearchUser = Helper.CreateMethod(Constants.Methods.ActiveDirectory.SearchUsers, "Performs a StartWith query on DisplayName, SamlAccountName and E-mail.", MethodType.List);
            mSearchUser.InputProperties.Add(Constants.Properties.ActiveDirectory.SubStringSearchInput);
            mSearchUser.InputProperties.Add(Constants.Properties.ActiveDirectory.MaxSearchResultSize);
            mSearchUser.Validation.RequiredProperties.Add(Constants.Properties.ActiveDirectory.SubStringSearchInput);
            mSearchUser.ReturnProperties.Add(Constants.Properties.ActiveDirectory.UserFQN);
            mSearchUser.ReturnProperties.Add(Constants.Properties.ActiveDirectory.SamAccountName);
            mSearchUser.ReturnProperties.Add(Constants.Properties.ActiveDirectory.DisplayName);
            mSearchUser.ReturnProperties.Add(Constants.Properties.ActiveDirectory.Email);
            soUser.Methods.Add(mSearchUser);


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


        #region SearchUsers

        private void SearchUsers()
        {
            string[] ldaps = base.LDAPPath.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            int maxResultSet = base.GetIntProperty(Constants.Properties.ActiveDirectory.MaxSearchResultSize, false);


            string searchval = base.GetStringProperty(Constants.Properties.ActiveDirectory.SubStringSearchInput, true);
            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();

            List<Thread> threads = new List<Thread>();

            foreach (string ldap in ldaps)
            {
                Thread t = new Thread(() => RunSearchUser(ldap, maxResultSet, searchval));
                t.Start();
                threads.Add(t);
            }

            foreach (Thread t in threads)
            {
                t.Join();
            }
        }


        private void RunSearchUser(string ldap, int maxResultSet, string searchval)
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

            SearchResultCollection col = searcher.FindAll();


            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            foreach (SearchResult res in col)
            {


                DataRow dr = results.NewRow();

                string saml = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.SamlAccountName);
                dr[Constants.Properties.ActiveDirectory.UserFQN] = base.NetBiosName + "\\" + saml;
                dr[Constants.Properties.ActiveDirectory.SamAccountName] = saml;
                dr[Constants.Properties.ActiveDirectory.DisplayName] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.DisplayName);
                dr[Constants.Properties.ActiveDirectory.Email] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.Email);
                lock (base.ServiceBroker.ServicePackage.ResultTable)
                {
                    results.Rows.Add(dr);
                }
            }
        }
        #endregion SearchUsers

        #region GetUsers

        private void GetUsers()
        {
            string[] ldaps = base.LDAPPath.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            int maxResultSet = base.GetIntProperty(Constants.Properties.ActiveDirectory.MaxSearchResultSize, false);

            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();

            List<Thread> threads = new List<Thread>();

            foreach (string ldap in ldaps)
            {
                Thread t = new Thread(() => RunGetUsers(ldap, maxResultSet));
                t.Start();
                threads.Add(t);
            }

            foreach (Thread t in threads)
            {
                t.Join();
            }
        }



        private void RunGetUsers(string ldap, int maxResultSet)
        {
            DirectorySearcher searcher = new DirectorySearcher(GetDirectoryEntry(ldap));

            StringBuilder searchFilter = new StringBuilder();
            searchFilter.Append("(&");
            searchFilter.Append("(objectcategory=person)(objectclass=user)");


            string displayName = base.GetStringProperty(Constants.Properties.ActiveDirectory.DisplayName, false);
            if (!string.IsNullOrEmpty(displayName))
            {
                searchFilter.AppendFormat("({0}={1})", AdProperties.DisplayName, displayName);
            }

            string email = base.GetStringProperty(Constants.Properties.ActiveDirectory.Email, false);
            if (!string.IsNullOrEmpty(email))
            {
                searchFilter.AppendFormat("({0}={1})", AdProperties.Email, email);
            }

            string userfqn = base.GetStringProperty(Constants.Properties.ActiveDirectory.UserFQN, false);
            if (!string.IsNullOrEmpty(userfqn))
            {
                string samlaccountname = userfqn.Substring(userfqn.IndexOf('\\') + 1);
                searchFilter.AppendFormat("({0}={1})", AdProperties.SamlAccountName, samlaccountname);
            }

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

            SearchResultCollection col = searcher.FindAll();


            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            foreach (SearchResult res in col)
            {

                DataRow dr = results.NewRow();

                string saml = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.SamlAccountName);
                dr[Constants.Properties.ActiveDirectory.UserFQN] = base.NetBiosName + "\\" + saml;
                dr[Constants.Properties.ActiveDirectory.SamAccountName] = saml;
                dr[Constants.Properties.ActiveDirectory.DisplayName] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.DisplayName);
                dr[Constants.Properties.ActiveDirectory.Email] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.Email);
                lock (base.ServiceBroker.ServicePackage.ResultTable)
                {
                    results.Rows.Add(dr);
                }
            }
        }

        #endregion GetUsers

        #region GetUserDetails

        private void GetUserDetails()
        {
            string userfqn = base.GetStringProperty(Constants.Properties.ActiveDirectory.UserFQN, true);
            string samlaccountname = userfqn.Substring(userfqn.IndexOf('\\') + 1);

            string[] ldaps = base.LDAPPath.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

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

                    dr[Constants.Properties.ActiveDirectory.SamAccountName] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.SamlAccountName);
                    dr[Constants.Properties.ActiveDirectory.DisplayName] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.DisplayName);
                    dr[Constants.Properties.ActiveDirectory.CommonName] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.CommonName);
                    dr[Constants.Properties.ActiveDirectory.GivenName] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.GivenName);
                    dr[Constants.Properties.ActiveDirectory.Initials] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.Initials);
                    dr[Constants.Properties.ActiveDirectory.Surname] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.Surname);
                    dr[Constants.Properties.ActiveDirectory.Email] = GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.Email);
                    dr[Constants.Properties.ActiveDirectory.OrganisationalUnit] = GetOUFromDistinguishedName(GetSingleStringPropertyCollectionValue(res.Properties, AdProperties.DistinguishedName));

                    results.Rows.Add(dr);
                    break; // there can be only one as this is a read method.
                }

            }


            #endregion GetUserDetails

        }
    }
}
