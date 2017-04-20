using System;

namespace K2Field.K2NE.ServiceBroker.Constants
{
    public static class ErrorMessages
    {
        //public const string RequiredPropertyNotFound = "{0} is a required property, but does not exist.";
        public const string RequiredParameterNotFound = "{0} is a required parameter, but does not exist.";
        public const string PropertyNotFound = "The property with name '{0}', could not be found.";
        public const string RoleNotExists = "The given role does not exist.";
        public const string RoleTypeNotSupported = "Role Type is not supported. Only User and group allowed.";

        //Error messages for WorkingHoursConfiguration ServiceObject
        public const string DayOfWeekNotFound = "The Day of Week not found. Please, use English variants: Monday, Tuesday...";
        public const string GMTOffSetValidationFailed = "GMTOffset must be between -13 and 13";
        public const string ZoneExists = "Time Zone already exists. Name: ";
        public const string SpecialCharactersAreNotAllowed = "Special characters are not allowed.";
        public const string ZoneDoesNotExist = "Time Zone does not exist. Name: ";
        public const string DateNotValid = "Time Zone does not exist. Name: The string could not be parsed into a valid date and time.";
        public const string WorkingHoursNotSet = "No working hours have been set. Please set the working hours using the K2 Workspace.";
        //Out Of Office
        public const string OutOfOfficeNotConfiguredForUser = "User does not have out of office configured.  Please configure the users out of office settings.";
        public const string FailedToSetOOF = "Failed to get/set the OOF status for the given user.";
        public const string MultipleOOFConfigurations = "Multiple OOF scenarios detected for this user which is supported by this method.";

        public const string ConfigOptionNotFound = "The Service Instance Configuration option '{0}' could not be found. Please specify it.";

        //Error Log
        //public const string ProfileNotFound = "Profile with name \"{0}\" was not found.";
    }
}
