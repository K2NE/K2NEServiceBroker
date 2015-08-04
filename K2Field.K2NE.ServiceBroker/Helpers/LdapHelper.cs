using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Security.Principal;
using System.Text;
using System.Xml;
using SourceCode.Hosting.Server.Interfaces;

namespace K2Field.K2NE.ServiceBroker.Helpers
{
    public class LdapHelper
    {

        #region Private Helper Methods
        /// <summary>
        /// Converts SMO properties to LDAP ones & changes the values in order to be suitable for LDAP.
        /// </summary>
        /// <param name="smoProp">Property Name</param>
        /// <param name="smoValue">Property Value</param>
        /// <returns>Property name + Value</returns>
        private static FilterItem ConvertSmoToLdapFilter(string smoProp, string smoValue)
        {
            FilterItem filter = new FilterItem();
            if (String.IsNullOrEmpty(smoProp))
            {
                return filter;
            }


            switch (smoProp)
            {
                case Constants.SOProperties.URM.FQN:
                    filter.Prop = Constants.Properties.AdProperties.sAMAccountName;
                    filter.Value = smoValue.Substring(smoValue.IndexOf('\\') + 1);
                    break;
                case Constants.SOProperties.URM.Name:
                    filter.Prop = Constants.Properties.AdProperties.sAMAccountName;
                    filter.Value = smoValue.Substring(smoValue.IndexOf('\\') + 1);
                    break;
                case Constants.SOProperties.URM.Email:
                    filter.Prop = Constants.Properties.AdProperties.Email;
                    filter.Value = smoValue;
                    break;
                case Constants.SOProperties.URM.Description:
                    filter.Prop = Constants.Properties.AdProperties.Description;
                    filter.Value = smoValue;
                    break;
                case Constants.SOProperties.URM.Manager:
                    filter.Prop = Constants.Properties.AdProperties.Manager;
                    filter.Value = smoValue;
                    break;
                case Constants.SOProperties.URM.DisplayName:
                    filter.Prop = Constants.Properties.AdProperties.DisplayName;
                    filter.Value = smoValue;
                    break;
                case Constants.SOProperties.URM.UserName:
                    filter.Prop = Constants.Properties.AdProperties.sAMAccountName;
                    filter.Value = smoValue;
                    break;
                case Constants.SOProperties.URM.ObjectSid:
                    filter.Prop = Constants.Properties.AdProperties.ObjectSID;
                    filter.Value = smoValue;
                    break;
                case Constants.SOProperties.URM.Saml:
                    filter.Prop = Constants.Properties.AdProperties.sAMAccountName;
                    filter.Value = smoValue;
                    break;
                case Constants.SOProperties.URM.GroupName:
                    filter.Prop = Constants.Properties.AdProperties.sAMAccountName;
                    filter.Value = smoValue.Substring(smoValue.IndexOf('\\') + 1);
                    break;
            }
            return filter;
        }
        /// <summary>
        /// Converts K2 xml filters to LDAP ones.
        /// </summary>
        /// <param name="k2XmlFilter">SMO method filter</param>
        /// <param name="changeContainsToStartsWith">If needed to change Contains operator to StartsWith for AD performance</param>
        /// <param name="previousOperator"></param>
        /// <returns></returns>
        private static string ConvertXMLFilterToLdapFilter(string k2XmlFilter, bool changeContainsToStartsWith, string previousOperator = "")
        {
            StringBuilder filterStringBuilder = new StringBuilder();
            if (string.IsNullOrEmpty(k2XmlFilter))
            {
                return string.Empty;
            }
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(k2XmlFilter);
            string nextOperator = xmlDocument.FirstChild.FirstChild.Name;

            if (nextOperator != previousOperator && nextOperator == Constants.StringFormats.LdapOperators.Or)
            {
                filterStringBuilder.Append("(|");
            }

            if (nextOperator != previousOperator && nextOperator == Constants.StringFormats.LdapOperators.And)
            {
                filterStringBuilder.Append("(&");
            }

            if (xmlDocument.FirstChild.FirstChild.ChildNodes.Count > 1 && (nextOperator == Constants.StringFormats.LdapOperators.And || nextOperator == Constants.StringFormats.LdapOperators.Or))
            {
                foreach (XmlNode childNode in xmlDocument.FirstChild.FirstChild.ChildNodes)
                {
                    filterStringBuilder.Append(ConvertXMLFilterToLdapFilter(childNode.OuterXml, changeContainsToStartsWith, nextOperator));
                }
            }
            else
            {
                filterStringBuilder.Append(GetLdapFilterPart(xmlDocument.FirstChild.InnerXml, changeContainsToStartsWith));
            }

            if (nextOperator != previousOperator && (nextOperator == Constants.StringFormats.LdapOperators.Or || nextOperator == Constants.StringFormats.LdapOperators.And))
            {
                filterStringBuilder.Append(")");
            }

            return filterStringBuilder.ToString();
        }
        /// <summary>
        /// Converts 1 K2 xml filter to ldap one.
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="changeContainsToStartsWith"></param>
        /// <returns></returns>
        private static string GetLdapFilterPart(string xml, bool changeContainsToStartsWith = false)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return string.Empty;
            }

            StringBuilder filterStringBuilder = new StringBuilder();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            string nextOperator = xmlDocument.FirstChild.Name;

            FilterItem filterItem;
            switch (nextOperator)
            {
                case Constants.StringFormats.LdapOperators.StartsWith:
                    filterItem = ConvertSmoToLdapFilter(xmlDocument.FirstChild.ChildNodes[0].ChildNodes[0].Attributes["name"].Value, xmlDocument.FirstChild.ChildNodes[1].ChildNodes[0].InnerText);
                    filterStringBuilder.AppendFormat(Constants.StringFormats.LdapCompareFormat.StartsWith, filterItem.Prop, filterItem.Value);
                    break;
                case Constants.StringFormats.LdapOperators.EndsWith:
                    filterItem = ConvertSmoToLdapFilter(xmlDocument.FirstChild.ChildNodes[0].ChildNodes[0].Attributes["name"].Value, xmlDocument.FirstChild.ChildNodes[1].ChildNodes[0].InnerText);
                    filterStringBuilder.AppendFormat(Constants.StringFormats.LdapCompareFormat.EndsWith, filterItem.Prop, filterItem.Value);
                    break;
                case Constants.StringFormats.LdapOperators.Contains:
                    filterItem = ConvertSmoToLdapFilter(xmlDocument.FirstChild.ChildNodes[0].ChildNodes[0].Attributes["name"].Value, xmlDocument.FirstChild.ChildNodes[1].ChildNodes[0].InnerText);
                    if (changeContainsToStartsWith)
                    {
                        filterStringBuilder.AppendFormat(Constants.StringFormats.LdapCompareFormat.StartsWith, filterItem.Prop, filterItem.Value);
                    }
                    else
                    {
                        filterStringBuilder.AppendFormat(Constants.StringFormats.LdapCompareFormat.Contains, filterItem.Prop, filterItem.Value);
                    }
                    break;
                case Constants.StringFormats.LdapOperators.Equal:
                    filterItem = ConvertSmoToLdapFilter(xmlDocument.FirstChild.ChildNodes[0].ChildNodes[0].Attributes["name"].Value, xmlDocument.FirstChild.ChildNodes[1].ChildNodes[0].InnerText);
                    filterStringBuilder.AppendFormat(Constants.StringFormats.LdapCompareFormat.Equal, filterItem.Prop, filterItem.Value);
                    break;
                case Constants.StringFormats.LdapOperators.GreaterThan:
                    filterItem = ConvertSmoToLdapFilter(xmlDocument.FirstChild.ChildNodes[0].ChildNodes[0].Attributes["name"].Value, xmlDocument.FirstChild.ChildNodes[1].ChildNodes[0].InnerText);
                    filterStringBuilder.AppendFormat(Constants.StringFormats.LdapCompareFormat.GreaterThan, filterItem.Prop, filterItem.Value);
                    break;
                case Constants.StringFormats.LdapOperators.LessThan:
                    filterItem =
                    ConvertSmoToLdapFilter(xmlDocument.FirstChild.ChildNodes[0].ChildNodes[0].Attributes["name"].Value, xmlDocument.FirstChild.ChildNodes[1].ChildNodes[0].InnerText);
                    filterStringBuilder.AppendFormat(Constants.StringFormats.LdapCompareFormat.LessThan, filterItem.Prop, filterItem.Value);
                    break;
                case Constants.StringFormats.LdapOperators.IsNull:
                    filterItem = ConvertSmoToLdapFilter(xmlDocument.FirstChild.ChildNodes[0].ChildNodes[0].Attributes["name"].Value, string.Empty);

                    filterStringBuilder.AppendFormat(Constants.StringFormats.LdapCompareFormat.IsNull, filterItem.Prop);
                    break;
                case Constants.StringFormats.LdapOperators.Not:
                    filterStringBuilder.Append("(!");
                    filterStringBuilder.Append(GetLdapFilterPart(xmlDocument.FirstChild.FirstChild.InnerXml));
                    filterStringBuilder.Append(")");
                    break;
                default:
                    break;
            }
            return filterStringBuilder.ToString();
        }
        #endregion
        #region Public methods
        public static string GetSingleStringPropertyCollectionValue(ResultPropertyCollection props, string name)
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
            if (string.Compare(name, Constants.Properties.AdProperties.ObjectSID) == 0)
            {
                byte[] sidInBytes = (byte[])pvc[0];
                SecurityIdentifier sid = new SecurityIdentifier(sidInBytes, 0);
                return Convert.ToString(sid);
            }
            else
            {
                return pvc[0] as string;

            }
        }
        /// <summary>
        /// Converts Input properties and SMO internal filtering to LDAP query filters
        /// </summary>
        /// <param name="inputProp">Dictionary of Input properties</param>
        /// <param name="smoFilterXml">Filter of the method</param>
        /// <param name="identityType">Group or User</param>
        /// <param name="changeContainsToStartsWith">IF needed to change Contains to Starts With operator for better performance</param>
        /// <returns>LDAP query filter</returns>
        public static string GetLdapQueryString(Dictionary<string, string> inputProp, string smoFilterXml, IdentityType identityType, bool changeContainsToStartsWith)
        {
            StringBuilder searchFilter = new StringBuilder();
            searchFilter.Append("(&");
 
            // Identity type
            switch (identityType)
            {
                case IdentityType.Group:
                    searchFilter.Append("(objectcategory=group)(objectclass=group)");
                    break;
                case IdentityType.User:
                    searchFilter.Append("(objectcategory=person)(objectclass=user)");
                    break;
                case IdentityType.Role:
                    throw new ArgumentException("Identity type role is not supported for this filtering.");
            }
   
            //Parameters
            foreach (KeyValuePair<string,string> item in inputProp)
            {
                if (!String.IsNullOrEmpty(item.Value))
                {
                    FilterItem filterItem = ConvertSmoToLdapFilter(item.Key, item.Value);
                    searchFilter.AppendFormat(Constants.StringFormats.LdapCompareFormat.Equal, filterItem.Prop, filterItem.Value);
                }
            }

            searchFilter.Append(ConvertXMLFilterToLdapFilter(smoFilterXml, changeContainsToStartsWith));
            searchFilter.Append(")");
            return searchFilter.ToString();
        }
        #endregion



        public static string GetSAMAccountName(string name)
        {
            if (name.Contains("\\"))
            {
                return name.Substring(name.IndexOf('\\') + 1);
            }
            if (name.Contains("@"))
            {
                return name.Substring(0, name.IndexOf('@'));
            }
            return name;
        }
    }

}
