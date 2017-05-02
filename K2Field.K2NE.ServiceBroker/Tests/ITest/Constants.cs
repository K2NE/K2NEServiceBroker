namespace K2Field.K2NE.ServiceBroker.ITest
{
    internal static class Constants
    {
        public static class Category
        {
            public const string CategoryK2NEServiceBrokerName = "K2NEServiceBroker";
        }

        public static class Process
        {
            public const string TestDeleteProcess = ProcessSetFolder.K2NEServiceBroker + @"\TestDeleteProcess";
            public const string TestProcess1 = ProcessSetFolder.K2NEServiceBroker + @"\TestProcess1";
            public const string TestProcess2 = ProcessSetFolder.K2NEServiceBroker + @"\TestProcess2";
        }

        public static class ProcessSetFolder
        {
            public const string K2NEServiceBroker = "K2NEServiceBroker";
        }

        public static class TestAttribute
        {
            public const string Worklist = "Worklist";
        }
    }
}