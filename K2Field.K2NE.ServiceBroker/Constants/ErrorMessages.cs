using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K2Field.K2NE.ServiceBroker.Constants
{
    public static class ErrorMessages
    {
        public const string RequiredPropertyNotFound = "{0} is a required property, but does not exist.";
        public const string PropertyNotFound = "The property with name '{0}', could not be found.";
        public const string RoleNotExists = "The given role does not exist.";
        public const string RoleTypeNotSupported = "Role Type is not supported. Only User and group allowed.";
        public const string OutOfOfficeNotConfiguredForUser = "User does not have out of office configured.  Please configure the users out of office settings.";
        public const string MultipleOOFConfigurations = "Multiple OOF scenarios detected for this user which is supported by this method.";
    }
    

}
