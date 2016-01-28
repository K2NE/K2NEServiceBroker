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


            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.CurrentPrincipalAuthType, SoType.Text, "The current principal's authentication type"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.CurrentPrincipalName, SoType.Text, "The current principal's authentication identity name."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.FQN, SoType.Text, "The K2 FQN of the user."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.ResolveContainers, SoType.YesNo, "If Identity containers should be also resolved."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.ResolveMembers, SoType.YesNo, "If Identity members should be also resolved."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.IdentityDescription, SoType.Text, "The users description."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.IdentityDisplayName, SoType.Text, "The users displayname."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.UserEmail, SoType.Text, "The users email address."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.UserManager, SoType.Text, "The users manager."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.UserName, SoType.Text, "The users name."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.UserUserLabel, SoType.Text, "The users label."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.CallingFQN, SoType.Text, "The FQN determined by the service instance."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.WindowsIdentityAuthType, SoType.Text, "Windows Identity Authentication Type"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.WindowsIdentityName, SoType.Text, "WIndows Identity Name"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.ServiceBrokerAuthType, SoType.Text, "Service Broker Authentication Type"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.ServiceBrokerUserName, SoType.Text, "Service Broker UserName"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.ServiceBrokerPassword, SoType.Text, "Service Broker password"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.UserWindowsImpersonation, SoType.YesNo, "Tells the service broker to use (windows) impersonation or not."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.DefaultNetworkCredentialsDomain, SoType.Text, "Default Network Credentials Domain."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.DefaultNetworkCredentialsPassword, SoType.Text, "Default Network Credentials Password."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.DefaultNetworkCredentialsUsername, SoType.Text, "Default Network Credentials Username."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.UserCultureDateTimeFormat, SoType.Text, "User Culture Date/Time format."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.UserCultureDisplayName, SoType.Text, "User Culture Display Name."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.UserCultureLCID, SoType.Text, "User Culture LCID."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.UserCultureName, SoType.Text, "User Culture Name."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.UserCultureNumberFormat, SoType.Text, "User Culture Number format."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Identity.K2ImpersonateUser, SoType.Text, "User to impersonate with K2 API."));



            Method mGetWorkflowClientIdentity = Helper.CreateMethod(Constants.Methods.Identity.ReadWorkflowClientIdentity, "Retrieve who you are for the K2 Client API", MethodType.Read);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.FQN);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.IdentityDescription);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.IdentityDisplayName);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.UserEmail);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.UserManager);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.UserName);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.UserUserLabel);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.CallingFQN);

            mGetWorkflowClientIdentity.InputProperties.Add(Constants.SOProperties.Identity.UserWindowsImpersonation);
            mGetWorkflowClientIdentity.InputProperties.Add(Constants.SOProperties.Identity.K2ImpersonateUser);
            so.Methods.Add(mGetWorkflowClientIdentity);

            Method mGetThreadIdentity = Helper.CreateMethod(Constants.Methods.Identity.ReadThreadIdentity, "Retrieve who you are for the API Identity", MethodType.Read);
            mGetThreadIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.CallingFQN);
            mGetThreadIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.CurrentPrincipalAuthType);
            mGetThreadIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.CurrentPrincipalName);
            mGetThreadIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.WindowsIdentityAuthType);
            mGetThreadIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.WindowsIdentityName);
            mGetThreadIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.ServiceBrokerAuthType);
            mGetThreadIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.ServiceBrokerUserName);
            mGetThreadIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.ServiceBrokerPassword);
            mGetThreadIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.DefaultNetworkCredentialsDomain);
            mGetThreadIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.DefaultNetworkCredentialsPassword);
            mGetThreadIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.DefaultNetworkCredentialsUsername);
            mGetThreadIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.UserCultureDateTimeFormat);
            mGetThreadIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.UserCultureDisplayName);
            mGetThreadIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.UserCultureLCID);
            mGetThreadIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.UserCultureName);
            mGetThreadIdentity.ReturnProperties.Add(Constants.SOProperties.Identity.UserCultureNumberFormat);
            mGetThreadIdentity.InputProperties.Add(Constants.SOProperties.Identity.UserWindowsImpersonation);
            so.Methods.Add(mGetThreadIdentity);

            Method mResolveUser = Helper.CreateMethod(Constants.Methods.Identity.ResolveUserIdentity, "Resolve User Identity", MethodType.Execute);
            mResolveUser.InputProperties.Add(Constants.SOProperties.Identity.FQN);
            mResolveUser.InputProperties.Add(Constants.SOProperties.Identity.ResolveContainers);
            mResolveUser.Validation.RequiredProperties.Add(Constants.SOProperties.Identity.FQN);
            so.Methods.Add(mResolveUser);

            Method mResolveGroup = Helper.CreateMethod(Constants.Methods.Identity.ResolveGroupIdentity, "Resolve Group Identity", MethodType.Execute);
            mResolveGroup.InputProperties.Add(Constants.SOProperties.Identity.FQN);
            mResolveGroup.InputProperties.Add(Constants.SOProperties.Identity.ResolveContainers);
            mResolveGroup.InputProperties.Add(Constants.SOProperties.Identity.ResolveMembers);
            mResolveGroup.Validation.RequiredProperties.Add(Constants.SOProperties.Identity.FQN);
            so.Methods.Add(mResolveGroup);

            Method mResolveRole = Helper.CreateMethod(Constants.Methods.Identity.ResolveRoleIdentity, "Resolve Role Identity", MethodType.Execute);
            mResolveRole.InputProperties.Add(Constants.SOProperties.Identity.FQN);
            mResolveRole.InputProperties.Add(Constants.SOProperties.Identity.ResolveContainers);
            mResolveRole.InputProperties.Add(Constants.SOProperties.Identity.ResolveMembers);
            mResolveRole.Validation.RequiredProperties.Add(Constants.SOProperties.Identity.FQN);
            so.Methods.Add(mResolveRole);

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
            if (GetBoolProperty(Constants.SOProperties.Identity.UserWindowsImpersonation))
            {
                System.Security.Principal.WindowsIdentity.Impersonate(IntPtr.Zero);
            }
            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            DataRow dr = results.NewRow();
            dr[Constants.SOProperties.Identity.WindowsIdentityName] = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            dr[Constants.SOProperties.Identity.WindowsIdentityAuthType] = System.Security.Principal.WindowsIdentity.GetCurrent().AuthenticationType;
            dr[Constants.SOProperties.Identity.CurrentPrincipalName] = System.Threading.Thread.CurrentPrincipal.Identity.Name;
            dr[Constants.SOProperties.Identity.CurrentPrincipalAuthType] = System.Threading.Thread.CurrentPrincipal.Identity.AuthenticationType;
            dr[Constants.SOProperties.Identity.ServiceBrokerUserName] = ServiceBroker.Service.ServiceConfiguration.ServiceAuthentication.UserName;
            dr[Constants.SOProperties.Identity.ServiceBrokerPassword] = ServiceBroker.Service.ServiceConfiguration.ServiceAuthentication.Password;
            dr[Constants.SOProperties.Identity.ServiceBrokerAuthType] = ServiceBroker.Service.ServiceConfiguration.ServiceAuthentication.AuthenticationMode.ToString();
            dr[Constants.SOProperties.Identity.DefaultNetworkCredentialsDomain] = System.Net.CredentialCache.DefaultNetworkCredentials.Domain;
            dr[Constants.SOProperties.Identity.DefaultNetworkCredentialsPassword] = System.Net.CredentialCache.DefaultNetworkCredentials.Password;
            dr[Constants.SOProperties.Identity.DefaultNetworkCredentialsUsername] = System.Net.CredentialCache.DefaultNetworkCredentials.UserName;
            dr[Constants.SOProperties.Identity.CallingFQN] = CallingFQN;
            dr[Constants.SOProperties.Identity.UserCultureName] = System.Globalization.CultureInfo.CurrentCulture.Name;
            dr[Constants.SOProperties.Identity.UserCultureDisplayName] = System.Globalization.CultureInfo.CurrentCulture.DisplayName;
            dr[Constants.SOProperties.Identity.UserCultureDateTimeFormat] = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat;
            dr[Constants.SOProperties.Identity.UserCultureLCID] = System.Globalization.CultureInfo.CurrentCulture.LCID;
            dr[Constants.SOProperties.Identity.UserCultureNumberFormat] = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            results.Rows.Add(dr);
        }

        public void WhoAmI()
        {
            if (GetBoolProperty(Constants.SOProperties.Identity.UserWindowsImpersonation))
            {
                System.Security.Principal.WindowsIdentity.Impersonate(IntPtr.Zero);
            }

            string k2imp = GetStringProperty(Constants.SOProperties.Identity.K2ImpersonateUser, false);

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            using (Connection k2Con = new Connection())
            {
                k2Con.Open(K2ClientConnectionSetup);
                if (!string.IsNullOrEmpty(k2imp))
                {
                    k2Con.ImpersonateUser(k2imp);
                }

                DataRow dr = results.NewRow();
                dr[Constants.SOProperties.Identity.FQN] = k2Con.User.FQN;
                dr[Constants.SOProperties.Identity.IdentityDescription] = k2Con.User.Description;
                dr[Constants.SOProperties.Identity.IdentityDisplayName] = k2Con.User.DisplayName;
                dr[Constants.SOProperties.Identity.UserEmail] = k2Con.User.Email;
                dr[Constants.SOProperties.Identity.UserManager] = k2Con.User.Manager;
                dr[Constants.SOProperties.Identity.UserName] = k2Con.User.Name;
                dr[Constants.SOProperties.Identity.UserUserLabel] = k2Con.User.UserLabel;
                dr[Constants.SOProperties.Identity.CallingFQN] = CallingFQN;
                results.Rows.Add(dr);
                k2Con.Close();
            }
        }


        public void ResolveIdentity(IdentityType iType)
        {
            string fqn = GetStringProperty(Constants.SOProperties.Identity.FQN, true);
            bool resolveContainers = GetBoolProperty(Constants.SOProperties.Identity.ResolveContainers);
            bool resolveMembers = GetBoolProperty(Constants.SOProperties.Identity.ResolveMembers);

            var fqnName = new FQName(fqn);

            base.ServiceBroker.IdentityService.ResolveIdentity(fqnName, iType, IdentityResolveOptions.Identity);

            if (resolveMembers)
            {
                base.ServiceBroker.IdentityService.ResolveIdentity(fqnName, iType, IdentitySection.Members);
            }

            if (resolveContainers)
            {
                base.ServiceBroker.IdentityService.ResolveIdentity(fqnName, iType, IdentitySection.Containers);
            }


        }
    }
}
