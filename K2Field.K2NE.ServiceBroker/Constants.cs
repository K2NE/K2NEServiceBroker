﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceCode.Workflow.Client;

namespace K2Field.K2NE.ServiceBroker
{
   
    public static class Constants
    {
        /// <summary>
        /// The system names of the ServiceObject properties. Make them 'nice' with the Helper.AddSpaceBeforeCaptialLetter() method.
        /// </summary>
        public static class Properties
        {
            public static class ErrorLog
            {
                public const string Profile = "Profile";
                public const string ProcessInstanceId = "ProcessInstanceId";
                public const string ProcessName = "ProcessDefinitionName";
                public const string Folio = "Folio";
                public const string ErrorDescription = "ErrorDescription";
                public const string ErrorItem = "ErrorItem";
                public const string ErrorId = "ErrorId";
                public const string ErrorDate = "ErrorDate";
                public const string TryNewVersion = "TryNewVersion";
            }

            public static class ManagementWorklist
            {
                public const string ActivityId = "ActivityId";
                public const string ActivityInstanceDestinationId = "ActivityInstanceDestinationId";
                public const string ActivityInstanceId = "ActivityInstanceId";
                public const string ActivityName = "ActivityName";
                public const string Destination = "Destination";
                public const string DestinationType = "DestinationType";
                public const string EventId = "EventId";
                public const string Folio = "Folio";
                public const string ProcessInstanceId = "ProcessInstanceId";
                public const string EventName = "EventName";
                public const string ProcessName = "ProcessName";
                public const string ProcessStatus = "ProcessStatus";
                public const string StartDate = "StartDate";
                public const string WorklistItemStatus = "Status";
                public const string WorklistItemId = "Id";
            }

            public static class ClientWorklist
            {
                public const string ProcessName = "ProcessName";
                public const string ProcessFolder = "Folder";
                public const string ProcessFullname = "ProcessFullname";
                public const string ProcessDescription = "ProcessDescription";
                public const string ProcessMetadata = "PocessMetadata";
                public const string ProcessStatus = "ProcessStatus";
                public const string ProcessPriority = "ProcessPriority";
                public const string ProcessStartdate = "ProcessStartDate";
                public const string ProcessExpectedDuration = "ProcessExpectedDuration";
                public const string ProcessGuid = "ProcessGuid";
                public const string ProcessId = "ProcessId";
                public const string ActivityId = "ActivityId";
                public const string ActivityName = "ActivityName";
                public const string ActivityPriority = "ActivityPriority";
                public const string ActivityDescription = "ActivityDescription";
                public const string ActivityMetadata = "ActivityMetadata";
                public const string ActivityStartdate = "ActivityStartDate";
                public const string ActivityExpectedDuration = "ActivityExpectedDuration";
                public const string EventId = "EventId";
                public const string EventName = "EventName";
                public const string EventMetadata = "EventMetadata";
                public const string EventDescription = "EventDescription";
                public const string EventPriority = "EventPriority";
                public const string EventStartDate = "EventStartDate";
                public const string EventExpectedDuration = "EventExpectedDuration";
                public const string WorklistItemStatus = "Status";
                public const string Folio = "Folio";
                public const string Data = "Data";
                public const string SerialNumber = "SerialNumber";
                public const string ActivityOverdue = "ActivityOverdue";
                public const string OriginalDestination ="OriginalDestination";

            }


            public static class Identity
            {
                public const string CurrentPrincipalAuthType = "CurrentPrincipalAuthType =";
                public const string CurrentPrincipalName = "CurrentPrincipalName";
                public const string FQN = "FQN";
                public const string UserDescription = "UserDescription";
                public const string UserDisplayName = "UserDisplayName";
                public const string UserEmail = "UserEmail";
                public const string UserManager = "UserManager";
                public const string UserName = "UserName";
                public const string UserUserLabel = "UserUserLabel";
                public const string CallingFQN = "CallingFQN";
                public const string WindowsIdentityAuthType = "WindowsIdentityAuthType";
                public const string WindowsIdentityName = "WindowsIdentityName";
                public const string ServiceBrokerAuthType = "ServiceBrokerAuthType";
                public const string ServiceBrokerUserName = "ServiceBrokerUserName";
                public const string UserWindowsImpersonation = "UserWindowsImpersonation";
            }

        }

        public static class ConfigurationProperties
        {
            public const string EnvironmentToUse = "Environment to use (empty for default)";
            public const string WorkflowManagmentPort = "Workflow Management Port";
            public const string WorkflowClientPort = "Workflow Client Port";
            public const string DefaultCulture = "Default Culture";
            public static string Platform;
        }

        public static class Methods
        {
            public static class ErrorLog
            {
                public const string GetErrors = "GetErrors";
                public const string RetryProcess = "RetryProcessInstance";
            }

            public static class ManagementWorklist
            {
                public const string GetWorklist = "GetWorklist";
            }

            public static class ClientWorklist
            {
                public const string GetWorklist = "GetWorklist";
                public const string ReleaseWorklistItem = "ReleaseWorklistItem";
            }

            public static class Identity
            {
                public const string ReadThreadIdentity = "ReadThreadIdentity";
                public const string ReadWorkflowClientIdentity = "ReadWorkflowClientIdentity";
            }
        }




        public static class ErrorMessages
        {
            public const string RequiredPropertyNotFound = "{0} is a required property, but does not exist.";
            public const string PropertyNotFound = "The property with name '{0}', could not be found.";
        }


       
    }
}
