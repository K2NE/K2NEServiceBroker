using System;
using System.Collections.Generic;
using K2Field.K2NE.ServiceBroker.Helpers;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using System.Data;
using SourceCode.Workflow.Client;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects
{
    class OutOfOfficeClientSO : ServiceObjectBase
    {
        public OutOfOfficeClientSO(K2NEServiceBroker api) : base(api) { }


        public override string ServiceFolder
        {
            get
            {
                return Constants.ServiceFolders.ClientAPI;
            }
        }

        public override List<ServiceObject> DescribeServiceObjects()
        {

            ServiceObject so = Helper.CreateServiceObject("OutOfOfficeClient", "Allows for self-service configuring user Out Of Office status");

            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.OutOfOffice.DestinationUser, SoType.Text, "User to forward worktask items to"));
            so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.OutOfOffice.UserStatus, SoType.Text, "Status of a user"));

            Method setOutOfOffice = Helper.CreateMethod(Constants.Methods.OutOfOfficeClient.SetOutOfOffice, "Set the office status of a current user to Out of Office", MethodType.Execute);
            so.Methods.Add(setOutOfOffice);

            Method setInOffice = Helper.CreateMethod(Constants.Methods.OutOfOfficeClient.SetInOffice, "Set the office status of a a current user to Available", MethodType.Execute);
            so.Methods.Add(setInOffice);

            Method getUserStatus = Helper.CreateMethod(Constants.Methods.OutOfOfficeClient.GetUserStatus, "Get the office status of a current user.", MethodType.Read);

            getUserStatus.ReturnProperties.Add(Constants.SOProperties.OutOfOffice.UserStatus);
            so.Methods.Add(getUserStatus);

            Method addOutOfOffice = Helper.CreateMethod(Constants.Methods.OutOfOfficeClient.AddOutOfOffice, "Add user share (destination user) for a current user.", MethodType.Read);
            addOutOfOffice.InputProperties.Add(Constants.SOProperties.OutOfOffice.DestinationUser);
            addOutOfOffice.Validation.RequiredProperties.Add(Constants.SOProperties.OutOfOffice.DestinationUser);
            addOutOfOffice.ReturnProperties.Add(Constants.SOProperties.OutOfOffice.DestinationUser);
            so.Methods.Add(addOutOfOffice);

            Method removeOutOfOffice = Helper.CreateMethod(Constants.Methods.OutOfOfficeClient.RemoveOutOfOffice, "Remove user share (destination user) from OOF Shares for a current user.", MethodType.Read);
            removeOutOfOffice.InputProperties.Add(Constants.SOProperties.OutOfOffice.DestinationUser);
            removeOutOfOffice.Validation.RequiredProperties.Add(Constants.SOProperties.OutOfOffice.DestinationUser);
            removeOutOfOffice.ReturnProperties.Add(Constants.SOProperties.OutOfOffice.DestinationUser);
            so.Methods.Add(removeOutOfOffice);

            Method listUserShares = Helper.CreateMethod(Constants.Methods.OutOfOfficeClient.ListUserShares, "Get all the destination users for OOF shares of current user", MethodType.List);
            listUserShares.ReturnProperties.Add(Constants.SOProperties.OutOfOffice.DestinationUser);
            so.Methods.Add(listUserShares);

            Method removeAllShares = Helper.CreateMethod(Constants.Methods.OutOfOfficeClient.RemoveAllShares, "Remove All Shares", MethodType.Execute);
            so.Methods.Add(removeAllShares);

            return new List<ServiceObject>() { so };
        }

        public override void Execute()
        {
            switch (base.ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.OutOfOfficeClient.GetUserStatus:
                    GetUserStatus();
                    break;
                case Constants.Methods.OutOfOfficeClient.SetInOffice:
                    SetStatus(UserStatuses.Available);
                    break;
                case Constants.Methods.OutOfOfficeClient.SetOutOfOffice:
                    SetStatus(UserStatuses.OOF);
                    break;
                case Constants.Methods.OutOfOfficeClient.AddOutOfOffice:
                    AddOutOfOffice();
                    break;
                case Constants.Methods.OutOfOfficeClient.RemoveOutOfOffice:
                    RemoveOutOfOffice();
                    break;
                case Constants.Methods.OutOfOfficeClient.ListUserShares:
                    ListSharedUsers();
                    break;
                case Constants.Methods.OutOfOfficeClient.RemoveAllShares:
                    RemoveAllShares();
                    break;
            }
        }

        /// <summary>
        /// Get OOF status for current user from Client API and return it
        /// TODO: Remove User FQN parameter
        /// </summary>
        private void GetUserStatus()
        {

            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            using (Connection k2Con = new Connection())
            {
                k2Con.Open(base.K2ClientConnectionSetup);

                SourceCode.Workflow.Client.UserStatuses status = k2Con.GetUserStatus();
                DataRow dr = results.NewRow();

                dr[Constants.SOProperties.OutOfOffice.UserStatus] = status.ToString();

                results.Rows.Add(dr);

                k2Con.Close();
            }


        }

        /// <summary>
        /// Set OOF status for a user
        /// </summary>
        /// <param name="status">OOF Status (Available, OOF, None)</param>
        private void SetStatus(SourceCode.Workflow.Client.UserStatuses status)
        {
            using (Connection k2Con = new Connection())
            {
                k2Con.Open(base.K2ClientConnectionSetup);
                // None for userstatus means the users is not configured, throw an exception
                if (UserStatuses.None == k2Con.GetUserStatus() && UserStatuses.OOF == status)
                {
                    // exception should be thrown only in case that user tries to  set OOF, 
                    throw new ApplicationException(Constants.ErrorMessages.OutOfOfficeNotConfiguredForUser);
                }

                try
                {
                    k2Con.SetUserStatus(status);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(Constants.ErrorMessages.FailedToSetOOF, ex);
                }

                k2Con.Close();
            }
        }


        // Example from page - http://help.k2.com/onlinehelp/k2blackpearl/DevRef/4.6.9/default.htm#How_to_set_a_users_Out_of_Office_Status.html%3FTocPath%3DRuntime%2520APIs%2520and%2520Services%7CWorkflow%7CWorkflow%2520Client%2520API%7CWorkflow%2520Client%2520API%2520Samples%7C_____10
        /// <summary>
        /// Add OOF share for current user
        /// </summary>
        private void AddOutOfOffice()
        {

            string destinationUser = base.GetStringProperty(Constants.SOProperties.OutOfOffice.DestinationUser);

            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;


            using (Connection k2Con = new Connection())
            {
                k2Con.Open(base.K2ClientConnectionSetup);

                WorklistShares wsColl = k2Con.GetCurrentSharingSettings(ShareType.OOF);

                //  Throw error if multiple configurations (WorklistShare objects) detected, as this method cannot support that 
                if (wsColl.Count > 1)
                {
                    throw new ApplicationException(Constants.ErrorMessages.MultipleOOFConfigurations);
                }


                //  If configuration exist already, add to it 
                else if (wsColl.Count == 1)
                {


                    WorklistShare worklistshare = wsColl[0];

                    int capacity = worklistshare.WorkTypes[0].Destinations.Count;

                    string[] existingDestinations = new string[capacity];

                    for (int i = 0; i < capacity; i++)
                    {
                        existingDestinations[i] = worklistshare.WorkTypes[0].Destinations[i].Name.ToUpper().Trim();
                    }

                    if (Array.IndexOf(existingDestinations, destinationUser.ToUpper().Trim()) == -1)
                    {
                        worklistshare.WorkTypes[0].Destinations.Add(new Destination(destinationUser, DestinationType.User));
                    }

                    bool result = k2Con.ShareWorkList(worklistshare);

                    DataRow dr = results.NewRow();

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
                    WorkType worktype = new WorkType("MyWork_" + k2Con.User.FQN, worklistcriteria, worktypedestinations);

                    WorklistShare worklistshare = new WorklistShare();
                    worklistshare.ShareType = ShareType.OOF;
                    worklistshare.WorkTypes.Add(worktype);

                    bool result = k2Con.ShareWorkList(worklistshare);
                    k2Con.SetUserStatus(UserStatuses.Available);

                    DataRow dr = results.NewRow();

                    dr[Constants.SOProperties.OutOfOffice.DestinationUser] = destinationUser;

                    results.Rows.Add(dr);

                }

                k2Con.Close();
            }
        }

        /// <summary>
        /// Remove OOF share for current user
        /// </summary>
        [Obsolete("Method is not working, please use RemoveAllShares instead.")]
        private void RemoveOutOfOffice()
        {

            string destinationUser = base.GetStringProperty(Constants.SOProperties.OutOfOffice.DestinationUser);

            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;


            using (Connection k2Con = new Connection())
            {
                k2Con.Open(base.K2ClientConnectionSetup);

                WorklistShares wsColl = k2Con.GetCurrentSharingSettings(ShareType.OOF);

                //  Throw error if multiple configurations (WorklistShare objects) detected, as this method cannot support that 
                if (wsColl.Count != 1)
                {
                    throw new ApplicationException(Constants.ErrorMessages.MultipleOOFConfigurations);
                }


                //  If configuration exist already, remove user and re-share 
                else if (wsColl.Count == 1)
                {



                    WorklistShare worklistshare = wsColl[0];

                    int capacity = worklistshare.WorkTypes[0].Destinations.Count;

                    string[] existingDestinations = new string[capacity];

                    for (int i = 0; i < capacity; i++)
                    {
                        existingDestinations[i] = worklistshare.WorkTypes[0].Destinations[i].Name.ToUpper().Trim();
                    }

                    int destinationIndex = Array.IndexOf(existingDestinations, destinationUser.ToUpper().Trim());

                    if (destinationIndex != -1)
                    {
                        worklistshare.WorkTypes[0].Destinations.Remove(destinationIndex);
                    }

                    bool result = k2Con.ShareWorkList(worklistshare);

                    DataRow dr = results.NewRow();

                    dr[Constants.SOProperties.OutOfOffice.DestinationUser] = destinationUser;

                    results.Rows.Add(dr); ;


                }

                k2Con.Close();
            }
        }


        /// <summary>
        /// List all existing shares for a current user
        /// </summary>
        private void ListSharedUsers()
        {


            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            using (Connection k2Con = new Connection())
            {
                k2Con.Open(base.K2ClientConnectionSetup);

                // None for userstatus means the users is not configured, throw an exception
                if (UserStatuses.None == k2Con.GetUserStatus())
                    throw new ApplicationException(Constants.ErrorMessages.OutOfOfficeNotConfiguredForUser);

                WorklistShares wsColl = k2Con.GetCurrentSharingSettings(ShareType.OOF);

                foreach (WorklistShare ws in wsColl)
                {
                    //throw new ApplicationException("collection count is: "+ wsColl.Count.ToString());
                    foreach (WorkType wt in ws.WorkTypes)
                    {
                        foreach (Destination dest in wt.Destinations)
                        {
                            DataRow dr = results.NewRow();
                            dr[Constants.SOProperties.OutOfOffice.DestinationUser] = dest.Name.ToString();
                            results.Rows.Add(dr);
                        }
                    }
                }

                k2Con.Close();

            }
        }

        /// <summary>
        /// Remove all existing shares for a current user
        /// </summary>
        private void RemoveAllShares()
        {

            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            using (Connection k2Con = new Connection())
            {
                k2Con.Open(base.K2ClientConnectionSetup);

                // None for userstatus means the users is not configured, throw an exception
                if (UserStatuses.None == k2Con.GetUserStatus())
                    throw new ApplicationException(Constants.ErrorMessages.OutOfOfficeNotConfiguredForUser);

                WorklistShares wsColl = k2Con.GetCurrentSharingSettings(ShareType.OOF);
                if (wsColl != null)
                {
                    if (wsColl.Count > 0)
                    {
                        k2Con.UnShareAll();

                    }
                }

                k2Con.Close();

            }

            // Necessary to prevent unwanted errors when configuring status
            SetStatus(UserStatuses.Available);

        }
 
    }
}