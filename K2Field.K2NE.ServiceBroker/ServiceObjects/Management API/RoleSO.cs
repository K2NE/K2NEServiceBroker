using K2Field.K2NE.ServiceBroker.Helpers;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceCode.Security.UserRoleManager.Management;
using System.Data;
using K2Field.K2NE.ServiceBroker.Properties;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects
{
    public class RoleManagementSO : ServiceObjectBase
    {
        public RoleManagementSO(K2NEServiceBroker broker) : base(broker) { }

        public override string ServiceFolder
        {
            get
            {
                return Constants.ServiceFolders.ManagementAPI;
            }
        }


        public override List<SourceCode.SmartObjects.Services.ServiceSDK.Objects.ServiceObject> DescribeServiceObjects()
        {
            ServiceObject soRoleItem = Helper.CreateServiceObject("RoleManagement", "K2 Role management (add/remove/list K2 roles)");


            soRoleItem.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Role.RoleName, SoType.Text, "The name of the role to manage."));
            soRoleItem.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Role.RoleItemType, SoType.Text, "The type of role item (Group, User, SmartObject)."));
            soRoleItem.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Role.RoleItem, SoType.Text, "The FQN name of the role item."));
            soRoleItem.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Role.RoleDescription, SoType.Text, "A short description of the role."));
            soRoleItem.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Role.RoleItem, SoType.Text, "The FQN name of the role item."));
            soRoleItem.Properties.Add(Helper.CreateProperty(Constants.SOProperties.Role.RoleDynamic, SoType.YesNo, "Is a role dynamic?"));

            Method addRoleItem = Helper.CreateMethod(Constants.Methods.Role.AddRoleItem, "Add a RoleItem to a role", MethodType.Create);
            addRoleItem.InputProperties.Add(Constants.SOProperties.Role.RoleName);
            addRoleItem.InputProperties.Add(Constants.SOProperties.Role.RoleItem);
            addRoleItem.InputProperties.Add(Constants.SOProperties.Role.RoleItemType);
            addRoleItem.Validation.RequiredProperties.Add(Constants.SOProperties.Role.RoleName);
            addRoleItem.Validation.RequiredProperties.Add(Constants.SOProperties.Role.RoleItem);
            addRoleItem.Validation.RequiredProperties.Add(Constants.SOProperties.Role.RoleItemType);
            soRoleItem.Methods.Add(addRoleItem);


            Method deleteRoleItem = Helper.CreateMethod(Constants.Methods.Role.RemoveRoleItem, "Remove a RoleItem from a role", MethodType.Delete);
            deleteRoleItem.InputProperties.Add(Constants.SOProperties.Role.RoleName);
            deleteRoleItem.InputProperties.Add(Constants.SOProperties.Role.RoleItem);
            deleteRoleItem.Validation.RequiredProperties.Add(Constants.SOProperties.Role.RoleName);
            deleteRoleItem.Validation.RequiredProperties.Add(Constants.SOProperties.Role.RoleItem);
            soRoleItem.Methods.Add(deleteRoleItem);

            Method listRoleItems = Helper.CreateMethod(Constants.Methods.Role.ListRoleItem, "List the RoleItems in a role", MethodType.List);
            listRoleItems.InputProperties.Add(Constants.SOProperties.Role.RoleName);
            listRoleItems.ReturnProperties.Add(Constants.SOProperties.Role.RoleItem);
            listRoleItems.ReturnProperties.Add(Constants.SOProperties.Role.RoleItemType);
            soRoleItem.Methods.Add(listRoleItems);


            Method addRole = Helper.CreateMethod(Constants.Methods.Role.AddRole, "Add K2 Role to K2 system", MethodType.Create);
            addRole.InputProperties.Add(Constants.SOProperties.Role.RoleName);
            addRole.InputProperties.Add(Constants.SOProperties.Role.RoleDescription);
            addRole.InputProperties.Add(Constants.SOProperties.Role.RoleDynamic);
            addRole.InputProperties.Add(Constants.SOProperties.Role.RoleItemType);
            addRole.InputProperties.Add(Constants.SOProperties.Role.RoleItem);
            addRole.Validation.RequiredProperties.Add(Constants.SOProperties.Role.RoleName);
            addRole.Validation.RequiredProperties.Add(Constants.SOProperties.Role.RoleItem);
            addRole.Validation.RequiredProperties.Add(Constants.SOProperties.Role.RoleItemType);
            soRoleItem.Methods.Add(addRole);

            Method removeRole = Helper.CreateMethod(Constants.Methods.Role.RemoveRole, "Remove K2 Role to K2 system", MethodType.Delete);
            removeRole.InputProperties.Add(Constants.SOProperties.Role.RoleName);
            removeRole.Validation.RequiredProperties.Add(Constants.SOProperties.Role.RoleName);
            soRoleItem.Methods.Add(removeRole);

            Method listRoles = Helper.CreateMethod(Constants.Methods.Role.ListRoles, "list all K2 Roles", MethodType.List);
            listRoles.ReturnProperties.Add(Constants.SOProperties.Role.RoleName);
            listRoles.ReturnProperties.Add(Constants.SOProperties.Role.RoleDescription);
            listRoles.ReturnProperties.Add(Constants.SOProperties.Role.RoleDynamic);
            soRoleItem.Methods.Add(listRoles);

            return new List<ServiceObject>() { soRoleItem };


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

            string roleName = base.GetStringProperty(Constants.SOProperties.Role.RoleName, true);

            UserRoleManager urmServer = this.ServiceBroker.K2Connection.GetConnection<UserRoleManager>();
            using (urmServer.Connection)
            {

                Role role = urmServer.GetRole(roleName);
                if (role == null)
                {
                    throw new ApplicationException(Resources.RoleNotExists);
                }

                string roleItemName = base.GetStringProperty(Constants.SOProperties.Role.RoleItem, true);
                RoleItem remItem = null;

                foreach (RoleItem ri in role.RoleItems)
                {
                    if (string.Compare(ri.Name, roleItemName, true) == 0)
                    {
                        remItem = ri;
                    }
                }
                if (remItem != null)
                {
                    role.RoleItems.Remove(remItem);
                }
                urmServer.UpdateRole(role);
            }
        }
        private void AddRoleItem()
        {
            ServiceObject serviceObject = this.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();


            UserRoleManager urmServer = this.ServiceBroker.K2Connection.GetConnection<UserRoleManager>();
            using (urmServer.Connection)
            {
                Role role = urmServer.GetRole(base.GetStringProperty(Constants.SOProperties.Role.RoleName, true));
                if (role == null)
                {
                    throw new ApplicationException(Resources.RoleNotExists);
                }
                string roleItemName = base.GetStringProperty(Constants.SOProperties.Role.RoleItem, true);
                string roleItemType = base.GetStringProperty(Constants.SOProperties.Role.RoleItemType, true);
                RoleItem ri;
                switch (roleItemType.ToUpper())
                {
                    case "GROUP":
                        ri = new GroupItem(roleItemName);
                        break;
                    case "USER":
                        ri = new UserItem(roleItemName);
                        break;
                    default:
                        throw new ApplicationException(string.Format(Resources.RoleTypeNotSupported, roleItemType));
                    //break;
                }
                role.RoleItems.Add(ri);

                urmServer.UpdateRole(role);
            }
        }
        private void ListRoleItems()
        {
            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;
            UserRoleManager urmServer = this.ServiceBroker.K2Connection.GetConnection<UserRoleManager>();
            using (urmServer.Connection)
            {
                Role role = urmServer.GetRole(base.GetStringProperty(Constants.SOProperties.Role.RoleName, true));
                if (role == null)
                {
                    throw new ApplicationException(Resources.RoleNotExists);
                }
                foreach (RoleItem ri in role.RoleItems)
                {
                    DataRow row = results.NewRow();
                    results.Rows.Add(FillRoleItemRow(row, ri));
                }
            }
        }
        private static DataRow FillRoleItemRow(DataRow row, RoleItem ri)
        {
            row[Constants.SOProperties.Role.RoleItem] = ri.Name;
            if (ri is GroupItem)
            {
                row[Constants.SOProperties.Role.RoleItemType] = "Group";
            }
            else if (ri is UserItem)
            {
                row[Constants.SOProperties.Role.RoleItemType] = "User";
            }
            else if (ri is SmartObjectItem)
            {
                row[Constants.SOProperties.Role.RoleItemType] = "SmartObject";
            }
            else
            {
                row[Constants.SOProperties.Role.RoleItemType] = "Unknown";
            }
            return row;
        }


        private void ListRoles()
        {
            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            UserRoleManager urmServer = this.ServiceBroker.K2Connection.GetConnection<UserRoleManager>();
            using (urmServer.Connection)
            {
                Role[] roles = urmServer.GetRoles();
                foreach (Role r in roles)
                {
                    DataRow row = results.NewRow();
                    row[Constants.SOProperties.Role.RoleName] = r.Name;
                    row[Constants.SOProperties.Role.RoleDescription] = r.Description;
                    row[Constants.SOProperties.Role.RoleDynamic] = r.IsDynamic;
                    results.Rows.Add(row);
                }
            }
        }
        private void AddRole()
        {
            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;
            Role role = new Role();
            UserRoleManager urmServer = this.ServiceBroker.K2Connection.GetConnection<UserRoleManager>();
            using (urmServer.Connection)
            {

                role.Name = base.GetStringProperty(Constants.SOProperties.Role.RoleName, true);
                role.Description = base.GetStringProperty(Constants.SOProperties.Role.RoleDescription); ;
                role.IsDynamic = base.GetBoolProperty(Constants.SOProperties.Role.RoleDynamic);

                // At least one roleItem has to be created with the new group
                string roleItemName = base.GetStringProperty(Constants.SOProperties.Role.RoleItem, true);
                string roleItemType = base.GetStringProperty(Constants.SOProperties.Role.RoleItemType, true);
                RoleItem ri;
                switch (roleItemType.ToUpper())
                {
                    case "GROUP":
                        ri = new GroupItem(roleItemName);
                        break;
                    case "USER":
                        ri = new UserItem(roleItemName);
                        break;
                    default:
                        throw new ApplicationException(string.Format(Resources.RoleTypeNotSupported, roleItemType));
                    //break;
                }
                role.RoleItems.Add(ri);
                urmServer.CreateRole(role);
                urmServer.Connection.Close();
            }
        }
        private void DeleteRole()
        {
            base.ServiceBroker.Service.ServiceObjects[0].Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;
            UserRoleManager urmServer = this.ServiceBroker.K2Connection.GetConnection<UserRoleManager>();
            using (urmServer.Connection)
            {
                string roleName = base.GetStringProperty(Constants.SOProperties.Role.RoleName, true);
                Role role = urmServer.GetRole(roleName);
                if (role == null)
                {
                    throw new ApplicationException(Resources.RoleNotExists);
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
