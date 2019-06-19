using K2Field.K2NE.ServiceBroker.Helpers;
using SourceCode.Data;
using SourceCode.Data.DacClient;
using SourceCode.Hosting.Server.Interfaces;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using SourceCode.Workflow.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;

namespace K2Field.K2NE.ServiceBroker.ServiceObjects
{
    class SQLCheck : ServiceObjectBase
    {
        public SQLCheck(K2NEServiceBroker api) : base(api) { }



        public override string ServiceFolder
        {
            get
            {
                return "SQLCheck"; // todo: replace with constant.
            }
        }

        public override List<ServiceObject> DescribeServiceObjects()
        {

            ServiceObject so = Helper.CreateServiceObject("ExecStoredProc", "YOLA");
            so.Properties.Add(Helper.CreateProperty("Id", SoType.Autonumber, "The ID of the data"));
            so.Properties.Add(Helper.CreateProperty("Data", SoType.Text, "Data"));
            so.Properties.Add(Helper.CreateProperty("CreatedOn", SoType.DateTime, "CreatedOn"));

            Method getdatemethod = Helper.CreateMethod("GetData", "Get the data", MethodType.List);
            getdatemethod.ReturnProperties.Add("Id");
            getdatemethod.ReturnProperties.Add("Data");
            getdatemethod.ReturnProperties.Add("CreatedOn");
            so.Methods.Add(getdatemethod);

            return new List<ServiceObject> { so };
        }

        public override void Execute()
        {


            ExecStoredProc();
        }


        private Property GetReturnProperty(ServiceObject serviceObject, string displayName)
        {
            if (serviceObject == null)
            {
                throw new ArgumentNullException("serviceObject");
            }
            if (string.IsNullOrEmpty(displayName))
            {
                throw new ArgumentNullException("displayName");
            }
            foreach (string returnProperty in serviceObject.Methods[0].ReturnProperties)
            {
                Property property = serviceObject.Properties[returnProperty];
                if (property != null && property.MetaData.DisplayName.Equals(displayName, StringComparison.CurrentCultureIgnoreCase))
                {
                    return property;
                }
            }
            return null;
        }


        private void ExecStoredProc()
        {

            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            SqlExecute sqlExec = CreateDirectSqlExecute();

            string filter = base.ServiceBroker.Service.ServiceObjects[0].Methods[0].SqlFilter.ToString();
            string realFilter = string.Empty;
            if (filter.StartsWith("[Data] like"))
            {
                realFilter = Regex.Match("User name (sales)", @"\%([^%]*)\%").Groups[1].Value;
            }

  



            DacSettings dacSettings = new DacSettings
            {
                ConnectionString = base.BuildConnectionString().ToString(),
                CommandTimeout = 30,
                UseThreadUserForConnection = true,
                KeepOpen = true
            };

            Dac dac = dacSettings.Create();
            DacCommand dacCommand = dac.CreateCommand("[dbo].[GetData]", CommandType.StoredProcedure);
            dacCommand.CommandType = CommandType.StoredProcedure;
            dacCommand.Parameters.Add(new SqlParameter("filter", realFilter).ToDacParameter());

            base.ServiceBroker.IsSqlExecute = false;
            base.ServiceBroker.SqlQueryExecute = null;

            base.ServiceBroker.ServicePackage.ResultTable = dacCommand.ExecuteDataTable();
            /*
            Dictionary<int, string> dictionary = new Dictionary<int, string>();
            for (int m = 0; m < base.ServiceBroker.ServicePackage.ResultTable.Columns.Count; m++)
            {
                DataColumn dataColumn2 = base.ServiceBroker.ServicePackage.ResultTable.Columns[m];
               
                Property returnProperty = GetReturnProperty(serviceObject, dataColumn2.ColumnName);
                dictionary.Add(m, returnProperty.Name);
                dataColumn2.ColumnName = "col" + m;
            }
            for (int n = 0; n < base.ServiceBroker.ServicePackage.ResultTable.Columns.Count; n++)
            {
                base.ServiceBroker.ServicePackage.ResultTable.Columns[n].ColumnName = dictionary[n];
            }*/


        }


    }
}
