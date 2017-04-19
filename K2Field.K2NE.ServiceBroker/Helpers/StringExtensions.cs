using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K2Field.K2NE.ServiceBroker.Helpers
{
    public static class StringExtensions
    {
        public static string ToStringOrEmpty(this Object value)
        {
            if (value == null)
            {
                return String.Empty;
            }
            else
            {
                return value.ToString(); 
            }
        }
    }
}
