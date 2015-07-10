using K2Field.K2NE.ServiceBroker.Helpers;
using SourceCode.Hosting.Server.Interfaces;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using SourceCode.Workflow.Client;
using System;
using System.Collections.Generic;
using System.Data;


namespace K2Field.K2NE.ServiceBroker.ServiceObjects
{
    public class IdentitySO : ServiceObjectBase
    {

        public IdentitySO(K2NEServiceBroker broker)
            : base(broker)
        {
        }

        public override List<ServiceObject> DescribeServiceObjects()
        {
            ServiceObject so = Helper.CreateServiceObject("Identity", "Useful methods to determine the identities being used.");


            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.CurrentPrincipalAuthType, SoType.Text, "The current principal's authentication type"));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.CurrentPrincipalName, SoType.Text, "The current principal's authentication identity name."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.FQN, SoType.Text, "The K2 FQN of the user."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.ResolveContainers, SoType.YesNo, "If Identity containers should be also resolved."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.ResolveMembers, SoType.YesNo, "If Identity members should be also resolved."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.IsResolved, SoType.YesNo, "Result if Resolving was successful."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.IdentityDescription, SoType.Text, "The users description."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.IdentityDisplayName, SoType.Text, "The users displayname."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.UserEmail, SoType.Text, "The users email address."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.UserManager, SoType.Text, "The users manager."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.UserName, SoType.Text, "The users name."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.UserUserLabel, SoType.Text, "The users label."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.CallingFQN, SoType.Text, "The FQN determined by the service instance."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.WindowsIdentityAuthType, SoType.Text, "Windows Identity Authentication Type"));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.WindowsIdentityName, SoType.Text, "WIndows Identity Name"));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.ServiceBrokerAuthType, SoType.Text, "Service Broker Authentication Type"));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.ServiceBrokerUserName, SoType.Text, "Service Broker UserName"));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.ServiceBrokerPassword, SoType.Text, "Service Broker password"));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.UserWindowsImpersonation, SoType.YesNo, "Tells the service broker to use (windows) impersonation or not."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.DefaultNetworkCredentialsDomain, SoType.Text, "Default Network Credentials Domain."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.DefaultNetworkCredentialsPassword, SoType.Text, "Default Network Credentials Password."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.DefaultNetworkCredentialsUsername, SoType.Text, "Default Network Credentials Username."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.UserCultureDateTimeFormat, SoType.Text, "User Culture Date/Time format."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.UserCultureDisplayName, SoType.Text, "User Culture Display Name."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.UserCultureLCID, SoType.Text, "User Culture LCID."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.UserCultureName, SoType.Text, "User Culture Name."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.UserCultureNumberFormat, SoType.Text, "User Culture Number format."));



            Method mGetWorkflowClientIdentity = Helper.CreateMethod(Constants.Methods.Identity.ReadWorkflowClientIdentity, "Retrieve who you are for the K2 Client API", MethodType.Read);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.Properties.Identity.FQN);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.Properties.Identity.IdentityDescription);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.Properties.Identity.IdentityDisplayName);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.Properties.Identity.UserEmail);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.Properties.Identity.UserManager);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.Properties.Identity.UserName);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.Properties.Identity.UserUserLabel);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.Properties.Identity.CallingFQN);

            mGetWorkflowClientIdentity.InputProperties.Add(Constants.Properties.Identity.UserWindowsImpersonation);
            so.Methods.Create(mGetWorkflowClientIdentity);

            Method mGetThreadIdentity = Helper.CreateMethod(Constants.Methods.Identity.ReadThreadIdentity, "Retrieve who you are for the API Identity", MethodType.Read);
            mGetThreadIdentity.ReturnProperties.Add(Constants.Properties.Identity.CallingFQN);
            mGetThreadIdentity.ReturnProperties.Add(Constants.Properties.Identity.CurrentPrincipalAuthType);
            mGetThreadIdentity.ReturnProperties.Add(Constants.Properties.Identity.CurrentPrincipalName);
            mGetThreadIdentity.ReturnProperties.Add(Constants.Properties.Identity.WindowsIdentityAuthType);
            mGetThreadIdentity.ReturnProperties.Add(Constants.Properties.Identity.WindowsIdentityName);
            mGetThreadIdentity.ReturnProperties.Add(Constants.Properties.Identity.ServiceBrokerAuthType);
            mGetThreadIdentity.ReturnProperties.Add(Constants.Properties.Identity.ServiceBrokerUserName);
            mGetThreadIdentity.ReturnProperties.Add(Constants.Properties.Identity.ServiceBrokerPassword);
            mGetThreadIdentity.ReturnProperties.Add(Constants.Properties.Identity.DefaultNetworkCredentialsDomain);
            mGetThreadIdentity.ReturnProperties.Add(Constants.Properties.Identity.DefaultNetworkCredentialsPassword);
            mGetThreadIdentity.ReturnProperties.Add(Constants.Properties.Identity.DefaultNetworkCredentialsUsername);
            mGetThreadIdentity.ReturnProperties.Add(Constants.Properties.Identity.UserCultureDateTimeFormat);
            mGetThreadIdentity.ReturnProperties.Add(Constants.Properties.Identity.UserCultureDisplayName);
            mGetThreadIdentity.ReturnProperties.Add(Constants.Properties.Identity.UserCultureLCID);
            mGetThreadIdentity.ReturnProperties.Add(Constants.Properties.Identity.UserCultureName);
            mGetThreadIdentity.ReturnProperties.Add(Constants.Properties.Identity.UserCultureNumberFormat);
            mGetThreadIdentity.InputProperties.Add(Constants.Properties.Identity.UserWindowsImpersonation);
            so.Methods.Create(mGetThreadIdentity);

            var mResolveUser = Helper.CreateMethod(Constants.Methods.Identity.ResolveUserIdentity, "Resolve User Identity",
                MethodType.Execute);
            mResolveUser.ReturnProperties.Add(Constants.Properties.Identity.IsResolved);
            mResolveUser.InputProperties.Add(Constants.Properties.Identity.FQN);
            mResolveUser.InputProperties.Add(Constants.Properties.Identity.ResolveContainers);
            mResolveUser.Validation.RequiredProperties.Add(Constants.Properties.Identity.FQN);
            so.Methods.Create(mResolveUser);

            var mResolveGroup = Helper.CreateMethod(Constants.Methods.Identity.ResolveGroupIdentity, "Resolve Group Identity",
                MethodType.Execute);
            mResolveGroup.ReturnProperties.Add(Constants.Properties.Identity.IsResolved);
            mResolveGroup.InputProperties.Add(Constants.Properties.Identity.FQN);
            mResolveGroup.InputProperties.Add(Constants.Properties.Identity.ResolveContainers);
            mResolveGroup.InputProperties.Add(Constants.Properties.Identity.ResolveMembers);
            mResolveGroup.Validation.RequiredProperties.Add(Constants.Properties.Identity.FQN);
            so.Methods.Create(mResolveGroup);

            var mResolveRole = Helper.CreateMethod(Constants.Methods.Identity.ResolveRoleIdentity, "Resolve Role Identity",
                MethodType.Execute);
            mResolveRole.ReturnProperties.Add(Constants.Properties.Identity.IsResolved);
            mResolveRole.InputProperties.Add(Constants.Properties.Identity.FQN);
            mResolveRole.InputProperties.Add(Constants.Properties.Identity.ResolveContainers);
            mResolveRole.InputProperties.Add(Constants.Properties.Identity.ResolveMembers);
            mResolveRole.Validation.RequiredProperties.Add(Constants.Properties.Identity.FQN);
            so.Methods.Create(mResolveRole);

            return new List<ServiceObject> { so };
        }
        

        public override void Execute()
        {
            switch (ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.Identity.ReadWorkflowClientIdentity:
                    WhoAmI();
                    break;

                case Constants.Methods.Identity.ReadThreadIdentity:
                    ReadThreadIdentity();
                    break;

                case Constants.Methods.Identity.ResolveUserIdentity:
                    ResolveIdentity(IdentityType.User);
                    break;
                case Constants.Methods.Identity.ResolveGroupIdentity:
                    ResolveIdentity(IdentityType.Group);
                    break;
                case Constants.Methods.Identity.ResolveRoleIdentity:
                    ResolveIdentity(IdentityType.Role);
                    break;
            }

        }


        public void ReadThreadIdentity()
        {
            if (GetBoolProperty(Constants.Properties.Identity.UserWindowsImpersonation))
            {
                System.Security.Principal.WindowsIdentity.Impersonate(IntPtr.Zero);
            }
            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            DataRow dr = results.NewRow();
            dr[Constants.Properties.Identity.WindowsIdentityName] = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            dr[Constants.Properties.Identity.WindowsIdentityAuthType] = System.Security.Principal.WindowsIdentity.GetCurrent().AuthenticationType;
            dr[Constants.Properties.Identity.CurrentPrincipalName] = System.Threading.Thread.CurrentPrincipal.Identity.Name;
            dr[Constants.Properties.Identity.CurrentPrincipalAuthType] = System.Threading.Thread.CurrentPrincipal.Identity.AuthenticationType;
            dr[Constants.Properties.Identity.ServiceBrokerUserName] = ServiceBroker.Service.ServiceConfiguration.ServiceAuthentication.UserName;
            dr[Constants.Properties.Identity.ServiceBrokerPassword] = ServiceBroker.Service.ServiceConfiguration.ServiceAuthentication.Password;
            dr[Constants.Properties.Identity.ServiceBrokerAuthType] = ServiceBroker.Service.ServiceConfiguration.ServiceAuthentication.AuthenticationMode.ToString();
            dr[Constants.Properties.Identity.DefaultNetworkCredentialsDomain] = System.Net.CredentialCache.DefaultNetworkCredentials.Domain;
            dr[Constants.Properties.Identity.DefaultNetworkCredentialsPassword] = System.Net.CredentialCache.DefaultNetworkCredentials.Password;
            dr[Constants.Properties.Identity.DefaultNetworkCredentialsUsername] = System.Net.CredentialCache.DefaultNetworkCredentials.UserName;
            dr[Constants.Properties.Identity.CallingFQN] = CallingFQN;
            dr[Constants.Properties.Identity.UserCultureName] = System.Globalization.CultureInfo.CurrentCulture.Name;
            dr[Constants.Properties.Identity.UserCultureDisplayName] = System.Globalization.CultureInfo.CurrentCulture.DisplayName;
            dr[Constants.Properties.Identity.UserCultureDateTimeFormat] = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat;
            dr[Constants.Properties.Identity.UserCultureLCID] = System.Globalization.CultureInfo.CurrentCulture.LCID;
            dr[Constants.Properties.Identity.UserCultureNumberFormat] = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            results.Rows.Add(dr);
        }

        public void WhoAmI()
        {
            if (GetBoolProperty(Constants.Properties.Identity.UserWindowsImpersonation))
            {
                System.Security.Principal.WindowsIdentity.Impersonate(IntPtr.Zero);
            }

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            using (Connection k2Con = new Connection())
            {
                k2Con.Open(K2ClientConnectionSetup);

                DataRow dr = results.NewRow();
                dr[Constants.Properties.Identity.FQN] = k2Con.User.FQN;
                dr[Constants.Properties.Identity.IdentityDescription] = k2Con.User.Description;
                dr[Constants.Properties.Identity.IdentityDisplayName] = k2Con.User.DisplayName;
                dr[Constants.Properties.Identity.UserEmail] = k2Con.User.Email;
                dr[Constants.Properties.Identity.UserManager] = k2Con.User.Manager; 
                dr[Constants.Properties.Identity.UserName] = k2Con.User.Name;
                dr[Constants.Properties.Identity.UserUserLabel] = k2Con.User.UserLabel;
                dr[Constants.Properties.Identity.CallingFQN] = CallingFQN;
                results.Rows.Add(dr);
                k2Con.Close();
            }
        }

        public void ResolveIdentity(IdentityType iType)
        {
            var fqn = GetStringProperty(Constants.Properties.Identity.FQN, true);
            var resolveContainers = GetBoolProperty(Constants.Properties.Identity.ResolveContainers);
            var resolveMembers = GetBoolProperty(Constants.Properties.Identity.ResolveMembers);
            var serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            var results = ServiceBroker.ServicePackage.ResultTable;
            bool isResolved;
            var fqnName = new FQName(fqn);

            if (resolveContainers)
            {
                isResolved = K2NEServiceBroker.IdentityService.ResolveIdentity(fqnName, iType, IdentitySection.Containers);
            }
            if (resolveMembers)
            {
                isResolved = K2NEServiceBroker.IdentityService.ResolveIdentity(fqnName, iType, IdentitySection.Members);
            }
            isResolved = K2NEServiceBroker.IdentityService.ResolveIdentity(fqnName, iType,IdentityResolveOptions.Identity);

            
            var dRow = results.NewRow();
            dRow[Constants.Properties.Identity.IsResolved] = isResolved.ToString();
            results.Rows.Add(dRow);
        }
    }
}
