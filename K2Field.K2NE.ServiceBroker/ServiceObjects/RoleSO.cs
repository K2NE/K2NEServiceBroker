﻿using K2Field.K2NE.ServiceBroker.Helpers;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceCode.Security.UserRoleManager.Management;
using System.Data;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects
{
    public class RoleManagementSO : ServiceObjectBase
    {
        public RoleManagementSO(K2NEServiceBroker broker) : base(broker) { }

        public override string ServiceFolder
        {
            get
            {
                return "Management API";
            }
        }


        public override List<SourceCode.SmartObjects.Services.ServiceSDK.Objects.ServiceObject> DescribeServiceObjects()
        {
            ServiceObject soRoleItem = Helper.CreateServiceObject("RoleManagement", "K2 Role management (add/remove/list K2 roles)");


            soRoleItem.Properties.Add(Helper.CreateProperty(Constants.Properties.Role.RoleName, SoType.Text, "The name of the role to manage."));
            soRoleItem.Properties.Add(Helper.CreateProperty(Constants.Properties.Role.RoleItemType, SoType.Text, "The type of role item (Group, User, SmartObject)."));
            soRoleItem.Properties.Add(Helper.CreateProperty(Constants.Properties.Role.RoleExclude, SoType.YesNo, "Excluded role item."));
            soRoleItem.Properties.Add(Helper.CreateProperty(Constants.Properties.Role.RoleItem, SoType.Text, "The FQN name of the role item."));
            soRoleItem.Properties.Add(Helper.CreateProperty(Constants.Properties.Role.RoleDescription, SoType.Text, "A short description of the role."));
            soRoleItem.Properties.Add(Helper.CreateProperty(Constants.Properties.Role.RoleItem, SoType.Text, "The FQN name of the role item."));
            soRoleItem.Properties.Add(Helper.CreateProperty(Constants.Properties.Role.RoleDynamic, SoType.YesNo, "Is a role dynamic?"));

            Method addRoleItem = Helper.CreateMethod(Constants.Methods.Role.AddRoleItem, "Add a RoleItem to a role" ,MethodType.Create);
            addRoleItem.InputProperties.Add(Constants.Properties.Role.RoleName);
            addRoleItem.InputProperties.Add(Constants.Properties.Role.RoleItem);
            addRoleItem.InputProperties.Add(Constants.Properties.Role.RoleExclude);
            addRoleItem.InputProperties.Add(Constants.Properties.Role.RoleItemType);
            addRoleItem.Validation.RequiredProperties.Add(Constants.Properties.Role.RoleName);
            addRoleItem.Validation.RequiredProperties.Add(Constants.Properties.Role.RoleItem);
            addRoleItem.Validation.RequiredProperties.Add(Constants.Properties.Role.RoleItemType);
            soRoleItem.Methods.Add(addRoleItem);


            Method deleteRoleItem = Helper.CreateMethod(Constants.Methods.Role.RemoveRoleItem, "Remove a RoleItem from a role", MethodType.Delete);
            deleteRoleItem.InputProperties.Add(Constants.Properties.Role.RoleName);
            deleteRoleItem.InputProperties.Add(Constants.Properties.Role.RoleItem);
            deleteRoleItem.Validation.RequiredProperties.Add(Constants.Properties.Role.RoleName);
            deleteRoleItem.Validation.RequiredProperties.Add(Constants.Properties.Role.RoleItem);
            soRoleItem.Methods.Add(deleteRoleItem);

            Method listRoleItems = Helper.CreateMethod(Constants.Methods.Role.ListRoleItem, "List the RoleItems in a role", MethodType.List);
            listRoleItems.InputProperties.Add(Constants.Properties.Role.RoleName);
            listRoleItems.ReturnProperties.Add(Constants.Properties.Role.RoleItem);
            listRoleItems.ReturnProperties.Add(Constants.Properties.Role.RoleExclude);
            listRoleItems.ReturnProperties.Add(Constants.Properties.Role.RoleItemType);
            soRoleItem.Methods.Add(listRoleItems);


            Method addRole = Helper.CreateMethod(Constants.Methods.Role.AddRole, "Add K2 Role to K2 system", MethodType.Create);
            addRole.InputProperties.Add(Constants.Properties.Role.RoleName);
            addRole.InputProperties.Add(Constants.Properties.Role.RoleDescription);
            addRole.InputProperties.Add(Constants.Properties.Role.RoleDynamic);
            addRole.InputProperties.Add(Constants.Properties.Role.RoleItemType);
            addRole.InputProperties.Add(Constants.Properties.Role.RoleItem);
            addRole.Validation.RequiredProperties.Add(Constants.Properties.Role.RoleName);
            addRole.Validation.RequiredProperties.Add(Constants.Properties.Role.RoleItem);
            addRole.Validation.RequiredProperties.Add(Constants.Properties.Role.RoleItemType);
            soRoleItem.Methods.Add(addRole);

            Method removeRole = Helper.CreateMethod(Constants.Methods.Role.RemoveRole, "Remove K2 Role to K2 system", MethodType.Delete);
            removeRole.InputProperties.Add(Constants.Properties.Role.RoleName);
            removeRole.Validation.RequiredProperties.Add(Constants.Properties.Role.RoleName);
            soRoleItem.Methods.Add(removeRole);

            Method listRoles = Helper.CreateMethod(Constants.Methods.Role.ListRoles, "list all K2 Roles", MethodType.List);
            listRoles.ReturnProperties.Add(Constants.Properties.Role.RoleName);
            listRoles.ReturnProperties.Add(Constants.Properties.Role.RoleDescription);
            listRoles.ReturnProperties.Add(Constants.Properties.Role.RoleDynamic);
            soRoleItem.Methods.Add(listRoles);

            return new List<ServiceObject>() {soRoleItem};


        }

        public override void Execute()
        {

            switch (base.ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
            
                case Constants.Methods.Role.ListRoleItem:
                    ListRoleItems();
                    break;
                case Constants.Methods.Role.AddRoleItem:
                    AddRoleItem();
                    break;
                case Constants.Methods.Role.RemoveRoleItem:
                    DeleteRoleItem();
                    break;
                case Constants.Methods.Role.ListRoles:
                    ListRoles();
                    break;
                case Constants.Methods.Role.AddRole:
                    AddRole();
                    break;
                case Constants.Methods.Role.RemoveRole:
                    DeleteRole();
                    break;
            
            }
        }



        private void DeleteRoleItem()
        {
            ServiceObject serviceObject = this.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();

            string roleName = base.GetStringProperty(Constants.Properties.Role.RoleName, true);
            UserRoleManager urmServer = new UserRoleManager();
            using (urmServer.CreateConnection())
            {
                urmServer.Connection.Open(base.BaseAPIConnectionString);
                Role role = urmServer.GetRole(roleName);
                if (role == null)
                {
                    throw new ApplicationException(Constants.ErrorMessages.RoleNotExists);
                }

                string roleItemName = base.GetStringProperty(Constants.Properties.Role.RoleItem, true);
                RoleItem remItem = null;
                foreach (RoleItem ri in role.Include)
                {
                    if (string.Compare(ri.Name, roleItemName, true) == 0)
                        remItem = ri;
                }
                if (remItem != null)
                {
                    role.Include.Remove(remItem);
                }
                else
                {
                    foreach (RoleItem ri in role.Exclude)
                    {
                        if (string.Compare(ri.Name, roleItemName, true) == 0)
                        {
                            remItem = ri;
                        }
                    }
                    if (remItem != null)
                    {
                        role.Exclude.Remove(remItem);
                    }
                }
                urmServer.UpdateRole(role);
            }
        }
        private void AddRoleItem()
        {
            ServiceObject serviceObject = this.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();


            UserRoleManager urmServer = new UserRoleManager();
            using (urmServer.CreateConnection())
            {
                urmServer.Connection.Open(base.BaseAPIConnectionString);
                Role role = urmServer.GetRole(base.GetStringProperty(Constants.Properties.Role.RoleName,true));
                if (role == null)
                {
                    throw new ApplicationException(Constants.ErrorMessages.RoleNotExists);
                }
                string roleItemName = base.GetStringProperty(Constants.Properties.Role.RoleItem, true);
                string roleItemType = base.GetStringProperty(Constants.Properties.Role.RoleItemType, true);
                bool exclude = base.GetBoolProperty(Constants.Properties.Role.RoleExclude);
                switch (roleItemType.ToUpper())
                {
                    case "GROUP":
                        GroupItem gi = new GroupItem(roleItemName);
                        if (exclude)
                            role.Exclude.Add(gi);
                        else
                            role.Include.Add(gi);
                        break;
                    case "USER":
                        UserItem ui = new UserItem(roleItemName);
                        if (exclude)
                            role.Exclude.Add(ui);
                        else
                            role.Include.Add(ui);
                        break;
                    default:
                        throw new ApplicationException(string.Format(Constants.ErrorMessages.RoleTypeNotSupported, roleItemType));
                    //break;
                }
                urmServer.UpdateRole(role);
            }
        }
        private void ListRoleItems()
        {
            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;
            UserRoleManager urmServer = new UserRoleManager();
            using (urmServer.CreateConnection())
            {
                urmServer.Connection.Open(base.BaseAPIConnectionString);
                Role role = urmServer.GetRole(base.GetStringProperty(Constants.Properties.Role.RoleName, true));
                if (role == null)
                {
                    throw new ApplicationException(Constants.ErrorMessages.RoleNotExists);
                }
                RoleItemCollection<Role, RoleItem> items = role.Include;
                foreach (RoleItem ri in items)
                {
                    DataRow row = results.NewRow();
                    results.Rows.Add(FillRoleItemRow(row, ri, false));
                }
                items = role.Exclude;
                foreach (RoleItem ri in items)
                {
                    DataRow row = results.NewRow();
                    results.Rows.Add(FillRoleItemRow(row, ri, true));
                }
            }
        }
        private static DataRow FillRoleItemRow(DataRow row, RoleItem ri, bool exclude)
        {
            row[Constants.Properties.Role.RoleItem] = ri.Name;
            row[Constants.Properties.Role.RoleExclude] = exclude;
            if (ri is GroupItem)
            {
                row[Constants.Properties.Role.RoleItemType] = "Group";
            }
            else if (ri is UserItem)
            {
                row[Constants.Properties.Role.RoleItemType] = "User" ;
            }
            else if (ri is SmartObjectItem)
            {
                row[Constants.Properties.Role.RoleItemType] = "SmartObject";
            }
            else
            {
                row[Constants.Properties.Role.RoleItemType] = "Unknown";
            }
            return row;
        }


        private void ListRoles()
        {
            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            UserRoleManager urmServer = new UserRoleManager();
            using (urmServer.CreateConnection())
            {
                urmServer.Connection.Open(base.BaseAPIConnectionString);
                Role[] roles = urmServer.GetRoles();
                foreach (Role r in roles)
                {
                    DataRow row = results.NewRow();
                    row[Constants.Properties.Role.RoleName] = r.Name;
                    row[Constants.Properties.Role.RoleDescription] = r.Description;
                    row[Constants.Properties.Role.RoleDynamic] = r.IsDynamic;
                    results.Rows.Add(row);
                }
            }
        }
        private void AddRole()
        {
            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;
            Role role = new Role();
            UserRoleManager urmServer = new UserRoleManager();
            using (urmServer.CreateConnection())
            {
                urmServer.Connection.Open(base.BaseAPIConnectionString);

                role.Name = base.GetStringProperty(Constants.Properties.Role.RoleName, true);
                role.Description = base.GetStringProperty(Constants.Properties.Role.RoleDescription);;
                role.IsDynamic = base.GetBoolProperty(Constants.Properties.Role.RoleDynamic);

                // At least one roleItem has to be created with the new group
                string roleItemName = base.GetStringProperty(Constants.Properties.Role.RoleItem, true);
                string roleItemType = base.GetStringProperty(Constants.Properties.Role.RoleItemType, true);
                switch (roleItemType.ToUpper())
                {
                    case "GROUP":
                        GroupItem gi = new GroupItem(roleItemName);
                        role.Include.Add(gi);
                        break;
                    case "USER":
                        UserItem ui = new UserItem(roleItemName);
                        role.Include.Add(ui);
                        break;
                    default:
                        throw new ApplicationException(string.Format(Constants.ErrorMessages.RoleTypeNotSupported, roleItemType));
                    //break;
                }
                urmServer.CreateRole(role);
                urmServer.Connection.Close();
            }
        }
        private void DeleteRole()
        {
            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;
            UserRoleManager urmServer = new UserRoleManager();
            using (urmServer.CreateConnection())
            {
                urmServer.Connection.Open(base.BaseAPIConnectionString);
                string roleName = base.GetStringProperty(Constants.Properties.Role.RoleName, true);
                Role role = urmServer.GetRole(roleName);
                if (role == null)
                {
                    throw new ApplicationException(Constants.ErrorMessages.RoleNotExists);
                }
                else
                {
                    urmServer.DeleteRole(role.Guid, role.Name);
                    urmServer.Connection.Close();
                }
            }
        }

    }
}
