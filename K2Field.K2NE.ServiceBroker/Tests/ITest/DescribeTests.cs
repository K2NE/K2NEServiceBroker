using K2Field.K2NE.ServiceBroker.ITest.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SourceCode.SmartObjects.Authoring;
using SourceCode.SmartObjects.Services.Management;
using SourceCode.SmartObjects.Services.Tests.Helpers;

namespace K2Field.K2NE.ServiceBroker.ITest
{
    [TestClass]
    public class DescribeTests
    {
        [TestMethod]
        [TestCategory("Describe K2NEServiceBroker")]
        public void A01_Describe_ServiceInstance_K2NEServiceBrokerIntegrationTests()
        {
            var serviceInstance = SmartObjectHelper.GetServiceInstance(K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance);

            // SERVICEINSTANCE
            Assert.AreEqual("K2NEServiceBrokerITest", serviceInstance.Name);
            Assert.AreEqual("K2NEServiceBroker Integration Tests", serviceInstance.Metadata.DisplayName);
            Assert.AreEqual("A Service Broker that provides various functional service objects that aid the implementation of a K2 project.", serviceInstance.Metadata.Description);

            // FOLDERS
            Assert.AreEqual(5, serviceInstance.Folders.Count);

            // Folder: Management API
            Assert.AreEqual("Management API", serviceInstance.Folders[0].Name);
            Assert.AreEqual("Management API", serviceInstance.Folders[0].Metadata.DisplayName);

            // ServiceObjects: Management API
            Assert.AreEqual(6, serviceInstance.Folders[0].ServiceObjects.Count);
            Assert.AreEqual("ManagementWorklist", serviceInstance.Folders[0].ServiceObjects[0].Name);
            Assert.AreEqual("ErrorLog", serviceInstance.Folders[0].ServiceObjects[1].Name);
            Assert.AreEqual("OutOfOfficeManagement", serviceInstance.Folders[0].ServiceObjects[2].Name);
            Assert.AreEqual("ProcessInstanceManagement", serviceInstance.Folders[0].ServiceObjects[3].Name);
            Assert.AreEqual("RoleManagement", serviceInstance.Folders[0].ServiceObjects[4].Name);
            Assert.AreEqual("WorkingHoursConfiguration", serviceInstance.Folders[0].ServiceObjects[5].Name);
            Assert.AreEqual(0, serviceInstance.Folders[0].Folders.Count);

            // Folder: Other
            Assert.AreEqual("Other", serviceInstance.Folders[1].Name);
            Assert.AreEqual("Other", serviceInstance.Folders[1].Metadata.DisplayName);

            // ServiceObjects: Other
            Assert.AreEqual(2, serviceInstance.Folders[1].ServiceObjects.Count);
            Assert.AreEqual("Identity", serviceInstance.Folders[1].ServiceObjects[0].Name);
            Assert.AreEqual("AD User", serviceInstance.Folders[1].ServiceObjects[1].Name);
            Assert.AreEqual(0, serviceInstance.Folders[1].Folders.Count);

            // Folder: Client API
            Assert.AreEqual("Client API", serviceInstance.Folders[2].Name);
            Assert.AreEqual("Client API", serviceInstance.Folders[2].Metadata.DisplayName);

            // ServiceObjects: Client API
            Assert.AreEqual(4, serviceInstance.Folders[2].ServiceObjects.Count);
            Assert.AreEqual("Worklist", serviceInstance.Folders[2].ServiceObjects[0].Name);
            Assert.AreEqual("WorklistItem", serviceInstance.Folders[2].ServiceObjects[1].Name);
            Assert.AreEqual("OutOfOfficeClient", serviceInstance.Folders[2].ServiceObjects[2].Name);
            Assert.AreEqual("ProcessInstanceClient", serviceInstance.Folders[2].ServiceObjects[3].Name);
            Assert.AreEqual(0, serviceInstance.Folders[2].Folders.Count);

            // Folder: URM
            Assert.AreEqual("URM", serviceInstance.Folders[3].Name);
            Assert.AreEqual("URM", serviceInstance.Folders[3].Metadata.DisplayName);

            // ServiceObjects: URM
            Assert.AreEqual(2, serviceInstance.Folders[3].ServiceObjects.Count);
            Assert.AreEqual("URMGroup", serviceInstance.Folders[3].ServiceObjects[0].Name);
            Assert.AreEqual("URMUser", serviceInstance.Folders[3].ServiceObjects[1].Name);
            Assert.AreEqual(0, serviceInstance.Folders[3].Folders.Count);

            // Folder: PowerShell
            Assert.AreEqual("PowerShell", serviceInstance.Folders[4].Name);
            Assert.AreEqual("PowerShell", serviceInstance.Folders[4].Metadata.DisplayName);

            // ServiceObjects: PowerShell
            Assert.AreEqual(2, serviceInstance.Folders[4].ServiceObjects.Count);
            Assert.AreEqual("PowershellVariables", serviceInstance.Folders[4].ServiceObjects[0].Name);
            Assert.AreEqual("SimplePowershell", serviceInstance.Folders[4].ServiceObjects[1].Name);
            Assert.AreEqual(0, serviceInstance.Folders[4].Folders.Count);
        }

        [TestMethod]
        [TestCategory("Describe K2NEServiceBroker")]
        public void A02_Describe_ServiceType_K2NEServiceBroker()
        {
            var serviceTypeInfo = SmartObjectHelper.GetServiceTypeInfo(K2NEServiceBrokerServiceTypeSettings.Instance.DefaultGuid);
            // SERVICETYPE
            Assert.AreEqual("K2Field.K2NE.ServiceBroker.K2NEServiceBroker", serviceTypeInfo.Class);
            Assert.AreEqual("K2Field.K2NE.ServiceBroker.K2NEServiceBroker", serviceTypeInfo.Description);
            Assert.AreEqual("K2NEServiceBroker", serviceTypeInfo.DisplayName);
            Assert.AreEqual("e84598f2-31cc-4593-bf99-a21dafcd0911", serviceTypeInfo.Guid.ToString());
            Assert.AreEqual("K2Field.K2NE.ServiceBroker.K2NEServiceBroker", serviceTypeInfo.Name);
            Assert.AreEqual("", serviceTypeInfo.Provider ?? string.Empty);
            Assert.AreEqual("K2Field.K2NE.ServiceBroker.K2NEServiceBroker", serviceTypeInfo.ConfigInfo.Assembly.Class);
            Assert.AreEqual(false, serviceTypeInfo.ConfigInfo.ServiceIntegration.IsFeature);
            // SERVICECONFIG
            var serviceConfigInfo = SmartObjectHelper.GetServiceConfigInfo(K2NEServiceBrokerServiceTypeSettings.Instance.DefaultGuid);
            Assert.AreEqual(AuthenticationMode.Impersonate, serviceConfigInfo.ServiceAuthentication.AuthenticationMode);
            Assert.AreEqual(12, serviceConfigInfo.ConfigSettings.Count);
            Assert.AreEqual("SMO data queries file path", serviceConfigInfo.ConfigSettings[0].DisplayName);
            Assert.AreEqual(false, serviceConfigInfo.ConfigSettings[0].IsRequired);
            Assert.AreEqual("SMO data queries file path", serviceConfigInfo.ConfigSettings[0].Name);
            Assert.AreEqual("", serviceConfigInfo.ConfigSettings[0].Value ?? string.Empty);
            Assert.AreEqual("AD Netbios Name", serviceConfigInfo.ConfigSettings[1].DisplayName);
            Assert.AreEqual(false, serviceConfigInfo.ConfigSettings[1].IsRequired);
            Assert.AreEqual("AD Netbios Name", serviceConfigInfo.ConfigSettings[1].Name);
            Assert.AreEqual("Denallix", serviceConfigInfo.ConfigSettings[1].Value ?? string.Empty);
            Assert.AreEqual("Environment to use (empty for default)", serviceConfigInfo.ConfigSettings[2].DisplayName);
            Assert.AreEqual(false, serviceConfigInfo.ConfigSettings[2].IsRequired);
            Assert.AreEqual("Environment to use (empty for default)", serviceConfigInfo.ConfigSettings[2].Name);
            Assert.AreEqual("", serviceConfigInfo.ConfigSettings[2].Value ?? string.Empty);
            Assert.AreEqual("AD LDAP Path", serviceConfigInfo.ConfigSettings[3].DisplayName);
            Assert.AreEqual(false, serviceConfigInfo.ConfigSettings[3].IsRequired);
            Assert.AreEqual("AD LDAP Path", serviceConfigInfo.ConfigSettings[3].Name);
            Assert.AreEqual("LDAP://DC=denallix,DC=COM", serviceConfigInfo.ConfigSettings[3].Value ?? string.Empty);
            Assert.AreEqual("AD Maximum Resultsize", serviceConfigInfo.ConfigSettings[4].DisplayName);
            Assert.AreEqual(false, serviceConfigInfo.ConfigSettings[4].IsRequired);
            Assert.AreEqual("AD Maximum Resultsize", serviceConfigInfo.ConfigSettings[4].Name);
            Assert.AreEqual("1000", serviceConfigInfo.ConfigSettings[4].Value ?? string.Empty);
            Assert.AreEqual("Change Contains operator to StartsWith for AD", serviceConfigInfo.ConfigSettings[5].DisplayName);
            Assert.AreEqual(true, serviceConfigInfo.ConfigSettings[5].IsRequired);
            Assert.AreEqual("Change Contains operator to StartsWith for AD", serviceConfigInfo.ConfigSettings[5].Name);
            Assert.AreEqual("true", serviceConfigInfo.ConfigSettings[5].Value ?? string.Empty);
            Assert.AreEqual("Default Culture", serviceConfigInfo.ConfigSettings[6].DisplayName);
            Assert.AreEqual(true, serviceConfigInfo.ConfigSettings[6].IsRequired);
            Assert.AreEqual("Default Culture", serviceConfigInfo.ConfigSettings[6].Name);
            Assert.AreEqual("EN-us", serviceConfigInfo.ConfigSettings[6].Value ?? string.Empty);
            Assert.AreEqual("PowerShell Subdirectories", serviceConfigInfo.ConfigSettings[7].DisplayName);
            Assert.AreEqual(false, serviceConfigInfo.ConfigSettings[7].IsRequired);
            Assert.AreEqual("PowerShell Subdirectories", serviceConfigInfo.ConfigSettings[7].Name);
            Assert.AreEqual("PowerShellScripts", serviceConfigInfo.ConfigSettings[7].Value ?? string.Empty);
            Assert.AreEqual("Additional AD properties delimited by ;", serviceConfigInfo.ConfigSettings[8].DisplayName);
            Assert.AreEqual(false, serviceConfigInfo.ConfigSettings[8].IsRequired);
            Assert.AreEqual("Additional AD properties delimited by ;", serviceConfigInfo.ConfigSettings[8].Name);
            Assert.AreEqual("", serviceConfigInfo.ConfigSettings[8].Value ?? string.Empty);
            Assert.AreEqual("SMO data queries delimited by ;", serviceConfigInfo.ConfigSettings[9].DisplayName);
            Assert.AreEqual(false, serviceConfigInfo.ConfigSettings[9].IsRequired);
            Assert.AreEqual("SMO data queries delimited by ;", serviceConfigInfo.ConfigSettings[9].Name);
            Assert.AreEqual("", serviceConfigInfo.ConfigSettings[9].Value ?? string.Empty);
            Assert.AreEqual("Allow Simple PowerShell Scripts", serviceConfigInfo.ConfigSettings[10].DisplayName);
            Assert.AreEqual(true, serviceConfigInfo.ConfigSettings[10].IsRequired);
            Assert.AreEqual("Allow Simple PowerShell Scripts", serviceConfigInfo.ConfigSettings[10].Name);
            Assert.AreEqual("true", serviceConfigInfo.ConfigSettings[10].Value ?? string.Empty);
            Assert.AreEqual("Platform to use", serviceConfigInfo.ConfigSettings[11].DisplayName);
            Assert.AreEqual(false, serviceConfigInfo.ConfigSettings[11].IsRequired);
            Assert.AreEqual("Platform to use", serviceConfigInfo.ConfigSettings[11].Name);
            Assert.AreEqual("ASP", serviceConfigInfo.ConfigSettings[11].Value ?? string.Empty);
        }

        [TestMethod]
        [TestCategory("Describe K2NEServiceBroker")]
        public void Describe_A_D_User()
        {
            var serviceObject = SmartObjectHelper.GetServiceObject(K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance, "AD User");

            // SERVICEOBJECT: AD User
            Assert.AreEqual("AD User", serviceObject.Name);
            Assert.AreEqual("A D User", serviceObject.Metadata.DisplayName);
            Assert.AreEqual("", serviceObject.ServiceObjectType ?? string.Empty);

            // PROPERTIES: AD User
            Assert.AreEqual(15, serviceObject.Properties.Count);

            Assert.AreEqual("UserFQN", serviceObject.Properties[0].Name);
            Assert.AreEqual("User FQN", serviceObject.Properties[0].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[0].Type);

            Assert.AreEqual("SubStringSearchInput", serviceObject.Properties[1].Name);
            Assert.AreEqual("Sub String Search Input", serviceObject.Properties[1].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[1].Type);

            Assert.AreEqual("sAMAcountName", serviceObject.Properties[2].Name);
            Assert.AreEqual("sAM Acount Name", serviceObject.Properties[2].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[2].Type);

            Assert.AreEqual("Name", serviceObject.Properties[3].Name);
            Assert.AreEqual("Name", serviceObject.Properties[3].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[3].Type);

            Assert.AreEqual("Description", serviceObject.Properties[4].Name);
            Assert.AreEqual("Description", serviceObject.Properties[4].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[4].Type);

            Assert.AreEqual("Email", serviceObject.Properties[5].Name);
            Assert.AreEqual("Email", serviceObject.Properties[5].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[5].Type);

            Assert.AreEqual("Manager", serviceObject.Properties[6].Name);
            Assert.AreEqual("Manager", serviceObject.Properties[6].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[6].Type);

            Assert.AreEqual("ObjectSID", serviceObject.Properties[7].Name);
            Assert.AreEqual("Object SID", serviceObject.Properties[7].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[7].Type);

            Assert.AreEqual("DisplayName", serviceObject.Properties[8].Name);
            Assert.AreEqual("Display Name", serviceObject.Properties[8].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[8].Type);

            Assert.AreEqual("CommonName", serviceObject.Properties[9].Name);
            Assert.AreEqual("Common Name", serviceObject.Properties[9].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[9].Type);

            Assert.AreEqual("GivenName", serviceObject.Properties[10].Name);
            Assert.AreEqual("Given Name", serviceObject.Properties[10].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[10].Type);

            Assert.AreEqual("Initials", serviceObject.Properties[11].Name);
            Assert.AreEqual("Initials", serviceObject.Properties[11].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[11].Type);

            Assert.AreEqual("Surname", serviceObject.Properties[12].Name);
            Assert.AreEqual("Surname", serviceObject.Properties[12].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[12].Type);

            Assert.AreEqual("MaxResultSize", serviceObject.Properties[13].Name);
            Assert.AreEqual("Max Result Size", serviceObject.Properties[13].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[13].Type);

            Assert.AreEqual("OrganisationalUnit", serviceObject.Properties[14].Name);
            Assert.AreEqual("Organisational Unit", serviceObject.Properties[14].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[14].Type);

            // METHODS: AD User
            Assert.AreEqual(4, serviceObject.Methods.Count);

            // Method: GetUsers
            Assert.AreEqual("Get Users", serviceObject.Methods[0].Metadata.DisplayName);
            Assert.AreEqual("GetUsers", serviceObject.Methods[0].Name);
            Assert.AreEqual(MethodDefinitionType.list, serviceObject.Methods[0].Type);

            // Input Properties: GetUsers
            Assert.AreEqual(4, serviceObject.Methods[0].InputProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[0].InputProperties[0].Name);
            Assert.AreEqual("DisplayName", serviceObject.Methods[0].InputProperties[1].Name);
            Assert.AreEqual("Email", serviceObject.Methods[0].InputProperties[2].Name);
            Assert.AreEqual("MaxResultSize", serviceObject.Methods[0].InputProperties[3].Name);

            // Required Properties: GetUsers
            Assert.AreEqual(0, serviceObject.Methods[0].Validation.RequiredProperties.Count);

            // Parameters: GetUsers
            Assert.AreEqual(0, serviceObject.Methods[0].Parameters.Count);

            // Return Properties: GetUsers
            Assert.AreEqual(4, serviceObject.Methods[0].ReturnProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[0].ReturnProperties[0].Name);
            Assert.AreEqual("sAMAcountName", serviceObject.Methods[0].ReturnProperties[1].Name);
            Assert.AreEqual("DisplayName", serviceObject.Methods[0].ReturnProperties[2].Name);
            Assert.AreEqual("Email", serviceObject.Methods[0].ReturnProperties[3].Name);

            // Method: GetUserDetails
            Assert.AreEqual("Get User Details", serviceObject.Methods[1].Metadata.DisplayName);
            Assert.AreEqual("GetUserDetails", serviceObject.Methods[1].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[1].Type);

            // Input Properties: GetUserDetails
            Assert.AreEqual(1, serviceObject.Methods[1].InputProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[1].InputProperties[0].Name);

            // Required Properties: GetUserDetails
            Assert.AreEqual(1, serviceObject.Methods[1].Validation.RequiredProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[1].Validation.RequiredProperties[0].Name);

            // Parameters: GetUserDetails
            Assert.AreEqual(0, serviceObject.Methods[1].Parameters.Count);

            // Return Properties: GetUserDetails
            Assert.AreEqual(8, serviceObject.Methods[1].ReturnProperties.Count);
            Assert.AreEqual("sAMAcountName", serviceObject.Methods[1].ReturnProperties[0].Name);
            Assert.AreEqual("DisplayName", serviceObject.Methods[1].ReturnProperties[1].Name);
            Assert.AreEqual("CommonName", serviceObject.Methods[1].ReturnProperties[2].Name);
            Assert.AreEqual("GivenName", serviceObject.Methods[1].ReturnProperties[3].Name);
            Assert.AreEqual("Initials", serviceObject.Methods[1].ReturnProperties[4].Name);
            Assert.AreEqual("Surname", serviceObject.Methods[1].ReturnProperties[5].Name);
            Assert.AreEqual("Email", serviceObject.Methods[1].ReturnProperties[6].Name);
            Assert.AreEqual("OrganisationalUnit", serviceObject.Methods[1].ReturnProperties[7].Name);

            // Method: SearchUsers
            Assert.AreEqual("Search Users", serviceObject.Methods[2].Metadata.DisplayName);
            Assert.AreEqual("SearchUsers", serviceObject.Methods[2].Name);
            Assert.AreEqual(MethodDefinitionType.list, serviceObject.Methods[2].Type);

            // Input Properties: SearchUsers
            Assert.AreEqual(2, serviceObject.Methods[2].InputProperties.Count);
            Assert.AreEqual("SubStringSearchInput", serviceObject.Methods[2].InputProperties[0].Name);
            Assert.AreEqual("MaxResultSize", serviceObject.Methods[2].InputProperties[1].Name);

            // Required Properties: SearchUsers
            Assert.AreEqual(1, serviceObject.Methods[2].Validation.RequiredProperties.Count);
            Assert.AreEqual("SubStringSearchInput", serviceObject.Methods[2].Validation.RequiredProperties[0].Name);

            // Parameters: SearchUsers
            Assert.AreEqual(0, serviceObject.Methods[2].Parameters.Count);

            // Return Properties: SearchUsers
            Assert.AreEqual(4, serviceObject.Methods[2].ReturnProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[2].ReturnProperties[0].Name);
            Assert.AreEqual("sAMAcountName", serviceObject.Methods[2].ReturnProperties[1].Name);
            Assert.AreEqual("DisplayName", serviceObject.Methods[2].ReturnProperties[2].Name);
            Assert.AreEqual("Email", serviceObject.Methods[2].ReturnProperties[3].Name);

            // Method: UMGetUsers
            Assert.AreEqual("UM Get Users", serviceObject.Methods[3].Metadata.DisplayName);
            Assert.AreEqual("UMGetUsers", serviceObject.Methods[3].Name);
            Assert.AreEqual(MethodDefinitionType.list, serviceObject.Methods[3].Type);

            // Input Properties: UMGetUsers
            Assert.AreEqual(6, serviceObject.Methods[3].InputProperties.Count);
            Assert.AreEqual("Name", serviceObject.Methods[3].InputProperties[0].Name);
            Assert.AreEqual("Description", serviceObject.Methods[3].InputProperties[1].Name);
            Assert.AreEqual("Email", serviceObject.Methods[3].InputProperties[2].Name);
            Assert.AreEqual("Manager", serviceObject.Methods[3].InputProperties[3].Name);
            Assert.AreEqual("DisplayName", serviceObject.Methods[3].InputProperties[4].Name);
            Assert.AreEqual("MaxResultSize", serviceObject.Methods[3].InputProperties[5].Name);

            // Required Properties: UMGetUsers
            Assert.AreEqual(0, serviceObject.Methods[3].Validation.RequiredProperties.Count);

            // Parameters: UMGetUsers
            Assert.AreEqual(1, serviceObject.Methods[3].Parameters.Count);
            Assert.AreEqual("Label", serviceObject.Methods[3].Parameters[0].Name);
            Assert.AreEqual("Label", serviceObject.Methods[3].Parameters[0].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Methods[3].Parameters[0].Type);
            Assert.AreEqual(true, serviceObject.Methods[3].Parameters[0].IsRequired);

            // Return Properties: UMGetUsers
            Assert.AreEqual(7, serviceObject.Methods[3].ReturnProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[3].ReturnProperties[0].Name);
            Assert.AreEqual("Name", serviceObject.Methods[3].ReturnProperties[1].Name);
            Assert.AreEqual("Description", serviceObject.Methods[3].ReturnProperties[2].Name);
            Assert.AreEqual("DisplayName", serviceObject.Methods[3].ReturnProperties[3].Name);
            Assert.AreEqual("Email", serviceObject.Methods[3].ReturnProperties[4].Name);
            Assert.AreEqual("Manager", serviceObject.Methods[3].ReturnProperties[5].Name);
            Assert.AreEqual("ObjectSID", serviceObject.Methods[3].ReturnProperties[6].Name);

            // ASSOCIATIONS: AD User
            Assert.AreEqual(0, serviceObject.Associations.Count);
        }

        [TestMethod]
        [TestCategory("Describe K2NEServiceBroker")]
        public void Describe_Error_Log()
        {
            var serviceObject = SmartObjectHelper.GetServiceObject(K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance, "ErrorLog");

            // SERVICEOBJECT: ErrorLog
            Assert.AreEqual("ErrorLog", serviceObject.Name);
            Assert.AreEqual("Error Log", serviceObject.Metadata.DisplayName);
            Assert.AreEqual("", serviceObject.ServiceObjectType ?? string.Empty);

            // PROPERTIES: ErrorLog
            Assert.AreEqual(12, serviceObject.Properties.Count);

            Assert.AreEqual("Profile", serviceObject.Properties[0].Name);
            Assert.AreEqual("Profile", serviceObject.Properties[0].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[0].Type);

            Assert.AreEqual("ProcessInstanceId", serviceObject.Properties[1].Name);
            Assert.AreEqual("Process Instance Id", serviceObject.Properties[1].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[1].Type);

            Assert.AreEqual("ProcessDefinitionName", serviceObject.Properties[2].Name);
            Assert.AreEqual("Process Definition Name", serviceObject.Properties[2].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[2].Type);

            Assert.AreEqual("Folio", serviceObject.Properties[3].Name);
            Assert.AreEqual("Folio", serviceObject.Properties[3].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[3].Type);

            Assert.AreEqual("ErrorDescription", serviceObject.Properties[4].Name);
            Assert.AreEqual("Error Description", serviceObject.Properties[4].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[4].Type);

            Assert.AreEqual("ErrorItem", serviceObject.Properties[5].Name);
            Assert.AreEqual("Error Item", serviceObject.Properties[5].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[5].Type);

            Assert.AreEqual("ErrorId", serviceObject.Properties[6].Name);
            Assert.AreEqual("Error Id", serviceObject.Properties[6].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[6].Type);

            Assert.AreEqual("ErrorDate", serviceObject.Properties[7].Name);
            Assert.AreEqual("Error Date", serviceObject.Properties[7].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.DateTime, serviceObject.Properties[7].Type);

            Assert.AreEqual("TryNewVersion", serviceObject.Properties[8].Name);
            Assert.AreEqual("Try New Version", serviceObject.Properties[8].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.YesNo, serviceObject.Properties[8].Type);

            Assert.AreEqual("TypeDescription", serviceObject.Properties[9].Name);
            Assert.AreEqual("Type Description", serviceObject.Properties[9].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[9].Type);

            Assert.AreEqual("StackTrace", serviceObject.Properties[10].Name);
            Assert.AreEqual("Stack Trace", serviceObject.Properties[10].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Memo, serviceObject.Properties[10].Type);

            Assert.AreEqual("ExecutingProcId", serviceObject.Properties[11].Name);
            Assert.AreEqual("Executing Proc Id", serviceObject.Properties[11].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[11].Type);

            // METHODS: ErrorLog
            Assert.AreEqual(2, serviceObject.Methods.Count);

            // Method: GetErrors
            Assert.AreEqual("Get Errors", serviceObject.Methods[0].Metadata.DisplayName);
            Assert.AreEqual("GetErrors", serviceObject.Methods[0].Name);
            Assert.AreEqual(MethodDefinitionType.list, serviceObject.Methods[0].Type);

            // Input Properties: GetErrors
            Assert.AreEqual(1, serviceObject.Methods[0].InputProperties.Count);
            Assert.AreEqual("Profile", serviceObject.Methods[0].InputProperties[0].Name);

            // Required Properties: GetErrors
            Assert.AreEqual(0, serviceObject.Methods[0].Validation.RequiredProperties.Count);

            // Parameters: GetErrors
            Assert.AreEqual(0, serviceObject.Methods[0].Parameters.Count);

            // Return Properties: GetErrors
            Assert.AreEqual(10, serviceObject.Methods[0].ReturnProperties.Count);
            Assert.AreEqual("ProcessInstanceId", serviceObject.Methods[0].ReturnProperties[0].Name);
            Assert.AreEqual("ProcessDefinitionName", serviceObject.Methods[0].ReturnProperties[1].Name);
            Assert.AreEqual("Folio", serviceObject.Methods[0].ReturnProperties[2].Name);
            Assert.AreEqual("ErrorDescription", serviceObject.Methods[0].ReturnProperties[3].Name);
            Assert.AreEqual("ErrorItem", serviceObject.Methods[0].ReturnProperties[4].Name);
            Assert.AreEqual("ErrorDate", serviceObject.Methods[0].ReturnProperties[5].Name);
            Assert.AreEqual("ErrorId", serviceObject.Methods[0].ReturnProperties[6].Name);
            Assert.AreEqual("TypeDescription", serviceObject.Methods[0].ReturnProperties[7].Name);
            Assert.AreEqual("StackTrace", serviceObject.Methods[0].ReturnProperties[8].Name);
            Assert.AreEqual("ExecutingProcId", serviceObject.Methods[0].ReturnProperties[9].Name);

            // Method: RetryProcessInstance
            Assert.AreEqual("Retry Process Instance", serviceObject.Methods[1].Metadata.DisplayName);
            Assert.AreEqual("RetryProcessInstance", serviceObject.Methods[1].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[1].Type);

            // Input Properties: RetryProcessInstance
            Assert.AreEqual(2, serviceObject.Methods[1].InputProperties.Count);
            Assert.AreEqual("ProcessInstanceId", serviceObject.Methods[1].InputProperties[0].Name);
            Assert.AreEqual("TryNewVersion", serviceObject.Methods[1].InputProperties[1].Name);

            // Required Properties: RetryProcessInstance
            Assert.AreEqual(1, serviceObject.Methods[1].Validation.RequiredProperties.Count);
            Assert.AreEqual("ProcessInstanceId", serviceObject.Methods[1].Validation.RequiredProperties[0].Name);

            // Parameters: RetryProcessInstance
            Assert.AreEqual(0, serviceObject.Methods[1].Parameters.Count);

            // Return Properties: RetryProcessInstance
            Assert.AreEqual(0, serviceObject.Methods[1].ReturnProperties.Count);

            // ASSOCIATIONS: ErrorLog
            Assert.AreEqual(0, serviceObject.Associations.Count);
        }

        [TestMethod]
        [TestCategory("Describe K2NEServiceBroker")]
        public void Describe_Identity()
        {
            var serviceObject = SmartObjectHelper.GetServiceObject(K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance, "Identity");

            // SERVICEOBJECT: Identity
            Assert.AreEqual("Identity", serviceObject.Name);
            Assert.AreEqual("Identity", serviceObject.Metadata.DisplayName);
            Assert.AreEqual("", serviceObject.ServiceObjectType ?? string.Empty);

            // PROPERTIES: Identity
            Assert.AreEqual(28, serviceObject.Properties.Count);

            Assert.AreEqual("CurrentPrincipalAuthType =", serviceObject.Properties[0].Name);
            Assert.AreEqual("Current Principal Auth Type =", serviceObject.Properties[0].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[0].Type);

            Assert.AreEqual("CurrentPrincipalName", serviceObject.Properties[1].Name);
            Assert.AreEqual("Current Principal Name", serviceObject.Properties[1].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[1].Type);

            Assert.AreEqual("CurrentPrincipalIdentityType", serviceObject.Properties[2].Name);
            Assert.AreEqual("Current Principal Identity Type", serviceObject.Properties[2].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[2].Type);

            Assert.AreEqual("FQN", serviceObject.Properties[3].Name);
            Assert.AreEqual("FQN", serviceObject.Properties[3].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[3].Type);

            Assert.AreEqual("ResolveContainers", serviceObject.Properties[4].Name);
            Assert.AreEqual("Resolve Containers", serviceObject.Properties[4].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.YesNo, serviceObject.Properties[4].Type);

            Assert.AreEqual("ResolveMembers", serviceObject.Properties[5].Name);
            Assert.AreEqual("Resolve Members", serviceObject.Properties[5].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.YesNo, serviceObject.Properties[5].Type);

            Assert.AreEqual("UserDescription", serviceObject.Properties[6].Name);
            Assert.AreEqual("User Description", serviceObject.Properties[6].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[6].Type);

            Assert.AreEqual("UserDisplayName", serviceObject.Properties[7].Name);
            Assert.AreEqual("User Display Name", serviceObject.Properties[7].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[7].Type);

            Assert.AreEqual("UserEmail", serviceObject.Properties[8].Name);
            Assert.AreEqual("User Email", serviceObject.Properties[8].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[8].Type);

            Assert.AreEqual("UserManager", serviceObject.Properties[9].Name);
            Assert.AreEqual("User Manager", serviceObject.Properties[9].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[9].Type);

            Assert.AreEqual("UserName", serviceObject.Properties[10].Name);
            Assert.AreEqual("User Name", serviceObject.Properties[10].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[10].Type);

            Assert.AreEqual("UserUserLabel", serviceObject.Properties[11].Name);
            Assert.AreEqual("User User Label", serviceObject.Properties[11].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[11].Type);

            Assert.AreEqual("CallingFQN", serviceObject.Properties[12].Name);
            Assert.AreEqual("Calling FQN", serviceObject.Properties[12].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[12].Type);

            Assert.AreEqual("WindowsIdentityAuthType", serviceObject.Properties[13].Name);
            Assert.AreEqual("Windows Identity Auth Type", serviceObject.Properties[13].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[13].Type);

            Assert.AreEqual("WindowsIdentityName", serviceObject.Properties[14].Name);
            Assert.AreEqual("Windows Identity Name", serviceObject.Properties[14].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[14].Type);

            Assert.AreEqual("ServiceBrokerAuthType", serviceObject.Properties[15].Name);
            Assert.AreEqual("Service Broker Auth Type", serviceObject.Properties[15].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[15].Type);

            Assert.AreEqual("ServiceBrokerUserName", serviceObject.Properties[16].Name);
            Assert.AreEqual("Service Broker User Name", serviceObject.Properties[16].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[16].Type);

            Assert.AreEqual("ServiceBrokerPassword", serviceObject.Properties[17].Name);
            Assert.AreEqual("Service Broker Password", serviceObject.Properties[17].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[17].Type);

            Assert.AreEqual("UserWindowsImpersonation", serviceObject.Properties[18].Name);
            Assert.AreEqual("User Windows Impersonation", serviceObject.Properties[18].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.YesNo, serviceObject.Properties[18].Type);

            Assert.AreEqual("DefaultNetworkCredentialsDomain", serviceObject.Properties[19].Name);
            Assert.AreEqual("Default Network Credentials Domain", serviceObject.Properties[19].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[19].Type);

            Assert.AreEqual("DefaultNetworkCredentialsPassword", serviceObject.Properties[20].Name);
            Assert.AreEqual("Default Network Credentials Password", serviceObject.Properties[20].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[20].Type);

            Assert.AreEqual("DefaultNetworkCredentialsUsername", serviceObject.Properties[21].Name);
            Assert.AreEqual("Default Network Credentials Username", serviceObject.Properties[21].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[21].Type);

            Assert.AreEqual("UserCultureDateTimeFormat", serviceObject.Properties[22].Name);
            Assert.AreEqual("User Culture Date Time Format", serviceObject.Properties[22].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[22].Type);

            Assert.AreEqual("UserCultureDisplayName", serviceObject.Properties[23].Name);
            Assert.AreEqual("User Culture Display Name", serviceObject.Properties[23].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[23].Type);

            Assert.AreEqual("UserCultureLCID", serviceObject.Properties[24].Name);
            Assert.AreEqual("User Culture LCID", serviceObject.Properties[24].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[24].Type);

            Assert.AreEqual("UserCultureName", serviceObject.Properties[25].Name);
            Assert.AreEqual("User Culture Name", serviceObject.Properties[25].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[25].Type);

            Assert.AreEqual("UserCultureNumberFormat", serviceObject.Properties[26].Name);
            Assert.AreEqual("User Culture Number Format", serviceObject.Properties[26].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[26].Type);

            Assert.AreEqual("K2ImpersonateUser", serviceObject.Properties[27].Name);
            Assert.AreEqual("K2 Impersonate User", serviceObject.Properties[27].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[27].Type);

            // METHODS: Identity
            Assert.AreEqual(6, serviceObject.Methods.Count);

            // Method: ReadWorkflowClientIdentity
            Assert.AreEqual("Read Workflow Client Identity", serviceObject.Methods[0].Metadata.DisplayName);
            Assert.AreEqual("ReadWorkflowClientIdentity", serviceObject.Methods[0].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[0].Type);

            // Input Properties: ReadWorkflowClientIdentity
            Assert.AreEqual(2, serviceObject.Methods[0].InputProperties.Count);
            Assert.AreEqual("UserWindowsImpersonation", serviceObject.Methods[0].InputProperties[0].Name);
            Assert.AreEqual("K2ImpersonateUser", serviceObject.Methods[0].InputProperties[1].Name);

            // Required Properties: ReadWorkflowClientIdentity
            Assert.AreEqual(0, serviceObject.Methods[0].Validation.RequiredProperties.Count);

            // Parameters: ReadWorkflowClientIdentity
            Assert.AreEqual(0, serviceObject.Methods[0].Parameters.Count);

            // Return Properties: ReadWorkflowClientIdentity
            Assert.AreEqual(8, serviceObject.Methods[0].ReturnProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[0].ReturnProperties[0].Name);
            Assert.AreEqual("UserDescription", serviceObject.Methods[0].ReturnProperties[1].Name);
            Assert.AreEqual("UserDisplayName", serviceObject.Methods[0].ReturnProperties[2].Name);
            Assert.AreEqual("UserEmail", serviceObject.Methods[0].ReturnProperties[3].Name);
            Assert.AreEqual("UserManager", serviceObject.Methods[0].ReturnProperties[4].Name);
            Assert.AreEqual("UserName", serviceObject.Methods[0].ReturnProperties[5].Name);
            Assert.AreEqual("UserUserLabel", serviceObject.Methods[0].ReturnProperties[6].Name);
            Assert.AreEqual("CallingFQN", serviceObject.Methods[0].ReturnProperties[7].Name);

            // Method: ReadThreadIdentity
            Assert.AreEqual("Read Thread Identity", serviceObject.Methods[1].Metadata.DisplayName);
            Assert.AreEqual("ReadThreadIdentity", serviceObject.Methods[1].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[1].Type);

            // Input Properties: ReadThreadIdentity
            Assert.AreEqual(1, serviceObject.Methods[1].InputProperties.Count);
            Assert.AreEqual("UserWindowsImpersonation", serviceObject.Methods[1].InputProperties[0].Name);

            // Required Properties: ReadThreadIdentity
            Assert.AreEqual(0, serviceObject.Methods[1].Validation.RequiredProperties.Count);

            // Parameters: ReadThreadIdentity
            Assert.AreEqual(0, serviceObject.Methods[1].Parameters.Count);

            // Return Properties: ReadThreadIdentity
            Assert.AreEqual(17, serviceObject.Methods[1].ReturnProperties.Count);
            Assert.AreEqual("CallingFQN", serviceObject.Methods[1].ReturnProperties[0].Name);
            Assert.AreEqual("CurrentPrincipalAuthType =", serviceObject.Methods[1].ReturnProperties[1].Name);
            Assert.AreEqual("CurrentPrincipalName", serviceObject.Methods[1].ReturnProperties[2].Name);
            Assert.AreEqual("CurrentPrincipalIdentityType", serviceObject.Methods[1].ReturnProperties[3].Name);
            Assert.AreEqual("WindowsIdentityAuthType", serviceObject.Methods[1].ReturnProperties[4].Name);
            Assert.AreEqual("WindowsIdentityName", serviceObject.Methods[1].ReturnProperties[5].Name);
            Assert.AreEqual("ServiceBrokerAuthType", serviceObject.Methods[1].ReturnProperties[6].Name);
            Assert.AreEqual("ServiceBrokerUserName", serviceObject.Methods[1].ReturnProperties[7].Name);
            Assert.AreEqual("ServiceBrokerPassword", serviceObject.Methods[1].ReturnProperties[8].Name);
            Assert.AreEqual("DefaultNetworkCredentialsDomain", serviceObject.Methods[1].ReturnProperties[9].Name);
            Assert.AreEqual("DefaultNetworkCredentialsPassword", serviceObject.Methods[1].ReturnProperties[10].Name);
            Assert.AreEqual("DefaultNetworkCredentialsUsername", serviceObject.Methods[1].ReturnProperties[11].Name);
            Assert.AreEqual("UserCultureDateTimeFormat", serviceObject.Methods[1].ReturnProperties[12].Name);
            Assert.AreEqual("UserCultureDisplayName", serviceObject.Methods[1].ReturnProperties[13].Name);
            Assert.AreEqual("UserCultureLCID", serviceObject.Methods[1].ReturnProperties[14].Name);
            Assert.AreEqual("UserCultureName", serviceObject.Methods[1].ReturnProperties[15].Name);
            Assert.AreEqual("UserCultureNumberFormat", serviceObject.Methods[1].ReturnProperties[16].Name);

            // Method: GetEmailFromIdentity
            Assert.AreEqual("Get Email From Identity", serviceObject.Methods[2].Metadata.DisplayName);
            Assert.AreEqual("GetEmailFromIdentity", serviceObject.Methods[2].Name);
            Assert.AreEqual(MethodDefinitionType.list, serviceObject.Methods[2].Type);

            // Input Properties: GetEmailFromIdentity
            Assert.AreEqual(1, serviceObject.Methods[2].InputProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[2].InputProperties[0].Name);

            // Required Properties: GetEmailFromIdentity
            Assert.AreEqual(1, serviceObject.Methods[2].Validation.RequiredProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[2].Validation.RequiredProperties[0].Name);

            // Parameters: GetEmailFromIdentity
            Assert.AreEqual(0, serviceObject.Methods[2].Parameters.Count);

            // Return Properties: GetEmailFromIdentity
            Assert.AreEqual(3, serviceObject.Methods[2].ReturnProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[2].ReturnProperties[0].Name);
            Assert.AreEqual("UserEmail", serviceObject.Methods[2].ReturnProperties[1].Name);
            Assert.AreEqual("UserDisplayName", serviceObject.Methods[2].ReturnProperties[2].Name);

            // Method: ResolveUserIdentity
            Assert.AreEqual("Resolve User Identity", serviceObject.Methods[3].Metadata.DisplayName);
            Assert.AreEqual("ResolveUserIdentity", serviceObject.Methods[3].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[3].Type);

            // Input Properties: ResolveUserIdentity
            Assert.AreEqual(2, serviceObject.Methods[3].InputProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[3].InputProperties[0].Name);
            Assert.AreEqual("ResolveContainers", serviceObject.Methods[3].InputProperties[1].Name);

            // Required Properties: ResolveUserIdentity
            Assert.AreEqual(1, serviceObject.Methods[3].Validation.RequiredProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[3].Validation.RequiredProperties[0].Name);

            // Parameters: ResolveUserIdentity
            Assert.AreEqual(0, serviceObject.Methods[3].Parameters.Count);

            // Return Properties: ResolveUserIdentity
            Assert.AreEqual(0, serviceObject.Methods[3].ReturnProperties.Count);

            // Method: ResolveGroupIdentity
            Assert.AreEqual("Resolve Group Identity", serviceObject.Methods[4].Metadata.DisplayName);
            Assert.AreEqual("ResolveGroupIdentity", serviceObject.Methods[4].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[4].Type);

            // Input Properties: ResolveGroupIdentity
            Assert.AreEqual(3, serviceObject.Methods[4].InputProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[4].InputProperties[0].Name);
            Assert.AreEqual("ResolveContainers", serviceObject.Methods[4].InputProperties[1].Name);
            Assert.AreEqual("ResolveMembers", serviceObject.Methods[4].InputProperties[2].Name);

            // Required Properties: ResolveGroupIdentity
            Assert.AreEqual(1, serviceObject.Methods[4].Validation.RequiredProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[4].Validation.RequiredProperties[0].Name);

            // Parameters: ResolveGroupIdentity
            Assert.AreEqual(0, serviceObject.Methods[4].Parameters.Count);

            // Return Properties: ResolveGroupIdentity
            Assert.AreEqual(0, serviceObject.Methods[4].ReturnProperties.Count);

            // Method: ResolveRoleIdentity
            Assert.AreEqual("Resolve Role Identity", serviceObject.Methods[5].Metadata.DisplayName);
            Assert.AreEqual("ResolveRoleIdentity", serviceObject.Methods[5].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[5].Type);

            // Input Properties: ResolveRoleIdentity
            Assert.AreEqual(3, serviceObject.Methods[5].InputProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[5].InputProperties[0].Name);
            Assert.AreEqual("ResolveContainers", serviceObject.Methods[5].InputProperties[1].Name);
            Assert.AreEqual("ResolveMembers", serviceObject.Methods[5].InputProperties[2].Name);

            // Required Properties: ResolveRoleIdentity
            Assert.AreEqual(1, serviceObject.Methods[5].Validation.RequiredProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[5].Validation.RequiredProperties[0].Name);

            // Parameters: ResolveRoleIdentity
            Assert.AreEqual(0, serviceObject.Methods[5].Parameters.Count);

            // Return Properties: ResolveRoleIdentity
            Assert.AreEqual(0, serviceObject.Methods[5].ReturnProperties.Count);

            // ASSOCIATIONS: Identity
            Assert.AreEqual(0, serviceObject.Associations.Count);
        }

        [TestMethod]
        [TestCategory("Describe K2NEServiceBroker")]
        public void Describe_Management_Worklist()
        {
            var serviceObject = SmartObjectHelper.GetServiceObject(K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance, "ManagementWorklist");

            // SERVICEOBJECT: ManagementWorklist
            Assert.AreEqual("ManagementWorklist", serviceObject.Name);
            Assert.AreEqual("Management Worklist", serviceObject.Metadata.DisplayName);
            Assert.AreEqual("", serviceObject.ServiceObjectType ?? string.Empty);

            // PROPERTIES: ManagementWorklist
            Assert.AreEqual(17, serviceObject.Properties.Count);

            Assert.AreEqual("ActivityId", serviceObject.Properties[0].Name);
            Assert.AreEqual("Activity Id", serviceObject.Properties[0].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[0].Type);

            Assert.AreEqual("ActivityInstanceDestinationId", serviceObject.Properties[1].Name);
            Assert.AreEqual("Activity Instance Destination Id", serviceObject.Properties[1].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[1].Type);

            Assert.AreEqual("ActivityInstanceId", serviceObject.Properties[2].Name);
            Assert.AreEqual("Activity Instance Id", serviceObject.Properties[2].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[2].Type);

            Assert.AreEqual("ActivityName", serviceObject.Properties[3].Name);
            Assert.AreEqual("Activity Name", serviceObject.Properties[3].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[3].Type);

            Assert.AreEqual("Destination", serviceObject.Properties[4].Name);
            Assert.AreEqual("Destination", serviceObject.Properties[4].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[4].Type);

            Assert.AreEqual("DestinationType", serviceObject.Properties[5].Name);
            Assert.AreEqual("Destination Type", serviceObject.Properties[5].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[5].Type);

            Assert.AreEqual("EventId", serviceObject.Properties[6].Name);
            Assert.AreEqual("Event Id", serviceObject.Properties[6].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[6].Type);

            Assert.AreEqual("EventName", serviceObject.Properties[7].Name);
            Assert.AreEqual("Event Name", serviceObject.Properties[7].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[7].Type);

            Assert.AreEqual("Folio", serviceObject.Properties[8].Name);
            Assert.AreEqual("Folio", serviceObject.Properties[8].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[8].Type);

            Assert.AreEqual("ProcessInstanceId", serviceObject.Properties[9].Name);
            Assert.AreEqual("Process Instance Id", serviceObject.Properties[9].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[9].Type);

            Assert.AreEqual("ProcessName", serviceObject.Properties[10].Name);
            Assert.AreEqual("Process Name", serviceObject.Properties[10].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[10].Type);

            Assert.AreEqual("ProcessStatus", serviceObject.Properties[11].Name);
            Assert.AreEqual("Process Status", serviceObject.Properties[11].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[11].Type);

            Assert.AreEqual("StartDate", serviceObject.Properties[12].Name);
            Assert.AreEqual("Start Date", serviceObject.Properties[12].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.DateTime, serviceObject.Properties[12].Type);

            Assert.AreEqual("Status", serviceObject.Properties[13].Name);
            Assert.AreEqual("Status", serviceObject.Properties[13].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[13].Type);

            Assert.AreEqual("Id", serviceObject.Properties[14].Name);
            Assert.AreEqual("Id", serviceObject.Properties[14].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[14].Type);

            Assert.AreEqual("FromUser", serviceObject.Properties[15].Name);
            Assert.AreEqual("From User", serviceObject.Properties[15].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[15].Type);

            Assert.AreEqual("ToUser", serviceObject.Properties[16].Name);
            Assert.AreEqual("To User", serviceObject.Properties[16].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[16].Type);

            // METHODS: ManagementWorklist
            Assert.AreEqual(3, serviceObject.Methods.Count);

            // Method: GetWorklist
            Assert.AreEqual("Get Worklist", serviceObject.Methods[0].Metadata.DisplayName);
            Assert.AreEqual("GetWorklist", serviceObject.Methods[0].Name);
            Assert.AreEqual(MethodDefinitionType.list, serviceObject.Methods[0].Type);

            // Input Properties: GetWorklist
            Assert.AreEqual(0, serviceObject.Methods[0].InputProperties.Count);

            // Required Properties: GetWorklist
            Assert.AreEqual(0, serviceObject.Methods[0].Validation.RequiredProperties.Count);

            // Parameters: GetWorklist
            Assert.AreEqual(0, serviceObject.Methods[0].Parameters.Count);

            // Return Properties: GetWorklist
            Assert.AreEqual(15, serviceObject.Methods[0].ReturnProperties.Count);
            Assert.AreEqual("ActivityId", serviceObject.Methods[0].ReturnProperties[0].Name);
            Assert.AreEqual("ActivityInstanceDestinationId", serviceObject.Methods[0].ReturnProperties[1].Name);
            Assert.AreEqual("ActivityInstanceId", serviceObject.Methods[0].ReturnProperties[2].Name);
            Assert.AreEqual("ActivityName", serviceObject.Methods[0].ReturnProperties[3].Name);
            Assert.AreEqual("Destination", serviceObject.Methods[0].ReturnProperties[4].Name);
            Assert.AreEqual("DestinationType", serviceObject.Methods[0].ReturnProperties[5].Name);
            Assert.AreEqual("EventId", serviceObject.Methods[0].ReturnProperties[6].Name);
            Assert.AreEqual("EventName", serviceObject.Methods[0].ReturnProperties[7].Name);
            Assert.AreEqual("Folio", serviceObject.Methods[0].ReturnProperties[8].Name);
            Assert.AreEqual("ProcessInstanceId", serviceObject.Methods[0].ReturnProperties[9].Name);
            Assert.AreEqual("ProcessName", serviceObject.Methods[0].ReturnProperties[10].Name);
            Assert.AreEqual("ProcessStatus", serviceObject.Methods[0].ReturnProperties[11].Name);
            Assert.AreEqual("StartDate", serviceObject.Methods[0].ReturnProperties[12].Name);
            Assert.AreEqual("Status", serviceObject.Methods[0].ReturnProperties[13].Name);
            Assert.AreEqual("Id", serviceObject.Methods[0].ReturnProperties[14].Name);

            // Method: RedirectWorklistItem
            Assert.AreEqual("Redirect Worklist Item", serviceObject.Methods[1].Metadata.DisplayName);
            Assert.AreEqual("RedirectWorklistItem", serviceObject.Methods[1].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[1].Type);

            // Input Properties: RedirectWorklistItem
            Assert.AreEqual(5, serviceObject.Methods[1].InputProperties.Count);
            Assert.AreEqual("ProcessInstanceId", serviceObject.Methods[1].InputProperties[0].Name);
            Assert.AreEqual("Id", serviceObject.Methods[1].InputProperties[1].Name);
            Assert.AreEqual("ActivityInstanceDestinationId", serviceObject.Methods[1].InputProperties[2].Name);
            Assert.AreEqual("FromUser", serviceObject.Methods[1].InputProperties[3].Name);
            Assert.AreEqual("ToUser", serviceObject.Methods[1].InputProperties[4].Name);

            // Required Properties: RedirectWorklistItem
            Assert.AreEqual(0, serviceObject.Methods[1].Validation.RequiredProperties.Count);

            // Parameters: RedirectWorklistItem
            Assert.AreEqual(0, serviceObject.Methods[1].Parameters.Count);

            // Return Properties: RedirectWorklistItem
            Assert.AreEqual(0, serviceObject.Methods[1].ReturnProperties.Count);

            // Method: ReleaseWorklistItem
            Assert.AreEqual("Release Worklist Item", serviceObject.Methods[2].Metadata.DisplayName);
            Assert.AreEqual("ReleaseWorklistItem", serviceObject.Methods[2].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[2].Type);

            // Input Properties: ReleaseWorklistItem
            Assert.AreEqual(1, serviceObject.Methods[2].InputProperties.Count);
            Assert.AreEqual("Id", serviceObject.Methods[2].InputProperties[0].Name);

            // Required Properties: ReleaseWorklistItem
            Assert.AreEqual(0, serviceObject.Methods[2].Validation.RequiredProperties.Count);

            // Parameters: ReleaseWorklistItem
            Assert.AreEqual(0, serviceObject.Methods[2].Parameters.Count);

            // Return Properties: ReleaseWorklistItem
            Assert.AreEqual(0, serviceObject.Methods[2].ReturnProperties.Count);

            // ASSOCIATIONS: ManagementWorklist
            Assert.AreEqual(0, serviceObject.Associations.Count);
        }

        [TestMethod]
        [TestCategory("Describe K2NEServiceBroker")]
        public void Describe_Out_Of_Office_Client()
        {
            var serviceObject = SmartObjectHelper.GetServiceObject(K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance, "OutOfOfficeClient");

            // SERVICEOBJECT: OutOfOfficeClient
            Assert.AreEqual("OutOfOfficeClient", serviceObject.Name);
            Assert.AreEqual("Out Of Office Client", serviceObject.Metadata.DisplayName);
            Assert.AreEqual("", serviceObject.ServiceObjectType ?? string.Empty);

            // PROPERTIES: OutOfOfficeClient
            Assert.AreEqual(2, serviceObject.Properties.Count);

            Assert.AreEqual("DestinationUser", serviceObject.Properties[0].Name);
            Assert.AreEqual("Destination User", serviceObject.Properties[0].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[0].Type);

            Assert.AreEqual("UserStatus", serviceObject.Properties[1].Name);
            Assert.AreEqual("User Status", serviceObject.Properties[1].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[1].Type);

            // METHODS: OutOfOfficeClient
            Assert.AreEqual(7, serviceObject.Methods.Count);

            // Method: SetOutOfOffice
            Assert.AreEqual("Set Out Of Office", serviceObject.Methods[0].Metadata.DisplayName);
            Assert.AreEqual("SetOutOfOffice", serviceObject.Methods[0].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[0].Type);

            // Input Properties: SetOutOfOffice
            Assert.AreEqual(0, serviceObject.Methods[0].InputProperties.Count);

            // Required Properties: SetOutOfOffice
            Assert.AreEqual(0, serviceObject.Methods[0].Validation.RequiredProperties.Count);

            // Parameters: SetOutOfOffice
            Assert.AreEqual(0, serviceObject.Methods[0].Parameters.Count);

            // Return Properties: SetOutOfOffice
            Assert.AreEqual(0, serviceObject.Methods[0].ReturnProperties.Count);

            // Method: SetInOffice
            Assert.AreEqual("Set In Office", serviceObject.Methods[1].Metadata.DisplayName);
            Assert.AreEqual("SetInOffice", serviceObject.Methods[1].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[1].Type);

            // Input Properties: SetInOffice
            Assert.AreEqual(0, serviceObject.Methods[1].InputProperties.Count);

            // Required Properties: SetInOffice
            Assert.AreEqual(0, serviceObject.Methods[1].Validation.RequiredProperties.Count);

            // Parameters: SetInOffice
            Assert.AreEqual(0, serviceObject.Methods[1].Parameters.Count);

            // Return Properties: SetInOffice
            Assert.AreEqual(0, serviceObject.Methods[1].ReturnProperties.Count);

            // Method: GetUserStatus
            Assert.AreEqual("Get User Status", serviceObject.Methods[2].Metadata.DisplayName);
            Assert.AreEqual("GetUserStatus", serviceObject.Methods[2].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[2].Type);

            // Input Properties: GetUserStatus
            Assert.AreEqual(0, serviceObject.Methods[2].InputProperties.Count);

            // Required Properties: GetUserStatus
            Assert.AreEqual(0, serviceObject.Methods[2].Validation.RequiredProperties.Count);

            // Parameters: GetUserStatus
            Assert.AreEqual(0, serviceObject.Methods[2].Parameters.Count);

            // Return Properties: GetUserStatus
            Assert.AreEqual(1, serviceObject.Methods[2].ReturnProperties.Count);
            Assert.AreEqual("UserStatus", serviceObject.Methods[2].ReturnProperties[0].Name);

            // Method: AddOutOfOffice
            Assert.AreEqual("Add Out Of Office", serviceObject.Methods[3].Metadata.DisplayName);
            Assert.AreEqual("AddOutOfOffice", serviceObject.Methods[3].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[3].Type);

            // Input Properties: AddOutOfOffice
            Assert.AreEqual(1, serviceObject.Methods[3].InputProperties.Count);
            Assert.AreEqual("DestinationUser", serviceObject.Methods[3].InputProperties[0].Name);

            // Required Properties: AddOutOfOffice
            Assert.AreEqual(1, serviceObject.Methods[3].Validation.RequiredProperties.Count);
            Assert.AreEqual("DestinationUser", serviceObject.Methods[3].Validation.RequiredProperties[0].Name);

            // Parameters: AddOutOfOffice
            Assert.AreEqual(0, serviceObject.Methods[3].Parameters.Count);

            // Return Properties: AddOutOfOffice
            Assert.AreEqual(1, serviceObject.Methods[3].ReturnProperties.Count);
            Assert.AreEqual("DestinationUser", serviceObject.Methods[3].ReturnProperties[0].Name);

            // Method: RemoveOutOfOffice
            Assert.AreEqual("Remove Out Of Office", serviceObject.Methods[4].Metadata.DisplayName);
            Assert.AreEqual("RemoveOutOfOffice", serviceObject.Methods[4].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[4].Type);

            // Input Properties: RemoveOutOfOffice
            Assert.AreEqual(1, serviceObject.Methods[4].InputProperties.Count);
            Assert.AreEqual("DestinationUser", serviceObject.Methods[4].InputProperties[0].Name);

            // Required Properties: RemoveOutOfOffice
            Assert.AreEqual(1, serviceObject.Methods[4].Validation.RequiredProperties.Count);
            Assert.AreEqual("DestinationUser", serviceObject.Methods[4].Validation.RequiredProperties[0].Name);

            // Parameters: RemoveOutOfOffice
            Assert.AreEqual(0, serviceObject.Methods[4].Parameters.Count);

            // Return Properties: RemoveOutOfOffice
            Assert.AreEqual(1, serviceObject.Methods[4].ReturnProperties.Count);
            Assert.AreEqual("DestinationUser", serviceObject.Methods[4].ReturnProperties[0].Name);

            // Method: ListUserShares
            Assert.AreEqual("List User Shares", serviceObject.Methods[5].Metadata.DisplayName);
            Assert.AreEqual("ListUserShares", serviceObject.Methods[5].Name);
            Assert.AreEqual(MethodDefinitionType.list, serviceObject.Methods[5].Type);

            // Input Properties: ListUserShares
            Assert.AreEqual(0, serviceObject.Methods[5].InputProperties.Count);

            // Required Properties: ListUserShares
            Assert.AreEqual(0, serviceObject.Methods[5].Validation.RequiredProperties.Count);

            // Parameters: ListUserShares
            Assert.AreEqual(0, serviceObject.Methods[5].Parameters.Count);

            // Return Properties: ListUserShares
            Assert.AreEqual(1, serviceObject.Methods[5].ReturnProperties.Count);
            Assert.AreEqual("DestinationUser", serviceObject.Methods[5].ReturnProperties[0].Name);

            // Method: RemoveAllShares
            Assert.AreEqual("Remove All Shares", serviceObject.Methods[6].Metadata.DisplayName);
            Assert.AreEqual("RemoveAllShares", serviceObject.Methods[6].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[6].Type);

            // Input Properties: RemoveAllShares
            Assert.AreEqual(0, serviceObject.Methods[6].InputProperties.Count);

            // Required Properties: RemoveAllShares
            Assert.AreEqual(0, serviceObject.Methods[6].Validation.RequiredProperties.Count);

            // Parameters: RemoveAllShares
            Assert.AreEqual(0, serviceObject.Methods[6].Parameters.Count);

            // Return Properties: RemoveAllShares
            Assert.AreEqual(0, serviceObject.Methods[6].ReturnProperties.Count);

            // ASSOCIATIONS: OutOfOfficeClient
            Assert.AreEqual(0, serviceObject.Associations.Count);
        }

        [TestMethod]
        [TestCategory("Describe K2NEServiceBroker")]
        public void Describe_Out_Of_Office_Management()
        {
            var serviceObject = SmartObjectHelper.GetServiceObject(K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance, "OutOfOfficeManagement");

            // SERVICEOBJECT: OutOfOfficeManagement
            Assert.AreEqual("OutOfOfficeManagement", serviceObject.Name);
            Assert.AreEqual("Out Of Office Management", serviceObject.Metadata.DisplayName);
            Assert.AreEqual("", serviceObject.ServiceObjectType ?? string.Empty);

            // PROPERTIES: OutOfOfficeManagement
            Assert.AreEqual(3, serviceObject.Properties.Count);

            Assert.AreEqual("UserFQN", serviceObject.Properties[0].Name);
            Assert.AreEqual("User FQN", serviceObject.Properties[0].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[0].Type);

            Assert.AreEqual("DestinationUser", serviceObject.Properties[1].Name);
            Assert.AreEqual("Destination User", serviceObject.Properties[1].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[1].Type);

            Assert.AreEqual("UserStatus", serviceObject.Properties[2].Name);
            Assert.AreEqual("User Status", serviceObject.Properties[2].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[2].Type);

            // METHODS: OutOfOfficeManagement
            Assert.AreEqual(7, serviceObject.Methods.Count);

            // Method: SetOutOfOffice
            Assert.AreEqual("Set Out Of Office", serviceObject.Methods[0].Metadata.DisplayName);
            Assert.AreEqual("SetOutOfOffice", serviceObject.Methods[0].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[0].Type);

            // Input Properties: SetOutOfOffice
            Assert.AreEqual(1, serviceObject.Methods[0].InputProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[0].InputProperties[0].Name);

            // Required Properties: SetOutOfOffice
            Assert.AreEqual(1, serviceObject.Methods[0].Validation.RequiredProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[0].Validation.RequiredProperties[0].Name);

            // Parameters: SetOutOfOffice
            Assert.AreEqual(0, serviceObject.Methods[0].Parameters.Count);

            // Return Properties: SetOutOfOffice
            Assert.AreEqual(0, serviceObject.Methods[0].ReturnProperties.Count);

            // Method: SetInOffice
            Assert.AreEqual("Set In Office", serviceObject.Methods[1].Metadata.DisplayName);
            Assert.AreEqual("SetInOffice", serviceObject.Methods[1].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[1].Type);

            // Input Properties: SetInOffice
            Assert.AreEqual(1, serviceObject.Methods[1].InputProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[1].InputProperties[0].Name);

            // Required Properties: SetInOffice
            Assert.AreEqual(1, serviceObject.Methods[1].Validation.RequiredProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[1].Validation.RequiredProperties[0].Name);

            // Parameters: SetInOffice
            Assert.AreEqual(0, serviceObject.Methods[1].Parameters.Count);

            // Return Properties: SetInOffice
            Assert.AreEqual(0, serviceObject.Methods[1].ReturnProperties.Count);

            // Method: GetUserStatus
            Assert.AreEqual("Get User Status", serviceObject.Methods[2].Metadata.DisplayName);
            Assert.AreEqual("GetUserStatus", serviceObject.Methods[2].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[2].Type);

            // Input Properties: GetUserStatus
            Assert.AreEqual(1, serviceObject.Methods[2].InputProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[2].InputProperties[0].Name);

            // Required Properties: GetUserStatus
            Assert.AreEqual(1, serviceObject.Methods[2].Validation.RequiredProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[2].Validation.RequiredProperties[0].Name);

            // Parameters: GetUserStatus
            Assert.AreEqual(0, serviceObject.Methods[2].Parameters.Count);

            // Return Properties: GetUserStatus
            Assert.AreEqual(2, serviceObject.Methods[2].ReturnProperties.Count);
            Assert.AreEqual("UserStatus", serviceObject.Methods[2].ReturnProperties[0].Name);
            Assert.AreEqual("UserFQN", serviceObject.Methods[2].ReturnProperties[1].Name);

            // Method: AddOutOfOffice
            Assert.AreEqual("Add Out Of Office", serviceObject.Methods[3].Metadata.DisplayName);
            Assert.AreEqual("AddOutOfOffice", serviceObject.Methods[3].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[3].Type);

            // Input Properties: AddOutOfOffice
            Assert.AreEqual(2, serviceObject.Methods[3].InputProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[3].InputProperties[0].Name);
            Assert.AreEqual("DestinationUser", serviceObject.Methods[3].InputProperties[1].Name);

            // Required Properties: AddOutOfOffice
            Assert.AreEqual(2, serviceObject.Methods[3].Validation.RequiredProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[3].Validation.RequiredProperties[0].Name);
            Assert.AreEqual("DestinationUser", serviceObject.Methods[3].Validation.RequiredProperties[1].Name);

            // Parameters: AddOutOfOffice
            Assert.AreEqual(0, serviceObject.Methods[3].Parameters.Count);

            // Return Properties: AddOutOfOffice
            Assert.AreEqual(2, serviceObject.Methods[3].ReturnProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[3].ReturnProperties[0].Name);
            Assert.AreEqual("DestinationUser", serviceObject.Methods[3].ReturnProperties[1].Name);

            // Method: ListUserShares
            Assert.AreEqual("List User Shares", serviceObject.Methods[4].Metadata.DisplayName);
            Assert.AreEqual("ListUserShares", serviceObject.Methods[4].Name);
            Assert.AreEqual(MethodDefinitionType.list, serviceObject.Methods[4].Type);

            // Input Properties: ListUserShares
            Assert.AreEqual(1, serviceObject.Methods[4].InputProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[4].InputProperties[0].Name);

            // Required Properties: ListUserShares
            Assert.AreEqual(1, serviceObject.Methods[4].Validation.RequiredProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[4].Validation.RequiredProperties[0].Name);

            // Parameters: ListUserShares
            Assert.AreEqual(0, serviceObject.Methods[4].Parameters.Count);

            // Return Properties: ListUserShares
            Assert.AreEqual(2, serviceObject.Methods[4].ReturnProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[4].ReturnProperties[0].Name);
            Assert.AreEqual("DestinationUser", serviceObject.Methods[4].ReturnProperties[1].Name);

            // Method: ListShares
            Assert.AreEqual("List Shares", serviceObject.Methods[5].Metadata.DisplayName);
            Assert.AreEqual("ListShares", serviceObject.Methods[5].Name);
            Assert.AreEqual(MethodDefinitionType.list, serviceObject.Methods[5].Type);

            // Input Properties: ListShares
            Assert.AreEqual(0, serviceObject.Methods[5].InputProperties.Count);

            // Required Properties: ListShares
            Assert.AreEqual(0, serviceObject.Methods[5].Validation.RequiredProperties.Count);

            // Parameters: ListShares
            Assert.AreEqual(0, serviceObject.Methods[5].Parameters.Count);

            // Return Properties: ListShares
            Assert.AreEqual(2, serviceObject.Methods[5].ReturnProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[5].ReturnProperties[0].Name);
            Assert.AreEqual("DestinationUser", serviceObject.Methods[5].ReturnProperties[1].Name);

            // Method: RemoveAllShares
            Assert.AreEqual("Remove All Shares", serviceObject.Methods[6].Metadata.DisplayName);
            Assert.AreEqual("RemoveAllShares", serviceObject.Methods[6].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[6].Type);

            // Input Properties: RemoveAllShares
            Assert.AreEqual(1, serviceObject.Methods[6].InputProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[6].InputProperties[0].Name);

            // Required Properties: RemoveAllShares
            Assert.AreEqual(1, serviceObject.Methods[6].Validation.RequiredProperties.Count);
            Assert.AreEqual("UserFQN", serviceObject.Methods[6].Validation.RequiredProperties[0].Name);

            // Parameters: RemoveAllShares
            Assert.AreEqual(0, serviceObject.Methods[6].Parameters.Count);

            // Return Properties: RemoveAllShares
            Assert.AreEqual(0, serviceObject.Methods[6].ReturnProperties.Count);

            // ASSOCIATIONS: OutOfOfficeManagement
            Assert.AreEqual(0, serviceObject.Associations.Count);
        }

        [TestMethod]
        [TestCategory("Describe K2NEServiceBroker")]
        public void Describe_Powershell_Variables()
        {
            var serviceObject = SmartObjectHelper.GetServiceObject(K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance, "PowershellVariables");

            // SERVICEOBJECT: PowershellVariables
            Assert.AreEqual("PowershellVariables", serviceObject.Name);
            Assert.AreEqual("Powershell Variables", serviceObject.Metadata.DisplayName);
            Assert.AreEqual("", serviceObject.ServiceObjectType ?? string.Empty);

            // PROPERTIES: PowershellVariables
            Assert.AreEqual(4, serviceObject.Properties.Count);

            Assert.AreEqual("Name", serviceObject.Properties[0].Name);
            Assert.AreEqual("Name", serviceObject.Properties[0].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[0].Type);

            Assert.AreEqual("Value", serviceObject.Properties[1].Name);
            Assert.AreEqual("Value", serviceObject.Properties[1].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Memo, serviceObject.Properties[1].Type);

            Assert.AreEqual("SerializedArray", serviceObject.Properties[2].Name);
            Assert.AreEqual("Serialized Array", serviceObject.Properties[2].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Memo, serviceObject.Properties[2].Type);

            Assert.AreEqual("SerializedItem", serviceObject.Properties[3].Name);
            Assert.AreEqual("Serialized Item", serviceObject.Properties[3].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Memo, serviceObject.Properties[3].Type);

            // METHODS: PowershellVariables
            Assert.AreEqual(7, serviceObject.Methods.Count);

            // Method: SerializeItem
            Assert.AreEqual("Serialize Item", serviceObject.Methods[0].Metadata.DisplayName);
            Assert.AreEqual("SerializeItem", serviceObject.Methods[0].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[0].Type);

            // Input Properties: SerializeItem
            Assert.AreEqual(2, serviceObject.Methods[0].InputProperties.Count);
            Assert.AreEqual("Name", serviceObject.Methods[0].InputProperties[0].Name);
            Assert.AreEqual("Value", serviceObject.Methods[0].InputProperties[1].Name);

            // Required Properties: SerializeItem
            Assert.AreEqual(2, serviceObject.Methods[0].Validation.RequiredProperties.Count);
            Assert.AreEqual("Name", serviceObject.Methods[0].Validation.RequiredProperties[0].Name);
            Assert.AreEqual("Value", serviceObject.Methods[0].Validation.RequiredProperties[1].Name);

            // Parameters: SerializeItem
            Assert.AreEqual(0, serviceObject.Methods[0].Parameters.Count);

            // Return Properties: SerializeItem
            Assert.AreEqual(1, serviceObject.Methods[0].ReturnProperties.Count);
            Assert.AreEqual("SerializedItem", serviceObject.Methods[0].ReturnProperties[0].Name);

            // Method: SerializeItemToArray
            Assert.AreEqual("Serialize Item To Array", serviceObject.Methods[1].Metadata.DisplayName);
            Assert.AreEqual("SerializeItemToArray", serviceObject.Methods[1].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[1].Type);

            // Input Properties: SerializeItemToArray
            Assert.AreEqual(2, serviceObject.Methods[1].InputProperties.Count);
            Assert.AreEqual("Name", serviceObject.Methods[1].InputProperties[0].Name);
            Assert.AreEqual("Value", serviceObject.Methods[1].InputProperties[1].Name);

            // Required Properties: SerializeItemToArray
            Assert.AreEqual(2, serviceObject.Methods[1].Validation.RequiredProperties.Count);
            Assert.AreEqual("Name", serviceObject.Methods[1].Validation.RequiredProperties[0].Name);
            Assert.AreEqual("Value", serviceObject.Methods[1].Validation.RequiredProperties[1].Name);

            // Parameters: SerializeItemToArray
            Assert.AreEqual(0, serviceObject.Methods[1].Parameters.Count);

            // Return Properties: SerializeItemToArray
            Assert.AreEqual(1, serviceObject.Methods[1].ReturnProperties.Count);
            Assert.AreEqual("SerializedArray", serviceObject.Methods[1].ReturnProperties[0].Name);

            // Method: AddSerializedItemToArray
            Assert.AreEqual("Add Serialized Item To Array", serviceObject.Methods[2].Metadata.DisplayName);
            Assert.AreEqual("AddSerializedItemToArray", serviceObject.Methods[2].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[2].Type);

            // Input Properties: AddSerializedItemToArray
            Assert.AreEqual(2, serviceObject.Methods[2].InputProperties.Count);
            Assert.AreEqual("SerializedArray", serviceObject.Methods[2].InputProperties[0].Name);
            Assert.AreEqual("SerializedItem", serviceObject.Methods[2].InputProperties[1].Name);

            // Required Properties: AddSerializedItemToArray
            Assert.AreEqual(2, serviceObject.Methods[2].Validation.RequiredProperties.Count);
            Assert.AreEqual("SerializedArray", serviceObject.Methods[2].Validation.RequiredProperties[0].Name);
            Assert.AreEqual("SerializedItem", serviceObject.Methods[2].Validation.RequiredProperties[1].Name);

            // Parameters: AddSerializedItemToArray
            Assert.AreEqual(0, serviceObject.Methods[2].Parameters.Count);

            // Return Properties: AddSerializedItemToArray
            Assert.AreEqual(1, serviceObject.Methods[2].ReturnProperties.Count);
            Assert.AreEqual("SerializedArray", serviceObject.Methods[2].ReturnProperties[0].Name);

            // Method: SerializeAddItemToArray
            Assert.AreEqual("Serialize Add Item To Array", serviceObject.Methods[3].Metadata.DisplayName);
            Assert.AreEqual("SerializeAddItemToArray", serviceObject.Methods[3].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[3].Type);

            // Input Properties: SerializeAddItemToArray
            Assert.AreEqual(3, serviceObject.Methods[3].InputProperties.Count);
            Assert.AreEqual("Name", serviceObject.Methods[3].InputProperties[0].Name);
            Assert.AreEqual("Value", serviceObject.Methods[3].InputProperties[1].Name);
            Assert.AreEqual("SerializedArray", serviceObject.Methods[3].InputProperties[2].Name);

            // Required Properties: SerializeAddItemToArray
            Assert.AreEqual(2, serviceObject.Methods[3].Validation.RequiredProperties.Count);
            Assert.AreEqual("Name", serviceObject.Methods[3].Validation.RequiredProperties[0].Name);
            Assert.AreEqual("Value", serviceObject.Methods[3].Validation.RequiredProperties[1].Name);

            // Parameters: SerializeAddItemToArray
            Assert.AreEqual(0, serviceObject.Methods[3].Parameters.Count);

            // Return Properties: SerializeAddItemToArray
            Assert.AreEqual(1, serviceObject.Methods[3].ReturnProperties.Count);
            Assert.AreEqual("SerializedArray", serviceObject.Methods[3].ReturnProperties[0].Name);

            // Method: Deserialize
            Assert.AreEqual("Deserialize", serviceObject.Methods[4].Metadata.DisplayName);
            Assert.AreEqual("Deserialize", serviceObject.Methods[4].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[4].Type);

            // Input Properties: Deserialize
            Assert.AreEqual(1, serviceObject.Methods[4].InputProperties.Count);
            Assert.AreEqual("SerializedItem", serviceObject.Methods[4].InputProperties[0].Name);

            // Required Properties: Deserialize
            Assert.AreEqual(1, serviceObject.Methods[4].Validation.RequiredProperties.Count);
            Assert.AreEqual("SerializedItem", serviceObject.Methods[4].Validation.RequiredProperties[0].Name);

            // Parameters: Deserialize
            Assert.AreEqual(0, serviceObject.Methods[4].Parameters.Count);

            // Return Properties: Deserialize
            Assert.AreEqual(2, serviceObject.Methods[4].ReturnProperties.Count);
            Assert.AreEqual("Name", serviceObject.Methods[4].ReturnProperties[0].Name);
            Assert.AreEqual("Value", serviceObject.Methods[4].ReturnProperties[1].Name);

            // Method: DeserializeItemFromArray
            Assert.AreEqual("Deserialize Item From Array", serviceObject.Methods[5].Metadata.DisplayName);
            Assert.AreEqual("DeserializeItemFromArray", serviceObject.Methods[5].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[5].Type);

            // Input Properties: DeserializeItemFromArray
            Assert.AreEqual(2, serviceObject.Methods[5].InputProperties.Count);
            Assert.AreEqual("SerializedArray", serviceObject.Methods[5].InputProperties[0].Name);
            Assert.AreEqual("Name", serviceObject.Methods[5].InputProperties[1].Name);

            // Required Properties: DeserializeItemFromArray
            Assert.AreEqual(2, serviceObject.Methods[5].Validation.RequiredProperties.Count);
            Assert.AreEqual("SerializedArray", serviceObject.Methods[5].Validation.RequiredProperties[0].Name);
            Assert.AreEqual("Name", serviceObject.Methods[5].Validation.RequiredProperties[1].Name);

            // Parameters: DeserializeItemFromArray
            Assert.AreEqual(0, serviceObject.Methods[5].Parameters.Count);

            // Return Properties: DeserializeItemFromArray
            Assert.AreEqual(2, serviceObject.Methods[5].ReturnProperties.Count);
            Assert.AreEqual("Name", serviceObject.Methods[5].ReturnProperties[0].Name);
            Assert.AreEqual("Value", serviceObject.Methods[5].ReturnProperties[1].Name);

            // Method: DeserializeArrayToList
            Assert.AreEqual("Deserialize Array To List", serviceObject.Methods[6].Metadata.DisplayName);
            Assert.AreEqual("DeserializeArrayToList", serviceObject.Methods[6].Name);
            Assert.AreEqual(MethodDefinitionType.list, serviceObject.Methods[6].Type);

            // Input Properties: DeserializeArrayToList
            Assert.AreEqual(1, serviceObject.Methods[6].InputProperties.Count);
            Assert.AreEqual("SerializedArray", serviceObject.Methods[6].InputProperties[0].Name);

            // Required Properties: DeserializeArrayToList
            Assert.AreEqual(1, serviceObject.Methods[6].Validation.RequiredProperties.Count);
            Assert.AreEqual("SerializedArray", serviceObject.Methods[6].Validation.RequiredProperties[0].Name);

            // Parameters: DeserializeArrayToList
            Assert.AreEqual(0, serviceObject.Methods[6].Parameters.Count);

            // Return Properties: DeserializeArrayToList
            Assert.AreEqual(2, serviceObject.Methods[6].ReturnProperties.Count);
            Assert.AreEqual("Name", serviceObject.Methods[6].ReturnProperties[0].Name);
            Assert.AreEqual("Value", serviceObject.Methods[6].ReturnProperties[1].Name);

            // ASSOCIATIONS: PowershellVariables
            Assert.AreEqual(0, serviceObject.Associations.Count);
        }

        [TestMethod]
        [TestCategory("Describe K2NEServiceBroker")]
        public void Describe_Process_Instance_Management()
        {
            var serviceObject = SmartObjectHelper.GetServiceObject(K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance, "ProcessInstanceManagement");

            // SERVICEOBJECT: ProcessInstanceManagement
            Assert.AreEqual("ProcessInstanceManagement", serviceObject.Name);
            Assert.AreEqual("Process Instance Management", serviceObject.Metadata.DisplayName);
            Assert.AreEqual("", serviceObject.ServiceObjectType ?? string.Empty);

            // PROPERTIES: ProcessInstanceManagement
            Assert.AreEqual(9, serviceObject.Properties.Count);

            Assert.AreEqual("ActivityName", serviceObject.Properties[0].Name);
            Assert.AreEqual("Activity Name", serviceObject.Properties[0].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[0].Type);

            Assert.AreEqual("ProcessInstanceId", serviceObject.Properties[1].Name);
            Assert.AreEqual("Process Instance Id", serviceObject.Properties[1].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[1].Type);

            Assert.AreEqual("IncludeStartActivity", serviceObject.Properties[2].Name);
            Assert.AreEqual("Include Start Activity", serviceObject.Properties[2].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.YesNo, serviceObject.Properties[2].Type);

            Assert.AreEqual("ActivityDescription", serviceObject.Properties[3].Name);
            Assert.AreEqual("Activity Description", serviceObject.Properties[3].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[3].Type);

            Assert.AreEqual("ActivityExpectedDuration", serviceObject.Properties[4].Name);
            Assert.AreEqual("Activity Expected Duration", serviceObject.Properties[4].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[4].Type);

            Assert.AreEqual("ActivityID", serviceObject.Properties[5].Name);
            Assert.AreEqual("Activity ID", serviceObject.Properties[5].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[5].Type);

            Assert.AreEqual("IsStartActivity", serviceObject.Properties[6].Name);
            Assert.AreEqual("Is Start Activity", serviceObject.Properties[6].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.YesNo, serviceObject.Properties[6].Type);

            Assert.AreEqual("ActivityMetaData", serviceObject.Properties[7].Name);
            Assert.AreEqual("Activity Meta Data", serviceObject.Properties[7].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[7].Type);

            Assert.AreEqual("ActivityPriority", serviceObject.Properties[8].Name);
            Assert.AreEqual("Activity Priority", serviceObject.Properties[8].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[8].Type);

            // METHODS: ProcessInstanceManagement
            Assert.AreEqual(2, serviceObject.Methods.Count);

            // Method: GotoActivity
            Assert.AreEqual("Goto Activity", serviceObject.Methods[0].Metadata.DisplayName);
            Assert.AreEqual("GotoActivity", serviceObject.Methods[0].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[0].Type);

            // Input Properties: GotoActivity
            Assert.AreEqual(2, serviceObject.Methods[0].InputProperties.Count);
            Assert.AreEqual("ActivityName", serviceObject.Methods[0].InputProperties[0].Name);
            Assert.AreEqual("ProcessInstanceId", serviceObject.Methods[0].InputProperties[1].Name);

            // Required Properties: GotoActivity
            Assert.AreEqual(2, serviceObject.Methods[0].Validation.RequiredProperties.Count);
            Assert.AreEqual("ProcessInstanceId", serviceObject.Methods[0].Validation.RequiredProperties[0].Name);
            Assert.AreEqual("ActivityName", serviceObject.Methods[0].Validation.RequiredProperties[1].Name);

            // Parameters: GotoActivity
            Assert.AreEqual(0, serviceObject.Methods[0].Parameters.Count);

            // Return Properties: GotoActivity
            Assert.AreEqual(0, serviceObject.Methods[0].ReturnProperties.Count);

            // Method: ListActivities
            Assert.AreEqual("List Activities", serviceObject.Methods[1].Metadata.DisplayName);
            Assert.AreEqual("ListActivities", serviceObject.Methods[1].Name);
            Assert.AreEqual(MethodDefinitionType.list, serviceObject.Methods[1].Type);

            // Input Properties: ListActivities
            Assert.AreEqual(2, serviceObject.Methods[1].InputProperties.Count);
            Assert.AreEqual("ProcessInstanceId", serviceObject.Methods[1].InputProperties[0].Name);
            Assert.AreEqual("IncludeStartActivity", serviceObject.Methods[1].InputProperties[1].Name);

            // Required Properties: ListActivities
            Assert.AreEqual(1, serviceObject.Methods[1].Validation.RequiredProperties.Count);
            Assert.AreEqual("ProcessInstanceId", serviceObject.Methods[1].Validation.RequiredProperties[0].Name);

            // Parameters: ListActivities
            Assert.AreEqual(0, serviceObject.Methods[1].Parameters.Count);

            // Return Properties: ListActivities
            Assert.AreEqual(7, serviceObject.Methods[1].ReturnProperties.Count);
            Assert.AreEqual("ActivityID", serviceObject.Methods[1].ReturnProperties[0].Name);
            Assert.AreEqual("ActivityName", serviceObject.Methods[1].ReturnProperties[1].Name);
            Assert.AreEqual("ActivityDescription", serviceObject.Methods[1].ReturnProperties[2].Name);
            Assert.AreEqual("ActivityExpectedDuration", serviceObject.Methods[1].ReturnProperties[3].Name);
            Assert.AreEqual("ActivityMetaData", serviceObject.Methods[1].ReturnProperties[4].Name);
            Assert.AreEqual("ActivityPriority", serviceObject.Methods[1].ReturnProperties[5].Name);
            Assert.AreEqual("IsStartActivity", serviceObject.Methods[1].ReturnProperties[6].Name);

            // ASSOCIATIONS: ProcessInstanceManagement
            Assert.AreEqual(0, serviceObject.Associations.Count);
        }

        [TestMethod]
        [TestCategory("Describe K2NEServiceBroker")]
        public void Describe_Role_Management()
        {
            var serviceObject = SmartObjectHelper.GetServiceObject(K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance, "RoleManagement");

            // SERVICEOBJECT: RoleManagement
            Assert.AreEqual("RoleManagement", serviceObject.Name);
            Assert.AreEqual("Role Management", serviceObject.Metadata.DisplayName);
            Assert.AreEqual("", serviceObject.ServiceObjectType ?? string.Empty);

            // PROPERTIES: RoleManagement
            Assert.AreEqual(6, serviceObject.Properties.Count);

            Assert.AreEqual("RoleName", serviceObject.Properties[0].Name);
            Assert.AreEqual("Role Name", serviceObject.Properties[0].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[0].Type);

            Assert.AreEqual("ItemType", serviceObject.Properties[1].Name);
            Assert.AreEqual("Item Type", serviceObject.Properties[1].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[1].Type);

            Assert.AreEqual("RoleItem", serviceObject.Properties[2].Name);
            Assert.AreEqual("Role Item", serviceObject.Properties[2].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[2].Type);

            Assert.AreEqual("Description", serviceObject.Properties[3].Name);
            Assert.AreEqual("Description", serviceObject.Properties[3].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[3].Type);

            Assert.AreEqual("RoleItem", serviceObject.Properties[4].Name);
            Assert.AreEqual("Role Item", serviceObject.Properties[4].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[4].Type);

            Assert.AreEqual("IsDynamic", serviceObject.Properties[5].Name);
            Assert.AreEqual("Is Dynamic", serviceObject.Properties[5].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.YesNo, serviceObject.Properties[5].Type);

            // METHODS: RoleManagement
            Assert.AreEqual(6, serviceObject.Methods.Count);

            // Method: AddRoleItem
            Assert.AreEqual("Add Role Item", serviceObject.Methods[0].Metadata.DisplayName);
            Assert.AreEqual("AddRoleItem", serviceObject.Methods[0].Name);
            Assert.AreEqual(MethodDefinitionType.create, serviceObject.Methods[0].Type);

            // Input Properties: AddRoleItem
            Assert.AreEqual(3, serviceObject.Methods[0].InputProperties.Count);
            Assert.AreEqual("RoleName", serviceObject.Methods[0].InputProperties[0].Name);
            Assert.AreEqual("RoleItem", serviceObject.Methods[0].InputProperties[1].Name);
            Assert.AreEqual("ItemType", serviceObject.Methods[0].InputProperties[2].Name);

            // Required Properties: AddRoleItem
            Assert.AreEqual(3, serviceObject.Methods[0].Validation.RequiredProperties.Count);
            Assert.AreEqual("RoleName", serviceObject.Methods[0].Validation.RequiredProperties[0].Name);
            Assert.AreEqual("RoleItem", serviceObject.Methods[0].Validation.RequiredProperties[1].Name);
            Assert.AreEqual("ItemType", serviceObject.Methods[0].Validation.RequiredProperties[2].Name);

            // Parameters: AddRoleItem
            Assert.AreEqual(0, serviceObject.Methods[0].Parameters.Count);

            // Return Properties: AddRoleItem
            Assert.AreEqual(0, serviceObject.Methods[0].ReturnProperties.Count);

            // Method: RemoveRoleItem
            Assert.AreEqual("Remove Role Item", serviceObject.Methods[1].Metadata.DisplayName);
            Assert.AreEqual("RemoveRoleItem", serviceObject.Methods[1].Name);
            Assert.AreEqual(MethodDefinitionType.delete, serviceObject.Methods[1].Type);

            // Input Properties: RemoveRoleItem
            Assert.AreEqual(2, serviceObject.Methods[1].InputProperties.Count);
            Assert.AreEqual("RoleName", serviceObject.Methods[1].InputProperties[0].Name);
            Assert.AreEqual("RoleItem", serviceObject.Methods[1].InputProperties[1].Name);

            // Required Properties: RemoveRoleItem
            Assert.AreEqual(2, serviceObject.Methods[1].Validation.RequiredProperties.Count);
            Assert.AreEqual("RoleName", serviceObject.Methods[1].Validation.RequiredProperties[0].Name);
            Assert.AreEqual("RoleItem", serviceObject.Methods[1].Validation.RequiredProperties[1].Name);

            // Parameters: RemoveRoleItem
            Assert.AreEqual(0, serviceObject.Methods[1].Parameters.Count);

            // Return Properties: RemoveRoleItem
            Assert.AreEqual(0, serviceObject.Methods[1].ReturnProperties.Count);

            // Method: ListRoleItem
            Assert.AreEqual("List Role Item", serviceObject.Methods[2].Metadata.DisplayName);
            Assert.AreEqual("ListRoleItem", serviceObject.Methods[2].Name);
            Assert.AreEqual(MethodDefinitionType.list, serviceObject.Methods[2].Type);

            // Input Properties: ListRoleItem
            Assert.AreEqual(1, serviceObject.Methods[2].InputProperties.Count);
            Assert.AreEqual("RoleName", serviceObject.Methods[2].InputProperties[0].Name);

            // Required Properties: ListRoleItem
            Assert.AreEqual(0, serviceObject.Methods[2].Validation.RequiredProperties.Count);

            // Parameters: ListRoleItem
            Assert.AreEqual(0, serviceObject.Methods[2].Parameters.Count);

            // Return Properties: ListRoleItem
            Assert.AreEqual(2, serviceObject.Methods[2].ReturnProperties.Count);
            Assert.AreEqual("RoleItem", serviceObject.Methods[2].ReturnProperties[0].Name);
            Assert.AreEqual("ItemType", serviceObject.Methods[2].ReturnProperties[1].Name);

            // Method: AddRole
            Assert.AreEqual("Add Role", serviceObject.Methods[3].Metadata.DisplayName);
            Assert.AreEqual("AddRole", serviceObject.Methods[3].Name);
            Assert.AreEqual(MethodDefinitionType.create, serviceObject.Methods[3].Type);

            // Input Properties: AddRole
            Assert.AreEqual(5, serviceObject.Methods[3].InputProperties.Count);
            Assert.AreEqual("RoleName", serviceObject.Methods[3].InputProperties[0].Name);
            Assert.AreEqual("Description", serviceObject.Methods[3].InputProperties[1].Name);
            Assert.AreEqual("IsDynamic", serviceObject.Methods[3].InputProperties[2].Name);
            Assert.AreEqual("ItemType", serviceObject.Methods[3].InputProperties[3].Name);
            Assert.AreEqual("RoleItem", serviceObject.Methods[3].InputProperties[4].Name);

            // Required Properties: AddRole
            Assert.AreEqual(3, serviceObject.Methods[3].Validation.RequiredProperties.Count);
            Assert.AreEqual("RoleName", serviceObject.Methods[3].Validation.RequiredProperties[0].Name);
            Assert.AreEqual("RoleItem", serviceObject.Methods[3].Validation.RequiredProperties[1].Name);
            Assert.AreEqual("ItemType", serviceObject.Methods[3].Validation.RequiredProperties[2].Name);

            // Parameters: AddRole
            Assert.AreEqual(0, serviceObject.Methods[3].Parameters.Count);

            // Return Properties: AddRole
            Assert.AreEqual(0, serviceObject.Methods[3].ReturnProperties.Count);

            // Method: RemoveRole
            Assert.AreEqual("Remove Role", serviceObject.Methods[4].Metadata.DisplayName);
            Assert.AreEqual("RemoveRole", serviceObject.Methods[4].Name);
            Assert.AreEqual(MethodDefinitionType.delete, serviceObject.Methods[4].Type);

            // Input Properties: RemoveRole
            Assert.AreEqual(1, serviceObject.Methods[4].InputProperties.Count);
            Assert.AreEqual("RoleName", serviceObject.Methods[4].InputProperties[0].Name);

            // Required Properties: RemoveRole
            Assert.AreEqual(1, serviceObject.Methods[4].Validation.RequiredProperties.Count);
            Assert.AreEqual("RoleName", serviceObject.Methods[4].Validation.RequiredProperties[0].Name);

            // Parameters: RemoveRole
            Assert.AreEqual(0, serviceObject.Methods[4].Parameters.Count);

            // Return Properties: RemoveRole
            Assert.AreEqual(0, serviceObject.Methods[4].ReturnProperties.Count);

            // Method: ListRoles
            Assert.AreEqual("List Roles", serviceObject.Methods[5].Metadata.DisplayName);
            Assert.AreEqual("ListRoles", serviceObject.Methods[5].Name);
            Assert.AreEqual(MethodDefinitionType.list, serviceObject.Methods[5].Type);

            // Input Properties: ListRoles
            Assert.AreEqual(0, serviceObject.Methods[5].InputProperties.Count);

            // Required Properties: ListRoles
            Assert.AreEqual(0, serviceObject.Methods[5].Validation.RequiredProperties.Count);

            // Parameters: ListRoles
            Assert.AreEqual(0, serviceObject.Methods[5].Parameters.Count);

            // Return Properties: ListRoles
            Assert.AreEqual(3, serviceObject.Methods[5].ReturnProperties.Count);
            Assert.AreEqual("RoleName", serviceObject.Methods[5].ReturnProperties[0].Name);
            Assert.AreEqual("Description", serviceObject.Methods[5].ReturnProperties[1].Name);
            Assert.AreEqual("IsDynamic", serviceObject.Methods[5].ReturnProperties[2].Name);

            // ASSOCIATIONS: RoleManagement
            Assert.AreEqual(0, serviceObject.Associations.Count);
        }

        [TestMethod]
        [TestCategory("Describe K2NEServiceBroker")]
        public void Describe_Simple_Powershell()
        {
            var serviceObject = SmartObjectHelper.GetServiceObject(K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance, "SimplePowershell");

            // SERVICEOBJECT: SimplePowershell
            Assert.AreEqual("SimplePowershell", serviceObject.Name);
            Assert.AreEqual("Simple Powershell", serviceObject.Metadata.DisplayName);
            Assert.AreEqual("", serviceObject.ServiceObjectType ?? string.Empty);

            // PROPERTIES: SimplePowershell
            Assert.AreEqual(4, serviceObject.Properties.Count);

            Assert.AreEqual("PowerShellScript", serviceObject.Properties[0].Name);
            Assert.AreEqual("Power Shell Script", serviceObject.Properties[0].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Memo, serviceObject.Properties[0].Type);

            Assert.AreEqual("Variables", serviceObject.Properties[1].Name);
            Assert.AreEqual("Variables", serviceObject.Properties[1].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Memo, serviceObject.Properties[1].Type);

            Assert.AreEqual("ScriptOutput", serviceObject.Properties[2].Name);
            Assert.AreEqual("Script Output", serviceObject.Properties[2].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Memo, serviceObject.Properties[2].Type);

            Assert.AreEqual("PowerShellFilePath", serviceObject.Properties[3].Name);
            Assert.AreEqual("Power Shell File Path", serviceObject.Properties[3].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Memo, serviceObject.Properties[3].Type);

            // METHODS: SimplePowershell
            Assert.AreEqual(2, serviceObject.Methods.Count);

            // Method: RunScript
            Assert.AreEqual("Run Script", serviceObject.Methods[0].Metadata.DisplayName);
            Assert.AreEqual("RunScript", serviceObject.Methods[0].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[0].Type);

            // Input Properties: RunScript
            Assert.AreEqual(2, serviceObject.Methods[0].InputProperties.Count);
            Assert.AreEqual("PowerShellScript", serviceObject.Methods[0].InputProperties[0].Name);
            Assert.AreEqual("Variables", serviceObject.Methods[0].InputProperties[1].Name);

            // Required Properties: RunScript
            Assert.AreEqual(1, serviceObject.Methods[0].Validation.RequiredProperties.Count);
            Assert.AreEqual("PowerShellScript", serviceObject.Methods[0].Validation.RequiredProperties[0].Name);

            // Parameters: RunScript
            Assert.AreEqual(0, serviceObject.Methods[0].Parameters.Count);

            // Return Properties: RunScript
            Assert.AreEqual(2, serviceObject.Methods[0].ReturnProperties.Count);
            Assert.AreEqual("ScriptOutput", serviceObject.Methods[0].ReturnProperties[0].Name);
            Assert.AreEqual("Variables", serviceObject.Methods[0].ReturnProperties[1].Name);

            // Method: RunScriptByFilePath
            Assert.AreEqual("Run Script By File Path", serviceObject.Methods[1].Metadata.DisplayName);
            Assert.AreEqual("RunScriptByFilePath", serviceObject.Methods[1].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[1].Type);

            // Input Properties: RunScriptByFilePath
            Assert.AreEqual(2, serviceObject.Methods[1].InputProperties.Count);
            Assert.AreEqual("PowerShellFilePath", serviceObject.Methods[1].InputProperties[0].Name);
            Assert.AreEqual("Variables", serviceObject.Methods[1].InputProperties[1].Name);

            // Required Properties: RunScriptByFilePath
            Assert.AreEqual(1, serviceObject.Methods[1].Validation.RequiredProperties.Count);
            Assert.AreEqual("PowerShellFilePath", serviceObject.Methods[1].Validation.RequiredProperties[0].Name);

            // Parameters: RunScriptByFilePath
            Assert.AreEqual(0, serviceObject.Methods[1].Parameters.Count);

            // Return Properties: RunScriptByFilePath
            Assert.AreEqual(2, serviceObject.Methods[1].ReturnProperties.Count);
            Assert.AreEqual("ScriptOutput", serviceObject.Methods[1].ReturnProperties[0].Name);
            Assert.AreEqual("Variables", serviceObject.Methods[1].ReturnProperties[1].Name);

            // ASSOCIATIONS: SimplePowershell
            Assert.AreEqual(0, serviceObject.Associations.Count);
        }

        [TestMethod]
        [TestCategory("Describe K2NEServiceBroker")]
        public void Describe_URM_Group()
        {
            var serviceObject = SmartObjectHelper.GetServiceObject(K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance, "URMGroup");

            // SERVICEOBJECT: URMGroup
            Assert.AreEqual("URMGroup", serviceObject.Name);
            Assert.AreEqual("URM Group", serviceObject.Metadata.DisplayName);
            Assert.AreEqual("", serviceObject.ServiceObjectType ?? string.Empty);

            // PROPERTIES: URMGroup
            Assert.AreEqual(6, serviceObject.Properties.Count);

            Assert.AreEqual("FQN", serviceObject.Properties[0].Name);
            Assert.AreEqual("FQN", serviceObject.Properties[0].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[0].Type);

            Assert.AreEqual("GroupName", serviceObject.Properties[1].Name);
            Assert.AreEqual("Group Name", serviceObject.Properties[1].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[1].Type);

            Assert.AreEqual("Name", serviceObject.Properties[2].Name);
            Assert.AreEqual("Name", serviceObject.Properties[2].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[2].Type);

            Assert.AreEqual("Description", serviceObject.Properties[3].Name);
            Assert.AreEqual("Description", serviceObject.Properties[3].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[3].Type);

            Assert.AreEqual("Email", serviceObject.Properties[4].Name);
            Assert.AreEqual("Email", serviceObject.Properties[4].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[4].Type);

            Assert.AreEqual("sAMAccountName", serviceObject.Properties[5].Name);
            Assert.AreEqual("sAM Account Name", serviceObject.Properties[5].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[5].Type);

            // METHODS: URMGroup
            Assert.AreEqual(2, serviceObject.Methods.Count);

            // Method: UMRGetGroupDetails
            Assert.AreEqual("UMR Get Group Details", serviceObject.Methods[0].Metadata.DisplayName);
            Assert.AreEqual("UMRGetGroupDetails", serviceObject.Methods[0].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[0].Type);

            // Input Properties: UMRGetGroupDetails
            Assert.AreEqual(1, serviceObject.Methods[0].InputProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[0].InputProperties[0].Name);

            // Required Properties: UMRGetGroupDetails
            Assert.AreEqual(1, serviceObject.Methods[0].Validation.RequiredProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[0].Validation.RequiredProperties[0].Name);

            // Parameters: UMRGetGroupDetails
            Assert.AreEqual(0, serviceObject.Methods[0].Parameters.Count);

            // Return Properties: UMRGetGroupDetails
            Assert.AreEqual(5, serviceObject.Methods[0].ReturnProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[0].ReturnProperties[0].Name);
            Assert.AreEqual("GroupName", serviceObject.Methods[0].ReturnProperties[1].Name);
            Assert.AreEqual("Name", serviceObject.Methods[0].ReturnProperties[2].Name);
            Assert.AreEqual("Description", serviceObject.Methods[0].ReturnProperties[3].Name);
            Assert.AreEqual("Email", serviceObject.Methods[0].ReturnProperties[4].Name);

            // Method: UMRGetGroups
            Assert.AreEqual("UMR Get Groups", serviceObject.Methods[1].Metadata.DisplayName);
            Assert.AreEqual("UMRGetGroups", serviceObject.Methods[1].Name);
            Assert.AreEqual(MethodDefinitionType.list, serviceObject.Methods[1].Type);

            // Input Properties: UMRGetGroups
            Assert.AreEqual(3, serviceObject.Methods[1].InputProperties.Count);
            Assert.AreEqual("Name", serviceObject.Methods[1].InputProperties[0].Name);
            Assert.AreEqual("Description", serviceObject.Methods[1].InputProperties[1].Name);
            Assert.AreEqual("sAMAccountName", serviceObject.Methods[1].InputProperties[2].Name);

            // Required Properties: UMRGetGroups
            Assert.AreEqual(0, serviceObject.Methods[1].Validation.RequiredProperties.Count);

            // Parameters: UMRGetGroups
            Assert.AreEqual(1, serviceObject.Methods[1].Parameters.Count);
            Assert.AreEqual("Label", serviceObject.Methods[1].Parameters[0].Name);
            Assert.AreEqual("Label", serviceObject.Methods[1].Parameters[0].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Methods[1].Parameters[0].Type);
            Assert.AreEqual(true, serviceObject.Methods[1].Parameters[0].IsRequired);

            // Return Properties: UMRGetGroups
            Assert.AreEqual(6, serviceObject.Methods[1].ReturnProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[1].ReturnProperties[0].Name);
            Assert.AreEqual("GroupName", serviceObject.Methods[1].ReturnProperties[1].Name);
            Assert.AreEqual("Name", serviceObject.Methods[1].ReturnProperties[2].Name);
            Assert.AreEqual("Description", serviceObject.Methods[1].ReturnProperties[3].Name);
            Assert.AreEqual("Email", serviceObject.Methods[1].ReturnProperties[4].Name);
            Assert.AreEqual("sAMAccountName", serviceObject.Methods[1].ReturnProperties[5].Name);

            // ASSOCIATIONS: URMGroup
            Assert.AreEqual(0, serviceObject.Associations.Count);
        }

        [TestMethod]
        [TestCategory("Describe K2NEServiceBroker")]
        public void Describe_URM_User()
        {
            var serviceObject = SmartObjectHelper.GetServiceObject(K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance, "URMUser");

            // SERVICEOBJECT: URMUser
            Assert.AreEqual("URMUser", serviceObject.Name);
            Assert.AreEqual("URM User", serviceObject.Metadata.DisplayName);
            Assert.AreEqual("", serviceObject.ServiceObjectType ?? string.Empty);

            // PROPERTIES: URMUser
            Assert.AreEqual(9, serviceObject.Properties.Count);

            Assert.AreEqual("FQN", serviceObject.Properties[0].Name);
            Assert.AreEqual("FQN", serviceObject.Properties[0].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[0].Type);

            Assert.AreEqual("UserName", serviceObject.Properties[1].Name);
            Assert.AreEqual("User Name", serviceObject.Properties[1].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[1].Type);

            Assert.AreEqual("Name", serviceObject.Properties[2].Name);
            Assert.AreEqual("Name", serviceObject.Properties[2].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[2].Type);

            Assert.AreEqual("Description", serviceObject.Properties[3].Name);
            Assert.AreEqual("Description", serviceObject.Properties[3].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[3].Type);

            Assert.AreEqual("Email", serviceObject.Properties[4].Name);
            Assert.AreEqual("Email", serviceObject.Properties[4].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[4].Type);

            Assert.AreEqual("Manager", serviceObject.Properties[5].Name);
            Assert.AreEqual("Manager", serviceObject.Properties[5].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[5].Type);

            Assert.AreEqual("ObjectSID", serviceObject.Properties[6].Name);
            Assert.AreEqual("Object SID", serviceObject.Properties[6].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[6].Type);

            Assert.AreEqual("DisplayName", serviceObject.Properties[7].Name);
            Assert.AreEqual("Display Name", serviceObject.Properties[7].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[7].Type);

            Assert.AreEqual("sAMAccountName", serviceObject.Properties[8].Name);
            Assert.AreEqual("sAM Account Name", serviceObject.Properties[8].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[8].Type);

            // METHODS: URMUser
            Assert.AreEqual(1, serviceObject.Methods.Count);

            // Method: UMRGetUsers
            Assert.AreEqual("UMR Get Users", serviceObject.Methods[0].Metadata.DisplayName);
            Assert.AreEqual("UMRGetUsers", serviceObject.Methods[0].Name);
            Assert.AreEqual(MethodDefinitionType.list, serviceObject.Methods[0].Type);

            // Input Properties: UMRGetUsers
            Assert.AreEqual(5, serviceObject.Methods[0].InputProperties.Count);
            Assert.AreEqual("Name", serviceObject.Methods[0].InputProperties[0].Name);
            Assert.AreEqual("Description", serviceObject.Methods[0].InputProperties[1].Name);
            Assert.AreEqual("Email", serviceObject.Methods[0].InputProperties[2].Name);
            Assert.AreEqual("DisplayName", serviceObject.Methods[0].InputProperties[3].Name);
            Assert.AreEqual("sAMAccountName", serviceObject.Methods[0].InputProperties[4].Name);

            // Required Properties: UMRGetUsers
            Assert.AreEqual(0, serviceObject.Methods[0].Validation.RequiredProperties.Count);

            // Parameters: UMRGetUsers
            Assert.AreEqual(1, serviceObject.Methods[0].Parameters.Count);
            Assert.AreEqual("Label", serviceObject.Methods[0].Parameters[0].Name);
            Assert.AreEqual("Label", serviceObject.Methods[0].Parameters[0].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Methods[0].Parameters[0].Type);
            Assert.AreEqual(true, serviceObject.Methods[0].Parameters[0].IsRequired);

            // Return Properties: UMRGetUsers
            Assert.AreEqual(9, serviceObject.Methods[0].ReturnProperties.Count);
            Assert.AreEqual("Name", serviceObject.Methods[0].ReturnProperties[0].Name);
            Assert.AreEqual("Description", serviceObject.Methods[0].ReturnProperties[1].Name);
            Assert.AreEqual("Email", serviceObject.Methods[0].ReturnProperties[2].Name);
            Assert.AreEqual("Manager", serviceObject.Methods[0].ReturnProperties[3].Name);
            Assert.AreEqual("DisplayName", serviceObject.Methods[0].ReturnProperties[4].Name);
            Assert.AreEqual("FQN", serviceObject.Methods[0].ReturnProperties[5].Name);
            Assert.AreEqual("UserName", serviceObject.Methods[0].ReturnProperties[6].Name);
            Assert.AreEqual("ObjectSID", serviceObject.Methods[0].ReturnProperties[7].Name);
            Assert.AreEqual("sAMAccountName", serviceObject.Methods[0].ReturnProperties[8].Name);

            // ASSOCIATIONS: URMUser
            Assert.AreEqual(0, serviceObject.Associations.Count);
        }

        [TestMethod]
        [TestCategory("Describe K2NEServiceBroker")]
        public void Describe_Working_Hours_Configuration()
        {
            var serviceObject = SmartObjectHelper.GetServiceObject(K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance, "WorkingHoursConfiguration");

            // SERVICEOBJECT: WorkingHoursConfiguration
            Assert.AreEqual("WorkingHoursConfiguration", serviceObject.Name);
            Assert.AreEqual("Working Hours Configuration", serviceObject.Metadata.DisplayName);
            Assert.AreEqual("", serviceObject.ServiceObjectType ?? string.Empty);

            // PROPERTIES: WorkingHoursConfiguration
            Assert.AreEqual(20, serviceObject.Properties.Count);

            Assert.AreEqual("DefaultZone", serviceObject.Properties[0].Name);
            Assert.AreEqual("Default Zone", serviceObject.Properties[0].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.YesNo, serviceObject.Properties[0].Type);

            Assert.AreEqual("Description", serviceObject.Properties[1].Name);
            Assert.AreEqual("Description", serviceObject.Properties[1].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Memo, serviceObject.Properties[1].Type);

            Assert.AreEqual("DurationHours", serviceObject.Properties[2].Name);
            Assert.AreEqual("Duration Hours", serviceObject.Properties[2].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[2].Type);

            Assert.AreEqual("DurationMinutes", serviceObject.Properties[3].Name);
            Assert.AreEqual("Duration Minutes", serviceObject.Properties[3].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[3].Type);

            Assert.AreEqual("DurationSeconds", serviceObject.Properties[4].Name);
            Assert.AreEqual("Duration Seconds", serviceObject.Properties[4].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[4].Type);

            Assert.AreEqual("GMTOffset", serviceObject.Properties[5].Name);
            Assert.AreEqual("GMT Offset", serviceObject.Properties[5].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[5].Type);

            Assert.AreEqual("IsNonWorkDate", serviceObject.Properties[6].Name);
            Assert.AreEqual("Is Non Work Date", serviceObject.Properties[6].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.YesNo, serviceObject.Properties[6].Type);

            Assert.AreEqual("TimeOfDayHours", serviceObject.Properties[7].Name);
            Assert.AreEqual("Time Of Day Hours", serviceObject.Properties[7].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[7].Type);

            Assert.AreEqual("TimeOfDayMinutes", serviceObject.Properties[8].Name);
            Assert.AreEqual("Time Of Day Minutes", serviceObject.Properties[8].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[8].Type);

            Assert.AreEqual("TimeOfDaySeconds", serviceObject.Properties[9].Name);
            Assert.AreEqual("Time Of Day Seconds", serviceObject.Properties[9].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[9].Type);

            Assert.AreEqual("WorkDate", serviceObject.Properties[10].Name);
            Assert.AreEqual("Work Date", serviceObject.Properties[10].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.DateTime, serviceObject.Properties[10].Type);

            Assert.AreEqual("WorkDay", serviceObject.Properties[11].Name);
            Assert.AreEqual("Work Day", serviceObject.Properties[11].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[11].Type);

            Assert.AreEqual("ZoneGUID", serviceObject.Properties[12].Name);
            Assert.AreEqual("Zone GUID", serviceObject.Properties[12].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Guid, serviceObject.Properties[12].Type);

            Assert.AreEqual("ZoneName", serviceObject.Properties[13].Name);
            Assert.AreEqual("Zone Name", serviceObject.Properties[13].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[13].Type);

            Assert.AreEqual("NewZoneName", serviceObject.Properties[14].Name);
            Assert.AreEqual("New Zone Name", serviceObject.Properties[14].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[14].Type);

            Assert.AreEqual("FQN", serviceObject.Properties[15].Name);
            Assert.AreEqual("FQN", serviceObject.Properties[15].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[15].Type);

            Assert.AreEqual("UserName", serviceObject.Properties[16].Name);
            Assert.AreEqual("User Name", serviceObject.Properties[16].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[16].Type);

            Assert.AreEqual("ZoneExists", serviceObject.Properties[17].Name);
            Assert.AreEqual("Zone Exists", serviceObject.Properties[17].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.YesNo, serviceObject.Properties[17].Type);

            Assert.AreEqual("StartDateTime", serviceObject.Properties[18].Name);
            Assert.AreEqual("Start Date Time", serviceObject.Properties[18].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.DateTime, serviceObject.Properties[18].Type);

            Assert.AreEqual("FinishDateTime", serviceObject.Properties[19].Name);
            Assert.AreEqual("Finish Date Time", serviceObject.Properties[19].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.DateTime, serviceObject.Properties[19].Type);

            // METHODS: WorkingHoursConfiguration
            Assert.AreEqual(14, serviceObject.Methods.Count);

            // Method: CreateZone
            Assert.AreEqual("Create Zone", serviceObject.Methods[0].Metadata.DisplayName);
            Assert.AreEqual("CreateZone", serviceObject.Methods[0].Name);
            Assert.AreEqual(MethodDefinitionType.create, serviceObject.Methods[0].Type);

            // Input Properties: CreateZone
            Assert.AreEqual(4, serviceObject.Methods[0].InputProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[0].InputProperties[0].Name);
            Assert.AreEqual("Description", serviceObject.Methods[0].InputProperties[1].Name);
            Assert.AreEqual("GMTOffset", serviceObject.Methods[0].InputProperties[2].Name);
            Assert.AreEqual("DefaultZone", serviceObject.Methods[0].InputProperties[3].Name);

            // Required Properties: CreateZone
            Assert.AreEqual(2, serviceObject.Methods[0].Validation.RequiredProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[0].Validation.RequiredProperties[0].Name);
            Assert.AreEqual("GMTOffset", serviceObject.Methods[0].Validation.RequiredProperties[1].Name);

            // Parameters: CreateZone
            Assert.AreEqual(0, serviceObject.Methods[0].Parameters.Count);

            // Return Properties: CreateZone
            Assert.AreEqual(0, serviceObject.Methods[0].ReturnProperties.Count);

            // Method: SaveZone
            Assert.AreEqual("Save Zone", serviceObject.Methods[1].Metadata.DisplayName);
            Assert.AreEqual("SaveZone", serviceObject.Methods[1].Name);
            Assert.AreEqual(MethodDefinitionType.update, serviceObject.Methods[1].Type);

            // Input Properties: SaveZone
            Assert.AreEqual(5, serviceObject.Methods[1].InputProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[1].InputProperties[0].Name);
            Assert.AreEqual("NewZoneName", serviceObject.Methods[1].InputProperties[1].Name);
            Assert.AreEqual("Description", serviceObject.Methods[1].InputProperties[2].Name);
            Assert.AreEqual("GMTOffset", serviceObject.Methods[1].InputProperties[3].Name);
            Assert.AreEqual("DefaultZone", serviceObject.Methods[1].InputProperties[4].Name);

            // Required Properties: SaveZone
            Assert.AreEqual(1, serviceObject.Methods[1].Validation.RequiredProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[1].Validation.RequiredProperties[0].Name);

            // Parameters: SaveZone
            Assert.AreEqual(0, serviceObject.Methods[1].Parameters.Count);

            // Return Properties: SaveZone
            Assert.AreEqual(0, serviceObject.Methods[1].ReturnProperties.Count);

            // Method: DeleteZone
            Assert.AreEqual("Delete Zone", serviceObject.Methods[2].Metadata.DisplayName);
            Assert.AreEqual("DeleteZone", serviceObject.Methods[2].Name);
            Assert.AreEqual(MethodDefinitionType.delete, serviceObject.Methods[2].Type);

            // Input Properties: DeleteZone
            Assert.AreEqual(1, serviceObject.Methods[2].InputProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[2].InputProperties[0].Name);

            // Required Properties: DeleteZone
            Assert.AreEqual(1, serviceObject.Methods[2].Validation.RequiredProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[2].Validation.RequiredProperties[0].Name);

            // Parameters: DeleteZone
            Assert.AreEqual(0, serviceObject.Methods[2].Parameters.Count);

            // Return Properties: DeleteZone
            Assert.AreEqual(0, serviceObject.Methods[2].ReturnProperties.Count);

            // Method: LoadZone
            Assert.AreEqual("Load Zone", serviceObject.Methods[3].Metadata.DisplayName);
            Assert.AreEqual("LoadZone", serviceObject.Methods[3].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[3].Type);

            // Input Properties: LoadZone
            Assert.AreEqual(1, serviceObject.Methods[3].InputProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[3].InputProperties[0].Name);

            // Required Properties: LoadZone
            Assert.AreEqual(1, serviceObject.Methods[3].Validation.RequiredProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[3].Validation.RequiredProperties[0].Name);

            // Parameters: LoadZone
            Assert.AreEqual(0, serviceObject.Methods[3].Parameters.Count);

            // Return Properties: LoadZone
            Assert.AreEqual(4, serviceObject.Methods[3].ReturnProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[3].ReturnProperties[0].Name);
            Assert.AreEqual("Description", serviceObject.Methods[3].ReturnProperties[1].Name);
            Assert.AreEqual("GMTOffset", serviceObject.Methods[3].ReturnProperties[2].Name);
            Assert.AreEqual("DefaultZone", serviceObject.Methods[3].ReturnProperties[3].Name);

            // Method: ListZones
            Assert.AreEqual("List Zones", serviceObject.Methods[4].Metadata.DisplayName);
            Assert.AreEqual("ListZones", serviceObject.Methods[4].Name);
            Assert.AreEqual(MethodDefinitionType.list, serviceObject.Methods[4].Type);

            // Input Properties: ListZones
            Assert.AreEqual(0, serviceObject.Methods[4].InputProperties.Count);

            // Required Properties: ListZones
            Assert.AreEqual(0, serviceObject.Methods[4].Validation.RequiredProperties.Count);

            // Parameters: ListZones
            Assert.AreEqual(0, serviceObject.Methods[4].Parameters.Count);

            // Return Properties: ListZones
            Assert.AreEqual(1, serviceObject.Methods[4].ReturnProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[4].ReturnProperties[0].Name);

            // Method: ListZoneUsers
            Assert.AreEqual("List Zone Users", serviceObject.Methods[5].Metadata.DisplayName);
            Assert.AreEqual("ListZoneUsers", serviceObject.Methods[5].Name);
            Assert.AreEqual(MethodDefinitionType.list, serviceObject.Methods[5].Type);

            // Input Properties: ListZoneUsers
            Assert.AreEqual(1, serviceObject.Methods[5].InputProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[5].InputProperties[0].Name);

            // Required Properties: ListZoneUsers
            Assert.AreEqual(1, serviceObject.Methods[5].Validation.RequiredProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[5].Validation.RequiredProperties[0].Name);

            // Parameters: ListZoneUsers
            Assert.AreEqual(0, serviceObject.Methods[5].Parameters.Count);

            // Return Properties: ListZoneUsers
            Assert.AreEqual(2, serviceObject.Methods[5].ReturnProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[5].ReturnProperties[0].Name);
            Assert.AreEqual("UserName", serviceObject.Methods[5].ReturnProperties[1].Name);

            // Method: GetDefaultZone
            Assert.AreEqual("Get Default Zone", serviceObject.Methods[6].Metadata.DisplayName);
            Assert.AreEqual("GetDefaultZone", serviceObject.Methods[6].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[6].Type);

            // Input Properties: GetDefaultZone
            Assert.AreEqual(0, serviceObject.Methods[6].InputProperties.Count);

            // Required Properties: GetDefaultZone
            Assert.AreEqual(0, serviceObject.Methods[6].Validation.RequiredProperties.Count);

            // Parameters: GetDefaultZone
            Assert.AreEqual(0, serviceObject.Methods[6].Parameters.Count);

            // Return Properties: GetDefaultZone
            Assert.AreEqual(1, serviceObject.Methods[6].ReturnProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[6].ReturnProperties[0].Name);

            // Method: SetDefaultZone
            Assert.AreEqual("Set Default Zone", serviceObject.Methods[7].Metadata.DisplayName);
            Assert.AreEqual("SetDefaultZone", serviceObject.Methods[7].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[7].Type);

            // Input Properties: SetDefaultZone
            Assert.AreEqual(1, serviceObject.Methods[7].InputProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[7].InputProperties[0].Name);

            // Required Properties: SetDefaultZone
            Assert.AreEqual(1, serviceObject.Methods[7].Validation.RequiredProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[7].Validation.RequiredProperties[0].Name);

            // Parameters: SetDefaultZone
            Assert.AreEqual(0, serviceObject.Methods[7].Parameters.Count);

            // Return Properties: SetDefaultZone
            Assert.AreEqual(0, serviceObject.Methods[7].ReturnProperties.Count);

            // Method: ZoneExists
            Assert.AreEqual("Zone Exists", serviceObject.Methods[8].Metadata.DisplayName);
            Assert.AreEqual("ZoneExists", serviceObject.Methods[8].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[8].Type);

            // Input Properties: ZoneExists
            Assert.AreEqual(1, serviceObject.Methods[8].InputProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[8].InputProperties[0].Name);

            // Required Properties: ZoneExists
            Assert.AreEqual(1, serviceObject.Methods[8].Validation.RequiredProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[8].Validation.RequiredProperties[0].Name);

            // Parameters: ZoneExists
            Assert.AreEqual(0, serviceObject.Methods[8].Parameters.Count);

            // Return Properties: ZoneExists
            Assert.AreEqual(1, serviceObject.Methods[8].ReturnProperties.Count);
            Assert.AreEqual("ZoneExists", serviceObject.Methods[8].ReturnProperties[0].Name);

            // Method: ZoneCalculateEvent
            Assert.AreEqual("Zone Calculate Event", serviceObject.Methods[9].Metadata.DisplayName);
            Assert.AreEqual("ZoneCalculateEvent", serviceObject.Methods[9].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[9].Type);

            // Input Properties: ZoneCalculateEvent
            Assert.AreEqual(5, serviceObject.Methods[9].InputProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[9].InputProperties[0].Name);
            Assert.AreEqual("StartDateTime", serviceObject.Methods[9].InputProperties[1].Name);
            Assert.AreEqual("DurationHours", serviceObject.Methods[9].InputProperties[2].Name);
            Assert.AreEqual("DurationMinutes", serviceObject.Methods[9].InputProperties[3].Name);
            Assert.AreEqual("DurationSeconds", serviceObject.Methods[9].InputProperties[4].Name);

            // Required Properties: ZoneCalculateEvent
            Assert.AreEqual(2, serviceObject.Methods[9].Validation.RequiredProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[9].Validation.RequiredProperties[0].Name);
            Assert.AreEqual("StartDateTime", serviceObject.Methods[9].Validation.RequiredProperties[1].Name);

            // Parameters: ZoneCalculateEvent
            Assert.AreEqual(0, serviceObject.Methods[9].Parameters.Count);

            // Return Properties: ZoneCalculateEvent
            Assert.AreEqual(1, serviceObject.Methods[9].ReturnProperties.Count);
            Assert.AreEqual("FinishDateTime", serviceObject.Methods[9].ReturnProperties[0].Name);

            // Method: UserSetZone
            Assert.AreEqual("User Set Zone", serviceObject.Methods[10].Metadata.DisplayName);
            Assert.AreEqual("UserSetZone", serviceObject.Methods[10].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[10].Type);

            // Input Properties: UserSetZone
            Assert.AreEqual(2, serviceObject.Methods[10].InputProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[10].InputProperties[0].Name);
            Assert.AreEqual("ZoneName", serviceObject.Methods[10].InputProperties[1].Name);

            // Required Properties: UserSetZone
            Assert.AreEqual(2, serviceObject.Methods[10].Validation.RequiredProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[10].Validation.RequiredProperties[0].Name);
            Assert.AreEqual("FQN", serviceObject.Methods[10].Validation.RequiredProperties[1].Name);

            // Parameters: UserSetZone
            Assert.AreEqual(0, serviceObject.Methods[10].Parameters.Count);

            // Return Properties: UserSetZone
            Assert.AreEqual(0, serviceObject.Methods[10].ReturnProperties.Count);

            // Method: UserGetZone
            Assert.AreEqual("User Get Zone", serviceObject.Methods[11].Metadata.DisplayName);
            Assert.AreEqual("UserGetZone", serviceObject.Methods[11].Name);
            Assert.AreEqual(MethodDefinitionType.read, serviceObject.Methods[11].Type);

            // Input Properties: UserGetZone
            Assert.AreEqual(1, serviceObject.Methods[11].InputProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[11].InputProperties[0].Name);

            // Required Properties: UserGetZone
            Assert.AreEqual(0, serviceObject.Methods[11].Validation.RequiredProperties.Count);

            // Parameters: UserGetZone
            Assert.AreEqual(0, serviceObject.Methods[11].Parameters.Count);

            // Return Properties: UserGetZone
            Assert.AreEqual(1, serviceObject.Methods[11].ReturnProperties.Count);
            Assert.AreEqual("ZoneName", serviceObject.Methods[11].ReturnProperties[0].Name);

            // Method: UserDeleteZone
            Assert.AreEqual("User Delete Zone", serviceObject.Methods[12].Metadata.DisplayName);
            Assert.AreEqual("UserDeleteZone", serviceObject.Methods[12].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[12].Type);

            // Input Properties: UserDeleteZone
            Assert.AreEqual(1, serviceObject.Methods[12].InputProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[12].InputProperties[0].Name);

            // Required Properties: UserDeleteZone
            Assert.AreEqual(0, serviceObject.Methods[12].Validation.RequiredProperties.Count);

            // Parameters: UserDeleteZone
            Assert.AreEqual(0, serviceObject.Methods[12].Parameters.Count);

            // Return Properties: UserDeleteZone
            Assert.AreEqual(0, serviceObject.Methods[12].ReturnProperties.Count);

            // Method: UserCalculateEvent
            Assert.AreEqual("User Calculate Event", serviceObject.Methods[13].Metadata.DisplayName);
            Assert.AreEqual("UserCalculateEvent", serviceObject.Methods[13].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[13].Type);

            // Input Properties: UserCalculateEvent
            Assert.AreEqual(5, serviceObject.Methods[13].InputProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[13].InputProperties[0].Name);
            Assert.AreEqual("StartDateTime", serviceObject.Methods[13].InputProperties[1].Name);
            Assert.AreEqual("DurationHours", serviceObject.Methods[13].InputProperties[2].Name);
            Assert.AreEqual("DurationMinutes", serviceObject.Methods[13].InputProperties[3].Name);
            Assert.AreEqual("DurationSeconds", serviceObject.Methods[13].InputProperties[4].Name);

            // Required Properties: UserCalculateEvent
            Assert.AreEqual(2, serviceObject.Methods[13].Validation.RequiredProperties.Count);
            Assert.AreEqual("FQN", serviceObject.Methods[13].Validation.RequiredProperties[0].Name);
            Assert.AreEqual("StartDateTime", serviceObject.Methods[13].Validation.RequiredProperties[1].Name);

            // Parameters: UserCalculateEvent
            Assert.AreEqual(0, serviceObject.Methods[13].Parameters.Count);

            // Return Properties: UserCalculateEvent
            Assert.AreEqual(1, serviceObject.Methods[13].ReturnProperties.Count);
            Assert.AreEqual("FinishDateTime", serviceObject.Methods[13].ReturnProperties[0].Name);

            // ASSOCIATIONS: WorkingHoursConfiguration
            Assert.AreEqual(0, serviceObject.Associations.Count);
        }

        [TestMethod]
        [TestCategory("Describe K2NEServiceBroker")]
        public void Describe_Worklist()
        {
            var serviceObject = SmartObjectHelper.GetServiceObject(K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance, "Worklist");

            // SERVICEOBJECT: Worklist
            Assert.AreEqual("Worklist", serviceObject.Name);
            Assert.AreEqual("Worklist", serviceObject.Metadata.DisplayName);
            Assert.AreEqual("", serviceObject.ServiceObjectType ?? string.Empty);

            // PROPERTIES: Worklist
            Assert.AreEqual(32, serviceObject.Properties.Count);

            Assert.AreEqual("ProcessName", serviceObject.Properties[0].Name);
            Assert.AreEqual("Process Name", serviceObject.Properties[0].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[0].Type);

            Assert.AreEqual("Folder", serviceObject.Properties[1].Name);
            Assert.AreEqual("Folder", serviceObject.Properties[1].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[1].Type);

            Assert.AreEqual("ProcessFullname", serviceObject.Properties[2].Name);
            Assert.AreEqual("Process Fullname", serviceObject.Properties[2].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[2].Type);

            Assert.AreEqual("ProcessDescription", serviceObject.Properties[3].Name);
            Assert.AreEqual("Process Description", serviceObject.Properties[3].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[3].Type);

            Assert.AreEqual("PocessMetadata", serviceObject.Properties[4].Name);
            Assert.AreEqual("Pocess Metadata", serviceObject.Properties[4].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[4].Type);

            Assert.AreEqual("ProcessStatus", serviceObject.Properties[5].Name);
            Assert.AreEqual("Process Status", serviceObject.Properties[5].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[5].Type);

            Assert.AreEqual("ProcessPriority", serviceObject.Properties[6].Name);
            Assert.AreEqual("Process Priority", serviceObject.Properties[6].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[6].Type);

            Assert.AreEqual("ProcessStartDate", serviceObject.Properties[7].Name);
            Assert.AreEqual("Process Start Date", serviceObject.Properties[7].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.DateTime, serviceObject.Properties[7].Type);

            Assert.AreEqual("ProcessExpectedDuration", serviceObject.Properties[8].Name);
            Assert.AreEqual("Process Expected Duration", serviceObject.Properties[8].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[8].Type);

            Assert.AreEqual("ProcessGuid", serviceObject.Properties[9].Name);
            Assert.AreEqual("Process Guid", serviceObject.Properties[9].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Guid, serviceObject.Properties[9].Type);

            Assert.AreEqual("ProcessId", serviceObject.Properties[10].Name);
            Assert.AreEqual("Process Id", serviceObject.Properties[10].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[10].Type);

            Assert.AreEqual("ActivityId", serviceObject.Properties[11].Name);
            Assert.AreEqual("Activity Id", serviceObject.Properties[11].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[11].Type);

            Assert.AreEqual("ActivityName", serviceObject.Properties[12].Name);
            Assert.AreEqual("Activity Name", serviceObject.Properties[12].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[12].Type);

            Assert.AreEqual("ActivityPriority", serviceObject.Properties[13].Name);
            Assert.AreEqual("Activity Priority", serviceObject.Properties[13].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[13].Type);

            Assert.AreEqual("ActivityDescription", serviceObject.Properties[14].Name);
            Assert.AreEqual("Activity Description", serviceObject.Properties[14].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[14].Type);

            Assert.AreEqual("ActivityMetadata", serviceObject.Properties[15].Name);
            Assert.AreEqual("Activity Metadata", serviceObject.Properties[15].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[15].Type);

            Assert.AreEqual("ActivityStartDate", serviceObject.Properties[16].Name);
            Assert.AreEqual("Activity Start Date", serviceObject.Properties[16].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.DateTime, serviceObject.Properties[16].Type);

            Assert.AreEqual("ActivityExpectedDuration", serviceObject.Properties[17].Name);
            Assert.AreEqual("Activity Expected Duration", serviceObject.Properties[17].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[17].Type);

            Assert.AreEqual("EventId", serviceObject.Properties[18].Name);
            Assert.AreEqual("Event Id", serviceObject.Properties[18].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[18].Type);

            Assert.AreEqual("EventName", serviceObject.Properties[19].Name);
            Assert.AreEqual("Event Name", serviceObject.Properties[19].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[19].Type);

            Assert.AreEqual("EventMetadata", serviceObject.Properties[20].Name);
            Assert.AreEqual("Event Metadata", serviceObject.Properties[20].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[20].Type);

            Assert.AreEqual("EventDescription", serviceObject.Properties[21].Name);
            Assert.AreEqual("Event Description", serviceObject.Properties[21].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[21].Type);

            Assert.AreEqual("EventPriority", serviceObject.Properties[22].Name);
            Assert.AreEqual("Event Priority", serviceObject.Properties[22].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[22].Type);

            Assert.AreEqual("EventStartDate", serviceObject.Properties[23].Name);
            Assert.AreEqual("Event Start Date", serviceObject.Properties[23].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.DateTime, serviceObject.Properties[23].Type);

            Assert.AreEqual("EventExpectedDuration", serviceObject.Properties[24].Name);
            Assert.AreEqual("Event Expected Duration", serviceObject.Properties[24].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[24].Type);

            Assert.AreEqual("Status", serviceObject.Properties[25].Name);
            Assert.AreEqual("Status", serviceObject.Properties[25].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[25].Type);

            Assert.AreEqual("OriginalDestination", serviceObject.Properties[26].Name);
            Assert.AreEqual("Original Destination", serviceObject.Properties[26].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[26].Type);

            Assert.AreEqual("Folio", serviceObject.Properties[27].Name);
            Assert.AreEqual("Folio", serviceObject.Properties[27].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[27].Type);

            Assert.AreEqual("Data", serviceObject.Properties[28].Name);
            Assert.AreEqual("Data", serviceObject.Properties[28].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[28].Type);

            Assert.AreEqual("SerialNumber", serviceObject.Properties[29].Name);
            Assert.AreEqual("Serial Number", serviceObject.Properties[29].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[29].Type);

            Assert.AreEqual("IncludeShared", serviceObject.Properties[30].Name);
            Assert.AreEqual("Include Shared", serviceObject.Properties[30].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.YesNo, serviceObject.Properties[30].Type);

            Assert.AreEqual("ExcludeAllocated", serviceObject.Properties[31].Name);
            Assert.AreEqual("Exclude Allocated", serviceObject.Properties[31].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.YesNo, serviceObject.Properties[31].Type);

            // METHODS: Worklist
            Assert.AreEqual(1, serviceObject.Methods.Count);

            // Method: GetWorklist
            Assert.AreEqual("Get Worklist", serviceObject.Methods[0].Metadata.DisplayName);
            Assert.AreEqual("GetWorklist", serviceObject.Methods[0].Name);
            Assert.AreEqual(MethodDefinitionType.list, serviceObject.Methods[0].Type);

            // Input Properties: GetWorklist
            Assert.AreEqual(27, serviceObject.Methods[0].InputProperties.Count);
            Assert.AreEqual("IncludeShared", serviceObject.Methods[0].InputProperties[0].Name);
            Assert.AreEqual("ExcludeAllocated", serviceObject.Methods[0].InputProperties[1].Name);
            Assert.AreEqual("ProcessName", serviceObject.Methods[0].InputProperties[2].Name);
            Assert.AreEqual("Folder", serviceObject.Methods[0].InputProperties[3].Name);
            Assert.AreEqual("ProcessFullname", serviceObject.Methods[0].InputProperties[4].Name);
            Assert.AreEqual("ProcessDescription", serviceObject.Methods[0].InputProperties[5].Name);
            Assert.AreEqual("PocessMetadata", serviceObject.Methods[0].InputProperties[6].Name);
            Assert.AreEqual("ProcessStatus", serviceObject.Methods[0].InputProperties[7].Name);
            Assert.AreEqual("ProcessPriority", serviceObject.Methods[0].InputProperties[8].Name);
            Assert.AreEqual("ProcessStartDate", serviceObject.Methods[0].InputProperties[9].Name);
            Assert.AreEqual("ProcessExpectedDuration", serviceObject.Methods[0].InputProperties[10].Name);
            Assert.AreEqual("ProcessId", serviceObject.Methods[0].InputProperties[11].Name);
            Assert.AreEqual("ActivityName", serviceObject.Methods[0].InputProperties[12].Name);
            Assert.AreEqual("ActivityPriority", serviceObject.Methods[0].InputProperties[13].Name);
            Assert.AreEqual("ActivityDescription", serviceObject.Methods[0].InputProperties[14].Name);
            Assert.AreEqual("ActivityMetadata", serviceObject.Methods[0].InputProperties[15].Name);
            Assert.AreEqual("ActivityStartDate", serviceObject.Methods[0].InputProperties[16].Name);
            Assert.AreEqual("ActivityExpectedDuration", serviceObject.Methods[0].InputProperties[17].Name);
            Assert.AreEqual("EventName", serviceObject.Methods[0].InputProperties[18].Name);
            Assert.AreEqual("EventMetadata", serviceObject.Methods[0].InputProperties[19].Name);
            Assert.AreEqual("EventDescription", serviceObject.Methods[0].InputProperties[20].Name);
            Assert.AreEqual("EventPriority", serviceObject.Methods[0].InputProperties[21].Name);
            Assert.AreEqual("EventStartDate", serviceObject.Methods[0].InputProperties[22].Name);
            Assert.AreEqual("EventExpectedDuration", serviceObject.Methods[0].InputProperties[23].Name);
            Assert.AreEqual("Status", serviceObject.Methods[0].InputProperties[24].Name);
            Assert.AreEqual("Folio", serviceObject.Methods[0].InputProperties[25].Name);
            Assert.AreEqual("SerialNumber", serviceObject.Methods[0].InputProperties[26].Name);

            // Required Properties: GetWorklist
            Assert.AreEqual(0, serviceObject.Methods[0].Validation.RequiredProperties.Count);

            // Parameters: GetWorklist
            Assert.AreEqual(0, serviceObject.Methods[0].Parameters.Count);

            // Return Properties: GetWorklist
            Assert.AreEqual(30, serviceObject.Methods[0].ReturnProperties.Count);
            Assert.AreEqual("ProcessName", serviceObject.Methods[0].ReturnProperties[0].Name);
            Assert.AreEqual("Folder", serviceObject.Methods[0].ReturnProperties[1].Name);
            Assert.AreEqual("ProcessFullname", serviceObject.Methods[0].ReturnProperties[2].Name);
            Assert.AreEqual("ProcessDescription", serviceObject.Methods[0].ReturnProperties[3].Name);
            Assert.AreEqual("PocessMetadata", serviceObject.Methods[0].ReturnProperties[4].Name);
            Assert.AreEqual("ProcessStatus", serviceObject.Methods[0].ReturnProperties[5].Name);
            Assert.AreEqual("ProcessPriority", serviceObject.Methods[0].ReturnProperties[6].Name);
            Assert.AreEqual("ProcessStartDate", serviceObject.Methods[0].ReturnProperties[7].Name);
            Assert.AreEqual("ProcessExpectedDuration", serviceObject.Methods[0].ReturnProperties[8].Name);
            Assert.AreEqual("ProcessGuid", serviceObject.Methods[0].ReturnProperties[9].Name);
            Assert.AreEqual("ProcessId", serviceObject.Methods[0].ReturnProperties[10].Name);
            Assert.AreEqual("ActivityId", serviceObject.Methods[0].ReturnProperties[11].Name);
            Assert.AreEqual("ActivityName", serviceObject.Methods[0].ReturnProperties[12].Name);
            Assert.AreEqual("ActivityPriority", serviceObject.Methods[0].ReturnProperties[13].Name);
            Assert.AreEqual("ActivityDescription", serviceObject.Methods[0].ReturnProperties[14].Name);
            Assert.AreEqual("ActivityMetadata", serviceObject.Methods[0].ReturnProperties[15].Name);
            Assert.AreEqual("ActivityStartDate", serviceObject.Methods[0].ReturnProperties[16].Name);
            Assert.AreEqual("ActivityExpectedDuration", serviceObject.Methods[0].ReturnProperties[17].Name);
            Assert.AreEqual("EventId", serviceObject.Methods[0].ReturnProperties[18].Name);
            Assert.AreEqual("EventName", serviceObject.Methods[0].ReturnProperties[19].Name);
            Assert.AreEqual("EventMetadata", serviceObject.Methods[0].ReturnProperties[20].Name);
            Assert.AreEqual("EventDescription", serviceObject.Methods[0].ReturnProperties[21].Name);
            Assert.AreEqual("EventPriority", serviceObject.Methods[0].ReturnProperties[22].Name);
            Assert.AreEqual("EventStartDate", serviceObject.Methods[0].ReturnProperties[23].Name);
            Assert.AreEqual("EventExpectedDuration", serviceObject.Methods[0].ReturnProperties[24].Name);
            Assert.AreEqual("Status", serviceObject.Methods[0].ReturnProperties[25].Name);
            Assert.AreEqual("OriginalDestination", serviceObject.Methods[0].ReturnProperties[26].Name);
            Assert.AreEqual("Folio", serviceObject.Methods[0].ReturnProperties[27].Name);
            Assert.AreEqual("Data", serviceObject.Methods[0].ReturnProperties[28].Name);
            Assert.AreEqual("SerialNumber", serviceObject.Methods[0].ReturnProperties[29].Name);

            // ASSOCIATIONS: Worklist
            Assert.AreEqual(0, serviceObject.Associations.Count);
        }

        [TestMethod]
        [TestCategory("Describe K2NEServiceBroker")]
        public void Describe_Worklist_Item()
        {
            var serviceObject = SmartObjectHelper.GetServiceObject(K2NEServiceBrokerIntegrationTestsServiceInstanceSettings.Instance, "WorklistItem");

            // SERVICEOBJECT: WorklistItem
            Assert.AreEqual("WorklistItem", serviceObject.Name);
            Assert.AreEqual("Worklist Item", serviceObject.Metadata.DisplayName);
            Assert.AreEqual("", serviceObject.ServiceObjectType ?? string.Empty);

            // PROPERTIES: WorklistItem
            Assert.AreEqual(5, serviceObject.Properties.Count);

            Assert.AreEqual("SerialNumber", serviceObject.Properties[0].Name);
            Assert.AreEqual("Serial Number", serviceObject.Properties[0].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[0].Type);

            Assert.AreEqual("FQN", serviceObject.Properties[1].Name);
            Assert.AreEqual("FQN", serviceObject.Properties[1].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[1].Type);

            Assert.AreEqual("ActionName", serviceObject.Properties[2].Name);
            Assert.AreEqual("Action Name", serviceObject.Properties[2].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[2].Type);

            Assert.AreEqual("ProcessId", serviceObject.Properties[3].Name);
            Assert.AreEqual("Process Id", serviceObject.Properties[3].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Number, serviceObject.Properties[3].Type);

            Assert.AreEqual("ActivityName", serviceObject.Properties[4].Name);
            Assert.AreEqual("Activity Name", serviceObject.Properties[4].Metadata.DisplayName);
            Assert.AreEqual(PropertyDefinitionType.Text, serviceObject.Properties[4].Type);

            // METHODS: WorklistItem
            Assert.AreEqual(3, serviceObject.Methods.Count);

            // Method: ReleaseWorklistItem
            Assert.AreEqual("Release Worklist Item", serviceObject.Methods[0].Metadata.DisplayName);
            Assert.AreEqual("ReleaseWorklistItem", serviceObject.Methods[0].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[0].Type);

            // Input Properties: ReleaseWorklistItem
            Assert.AreEqual(1, serviceObject.Methods[0].InputProperties.Count);
            Assert.AreEqual("SerialNumber", serviceObject.Methods[0].InputProperties[0].Name);

            // Required Properties: ReleaseWorklistItem
            Assert.AreEqual(1, serviceObject.Methods[0].Validation.RequiredProperties.Count);
            Assert.AreEqual("SerialNumber", serviceObject.Methods[0].Validation.RequiredProperties[0].Name);

            // Parameters: ReleaseWorklistItem
            Assert.AreEqual(0, serviceObject.Methods[0].Parameters.Count);

            // Return Properties: ReleaseWorklistItem
            Assert.AreEqual(0, serviceObject.Methods[0].ReturnProperties.Count);

            // Method: ActionWorklistItem
            Assert.AreEqual("Action Worklist Item", serviceObject.Methods[1].Metadata.DisplayName);
            Assert.AreEqual("ActionWorklistItem", serviceObject.Methods[1].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[1].Type);

            // Input Properties: ActionWorklistItem
            Assert.AreEqual(3, serviceObject.Methods[1].InputProperties.Count);
            Assert.AreEqual("ProcessId", serviceObject.Methods[1].InputProperties[0].Name);
            Assert.AreEqual("ActivityName", serviceObject.Methods[1].InputProperties[1].Name);
            Assert.AreEqual("ActionName", serviceObject.Methods[1].InputProperties[2].Name);

            // Required Properties: ActionWorklistItem
            Assert.AreEqual(3, serviceObject.Methods[1].Validation.RequiredProperties.Count);
            Assert.AreEqual("ProcessId", serviceObject.Methods[1].Validation.RequiredProperties[0].Name);
            Assert.AreEqual("ActivityName", serviceObject.Methods[1].Validation.RequiredProperties[1].Name);
            Assert.AreEqual("ActionName", serviceObject.Methods[1].Validation.RequiredProperties[2].Name);

            // Parameters: ActionWorklistItem
            Assert.AreEqual(0, serviceObject.Methods[1].Parameters.Count);

            // Return Properties: ActionWorklistItem
            Assert.AreEqual(0, serviceObject.Methods[1].ReturnProperties.Count);

            // Method: RedirectWorklistItem
            Assert.AreEqual("Redirect Worklist Item", serviceObject.Methods[2].Metadata.DisplayName);
            Assert.AreEqual("RedirectWorklistItem", serviceObject.Methods[2].Name);
            Assert.AreEqual(MethodDefinitionType.execute, serviceObject.Methods[2].Type);

            // Input Properties: RedirectWorklistItem
            Assert.AreEqual(2, serviceObject.Methods[2].InputProperties.Count);
            Assert.AreEqual("SerialNumber", serviceObject.Methods[2].InputProperties[0].Name);
            Assert.AreEqual("FQN", serviceObject.Methods[2].InputProperties[1].Name);

            // Required Properties: RedirectWorklistItem
            Assert.AreEqual(2, serviceObject.Methods[2].Validation.RequiredProperties.Count);
            Assert.AreEqual("SerialNumber", serviceObject.Methods[2].Validation.RequiredProperties[0].Name);
            Assert.AreEqual("FQN", serviceObject.Methods[2].Validation.RequiredProperties[1].Name);

            // Parameters: RedirectWorklistItem
            Assert.AreEqual(0, serviceObject.Methods[2].Parameters.Count);

            // Return Properties: RedirectWorklistItem
            Assert.AreEqual(0, serviceObject.Methods[2].ReturnProperties.Count);

            // ASSOCIATIONS: WorklistItem
            Assert.AreEqual(0, serviceObject.Associations.Count);
        }
    }
}