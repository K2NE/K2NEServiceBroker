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
                return Constants.ServiceFolders.ManagementAPI;
            }
        }

        public override List<ServiceObject> DescribeServiceObjects()
        {

            ServiceObject so = Helper.CreateServiceObject("OutOfOfficeManagement", "Allows for managing user Out Of Offcie status");

            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.OutOfOffice.UserFQN, SoType.Text, @"The users FQN with label, ex.'K2:DOMAIN\User' (no quotes)."));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.OutOfOffice.DestinationUser, SoType.Text, "User to forward worktask items to"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.OutOfOffice.UserStatus, SoType.Text, "Status of a user"));

            Method setOutOfOffice = Helper.CreateMethod(Constants.Methods.OutOfOffice.SetOutOfOffice, "Set the office status of a user to Out of Office", MethodType.Execute);
            setOutOfOffice.InputProperties.Add(Constants.SOProperties.OutOfOffice.UserFQN);
            setOutOfOffice.Validation.RequiredProperties.Add(Constants.SOProperties.OutOfOffice.UserFQN);
            so.Methods.Add(setOutOfOffice);

            Method setInOffice = Helper.CreateMethod(Constants.Methods.OutOfOffice.SetInOffice, "Set the office status of a users.", MethodType.Execute);
            setInOffice.InputProperties.Add(Constants.SOProperties.OutOfOffice.UserFQN);
            setInOffice.Validation.RequiredProperties.Add(Constants.SOProperties.OutOfOffice.UserFQN);
            so.Methods.Add(setInOffice);

            Method getUserStatus = Helper.CreateMethod(Constants.Methods.OutOfOffice.GetUserStatus, "Get the office status of a user.", MethodType.Read);
            getUserStatus.InputProperties.Add(Constants.SOProperties.OutOfOffice.UserFQN);
            getUserStatus.Validation.RequiredProperties.Add(Constants.SOProperties.OutOfOffice.UserFQN);
            getUserStatus.ReturnProperties.Add(Constants.SOProperties.OutOfOffice.UserStatus);
            getUserStatus.ReturnProperties.Add(Constants.SOProperties.OutOfOffice.UserFQN);
            so.Methods.Add(getUserStatus);

            Method addOutOfOffice = Helper.CreateMethod(Constants.Methods.OutOfOffice.AddOutOfOffice, "Get the office status of a user.", MethodType.Read);
            addOutOfOffice.InputProperties.Add(Constants.SOProperties.OutOfOffice.UserFQN);
            addOutOfOffice.InputProperties.Add(Constants.SOProperties.OutOfOffice.DestinationUser);
            addOutOfOffice.Validation.RequiredProperties.Add(Constants.SOProperties.OutOfOffice.UserFQN);
            addOutOfOffice.Validation.RequiredProperties.Add(Constants.SOProperties.OutOfOffice.DestinationUser);
            addOutOfOffice.ReturnProperties.Add(Constants.SOProperties.OutOfOffice.UserFQN);
            addOutOfOffice.ReturnProperties.Add(Constants.SOProperties.OutOfOffice.DestinationUser);
            so.Methods.Add(addOutOfOffice);

            Method listUserShares = Helper.CreateMethod(Constants.Methods.OutOfOffice.ListUserShares, "Get the destination/delegates for a specific user", MethodType.List);
            listUserShares.InputProperties.Add(Constants.SOProperties.OutOfOffice.UserFQN);
            listUserShares.Validation.RequiredProperties.Add(Constants.SOProperties.OutOfOffice.UserFQN);
            listUserShares.ReturnProperties.Add(Constants.SOProperties.OutOfOffice.UserFQN);
            listUserShares.ReturnProperties.Add(Constants.SOProperties.OutOfOffice.DestinationUser);
            so.Methods.Add(listUserShares);

            Method listShares = Helper.CreateMethod(Constants.Methods.OutOfOffice.ListShares, "Get the destination/delegates for all users", MethodType.List);
            listShares.ReturnProperties.Add(Constants.SOProperties.OutOfOffice.UserFQN);
            listShares.ReturnProperties.Add(Constants.SOProperties.OutOfOffice.DestinationUser);
            so.Methods.Add(listShares);

            Method removeAllShares = Helper.CreateMethod(Constants.Methods.OutOfOffice.RemoveAllShares, "Remove All Shares", MethodType.Execute);
            removeAllShares.InputProperties.Add(Constants.SOProperties.OutOfOffice.UserFQN);
            removeAllShares.Validation.RequiredProperties.Add(Constants.SOProperties.OutOfOffice.UserFQN);
            so.Methods.Add(removeAllShares);

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
                    SetStatus(UserStatuses.Available);
                    break;
                case Constants.Methods.OutOfOffice.SetOutOfOffice:
                    SetStatus(UserStatuses.OOF);
                    break;
                case Constants.Methods.OutOfOffice.AddOutOfOffice:
                    AddOutOfOffice();
                    break;
                case Constants.Methods.OutOfOffice.ListUserShares:
                    ListSharedUsers();
                    break;
                case Constants.Methods.OutOfOffice.ListShares:
                    ListUsers();
                    break;
                case Constants.Methods.OutOfOffice.RemoveAllShares:
                    RemoveAllShares();
                    break;
            }
        }



        private void GetUserStatus()
        {
            string userFQN = base.GetStringProperty(Constants.SOProperties.OutOfOffice.UserFQN);

            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = new WorkflowManagementServer();

            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);
                UserStatuses status = mngServer.GetUserStatus(userFQN);

                DataRow dr = results.NewRow();
                dr[Constants.SOProperties.OutOfOffice.UserFQN] = userFQN;
                dr[Constants.SOProperties.OutOfOffice.UserStatus] = status.ToString();

                results.Rows.Add(dr);
            }
        }

        private void SetStatus(UserStatuses status)
        {
            string userFQN = base.GetStringProperty(Constants.SOProperties.OutOfOffice.UserFQN);

            WorkflowManagementServer mngServer = new WorkflowManagementServer();

            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);
                // None for userstatus means the users is not configured, throw an exception
                if (UserStatuses.None == mngServer.GetUserStatus(userFQN))
                {
                    throw new ApplicationException(Constants.ErrorMessages.OutOfOfficeNotConfiguredForUser);
                }
                bool result = mngServer.SetUserStatus(userFQN, status);
                if (!result)
                {
                    throw new ApplicationException(Constants.ErrorMessages.FailedToSetOOF);
                }
            }
        }



        private void AddOutOfOffice()
        {
            string userFQN = base.GetStringProperty(Constants.SOProperties.OutOfOffice.UserFQN, true);
            string destinationUser = base.GetStringProperty(Constants.SOProperties.OutOfOffice.DestinationUser, true);

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
                    if (!result)
                    {
                        throw new ApplicationException(Constants.ErrorMessages.FailedToSetOOF);
                    }
                    DataRow dr = results.NewRow();
                    dr[Constants.SOProperties.OutOfOffice.UserFQN] = userFQN;
                    dr[Constants.SOProperties.OutOfOffice.DestinationUser] = destinationUser;

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
                    if (!result)
                    {
                        throw new ApplicationException(Constants.ErrorMessages.FailedToSetOOF);
                    }
                    result = mngServer.SetUserStatus(userFQN, UserStatuses.Available);
                    if (!result)
                    {
                        throw new ApplicationException(Constants.ErrorMessages.FailedToSetOOF);
                    }

                    DataRow dr = results.NewRow();
                    dr[Constants.SOProperties.OutOfOffice.UserFQN] = userFQN;
                    dr[Constants.SOProperties.OutOfOffice.DestinationUser] = destinationUser;
                    results.Rows.Add(dr);
                }

            }
        }

        private void ListUsers()
        {

            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;
            WorkflowManagementServer mngServer = new WorkflowManagementServer();

            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);
                SourceCode.Workflow.Management.OOF.Users users = mngServer.GetUsers(ShareType.OOF);


                foreach (SourceCode.Workflow.Management.OOF.User user in users)
                {
                    if (user.Status != UserStatuses.None)
                    {
                        WorklistShares wsColl = mngServer.GetCurrentSharingSettings(user.FQN, ShareType.OOF);
                        foreach (WorklistShare ws in wsColl)
                        {
                            foreach (WorkType wt in ws.WorkTypes)
                            {
                                foreach (Destination dest in wt.Destinations)
                                {
                                    DataRow dr = results.NewRow();
                                    dr[Constants.SOProperties.OutOfOffice.UserFQN] = user.FQN;
                                    dr[Constants.SOProperties.OutOfOffice.DestinationUser] = dest.Name;
                                    results.Rows.Add(dr);
                                }
                            }
                        }
                    }
                }

            }
        }

        private void ListSharedUsers()
        {
            string userFQN = base.GetStringProperty(Constants.SOProperties.OutOfOffice.UserFQN);

            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = new WorkflowManagementServer();

            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);

                // None for userstatus means the users is not configured, throw an exception
                if (mngServer.GetUserStatus(userFQN) == UserStatuses.None)
                {
                    throw new ApplicationException(Constants.ErrorMessages.OutOfOfficeNotConfiguredForUser);
                }
                WorklistShares wsColl = mngServer.GetCurrentSharingSettings(userFQN, ShareType.OOF);

                foreach (WorklistShare ws in wsColl)
                {
                    foreach (WorkType wt in ws.WorkTypes)
                    {
                        foreach (Destination dest in wt.Destinations)
                        {
                            DataRow dr = results.NewRow();
                            dr[Constants.SOProperties.OutOfOffice.UserFQN] = userFQN;
                            dr[Constants.SOProperties.OutOfOffice.DestinationUser] = dest.Name.ToString();
                            results.Rows.Add(dr);
                        }
                    }
                }

            }
        }


        private void RemoveAllShares()
        {
            string userFQN = base.GetStringProperty(Constants.SOProperties.OutOfOffice.UserFQN);

            WorkflowManagementServer mngServer = new WorkflowManagementServer();

            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);
                if (mngServer.GetUserStatus(userFQN) == UserStatuses.None)
                {
                    throw new ApplicationException(Constants.ErrorMessages.OutOfOfficeNotConfiguredForUser);
                }

                WorklistShares shares = mngServer.GetCurrentSharingSettings(userFQN, ShareType.OOF);

                if (shares == null || shares.Count == 0)
                {
                    mngServer.UnShareAll(userFQN);
                }

                mngServer.Connection.Close();
            }
            SetStatus(UserStatuses.Available);
        }
    }
}
