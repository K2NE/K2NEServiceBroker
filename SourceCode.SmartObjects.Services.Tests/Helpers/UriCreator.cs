using System;
using System.Web;

namespace SourceCode.SmartObjects.Services.Tests.Helpers
{
    /// <summary>
    /// UriCreator
    /// </summary>
    internal static class UriCreator
    {
        internal static Uri CreateSanitizedPathUri(UriKind uriKind, params string[] pathSegments)
        {
            if (pathSegments == null ||
                pathSegments.Length == 0)
            {
                return null;
            }

            string pathBuilder = null;

            foreach (var pathSegment in pathSegments)
            {
                if (string.IsNullOrEmpty(pathSegment))
                {
                    continue;
                }

                if (pathBuilder != null)
                {
                    pathBuilder = System.IO.Path.Combine(pathBuilder, HttpUtility.UrlEncode(pathSegment));
                }
                else
                {
                    pathBuilder = pathSegment;
                }
            }

            if (string.IsNullOrEmpty(pathBuilder))
            {
                return null;
            }

            var returnUrl = new Uri(pathBuilder.Replace(@"\", @"/"), uriKind);
            return returnUrl;
        }
    }
}