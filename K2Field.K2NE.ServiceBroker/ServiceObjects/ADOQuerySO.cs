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
using System.Text.RegularExpressions;


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

                DataTable results = new DataTable();
                /*To do: parsing properties. Without that queries contains WHERE and @parameters will not work.
                foreach (Match match in Regex.Matches(query.Value, "\\@\\w+"))
                {
                }
                */
                results = GetData(query.Value, new Properties(), true);

                Method soMethod = Helper.CreateMethod("List", "Returns result of query.", MethodType.List);
                soMethod.MetaData.AddServiceElement("Query", query.Value);
                foreach (DataColumn col in results.Columns)
                {
                    string name = col.ColumnName;
                    SoType type = MapHelper.GetSoTypeByType(col.DataType);
                    so.Properties.Add(Helper.CreateProperty(name, type, name));
                    soMethod.InputProperties.Add(Helper.CreateProperty(name, type, name));
                    soMethod.ReturnProperties.Add(Helper.CreateProperty(name, type, name));
                }
                so.Methods.Add(soMethod);
                sos.Add(so);
            }

            return sos;
        }

        private void RunADOQuery()
        {
            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            string query = serviceObject.Methods[0].MetaData.GetServiceElement<string>("Query");
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;
            results.Load(GetData(query, serviceObject.Properties, false).CreateDataReader());
        }

        private DataTable GetData(string query, Properties props, bool schemaOnly)
        {
            DataTable results = new DataTable();

            using (SOConnection connection = new SOConnection(base.BaseAPIConnectionString))
            {
                using (SOCommand command = new SOCommand(query, connection))
                {
                    using (SODataAdapter adapter = new SODataAdapter(command))
                    {

                        foreach (Property prop in props)
                        {
                            if (prop.Value != null)
                            {
                                command.Parameters.AddWithValue(prop.Name, prop.Value);
                            }
                        }
                        
                        connection.DirectExecution = true;
                        connection.Open();
                        
                        if (schemaOnly)
                            adapter.FillSchema(results, SchemaType.Source);
                        else
                            adapter.Fill(results);
                    }
                }
                connection.Close();
            }
            return results;
        }
    }
}
