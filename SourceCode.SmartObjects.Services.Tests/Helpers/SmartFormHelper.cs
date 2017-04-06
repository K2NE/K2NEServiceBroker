using System.Net;
using SourceCode.Forms.Utilities;

namespace SourceCode.SmartObjects.Services.Tests.Helpers
{
    public static class SmartFormHelper
    {
        public static HttpWebResponse GetFormHttpResponse(string formName)
        {
            return GetHttpResponse("Form", formName);
        }

        public static HttpWebResponse GetViewHttpResponse(string viewName)
        {
            return GetHttpResponse("View", viewName);
        }

        private static HttpWebResponse GetHttpResponse(string prePathSegement, string name)
        {
            var smartRuntimeUrl = EnvironmentHelper.GetEnvironmentFieldByName(EnvironmentHelper.FieldNames.SmartFormsRuntime);
            var uri = UriCreator.CreateSanitizedPathUri(System.UriKind.RelativeOrAbsolute, smartRuntimeUrl, prePathSegement, UrlHelper.VanityEncode(name), "/");

            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.94 Safari/537.36";
            request.UseDefaultCredentials = true;
            return (HttpWebResponse)request.GetResponse();
        }
    }
}