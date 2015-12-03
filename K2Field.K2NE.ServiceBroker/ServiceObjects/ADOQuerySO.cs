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

            

            foreach (KeyValuePair<string, string> query in ADOQueries)
            {
                ServiceObject so = Helper.CreateServiceObject(query.Key, "ADO.NET query.");

                Method soMethod = Helper.CreateMethod("List", "Returns result of query.", MethodType.List);
                soMethod.MetaData.AddServiceElement("Query", query.Value);

                using (SOConnection connection = new SOConnection(base.BaseAPIConnectionString)) {
                    using (SOCommand command = new SOCommand(query.Value, connection)) {
                        using (SODataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection, 0, 1))
                        {
                            connection.DirectExecution = true;
                            connection.Open();
                            reader.Read();

                            int fieldCount = 0;
                            while (fieldCount < reader.FieldCount)
                            {
                                string name = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(reader.GetName(fieldCount));
                                SoType type = MapHelper.GetSoTypeByType(reader.GetFieldType(fieldCount));

                                so.Properties.Add(Helper.CreateProperty(name, type, name, false));

                                soMethod.InputProperties.Add(Helper.CreateProperty(name, type, name, false));
                                soMethod.ReturnProperties.Add(Helper.CreateProperty(name, type, name, false));
                                fieldCount++;
                            }
                        }
                    }
                    connection.Close();
                }

                so.Methods.Add(soMethod);
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
