using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.Workflow.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace K2Field.K2NE.ServiceBroker
{
    public class IdentitySO : ServiceObjectBase
    {

        public IdentitySO(K2NEServiceBroker broker)
            : base(broker)
        {
        }

        public override List<SourceCode.SmartObjects.Services.ServiceSDK.Objects.ServiceObject> DescribeServiceObjects()
        {
            ServiceObject so = Helper.CreateServiceObject("Identity", "Useful methods to determine the identities being used.");


            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.CurrentPrincipalAuthType, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The current principal's authentication type"));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.CurrentPrincipalName, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The current principal's authentication identity name."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.FQN, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The K2 FQN of the user."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.UserDescription, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The users description."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.UserDisplayName, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The users displayname."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.UserEmail, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The users email address."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.UserManager, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The users manager."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.UserName, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The users name."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.UserUserLabel, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The users label."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.CallingFQN, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The FQN determined by the service instance."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.WindowsIdentityAuthType, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "Windows Identity Authentication Type"));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.WindowsIdentityName, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "WIndows Identity Name"));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.ServiceBrokerAuthType, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "Service Broker Authentication Type"));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.ServiceBrokerUserName, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "Service Broker UserName"));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.ServiceBrokerPassword, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "Service Broker password"));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.UserWindowsImpersonation, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.YesNo, "Tells the service broker to use (windows) impersonation or not."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.DefaultNetworkCredentialsDomain, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "Default Network Credentials Domain."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.DefaultNetworkCredentialsPassword, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "Default Network Credentials Password."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.DefaultNetworkCredentialsUsername, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "Default Network Credentials Username."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.UserCultureDateTimeFormat, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "User Culture Date/Time format."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.UserCultureDisplayName, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "User Culture Display Name."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.UserCultureLCID, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "User Culture LCID."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.UserCultureName, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "User Culture Name."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.Identity.UserCultureNumberFormat, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "User Culture Number format."));



            Method mGetWorkflowClientIdentity = Helper.CreateMethod(Constants.Methods.Identity.ReadWorkflowClientIdentity, "Retrieve who you are for the K2 Client API", SourceCode.SmartObjects.Services.ServiceSDK.Types.MethodType.Read);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.Properties.Identity.FQN);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.Properties.Identity.UserDescription);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.Properties.Identity.UserDisplayName);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.Properties.Identity.UserEmail);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.Properties.Identity.UserManager);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.Properties.Identity.UserName);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.Properties.Identity.UserUserLabel);
            mGetWorkflowClientIdentity.ReturnProperties.Add(Constants.Properties.Identity.CallingFQN);

            mGetWorkflowClientIdentity.InputProperties.Add(Constants.Properties.Identity.UserWindowsImpersonation);
            so.Methods.Create(mGetWorkflowClientIdentity);

            Method mGetThreadIdentity = Helper.CreateMethod(Constants.Methods.Identity.ReadThreadIdentity, "Retrieve who you are for the API Identity", SourceCode.SmartObjects.Services.ServiceSDK.Types.MethodType.Read);
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
            }

        }


        public void ReadThreadIdentity()
        {
            if (base.GetBoolProperty(Constants.Properties.Identity.UserWindowsImpersonation))
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
            dr[Constants.Properties.Identity.ServiceBrokerUserName] = base.ServiceBroker.Service.ServiceConfiguration.ServiceAuthentication.UserName;
            dr[Constants.Properties.Identity.ServiceBrokerPassword] = base.ServiceBroker.Service.ServiceConfiguration.ServiceAuthentication.Password;
            dr[Constants.Properties.Identity.ServiceBrokerAuthType] = base.ServiceBroker.Service.ServiceConfiguration.ServiceAuthentication.AuthenticationMode.ToString();
            dr[Constants.Properties.Identity.DefaultNetworkCredentialsDomain] = System.Net.CredentialCache.DefaultNetworkCredentials.Domain;
            dr[Constants.Properties.Identity.DefaultNetworkCredentialsPassword] = System.Net.CredentialCache.DefaultNetworkCredentials.Password;
            dr[Constants.Properties.Identity.DefaultNetworkCredentialsUsername] = System.Net.CredentialCache.DefaultNetworkCredentials.UserName;
            dr[Constants.Properties.Identity.CallingFQN] = base.CallingFQN;
            dr[Constants.Properties.Identity.UserCultureName] = System.Globalization.CultureInfo.CurrentCulture.Name;
            dr[Constants.Properties.Identity.UserCultureDisplayName] = System.Globalization.CultureInfo.CurrentCulture.DisplayName;
            dr[Constants.Properties.Identity.UserCultureDateTimeFormat] = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat;
            dr[Constants.Properties.Identity.UserCultureLCID] = System.Globalization.CultureInfo.CurrentCulture.LCID;
            dr[Constants.Properties.Identity.UserCultureNumberFormat] = System.Globalization.CultureInfo.CurrentCulture.NumberFormat;
            results.Rows.Add(dr);
        }

        public void WhoAmI()
        {
            if (base.GetBoolProperty(Constants.Properties.Identity.UserWindowsImpersonation))
            {
                System.Security.Principal.WindowsIdentity.Impersonate(IntPtr.Zero);
            }

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;

            using (Connection k2Con = new Connection())
            {
                k2Con.Open(base.K2ClientConnectionSetup);

                DataRow dr = results.NewRow();
                dr[Constants.Properties.Identity.FQN] = k2Con.User.FQN;
                dr[Constants.Properties.Identity.UserDescription] = k2Con.User.Description;
                dr[Constants.Properties.Identity.UserDisplayName] = k2Con.User.DisplayName;
                dr[Constants.Properties.Identity.UserEmail] = k2Con.User.Email;
                dr[Constants.Properties.Identity.UserManager] = k2Con.User.Manager;
                dr[Constants.Properties.Identity.UserName] = k2Con.User.Name;
                dr[Constants.Properties.Identity.UserUserLabel] = k2Con.User.UserLabel;

                dr[Constants.Properties.Identity.CallingFQN] = base.CallingFQN;

                results.Rows.Add(dr);

                k2Con.Close();
            }

        }
    }
}
