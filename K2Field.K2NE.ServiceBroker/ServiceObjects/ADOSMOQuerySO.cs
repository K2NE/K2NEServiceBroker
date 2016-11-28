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
    class ADOSMOQuerySO : ServiceObjectBase
    {
        public ADOSMOQuerySO(K2NEServiceBroker api) : base(api) { }


        public override void Execute()
        {
            RunADOQuery();
        }


        public override string ServiceFolder
        {
            get
            {
                return Constants.ServiceFolders.ADONETQuery;
            }
        }

        public override List<ServiceObject> DescribeServiceObjects()
        {
            List<ServiceObject> sos = new List<ServiceObject>();


            foreach (KeyValuePair<string, string> query in ADOSMOQueries)
            {
                ServiceObject so = Helper.CreateServiceObject(query.Key, "ADO.NET SMO query.");

                so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ExportToExcel.FileName, SoType.Text, "The name of generated file"));

                //so.Properties.Add(Helper.CreateProperty(Constants.SOProperties.ExportToExcel.ExcelFile, SoType.File, "The name of generated file"));

                FileProperty fpropData = new FileProperty(Constants.SOProperties.ExportToExcel.ExcelFile, new MetaData(), String.Empty, String.Empty);
                fpropData.MetaData.DisplayName = Constants.SOProperties.ExportToExcel.ExcelFile;
                fpropData.MetaData.Description = "The name of generated file";
                so.Properties.Add(fpropData);

                DataTable results = new DataTable();

                /* To do: parsing properties. Without that queries contains WHERE and @parameters will not work on initialization level (no properties created).
                 * The queries like this do not work at the moment:
                 * SELECT * FROM table WHERE type = @type
                 * There is no custom error message, only system one, because
                 * it's impossible to found if there are @parameters used within WHERE clause, because these queries will work:
                 * SELECT * FROM table WHERE type='Type1' HAVING (id = @id)
                */

                Dictionary<string, string> props = new Dictionary<string, string>();
                foreach (Match match in Regex.Matches(query.Value, "\\@\\w+"))
                {
                    if (!props.ContainsKey(match.ToString()))
                    {
                        props.Add(match.ToString(), "0");
                    }
                }

                results = GetSchema(query.Value, props);

                Method soMethod = Helper.CreateMethod(Constants.Methods.ADOQuery.ListQueryData, "Returns result of SMO query.", MethodType.List);
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

                Method mExcelFromADOQuery = Helper.CreateMethod(Constants.Methods.ExportToExcel.ExcelFromADOQuery, "Generate an Excel file based on ADO query", MethodType.Read);
                mExcelFromADOQuery.MetaData.AddServiceElement("Query", query.Value);
                mExcelFromADOQuery.ReturnProperties.Add(Constants.SOProperties.ExportToExcel.ExcelFile);

                mExcelFromADOQuery.InputProperties.Add(Constants.SOProperties.ExportToExcel.FileName);
                mExcelFromADOQuery.Validation.RequiredProperties.Add(Constants.SOProperties.ExportToExcel.FileName);
                so.Methods.Add(mExcelFromADOQuery);


                sos.Add(so);
            }

            return sos;
        }

        private void RunADOQuery()
        {
            //ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            //string query = serviceObject.Methods[0].MetaData.GetServiceElement<string>("Query");
            //serviceObject.Properties.InitResultTable();
            //DataTable results = base.ServiceBroker.ServicePackage.ResultTable;
            //results.Load(GetData(query, serviceObject.Properties));


            switch (base.ServiceBroker.Service.ServiceObjects[0].Methods[0].Name)
            {
                case Constants.Methods.ADOQuery.ListQueryData:
                    GetData();
                    break;
                case Constants.Methods.ExportToExcel.ExcelFromADOQuery:
                    ExportToExcel();
                    break;

            }
        }


        private DataTableReader GetData(string query, Properties props)
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
                        adapter.Fill(results);
                    }
                }
                connection.Close();
            }
            return results.CreateDataReader();
        }
        private DataTableReader GetData()
        {
            //DataTable results = new DataTable();

            ServiceObject serviceObject = base.ServiceBroker.Service.ServiceObjects[0];
            string query = serviceObject.Methods[0].MetaData.GetServiceElement<string>("Query");
            serviceObject.Properties.InitResultTable();
            DataTable results = base.ServiceBroker.ServicePackage.ResultTable;

            using (SOConnection connection = new SOConnection(base.BaseAPIConnectionString))
            {
                using (SOCommand command = new SOCommand(query, connection))
                {
                    using (SODataAdapter adapter = new SODataAdapter(command))
                    {
                        foreach (Property prop in serviceObject.Properties)
                        {
                            if (prop.Value != null)
                            {
                                command.Parameters.AddWithValue(prop.Name, prop.Value);
                            }
                        }
                        connection.DirectExecution = true;
                        connection.Open();
                        adapter.Fill(results);
                    }
                }
                connection.Close();
            }
            return results.CreateDataReader();
        }
        private DataTable GetSchema(string query, Dictionary<string, string> props)
        {
            DataTable results = new DataTable();

            using (SOConnection connection = new SOConnection(base.BaseAPIConnectionString))
            {
                using (SOCommand command = new SOCommand(query, connection))
                {
                    using (SODataAdapter adapter = new SODataAdapter(command))
                    {
                        foreach (KeyValuePair<string, string> prop in props)
                        {
                            if (prop.Value != null)
                            {
                                command.Parameters.AddWithValue(prop.Key, prop.Value);
                            }
                        }

                        connection.DirectExecution = true;
                        connection.Open();
                        adapter.FillSchema(results, SchemaType.Source);
                    }
                }
                connection.Close();
            }
            return results;
        }

        private void ExportToExcel()
        {


            ServiceObject serviceObject = ServiceBroker.Service.ServiceObjects[0];
            string ADOQuery = serviceObject.Methods[0].MetaData.GetServiceElement<string>("Query");
            serviceObject.Properties.InitResultTable();
            DataTable results = ServiceBroker.ServicePackage.ResultTable;
            DataTable SOQueryResult = new DataTable();
            string FileName = GetStringProperty(Constants.SOProperties.ExportToExcel.FileName, true);

            CreateExcel excel = new CreateExcel();
            using (SOConnection connection = new SOConnection(base.BaseAPIConnectionString))
            using (SOCommand command = new SOCommand(ADOQuery, connection))
            using (SODataAdapter adapter = new SODataAdapter(command))
            {
                connection.DirectExecution = true;
                adapter.Fill(SOQueryResult);
            }
            DataRow dr = results.NewRow();
            //Calling the helper method with dataresult and expecting a File in return.
            dr[Constants.SOProperties.ExportToExcel.ExcelFile] = excel.GetExcelFromADOQuery(SOQueryResult, FileName).ToString();

            results.Rows.Add(dr);

        }



    }
}
