using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K2Field.K2NE.ServiceBroker
{
    public class Logger : IDisposable
    {
        private SourceCode.Logging.ILogger logger = null;
        private readonly string LogIdentifier = "K2NE Service Broker";


        public Logger(SourceCode.Logging.ILogger log)
        {
            logger = log;
        }

        public void LogDebug(string inputLine, params object[] args)
        {
            string line = string.Format(inputLine, args);
            logger.LogDebugMessage(LogIdentifier, line);
        }

        public void LogDebug(string inputLine)
        {
            logger.LogDebugMessage(LogIdentifier, inputLine);
        }

        public void LogInfo(string inputLine, params object[] args)
        {
            string line = string.Format(inputLine, args);
            logger.LogInfoMessage(LogIdentifier, line);
        }

        public void LogInfo(string inputLine)
        {
            logger.LogInfoMessage(LogIdentifier, inputLine);
        }

        public void LogError(string inputLine, params object[] args)
        {
            string line = string.Format(inputLine, args);
            logger.LogErrorMessage(LogIdentifier, line);
        }

        public void LogError(string inputLine)
        {
            logger.LogErrorMessage(LogIdentifier, inputLine);
        }

        public void Dispose()
        {
            logger.LogDebugMessage(LogIdentifier, "Unloading logger");
            logger = null;
        }

        internal void LogException(Exception ex)
        {
            StringBuilder error = new StringBuilder();
            this.LogError("Exception.Message: {0}\n", ex.Message);
            this.LogError("Exception.StackTrace: {0}\n", ex.StackTrace);

            Exception innerEx = ex;
            int i = 0;
            while (innerEx.InnerException != null)
            {
                this.LogError("{0} InnerException.Message: {1}\n", i, innerEx.InnerException.Message);
                this.LogError("{0} InnerException.StackTrace: {1}\n", i, innerEx.InnerException.StackTrace);
                innerEx = innerEx.InnerException;
                i++;
            }

        }
    }
}
