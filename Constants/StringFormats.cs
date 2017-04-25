using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K2Field.K2NE.ServiceBroker.Constants
{
    public static class StringFormats
    {
        public static class LdapCompareFormat
        {
            public const string StartsWith = "({0}={1}*)";
            public const string EndsWith = "({0}=*{1})";
            public const string Contains = "({0}=*{1}*)";
            public const string Equal = "({0}={1})";
            public const string GreaterThan = "({0}>{1})";
            public const string LessThan = "({0}<{1})";
            public const string IsNull = "(!{0}=*)";
        }
        internal static class LdapOperators
        {
            public const string And = "and";
            public const string Or = "or";
            public const string StartsWith = "startswith";
            public const string EndsWith = "endswith";
            public const string Contains = "contains";
            public const string Not = "not";
            public const string Equal = "equals";
            public const string GreaterThan = "greaterthan";
            public const string LessThan = "lessthan";
            public const string IsNull = "isnull";
        }
    }
}
