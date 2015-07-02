using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using K2Field.K2NE.ServiceBroker.Helpers;
using SourceCode.Workflow.Management;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using System.Text.RegularExpressions;

namespace K2Field.K2NE.ServiceBroker
{
    public class WorkingHoursConfigurationSO : ServiceObjectBase
    {
        public WorkingHoursConfigurationSO(K2NEServiceBroker api) : base(api) { }

        public override string ServiceFolder
        {
            get
            {
                return "Management API";
            }
        }

        public override List<ServiceObject> DescribeServiceObjects()
        {
            ServiceObject so = Helper.CreateServiceObject("WorkingHoursConfiguration", "Service Object that exposes the Working Hours Configuration of the K2 server.");
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.DefaultZone, SoType.YesNo, "Shows if the zone is default"));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.Description, SoType.Memo, "Used for description of various objects."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.DurationHours, SoType.Number, "Hours for a timespan."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.DurationMinutes, SoType.Number, "Minutes for a timespan."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.DurationSeconds, SoType.Number, "Seconds for a timespan."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.GMTOffset, SoType.Number, "Time zone - from -13 to +13."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.IsNonWorkDate, SoType.YesNo, "Sets or shows if the day is working or not."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.TimeOfDayHours, SoType.Number, "Hours for a timespan for time of a day."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.TimeOfDayMinutes, SoType.Number, "Minutes for a timespan for time of a day."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.TimeOfDaySeconds, SoType.Number, "Seconds for a timespan for time of a day."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.WorkDate, SoType.DateTime, "Date of the exception date - workdate."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.WorkDay, SoType.Text, "Day of the week: Monday, Tuesday, Wednesday etc."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.ZoneGUID, SoType.Guid, "GUID of the zone."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.ZoneName, SoType.Text, "Name of the zone."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.NewZoneName, SoType.Text, "New name of the existing zone."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.FQN, SoType.Text, "FQN of users from a zone."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.UserName, SoType.Text, "UserName of users from a zone."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.ZoneExists, SoType.YesNo, "Does a zone exist."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.StartDateTime, SoType.DateTime, "Starting datetime."));
            so.Properties.Add(Helper.CreateProperty(Constants.Properties.WorkingHoursConfiguration.FinishDateTime, SoType.DateTime, "Finishing datetime."));

            Method createZone = Helper.CreateMethod(Constants.Methods.WorkingHoursConfiguration.CreateZone, "Creates a new Zone", MethodType.Create);
            createZone.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            createZone.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.Description);
            createZone.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.GMTOffset);
            createZone.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.DefaultZone);
            createZone.Validation.RequiredProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            createZone.Validation.RequiredProperties.Add(Constants.Properties.WorkingHoursConfiguration.GMTOffset);
            so.Methods.Add(createZone);

            Method saveZone = Helper.CreateMethod(Constants.Methods.WorkingHoursConfiguration.SaveZone, "Updates an existing zone", MethodType.Update);
            saveZone.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            saveZone.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.NewZoneName);
            saveZone.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.Description);
            saveZone.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.GMTOffset);
            saveZone.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.DefaultZone);
            saveZone.Validation.RequiredProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            so.Methods.Add(saveZone);

            Method deleteZone = Helper.CreateMethod(Constants.Methods.WorkingHoursConfiguration.DeleteZone, "Deletes an existing zone", MethodType.Delete);
            deleteZone.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            deleteZone.Validation.RequiredProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            so.Methods.Add(deleteZone);

            Method loadZone = Helper.CreateMethod(Constants.Methods.WorkingHoursConfiguration.LoadZone, "Loads an existing zone", MethodType.Read);
            loadZone.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            loadZone.Validation.RequiredProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            loadZone.ReturnProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            loadZone.ReturnProperties.Add(Constants.Properties.WorkingHoursConfiguration.Description);
            loadZone.ReturnProperties.Add(Constants.Properties.WorkingHoursConfiguration.GMTOffset);
            loadZone.ReturnProperties.Add(Constants.Properties.WorkingHoursConfiguration.DefaultZone);
            so.Methods.Add(loadZone);

            Method listZones = Helper.CreateMethod(Constants.Methods.WorkingHoursConfiguration.ListZones, "Lists all existing zones", MethodType.List);
            listZones.ReturnProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            so.Methods.Add(listZones);

            Method listZoneUsers = Helper.CreateMethod(Constants.Methods.WorkingHoursConfiguration.ListZoneUsers, "Lists all users for a certain zone", MethodType.List);
            listZoneUsers.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            listZoneUsers.Validation.RequiredProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            listZoneUsers.ReturnProperties.Add(Constants.Properties.WorkingHoursConfiguration.FQN);
            listZoneUsers.ReturnProperties.Add(Constants.Properties.WorkingHoursConfiguration.UserName);
            so.Methods.Add(listZoneUsers);

            Method getDefaultZone = Helper.CreateMethod(Constants.Methods.WorkingHoursConfiguration.GetDefaultZone, "Gets the name of the default zone", MethodType.Read);
            getDefaultZone.ReturnProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            so.Methods.Add(getDefaultZone);

            Method setDefaultZone = Helper.CreateMethod(Constants.Methods.WorkingHoursConfiguration.SetDefaultZone, "Sets the zone as a default one", MethodType.Execute);
            setDefaultZone.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            setDefaultZone.Validation.RequiredProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            so.Methods.Add(setDefaultZone);

            Method zoneExists = Helper.CreateMethod(Constants.Methods.WorkingHoursConfiguration.ZoneExists, "Returns true or false if a zone exists", MethodType.Execute);
            zoneExists.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            zoneExists.Validation.RequiredProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            zoneExists.ReturnProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneExists);
            so.Methods.Add(zoneExists);

            Method zoneCalculateEvent = Helper.CreateMethod(Constants.Methods.WorkingHoursConfiguration.ZoneCalculateEvent, "Adding working hours of a zone to datetime", MethodType.Execute);
            zoneCalculateEvent.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            zoneCalculateEvent.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.StartDateTime);
            zoneCalculateEvent.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.DurationHours);
            zoneCalculateEvent.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.DurationMinutes);
            zoneCalculateEvent.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.DurationSeconds);
            zoneCalculateEvent.Validation.RequiredProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            zoneCalculateEvent.Validation.RequiredProperties.Add(Constants.Properties.WorkingHoursConfiguration.StartDateTime);
            zoneCalculateEvent.ReturnProperties.Add(Constants.Properties.WorkingHoursConfiguration.FinishDateTime);
            so.Methods.Add(zoneCalculateEvent);

            Method userSetZone = Helper.CreateMethod(Constants.Methods.WorkingHoursConfiguration.UserSetZone, "Associate user with a certain zone", MethodType.Execute);
            userSetZone.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.FQN);
            userSetZone.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            userSetZone.Validation.RequiredProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            userSetZone.Validation.RequiredProperties.Add(Constants.Properties.WorkingHoursConfiguration.FQN);
            so.Methods.Add(userSetZone);

            Method userGetZone = Helper.CreateMethod(Constants.Methods.WorkingHoursConfiguration.UserGetZone, "Gets the associated zone of a user", MethodType.Read);
            userGetZone.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.FQN);
            userGetZone.ReturnProperties.Add(Constants.Properties.WorkingHoursConfiguration.ZoneName);
            so.Methods.Add(userGetZone);

            Method userDeleteZone = Helper.CreateMethod(Constants.Methods.WorkingHoursConfiguration.UserDeleteZone, "Deletes the associated zone of a user", MethodType.Execute);
            userDeleteZone.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.FQN);
            so.Methods.Add(userDeleteZone);

            Method userCalculateEvent = Helper.CreateMethod(Constants.Methods.WorkingHoursConfiguration.UserCalculateEvent, "Adding working hours of a user zone to datetime", MethodType.Execute);
            userCalculateEvent.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.FQN);
            userCalculateEvent.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.StartDateTime);
            userCalculateEvent.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.DurationHours);
            userCalculateEvent.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.DurationMinutes);
            userCalculateEvent.InputProperties.Add(Constants.Properties.WorkingHoursConfiguration.DurationSeconds);
            userCalculateEvent.Validation.RequiredProperties.Add(Constants.Properties.WorkingHoursConfiguration.FQN);
            userCalculateEvent.Validation.RequiredProperties.Add(Constants.Properties.WorkingHoursConfiguration.StartDateTime);
            userCalculateEvent.ReturnProperties.Add(Constants.Properties.WorkingHoursConfiguration.FinishDateTime);
            so.Methods.Add(userCalculateEvent);
            
            return new List<ServiceObject> { so };
        }

        public override void Execute()
        {
            switch (base.ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.WorkingHoursConfiguration.CreateZone:
                    CreateZone();
                    break;
                case Constants.Methods.WorkingHoursConfiguration.DeleteZone:
                    DeleteZone();
                    break;
                case Constants.Methods.WorkingHoursConfiguration.LoadZone:
                    LoadZone();
                    break;
                case Constants.Methods.WorkingHoursConfiguration.SaveZone:
                    SaveZone();
                    break;
                case Constants.Methods.WorkingHoursConfiguration.ListZones:
                    ListZones();
                    break;
                case Constants.Methods.WorkingHoursConfiguration.ListZoneUsers:
                    ListZoneUsers();
                    break;
                case Constants.Methods.WorkingHoursConfiguration.GetDefaultZone:
                    GetDefaultZone();
                    break;
                case Constants.Methods.WorkingHoursConfiguration.SetDefaultZone:
                    SetDefaultZone();
                    break;
                case Constants.Methods.WorkingHoursConfiguration.ZoneExists:
                    ZoneExists();
                    break;
                case Constants.Methods.WorkingHoursConfiguration.ZoneCalculateEvent:
                    ZoneCalculateEvent();
                    break;
                case Constants.Methods.WorkingHoursConfiguration.UserSetZone:
                    UserSetZone();
                    break;
                case Constants.Methods.WorkingHoursConfiguration.UserGetZone:
                    UserGetZone();
                    break;
                case Constants.Methods.WorkingHoursConfiguration.UserDeleteZone:
                    UserDeleteZone();
                    break;
                case Constants.Methods.WorkingHoursConfiguration.UserCalculateEvent:
                    UserCalculateEvent();
                    break;
            }

        }

        private void CreateZone()
        {
            string ZoneName = base.GetStringProperty(Constants.Properties.WorkingHoursConfiguration.ZoneName, true);
            string Description = base.GetStringProperty(Constants.Properties.WorkingHoursConfiguration.Description);
            int GmtOffSet = base.GetIntProperty(Constants.Properties.WorkingHoursConfiguration.GMTOffset, true);
            bool DefaultZone = base.GetBoolProperty(Constants.Properties.WorkingHoursConfiguration.DefaultZone);

            WorkflowManagementServer mngServer = new WorkflowManagementServer();
            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);

                if (!Helper.SpecialCharactersExist(ZoneName))
                {
                    throw new ApplicationException(Constants.ErrorMessages.SpecialCharactersAreNotAllowed);
                }
                else if (mngServer.ZoneExists(ZoneName))
                {
                    throw new ApplicationException(Constants.ErrorMessages.ZoneExists + ZoneName + ".");
                }
                else if (GmtOffSet > 13 || GmtOffSet < -13)
                {
                    throw new ApplicationException(Constants.ErrorMessages.GMTOffSetValidationFailed);
                }
                else
                {
                    AvailabilityZone aZone = new AvailabilityZone();
                    aZone.ZoneName = ZoneName;
                    aZone.ID = Guid.NewGuid();
                    aZone.ZoneDescription = Description;
                    aZone.ZoneGMTOffset = GmtOffSet;
                    aZone.DefaultZone = DefaultZone;

                    mngServer.ZoneCreateNew(aZone);
                }                    
            }
        }
        private void SaveZone()
        {
            string CurrentZoneName = base.GetStringProperty(Constants.Properties.WorkingHoursConfiguration.ZoneName, true);
            string NewZoneName = base.GetStringProperty(Constants.Properties.WorkingHoursConfiguration.NewZoneName);
            Property DescriptionProperty = ServiceBroker.Service.ServiceObjects[0].Properties[Constants.Properties.WorkingHoursConfiguration.Description];
            Property GMTOffsetProperty = ServiceBroker.Service.ServiceObjects[0].Properties[Constants.Properties.WorkingHoursConfiguration.GMTOffset];
            int GmtOffSet = base.GetIntProperty(Constants.Properties.WorkingHoursConfiguration.GMTOffset);
            bool DefaultZone = base.GetBoolProperty(Constants.Properties.WorkingHoursConfiguration.DefaultZone);

            WorkflowManagementServer mngServer = new WorkflowManagementServer();
            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);

                if (!String.IsNullOrEmpty(NewZoneName) && !Helper.SpecialCharactersExist(NewZoneName))
                {
                    throw new ApplicationException(Constants.ErrorMessages.SpecialCharactersAreNotAllowed);
                }
                else if (!String.IsNullOrEmpty(NewZoneName) && mngServer.ZoneExists(NewZoneName))
                {
                    throw new ApplicationException(Constants.ErrorMessages.ZoneExists + NewZoneName + ".");
                }
                else if (!mngServer.ZoneExists(CurrentZoneName))
                {
                    throw new ApplicationException(Constants.ErrorMessages.ZoneDoesNotExist + CurrentZoneName + ".");
                }
                else if (GmtOffSet > 13 || GmtOffSet < -13)
                {
                    throw new ApplicationException(Constants.ErrorMessages.GMTOffSetValidationFailed);
                }
                else
                {
                    AvailabilityZone aZone = mngServer.ZoneLoad(CurrentZoneName);
                    aZone.ZoneName = String.IsNullOrEmpty(NewZoneName) ? CurrentZoneName : NewZoneName;
                    if ((DescriptionProperty.Value != null) || (DescriptionProperty.IsClear))
                    {
                        aZone.ZoneDescription = DescriptionProperty.Value == null ? String.Empty : DescriptionProperty.Value as string;
                    }
                    if ((GmtOffSet != 0) || (GMTOffsetProperty.IsClear))
                    {
                        aZone.ZoneGMTOffset = GmtOffSet;
                    }
                    aZone.DefaultZone = DefaultZone; //even if the value is false, you cannot make a zone nonDefault without setting other zone to default
                    mngServer.ZoneSave(CurrentZoneName, aZone);
                }
            }

        }
        private void DeleteZone()
        {
            string ZoneName = base.GetStringProperty(Constants.Properties.WorkingHoursConfiguration.ZoneName, true);
            
            WorkflowManagementServer mngServer = new WorkflowManagementServer();
            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);

                if (!mngServer.ZoneExists(ZoneName))
                {
                    throw new ApplicationException(Constants.ErrorMessages.ZoneDoesNotExist + ZoneName + ".");
                }
                else
                {
                    mngServer.ZoneDelete(ZoneName);
                }
            }

        }
        private void LoadZone()
        {
            string ZoneName = base.GetStringProperty(Constants.Properties.WorkingHoursConfiguration.ZoneName, true);
            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = new WorkflowManagementServer();
            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);

                if (!mngServer.ZoneExists(ZoneName))
                {
                    throw new ApplicationException(Constants.ErrorMessages.ZoneDoesNotExist + ZoneName + ".");
                }
                else
                {
                    AvailabilityZone aZone = mngServer.ZoneLoad(ZoneName);
                    DataRow dRow = results.NewRow();
                    dRow[Constants.Properties.WorkingHoursConfiguration.ZoneName] = aZone.ZoneName;
                    dRow[Constants.Properties.WorkingHoursConfiguration.DefaultZone] = aZone.DefaultZone;
                    dRow[Constants.Properties.WorkingHoursConfiguration.GMTOffset] = aZone.ZoneGMTOffset;
                    dRow[Constants.Properties.WorkingHoursConfiguration.Description] = aZone.ZoneDescription;
                    results.Rows.Add(dRow);
                }
            }        
        }
        private void ListZones()
        {
            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;
            WorkflowManagementServer mngServer = new WorkflowManagementServer();
            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);
                List<string> zoneList = mngServer.ZoneListAll();
                foreach (string zone in zoneList)
                {
                    DataRow dRow = results.NewRow();
                    dRow[Constants.Properties.WorkingHoursConfiguration.ZoneName] = zone;
                    results.Rows.Add(dRow);
                }
            }        
        }
        private void ListZoneUsers()
        {
            string ZoneName = base.GetStringProperty(Constants.Properties.WorkingHoursConfiguration.ZoneName, true);
            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = new WorkflowManagementServer();
            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);
                if (!mngServer.ZoneExists(ZoneName))
                {
                    throw new ApplicationException(Constants.ErrorMessages.ZoneDoesNotExist + ZoneName + ".");
                }
                else
                {
                    List<string> userList = mngServer.ZoneListUsers(ZoneName);
                    foreach (string user in userList)
                    {
                        DataRow dRow = results.NewRow();
                        dRow[Constants.Properties.WorkingHoursConfiguration.FQN] = user;
                        dRow[Constants.Properties.WorkingHoursConfiguration.UserName] = Helper.DeleteLabel(user);
                        results.Rows.Add(dRow);
                    }
                }
            }        
        }
        private void GetDefaultZone()
        {
            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = new WorkflowManagementServer();
            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);
                string defaultZone = mngServer.ZoneGetDefault();

                DataRow dRow = results.NewRow();
                dRow[Constants.Properties.WorkingHoursConfiguration.ZoneName] = defaultZone;
                results.Rows.Add(dRow);                
            }     
        }
        private void SetDefaultZone()
        {
            string ZoneName = base.GetStringProperty(Constants.Properties.WorkingHoursConfiguration.ZoneName, true);
                        
            WorkflowManagementServer mngServer = new WorkflowManagementServer();
            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);
                if (!mngServer.ZoneExists(ZoneName))
                {
                    throw new ApplicationException(Constants.ErrorMessages.ZoneDoesNotExist + ZoneName + ".");
                }
                else
                {
                    mngServer.ZoneSetDefault(ZoneName);
                }
            }        
        }
        private void ZoneExists()
        {
            string ZoneName = base.GetStringProperty(Constants.Properties.WorkingHoursConfiguration.ZoneName, true);
            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = new WorkflowManagementServer();
            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);                
                DataRow dRow = results.NewRow();
                dRow[Constants.Properties.WorkingHoursConfiguration.ZoneExists] = mngServer.ZoneExists(ZoneName);
                results.Rows.Add(dRow);
            }        
        }
        private void ZoneCalculateEvent()
        {
            string ZoneName = base.GetStringProperty(Constants.Properties.WorkingHoursConfiguration.ZoneName, true);
            string Start = base.GetStringProperty(Constants.Properties.WorkingHoursConfiguration.StartDateTime, true);
            int DurationHours = base.GetIntProperty(Constants.Properties.WorkingHoursConfiguration.DurationHours);
            int DurationMinutes = base.GetIntProperty(Constants.Properties.WorkingHoursConfiguration.DurationMinutes);
            int DurationSeconds = base.GetIntProperty(Constants.Properties.WorkingHoursConfiguration.DurationSeconds);            

            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = new WorkflowManagementServer();
            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);
                if (!mngServer.ZoneExists(ZoneName))
                {
                    throw new ApplicationException(Constants.ErrorMessages.ZoneDoesNotExist + ZoneName + ".");
                }
                else
                {
                    TimeSpan Duration = new TimeSpan(DurationHours, DurationMinutes, DurationSeconds);

                    DateTime dt;
                    if (!DateTime.TryParse(Start, out dt))
                    {
                        throw new ApplicationException(Constants.ErrorMessages.DateNotValid);
                    }

                    AvailabilityZone zone = mngServer.ZoneLoad(ZoneName);
                    if (zone.AvailabilityHoursList == null || zone.AvailabilityHoursList.Count == 0)
                    {
                        throw new ApplicationException(Constants.ErrorMessages.WorkingHoursNotSet);
                    }

                    DataRow dRow = results.NewRow();
                    dRow[Constants.Properties.WorkingHoursConfiguration.FinishDateTime] = mngServer.ZoneCalculateEvent(ZoneName, dt, Duration);
                    results.Rows.Add(dRow);
                }
            }
        }
        private void UserSetZone()
        {
            string ZoneName = base.GetStringProperty(Constants.Properties.WorkingHoursConfiguration.ZoneName, true);
            string FQN = base.GetStringProperty(Constants.Properties.WorkingHoursConfiguration.FQN, true);
            
            WorkflowManagementServer mngServer = new WorkflowManagementServer();
            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);
                if (!mngServer.ZoneExists(ZoneName))
                {
                    throw new ApplicationException(Constants.ErrorMessages.ZoneDoesNotExist + ZoneName + ".");
                }
                else
                {
                    mngServer.UserSetZone(FQN, ZoneName);
                }
            }

        }
        private void UserGetZone()
        {
            string FQN = base.GetStringProperty(Constants.Properties.WorkingHoursConfiguration.FQN, true);
            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = new WorkflowManagementServer();
            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);
                DataRow dRow = results.NewRow();
                dRow[Constants.Properties.WorkingHoursConfiguration.ZoneName] = mngServer.UserGetZone(FQN);
                results.Rows.Add(dRow);                
            }
        }
        private void UserDeleteZone()
        {
            string FQN = base.GetStringProperty(Constants.Properties.WorkingHoursConfiguration.FQN, true);
            
            WorkflowManagementServer mngServer = new WorkflowManagementServer();
            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);
                mngServer.UserDeleteZone(FQN);                
            }
        }

        private void UserCalculateEvent()
        {
            string FQN = base.GetStringProperty(Constants.Properties.WorkingHoursConfiguration.FQN, true);
            string Start = base.GetStringProperty(Constants.Properties.WorkingHoursConfiguration.StartDateTime, true);
            int DurationHours = base.GetIntProperty(Constants.Properties.WorkingHoursConfiguration.DurationHours);
            int DurationMinutes = base.GetIntProperty(Constants.Properties.WorkingHoursConfiguration.DurationMinutes);
            int DurationSeconds = base.GetIntProperty(Constants.Properties.WorkingHoursConfiguration.DurationSeconds);

            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            WorkflowManagementServer mngServer = new WorkflowManagementServer();
            using (mngServer.CreateConnection())
            {
                mngServer.Open(BaseAPIConnectionString);

                TimeSpan Duration = new TimeSpan(DurationHours, DurationMinutes, DurationSeconds);

                DateTime dt;
                if (!DateTime.TryParse(Start, out dt))
                {
                    throw new ApplicationException(Constants.ErrorMessages.DateNotValid);
                }

                AvailabilityZone zone = mngServer.ZoneLoad(mngServer.UserGetZone(FQN));
                if (zone.AvailabilityHoursList == null || zone.AvailabilityHoursList.Count == 0)
                {
                    throw new ApplicationException(Constants.ErrorMessages.WorkingHoursNotSet);
                }

                DataRow dRow = results.NewRow();
                dRow[Constants.Properties.WorkingHoursConfiguration.FinishDateTime] = mngServer.UserCalculateEvent(FQN, dt, Duration);
                results.Rows.Add(dRow);                
            }
        }
    }
}
