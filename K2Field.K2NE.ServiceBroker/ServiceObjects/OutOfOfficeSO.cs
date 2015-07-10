using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using K2Field.K2NE.ServiceBroker.Helpers;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using SourceCode.Workflow.Management;
using System.Data;

using SourceCode.Workflow.Management.OOF;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects
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

            so.Properties.Add(Helper.CreateProperty(Constants.Properties.OutOfOffice.UserFQN, SoType.Text, @"The users FQN with label, ex.'K2:DOMAIN\User' (no quotes)."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.OutOfOffice.DestinationUser, SoType.Text, "User to forward worktask items to"));
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

            Method addOutOfOffice = Helper.CreateMethod(Constants.Methods.OutOfOffice.AddOutOfOffice, "Get the office status of a user.", MethodType.Read);
            addOutOfOffice.InputProperties.Add(Constants.Properties.OutOfOffice.UserFQN);
            addOutOfOffice.InputProperties.Add(Constants.Properties.OutOfOffice.DestinationUser);
            addOutOfOffice.Validation.RequiredProperties.Add(Constants.Properties.OutOfOffice.UserFQN);
            addOutOfOffice.Validation.RequiredProperties.Add(Constants.Properties.OutOfOffice.DestinationUser);
            addOutOfOffice.ReturnProperties.Add(Constants.Properties.OutOfOffice.UserFQN);
            addOutOfOffice.ReturnProperties.Add(Constants.Properties.OutOfOffice.DestinationUser);
            addOutOfOffice.ReturnProperties.Add(Constants.Properties.OutOfOffice.CallSuccess);
            so.Methods.Add(addOutOfOffice);

            Method listSharedUsers = Helper.CreateMethod(Constants.Methods.OutOfOffice.ListSharedUsers, "Get the destiination users for OOF user", MethodType.List);
            listSharedUsers.InputProperties.Add(Constants.Properties.OutOfOffice.UserFQN);
            listSharedUsers.Validation.RequiredProperties.Add(Constants.Properties.OutOfOffice.UserFQN);
            listSharedUsers.ReturnProperties.Add(Constants.Properties.OutOfOffice.DestinationUser);
            so.Methods.Add(listSharedUsers);

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
                case Constants.Methods.OutOfOffice.AddOutOfOffice:
                    AddOutOfOffice();
                    break;
                case Constants.Methods.OutOfOffice.ListSharedUsers:
                    ListSharedUsers();
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

        // Example from page - http://help.k2.com/onlinehelp/k2blackpearl/DevRef/4.6.9/default.htm#How_to_set_a_users_Out_of_Office_Status.html%3FTocPath%3DRuntime%2520APIs%2520and%2520Services%7CWorkflow%7CWorkflow%2520Client%2520API%7CWorkflow%2520Client%2520API%2520Samples%7C_____10

        private void AddOutOfOffice()
        {
            string userFQN = base.GetStringProperty(Constants.Properties.OutOfOffice.UserFQN);
            string destinationUser = base.GetStringProperty(Constants.Properties.OutOfOffice.DestinationUser);

            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = new WorkflowManagementServer();

            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);

                WorklistShares wsColl = mngServer.GetCurrentSharingSettings(userFQN, ShareType.OOF);

                //  Throw error if multiple configurations (WorklistShare objects) detected, as this method cannot support that
                if (wsColl.Count > 1)
                {
                    throw new ApplicationException(Constants.ErrorMessages.MultipleOOFConfigurations);
                }

                //  If configuration exist already, add to it
                if (wsColl.Count == 1)
                {
                    
                    WorklistShare worklistshare = wsColl[0];
                    worklistshare.WorkTypes[0].Destinations.Add(new Destination(destinationUser, DestinationType.User));
                    bool result = mngServer.ShareWorkList(userFQN, worklistshare);

                    DataRow dr = results.NewRow();
                    dr[Constants.Properties.OutOfOffice.UserFQN] = userFQN;
                    dr[Constants.Properties.OutOfOffice.DestinationUser] = destinationUser;
                    dr[Constants.Properties.OutOfOffice.CallSuccess] = result;
                    results.Rows.Add(dr); ;

                }
                // New user, create configuration for OOF
                else
                {
                    // ALL Work that remains which does not form part of any "WorkTypeException" Filter 
                    WorklistCriteria worklistcriteria = new WorklistCriteria();
                    worklistcriteria.Platform = "ASP";

                    // Send ALL Work based on the above Filter to the following User 
                    Destinations worktypedestinations = new Destinations();
                    worktypedestinations.Add(new Destination(destinationUser, DestinationType.User));

                    // Link the filters and destinations to the Work 
                    WorkType worktype = new WorkType("MyWork", worklistcriteria, worktypedestinations);

                    WorklistShare worklistshare = new WorklistShare();
                    worklistshare.ShareType = ShareType.OOF;
                    worklistshare.WorkTypes.Add(worktype);

                    bool result = mngServer.ShareWorkList(userFQN, worklistshare);
                    mngServer.SetUserStatus(userFQN, UserStatuses.Available);

                    DataRow dr = results.NewRow();
                    dr[Constants.Properties.OutOfOffice.UserFQN] = userFQN;
                    dr[Constants.Properties.OutOfOffice.DestinationUser] = destinationUser;
                    dr[Constants.Properties.OutOfOffice.CallSuccess] = result;
                    results.Rows.Add(dr);
                }
                 
            }
        }
        
        private void ListSharedUsers()
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
                                
                WorklistShares wsColl = mngServer.GetCurrentSharingSettings(userFQN, ShareType.OOF);

                foreach (WorklistShare ws in wsColl)
                {
                    //throw new ApplicationException("collection count is: "+ wsColl.Count.ToString());
                    foreach (WorkType wt in ws.WorkTypes)
                    {
                        foreach (Destination dest in wt.Destinations)
                        {
                            DataRow dr = results.NewRow();
                            dr[Constants.Properties.OutOfOffice.DestinationUser] = dest.Name.ToString();
                            results.Rows.Add(dr);
                        }
                    }
                }
                
            }
        }
    }
}
