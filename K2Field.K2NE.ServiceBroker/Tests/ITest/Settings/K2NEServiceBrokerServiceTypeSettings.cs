using System;
using SourceCode.SmartObjects.Services.Tests.Managers;

namespace K2Field.K2NE.ServiceBroker.ITest.Settings
{
    public class K2NEServiceBrokerServiceTypeSettings : ServiceTypeSettings
    {
        private static readonly Lazy<K2NEServiceBrokerServiceTypeSettings> lazy =
            new Lazy<K2NEServiceBrokerServiceTypeSettings>(() => new K2NEServiceBrokerServiceTypeSettings());

        private K2NEServiceBrokerServiceTypeSettings()
        {
        }

        public static K2NEServiceBrokerServiceTypeSettings Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        public override string ClassName
        {
            get
            {
                return "K2Field.K2NE.ServiceBroker.K2NEServiceBroker";
            }
        }

        public override string DefaultDisplayName
        {
            get
            {
                return "K2NEServiceBroker";
            }
        }

        public override Guid DefaultGuid
        {
            get
            {
                return new Guid("e84598f2-31cc-4593-bf99-a21dafcd0911");
            }
        }

        public override string DefaultName
        {
            get
            {
                return "K2Field.K2NE.ServiceBroker.K2NEServiceBroker";
            }
        }
    }
}