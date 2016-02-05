using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using SourceCode.SmartObjects.Services.ServiceSDK.Objects;
using SourceCode.SmartObjects.Services.ServiceSDK.Types;
using SourceCode.Data.SmartObjectsClient;
using System;

namespace K2Field.K2NE.ServiceBroker.Helpers
{
    public class ADOSMODataHelper
    {
        public static DataTableReader GetData(string connStr, string query, Properties props)
        {
            DataTable results = new DataTable();

            using (SOConnection connection = new SOConnection(connStr))
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
        public static DataTable GetSchema(string connStr, string query, Dictionary<string,string> props)
        {
            DataTable results = new DataTable();

            using (SOConnection connection = new SOConnection(connStr))
            {
                using (SOCommand command = new SOCommand(query, connection))
                {
                    using (SODataAdapter adapter = new SODataAdapter(command))
                    {
                        foreach (KeyValuePair<string,string> prop in props)
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
    }
}
