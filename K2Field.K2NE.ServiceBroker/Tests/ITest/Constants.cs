using System;

namespace K2Field.K2NE.ServiceBroker.ITest
{
    internal static class Constants
    {
        public static string[] Regions = new[] { "North", "South", "East", "West" };

        public static class ActiveDirectory
        {
            public const string ADService2 = "ADService2";
            public static Guid ADService2_ServiceInstance_Guid = new Guid("83ac8763-ab63-4f86-8bc8-9bde8729b735");
            public static Guid ADService2_ServiceType_Guid = new Guid("5c50e5ca-985d-49db-8486-cf8dfb702dac");
        }

        public static class Category
        {
            public const string CategoryK2NEServiceBrokerName = "K2NEServiceBroker";
            public const string SystemControlsImageCategoryName = "Image";
            public const string WorkflowGeneral = @"Workflow Reports/Workflow General";
            public static readonly int SystemId = 1;
        }

        public static class ErrorProfile
        {
            internal const string All = "All";
            internal const string Profile1 = "Profile1";
        }

        public static class Form
        {
            public const string ApplicationFormName = "K2Tests_TestApplicationForm";
            public const string NonApplicationFormName = "K2Tests_TestNonApplicationForm";
            public const string TestCategoryPath = "K2NEServiceBroker";
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

        public static class Security
        {
            public static string User1Name = @"DENALLIX\Rob";
            public static string User1Password = @"K2pass!";
            public static string User2Name = @"DENALLIX\Sean";
            public static string User2Password = @"K2pass!";
            public static string User3GroupName = @"Legal";
            public static string User3Name = @"DENALLIX\Eric";
            public static string User3Password = @"K2pass!";
            public static string User4Name = @"DENALLIX\Chad";
            public static string User4Password = @"K2pass!";
        }

        public static class SmartObject
        {
            public const string Employee = "Employee_K2NEServiceBroker";
            public const string Region = "Region_K2NEServiceBroker";
        }
    }
}