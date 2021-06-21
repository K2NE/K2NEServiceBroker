using System;

namespace K2Field.K2NE.ServiceBroker.Constants
{

    public static class Methods
    {
        public static class ADOSMOQuery
        {
            public const string ExcelFromADOQuery = "ExcelFromADOQuery";
            public const string ListQueryData = "List";
            public const string ADOQuery2Excel = "ADOQueryToExcel";
        }
        public static class ADOHelper
        {
            public const string Join = "Join";
        }

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
            public const string RedirectWorklistItem = "RedirectWorklistItem";
            public const string ActionWorklistItem = "ActionWorklistItem";
        }

        public static class Identity
        {
            public const string ReadThreadIdentity = "ReadThreadIdentity";
            public const string ReadWorkflowClientIdentity = "ReadWorkflowClientIdentity";
            public const string ResolveUserIdentity = "ResolveUserIdentity";
            public const string ResolveGroupIdentity = "ResolveGroupIdentity";
            public const string ResolveRoleIdentity = "ResolveRoleIdentity";
            public const string GetIdentities = "GetEmailFromIdentity";
            public const string GetIdentityAndContainersDelimited = "GetIdentityAndContainersDelimited";
        }

        public static class ProcessInstanceManagement
        {
            public const string GotoActivity = "GotoActivity";
            public const string GotoActivityRead = "GotoActivityRead";
            public const string ListActivities = "ListActivities";
        }

        public static class ProcessInstanceClient
        {
            public const string StartProcessInstance = "StartProcessInstance";
            public const string StartProcess = "Start";
            public const string SetFolio = "SetFolio";
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

        public static class ActiveDirectory
        {
            public const string GetUsers = "GetUsers";
            public const string GetUserDetails = "GetUserDetails";
            public const string SearchUsers = "SearchUsers";
            public const string UMGetUsers = "UMGetUsers";
        }

        public static class WorkingHoursConfiguration
        {
            public const string CreateZone = "CreateZone";
            public const string SaveZone = "SaveZone";
            public const string LoadZone = "LoadZone";
            public const string DeleteZone = "DeleteZone";
            public const string ListZones = "ListZones";
            public const string ListZoneUsers = "ListZoneUsers";
            public const string GetDefaultZone = "GetDefaultZone";
            public const string SetDefaultZone = "SetDefaultZone";
            public const string ZoneExists = "ZoneExists";
            public const string ZoneCalculateEvent = "ZoneCalculateEvent";
            public const string UserSetZone = "UserSetZone";
            public const string UserGetZone = "UserGetZone";
            public const string UserDeleteZone = "UserDeleteZone";
            public const string UserCalculateEvent = "UserCalculateEvent";
            
            //TODO: Later if needed
            public const string ListZoneWorkingHours = "ListZoneWorkingHours";
            public const string ListZoneAvailabilityDates = "ListZoneAvailabilityDates";
        }
        public static class OutOfOffice
        {
            public const string SetOutOfOffice = "SetOutOfOffice";
            public const string SetInOffice = "SetInOffice";
            public const string ListUserShares = "ListUserShares";
            public const string AddOutOfOffice = "AddOutOfOffice";
            public const string GetUserStatus = "GetUserStatus";
            public const string ListShares = "ListShares";
            public const string RemoveAllShares = "RemoveAllShares";
        }

        public static class OutOfOfficeClient
        {
            public const string SetOutOfOffice = "SetOutOfOffice";
            public const string SetInOffice = "SetInOffice";
            public const string ListUserShares = "ListUserShares";
            public const string AddOutOfOffice = "AddOutOfOffice";
            public const string GetUserStatus = "GetUserStatus";
            public const string ListShares = "ListShares";
            public const string RemoveAllShares = "RemoveAllShares";
        }

        public static class Group
        {
            public const string GetGroupDetails = "UMRGetGroupDetails";
            public const string GetGroups = "UMRGetGroups";
            public const string FindUserGroups = "UMRFindUserGroups";
            public const string FindUserGroupsFQNDelimited = "URMFindUserGroupsFQNDelimited";
        }
        public static class User
        {
            public const string GetUsers = "UMRGetUsers";            
        }

        public static class PowershellVariables
        {
            public const string SerializeItem = "SerializeItem";
            public const string SerializeItemToArray = "SerializeItemToArray";
            public const string AddSerializedItemToArray = "AddSerializedItemToArray";
            public const string SerializeAddItemToArray = "SerializeAddItemToArray";
            public const string Deserialize = "Deserialize";
            public const string DeserializeItemFromArray = "DeserializeItemFromArray";
            public const string DeserializeArrayToList = "DeserializeArrayToList";
        }

        public static class SimplePowerShell
        {
            public const string RunScriptCode = "RunScript";
            public const string RunScriptByFilePath = "RunScriptByFilePath";
        }

        public static class DynamicPowerShell
        {
            public const string RunScript = "RunScript";
        }

        public static class ExcelDocumentServices
        {
            public const string GetCellValue = "GetCellValue";
            public const string SaveCellValue = "SaveCellValue";
            public const string GetWorkSheetNames = "GetWorkSheetNames";
            public const string GetMultipleCellValues = "GetMultipleCellValues";
            public const string GetMultipleCellValuesList = "GetMultipleCellValuesList";
            public const string SaveMultipleCellValuesList = "SaveMultipleCellValuesList";
            public const string SaveMultipleCellValues = "SaveMultipleCellValues";
        }
        
        public static class ExcelImportServices
        {
            public const string UploadExcelDataToASmartObject = "UploadExcelDataToASmartObject";
        }
        
        public static class FilesToZip
        {
            public const string FilesToZipMethod = "FilesToZipMethod";
        }
    }
}
