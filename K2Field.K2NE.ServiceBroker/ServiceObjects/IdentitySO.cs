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


            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.CurrentPrincipalAuthType, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The current principal's authentication type"));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.CurrentPrincipalName, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The current principal's authentication identity name."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.FQN, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The K2 FQN of the user."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.UserDescription, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The users description."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.UserDisplayName, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The users displayname."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.UserEmail, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The users email address."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.UserManager, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The users manager."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.UserName, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The users name."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.UserUserLabel, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The users label."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.CallingFQN, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "The FQN determined by the service instance."));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.WindowsIdentityAuthType, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "Windows Identity Authentication Type"));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.WindowsIdentityName, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "WIndows Identity Name"));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.ServiceBrokerAuthType, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "Service Broker Authentication Type"));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.ServiceBrokerUserName, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.Text, "Service Broker UserName"));
            so.Properties.Create(Helper.CreateProperty(Constants.Properties.Identity.UserWindowsImpersonation, SourceCode.SmartObjects.Services.ServiceSDK.Types.SoType.YesNo, "Tells the service broker to use (windows) impersonation or not."));


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
            dr[Constants.Properties.Identity.ServiceBrokerAuthType] = base.ServiceBroker.Service.ServiceConfiguration.ServiceAuthentication.AuthenticationMode.ToString();
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
