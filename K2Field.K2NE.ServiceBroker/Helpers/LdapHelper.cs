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
        #region Private Helper Classes
        private static class Operator
        {
            public const string And = "and";
            public const string Or = "or";
            public const string StartsWith = "startswith";
            public const string EndsWith = "endswith";
            public const string Contains = "contains";
            public const string Not = "not";
            public const string Equals = "equals";
            public const string GreaterThan = "greaterthan";
            public const string LessThan = "lessthan";
            public const string IsNull = "isnull";

        }
        private static class Format
        {
            public const string StartsWith = "({0}={1}*)";
            public const string EndsWith = "({0}=*{1})";
            public const string Contains = "({0}=*{1}*)";
            public const string Equals = "({0}={1})";
            public const string GreaterThan = "({0}>{1})";
            public const string LessThan = "({0}<{1})";
            public const string IsNull = "(!{0}=*)";
        }
        #endregion
        private class FilterItem
        {
            public string Prop { get; set; }
            public string Value { get; set; }
        }
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
        #region Private Helper Methods
        /// <summary>
        /// Converts SMO properties to LDAP ones & changes the values in order to be suitable for LDAP.
        /// </summary>
        /// <param name="smoProp">Property Name</param>
        /// <param name="smoValue">Property Value</param>
        /// <returns>Property name + Value</returns>
        private static FilterItem GetLdapFilterItem(string smoProp, string smoValue)
        {
            if (String.IsNullOrEmpty(smoProp)) return new FilterItem();

            var filter = new FilterItem();

            switch (smoProp)
            {
                case Constants.Properties.URM.FQN:
                    filter.Prop = AdProperties.sAMAccountName;
                    filter.Value = smoValue.Substring(smoValue.IndexOf('\\') + 1);
                    break;
                case Constants.Properties.URM.Name:
                    filter.Prop = AdProperties.sAMAccountName;
                    filter.Value = smoValue.Substring(smoValue.IndexOf('\\') + 1);
                    break;
                case Constants.Properties.URM.Email:
                    filter.Prop = AdProperties.Email;
                    filter.Value = smoValue;
                    break;
                case Constants.Properties.URM.Description:
                    filter.Prop = AdProperties.Description;
                    filter.Value = smoValue;
                    break;
                case Constants.Properties.URM.Manager:
                    filter.Prop = AdProperties.Manager;
                    filter.Value = smoValue;
                    break;
                case Constants.Properties.URM.DisplayName:
                    filter.Prop = AdProperties.DisplayName;
                    filter.Value = smoValue;
                    break;
                case Constants.Properties.URM.UserName:
                    filter.Prop = AdProperties.sAMAccountName;
                    filter.Value = smoValue;
                    break;
                case Constants.Properties.URM.ObjectSid:
                    filter.Prop = AdProperties.ObjectSID;
                    filter.Value = smoValue;
                    break;
                case Constants.Properties.URM.Saml:
                    filter.Prop = AdProperties.sAMAccountName;
                    filter.Value = smoValue;
                    break;
                case Constants.Properties.URM.GroupName:
                    filter.Prop = AdProperties.sAMAccountName;
                    filter.Value = smoValue.Substring(smoValue.IndexOf('\\') + 1);
                    break;
            }
            return filter;
        }
        /// <summary>
        /// Converts K2 xml filters to LDAP ones.
        /// </summary>
        /// <param name="xml">SMO method filter</param>
        /// <param name="changeContainsToStartsWith">If needed to change Contains operator to StartsWith for AD performance</param>
        /// <param name="previousOperator"></param>
        /// <returns></returns>
        private static string ConvertSmoToLdapFilters(string xml, bool changeContainsToStartsWith, string previousOperator)
        {
            StringBuilder filterStringBuilder = new StringBuilder();
            if (string.IsNullOrEmpty(xml))
            {
                return string.Empty;
            }
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            string nextOperator = xmlDocument.FirstChild.FirstChild.Name;

            if (nextOperator != previousOperator && nextOperator == Operator.Or)
            {
                filterStringBuilder.Append("(|");
            }

            if (nextOperator != previousOperator && nextOperator == Operator.And)
            {
                filterStringBuilder.Append("(&");
            }

            if (xmlDocument.FirstChild.FirstChild.ChildNodes.Count > 1 && (nextOperator == Operator.And || nextOperator == Operator.Or))
            {
                foreach (XmlNode childNode in xmlDocument.FirstChild.FirstChild.ChildNodes)
                {
                    filterStringBuilder.Append(ConvertSmoToLdapFilters(childNode.OuterXml, changeContainsToStartsWith, nextOperator));
                }
            }
            else
            {
                filterStringBuilder.Append(GetLdapFilter(xmlDocument.FirstChild.InnerXml, changeContainsToStartsWith));
            }

            if (nextOperator != previousOperator && (nextOperator == Operator.Or || nextOperator == Operator.And))
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
        private static string GetLdapFilter(string xml, bool changeContainsToStartsWith = false)
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
                case Operator.StartsWith:
                    filterItem = GetLdapFilterItem(xmlDocument.FirstChild.ChildNodes[0].ChildNodes[0].Attributes["name"].Value, xmlDocument.FirstChild.ChildNodes[1].ChildNodes[0].InnerText);
                    filterStringBuilder.AppendFormat(Format.StartsWith, filterItem.Prop, filterItem.Value);
                    break;
                case Operator.EndsWith:
                    filterItem = GetLdapFilterItem(xmlDocument.FirstChild.ChildNodes[0].ChildNodes[0].Attributes["name"].Value, xmlDocument.FirstChild.ChildNodes[1].ChildNodes[0].InnerText);
                    filterStringBuilder.AppendFormat(Format.EndsWith, filterItem.Prop, filterItem.Value);
                    break;
                case Operator.Contains:
                    filterItem = GetLdapFilterItem(xmlDocument.FirstChild.ChildNodes[0].ChildNodes[0].Attributes["name"].Value, xmlDocument.FirstChild.ChildNodes[1].ChildNodes[0].InnerText);
                    if (changeContainsToStartsWith)
                    {
                        filterStringBuilder.AppendFormat(Format.StartsWith, filterItem.Prop, filterItem.Value);
                    }
                    else
                    {
                        filterStringBuilder.AppendFormat(Format.Contains, filterItem.Prop, filterItem.Value);
                    }
                    break;
                case Operator.Equals:
                    filterItem = GetLdapFilterItem(xmlDocument.FirstChild.ChildNodes[0].ChildNodes[0].Attributes["name"].Value, xmlDocument.FirstChild.ChildNodes[1].ChildNodes[0].InnerText);
                    filterStringBuilder.AppendFormat(Format.Equals, filterItem.Prop, filterItem.Value);
                    break;
                case Operator.GreaterThan:
                    filterItem = GetLdapFilterItem(xmlDocument.FirstChild.ChildNodes[0].ChildNodes[0].Attributes["name"].Value, xmlDocument.FirstChild.ChildNodes[1].ChildNodes[0].InnerText);
                    filterStringBuilder.AppendFormat(Format.GreaterThan, filterItem.Prop, filterItem.Value);
                    break;
                case Operator.LessThan:
                    filterItem =
                    GetLdapFilterItem(xmlDocument.FirstChild.ChildNodes[0].ChildNodes[0].Attributes["name"].Value, xmlDocument.FirstChild.ChildNodes[1].ChildNodes[0].InnerText);
                    filterStringBuilder.AppendFormat(Format.LessThan, filterItem.Prop, filterItem.Value);
                    break;
                case Operator.IsNull:
                    filterItem = GetLdapFilterItem(xmlDocument.FirstChild.ChildNodes[0].ChildNodes[0].Attributes["name"].Value, string.Empty);

                    filterStringBuilder.AppendFormat(Format.IsNull, filterItem.Prop);
                    break;
                case Operator.Not:
                    filterStringBuilder.Append("(!");
                    filterStringBuilder.Append(GetLdapFilter(xmlDocument.FirstChild.FirstChild.InnerXml));
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
            if (string.Compare(name, AdProperties.ObjectSID) == 0)
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
        /// <param name="iType">Group or User</param>
        /// <param name="changeContainsToStartsWith">IF needed to change Contains to Starts With operator for better performance</param>
        /// <returns>LDAP query filter</returns>
        public static string GetLdapFilters(Dictionary<string, string> inputProp, string smoFilterXml, IdentityType iType, bool changeContainsToStartsWith)
        {
            StringBuilder searchFilter = new StringBuilder();
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
                    FilterItem filterItem = GetLdapFilterItem(item.Key, item.Value);
                    searchFilter.AppendFormat(Format.Equals, filterItem.Prop, filterItem.Value);
                }
            }
            searchFilter.Append(ConvertSmoToLdapFilters(smoFilterXml, changeContainsToStartsWith, string.Empty));
            searchFilter.Append(")");
            return searchFilter.ToString();
        }
        #endregion
    }

}
