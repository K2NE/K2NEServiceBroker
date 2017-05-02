using System;
using System.Text;
using SourceCode.SmartObjects.Client;

namespace SourceCode.SmartObjects.Services.Tests.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetExceptionMessage(this Exception exception)
        {
            var smartObjectException = exception as SmartObjectException;

            if (smartObjectException == null)
            {
                return exception.Message;
            }
            else
            {
                var errorMessage = new StringBuilder();

                foreach (SmartObjectExceptionData smartobjectExceptionData in smartObjectException.BrokerData)
                {
                    string message = smartobjectExceptionData.Message;
                    string service = smartobjectExceptionData.ServiceName;
                    string serviceGuid = smartobjectExceptionData.ServiceGuid;
                    string severity = smartobjectExceptionData.Severity.ToString();
                    string innermessage = smartobjectExceptionData.InnerExceptionMessage;

                    errorMessage.AppendLine("Service: " + service);
                    errorMessage.AppendLine("Service Guid: " + serviceGuid);
                    errorMessage.AppendLine("Severity: " + severity);
                    errorMessage.AppendFormat("Error Message: {0}", message);

                    if (!string.IsNullOrEmpty(innermessage))
                    {
                        errorMessage.AppendLine();
                        errorMessage.AppendFormat("InnerException Message: {0}", innermessage);
                    }
                }

                return errorMessage.ToString();
            }
        }
    }
}