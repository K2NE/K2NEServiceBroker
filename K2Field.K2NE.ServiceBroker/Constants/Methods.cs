﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K2Field.K2NE.ServiceBroker.Constants
{

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
            public const string RedirectWorklistItem = "RedirectWorklistItem";
            public const string ReleaseWorklistItem = "ReleaseWorklistItem";
            public const string DelegateWorklistItem = "DelegateWorklistItem";
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

        public static class ProcessInstanceManagement
        {
                public const string GotoActivity = "GotoActivity";
                public const string StartProcessInstance = "StartProcessInstance";

        }

        public static class Role
        {
            public const string ListRoles = "ListRoles";
            public const string AddRole = "AddRole";
            public const string RemoveRole = "RemoveRole";
            public const string SetRoleDynamic = "SetRoleDynamic";
            public const string AddRoleItem = "AddRoleItem";
            public const string RemoveRoleItem = "RemoveRoleItem";
            public const string ListRoleItem = "ListRoleItem";


        }
    }
}
