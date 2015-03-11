using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace K2Field.K2NE.ServiceBroker
{
    public class RoleSO : ServiceObjectBase
    {
        public RoleSO(K2NEServiceBroker broker) : base(broker) { }

        public override List<SourceCode.SmartObjects.Services.ServiceSDK.Objects.ServiceObject> DescribeServiceObjects()
        {
            ServiceObject soRoleItem = Helper.CreateServiceObject("Role", "K2 Role management (add/remove/list K2 roles)");


            soRoleItem.Properties.Add(Helper.CreateProperty(Constants.Properties.Role.RoleName, SoType.Text, "The name of the role to manage."));
            soRoleItem.Properties.Add(Helper.CreateProperty(Constants.Properties.Role.RoleItemType, SoType.Text, "The type of role item (Group, User, SmartObject)."));
            soRoleItem.Properties.Add(Helper.CreateProperty(Constants.Properties.Role.RoleExtraData, SoType.Text, "Extradata for the role."));
            soRoleItem.Properties.Add(Helper.CreateProperty(Constants.Properties.Role.RoleExclude, SoType.YesNo, "Excluded role item."));
            soRoleItem.Properties.Add(Helper.CreateProperty(Constants.Properties.Role.RoleItem, SoType.Text, "The FQN name of the role item."));


            Method addRoleItem = Helper.CreateMethod(Constants.Methods.Role.AddRoleItem, "Add a RoleItem to a role" ,MethodType.Create);
            addRoleItem.InputProperties.Add(Constants.Properties.Role.RoleName);
            addRoleItem.InputProperties.Add(Constants.Properties.Role.RoleItem);
            addRoleItem.InputProperties.Add(Constants.Properties.Role.RoleExclude);
            addRoleItem.InputProperties.Add(Constants.Properties.Role.RoleItemType);
            addRoleItem.InputProperties.Add(Constants.Properties.Role.RoleExtraData);
            soRoleItem.Methods.Add(addRoleItem);


            Method deleteRoleItem = Helper.CreateMethod(Constants.Methods.Role.RemoveRoleItem, "Remove a RoleItem from a role", MethodType.Delete);
            deleteRoleItem.InputProperties.Add(Constants.Properties.Role.RoleName);
            deleteRoleItem.InputProperties.Add(Constants.Properties.Role.RoleItem);
            soRoleItem.Methods.Add(deleteRoleItem);

            Method listRoleItems = Helper.CreateMethod(Constants.Methods.Role.ListRoleItem, "List the RoleItems in a role", MethodType.List);
            listRoleItems.InputProperties.Add(Constants.Properties.Role.RoleName);
            listRoleItems.ReturnProperties.Add(Constants.Properties.Role.RoleItem);
            listRoleItems.ReturnProperties.Add(Constants.Properties.Role.RoleExclude);
            listRoleItems.ReturnProperties.Add(Constants.Properties.Role.RoleItemType);
            listRoleItems.ReturnProperties.Add(Constants.Properties.Role.RoleExtraData);
            soRoleItem.Methods.Add(listRoleItems);


            return new List<ServiceObject>() {soRoleItem};


        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
