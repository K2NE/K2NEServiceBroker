using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K2Field.K2NE.ServiceBroker
{
    internal static class ExtensionMethods
    {
        // SourceCode.SmartObjects.Services.SQL.ExtensionMethods
        public static void TrimTrailingToken(this StringBuilder sb, string token)
        {
            if (sb != null && !string.IsNullOrEmpty(token) && sb.Length > token.Length)
            {
                string text = sb.ToString();
                if (text.EndsWith(token, StringComparison.OrdinalIgnoreCase))
                {
                    sb.Remove(sb.Length - token.Length, token.Length);
                }
            }
        }
    }
}
