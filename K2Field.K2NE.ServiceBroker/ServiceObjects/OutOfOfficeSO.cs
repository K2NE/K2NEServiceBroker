using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using SourceCode.Workflow.Management;
using System.Data;

using SourceCode.Workflow.Management.OOF;

namespace K2Field.K2NE.ServiceBroker
{
    class OutOfOfficeSO : ServiceObjectBase
    {
        public OutOfOfficeSO(K2NEServiceBroker api) : base(api) { }

        
        public override string ServiceFolder
        {
            get
            {
                return "Management API";
            }
        }
        
        public override List<ServiceObject> DescribeServiceObjects()
        {

            ServiceObject so = Helper.CreateServiceObject("OutOfOfficeManagement", "Allows for managing user Out Of Offcie status");

            so.Properties.Add(Helper.CreateProperty(Constants.Properties.OutOfOffice.UserFQN, SoType.Text, "The users FQN."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.OutOfOffice.UserStatus, SoType.Text, "Status of a user"));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.OutOfOffice.CallSuccess, SoType.YesNo, "Success of method call"));

            Method setOutOfOffice = Helper.CreateMethod(Constants.Methods.OutOfOffice.SetOutOfOffice, "Set the office status of a user to Out of Office", MethodType.Execute);
            setOutOfOffice.InputProperties.Add(Constants.Properties.OutOfOffice.UserFQN);
            setOutOfOffice.Validation.RequiredProperties.Add(Constants.Properties.OutOfOffice.UserFQN);
            setOutOfOffice.ReturnProperties.Add(Constants.Properties.OutOfOffice.UserFQN);
            setOutOfOffice.ReturnProperties.Add(Constants.Properties.OutOfOffice.CallSuccess);
            so.Methods.Add(setOutOfOffice);

            Method setInOffice = Helper.CreateMethod(Constants.Methods.OutOfOffice.SetInOffice, "Set the office status of a users.", MethodType.Execute);
            setInOffice.InputProperties.Add(Constants.Properties.OutOfOffice.UserFQN);
            setInOffice.Validation.RequiredProperties.Add(Constants.Properties.OutOfOffice.UserFQN);
            setInOffice.ReturnProperties.Add(Constants.Properties.OutOfOffice.UserFQN);
            setInOffice.ReturnProperties.Add(Constants.Properties.OutOfOffice.CallSuccess);
            so.Methods.Add(setInOffice);

            Method getUserStatus = Helper.CreateMethod(Constants.Methods.OutOfOffice.GetUserStatus, "Get the office status of a user.", MethodType.Read);
            getUserStatus.InputProperties.Add(Constants.Properties.OutOfOffice.UserFQN);
            getUserStatus.Validation.RequiredProperties.Add(Constants.Properties.OutOfOffice.UserFQN);
            getUserStatus.ReturnProperties.Add(Constants.Properties.OutOfOffice.UserStatus);
            getUserStatus.ReturnProperties.Add(Constants.Properties.OutOfOffice.UserFQN);
            so.Methods.Add(getUserStatus);

            return new List<ServiceObject>() { so };
        }

        public override void Execute()
        {
            switch (base.ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.OutOfOffice.GetUserStatus:
                    GetUserStatus();
                    break;

                case Constants.Methods.OutOfOffice.SetInOffice:
                    SetInOffice();
                    break;
                case Constants.Methods.OutOfOffice.SetOutOfOffice:
                    SetOutOfOffice();
                    break;
            }
        }

        private void GetUserStatus()
        {
            string userFQN = base.GetStringProperty(Constants.Properties.OutOfOffice.UserFQN);

            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = new WorkflowManagementServer();

            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);  
                UserStatuses status = mngServer.GetUserStatus(userFQN);

                DataRow dr = results.NewRow();
                dr[Constants.Properties.OutOfOffice.UserFQN] = userFQN;
                dr[Constants.Properties.OutOfOffice.UserStatus] = status.ToString();

                results.Rows.Add(dr);
            }
        }

        private void SetInOffice()
        {
            string userFQN = base.GetStringProperty(Constants.Properties.OutOfOffice.UserFQN);

            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = new WorkflowManagementServer();

            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);
                // None for userstatus means the users is not configured, throw an exception
                if (UserStatuses.None == mngServer.GetUserStatus(userFQN))
                    throw new ApplicationException(Constants.ErrorMessages.OutOfOfficeNotConfiguredForUser);
                
                bool result = mngServer.SetUserStatus(userFQN, UserStatuses.Available);


                DataRow dr = results.NewRow();
                dr[Constants.Properties.OutOfOffice.UserFQN] = userFQN;
                dr[Constants.Properties.OutOfOffice.CallSuccess] = result;
                results.Rows.Add(dr);
            }
        }

      
        private void SetOutOfOffice()
        {
            string userFQN = base.GetStringProperty(Constants.Properties.OutOfOffice.UserFQN);

            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = new WorkflowManagementServer();

            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);

                // None for userstatus means the users is not configured, throw an exception
                if (UserStatuses.None == mngServer.GetUserStatus(userFQN))
                    throw new ApplicationException(Constants.ErrorMessages.OutOfOfficeNotConfiguredForUser);

                bool result = mngServer.SetUserStatus(userFQN, UserStatuses.OOF);
                DataRow dr = results.NewRow();
                dr[Constants.Properties.OutOfOffice.UserFQN] = userFQN;
                dr[Constants.Properties.OutOfOffice.CallSuccess] = result;
                results.Rows.Add(dr);
            }
        }
    }
}
