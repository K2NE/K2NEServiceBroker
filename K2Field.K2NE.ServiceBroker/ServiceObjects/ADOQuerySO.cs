using K2Field.K2NE.ServiceBroker.Helpers;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using CLIENT = SourceCode.Workflow.Client;
using System;
using System.Collections.Generic;
using System.Data;
using SourceCode.Workflow.Management;
using System.Data.SqlClient;

using SourceCode.Data.SmartObjectsClient;


namespace K2Field.K2NE.ServiceBroker.ServiceObjects
{
    class ADOQuerySO : ServiceObjectBase
    {
        public ADOQuerySO(K2NEServiceBroker api) : base(api) { }


        public override void Execute()
        {
            RunADOQuery();
        }


        public override string ServiceFolder
        {
            get
            {
                return "ADOQuery";
            }
        }

        public override List<ServiceObject> DescribeServiceObjects()
        {
            List<ServiceObject> sos = new List<ServiceObject>();

            SOConnectionStringBuilder sbConnection = new SOConnectionStringBuilder();
            sbConnection.Server = "localhost";
            sbConnection.Port = int.Parse(ServiceBroker.Service.ServiceConfiguration[Constants.ConfigurationProperties.WorkflowClientPort].ToString());

            foreach (KeyValuePair<string, string> query in ADOQueries)
            {
                ServiceObject so = Helper.CreateServiceObject(query.Key, "Description.", false);

                Method runADOQuery = Helper.CreateMethod("List", "Returns result of query.", MethodType.List, false);
                runADOQuery.MetaData.AddServiceElement("Query", query.Value);

                using (SOConnection connection = new SOConnection())
                using (SOCommand command = new SOCommand(query.Value, connection))
                using (SODataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection, 0, 1))
                {
                    connection.DirectExecution = true;
                    connection.Open();
                    reader.Read();

                    int z = 0;
                    while (z < reader.FieldCount)
                    {
                        string name = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(reader.GetName(z));
                        SoType type = MapHelper.GetSoTypeByType(reader.GetFieldType(z));

                        so.Properties.Add(Helper.CreateProperty(name, type, name, false));

                        runADOQuery.InputProperties.Add(Helper.CreateProperty(name, type, name, false));
                        runADOQuery.ReturnProperties.Add(Helper.CreateProperty(name, type, name, false));
                        z++;
                    }

                }

                so.Methods.Add(runADOQuery);
                sos.Add(so);
            }

            return sos;
        }



        private void RunADOQuery()
        {
            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;
            SqlExecute sqlQuery = base.CreateDirectSqlExecute();
            sqlQuery.SqlQuery = serviceObject.Methods[0].MetaData.GetServiceElement<string>("Query");
        }
    }
}
