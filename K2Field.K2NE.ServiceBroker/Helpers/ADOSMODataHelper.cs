using System.Text;
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
                        string _query = String.Format("SELECT * FROM ({0})", query) + AppendInputParameters(props);
                        connection.DirectExecution = true;
                        connection.Open();
                        adapter.Fill(results);
                    }
                }
                connection.Close();
            }
            return results.CreateDataReader();
        }
        public static DataTable GetSchema(string connStr, string query)
        {
            DataTable results = new DataTable();

            using (SOConnection connection = new SOConnection(connStr))
            {
                using (SOCommand command = new SOCommand(query, connection))
                {
                    using (SODataAdapter adapter = new SODataAdapter(command))
                    {
                        connection.DirectExecution = true;
                        connection.Open();
                        adapter.FillSchema(results, SchemaType.Source);
                    }
                }
                connection.Close();
            }
            return results;
        }
        
        private static string AppendInputParameters(Properties props)
        {
            StringBuilder sql = new StringBuilder();

            if (props.Any(q => q.Value != null))
            {
                sql.Append(" WHERE ");
            }

            foreach (var prop in props.Where(q => q.Value != null))
            {
                string[] propValues = prop.Value.ToString().Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder queryPropValues = new StringBuilder();

                bool first = true;
                foreach (string propValue in propValues)
                {
                    queryPropValues.AppendFormat(first ? "'{0}'" : ",'{0}'", propValue);
                    first = false;
                }

                sql.AppendFormat(" \"{0}\" IN ({1}) AND", prop.Name, queryPropValues);
            }

            string returnString = sql.ToString().EndsWith("AND") ? sql.ToString().Substring(0, sql.ToString().Length - 3) : sql.ToString();

            return returnString;
        }

    }
}
