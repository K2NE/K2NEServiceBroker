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

        //Error messages for WorkingHoursConfiguration ServiceObject
        public const string DayOfWeekNotFound = "The Day of Week not found. Please, use English variants: Monday, Tuesday...";
        public const string GMTOffSetValidationFailed = "GMTOffset must be between -13 and 13";
        public const string ZoneExists = "Time Zone already exists. Name: ";
        public const string SpecialCharactersAreNotAllowed = "Special characters are not allowed.";
        public const string ZoneDoesNotExist = "Time Zone does not exist. Name: ";
    }


}
