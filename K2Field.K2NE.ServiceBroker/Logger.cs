using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K2Field.K2NE.ServiceBroker
{
    public class Logger
    {
        private SourceCode.Logging.Logger logger;
        private readonly string LogIdentifier = "CMSF";

        public bool SelfLoaded { get; private set; }
        

        public Logger()
        {
            SelfLoaded = true;
            logger = new SourceCode.Logging.Logger("HostServerLogging.config");
            logger.StartLogger();
        }

        public Logger(SourceCode.Logging.Logger log)
        {
            SelfLoaded = false;
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
    }
}
