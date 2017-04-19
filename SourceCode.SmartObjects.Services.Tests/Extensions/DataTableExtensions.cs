using System.Data;
using System.Linq;
using System.Text;

namespace SourceCode.SmartObjects.Services.Tests.Extensions
{
    public static class DataTableExtensions
    {
        /// <summary>
        /// Used as starting point to validate a list method's values
        /// </summary>
        /// <param name="dataTable">DataTable object with the results of the list method.</param>
        /// <returns></returns>
        public static string GenerateGetAssertHasValue(this DataTable dataTable)
        {
            var row = dataTable.Rows.Cast<DataRow>().FirstOrDefault();
            row.ThrowIfNull("row");

            var returnResult = new StringBuilder();
            var emptyStringBuilder = new StringBuilder();
            foreach (DataColumn column in row.Table.Columns)
            {
                var assertHasValue = string.Format("row.AssertHasValue<{0}>(\"{1}\");", column.DataType.Name, column.ColumnName);

                if (row[column] != null &&
                    !string.IsNullOrEmpty(row[column].ToString()))
                {
                    returnResult.AppendLine(assertHasValue);
                }
                else
                {
                    emptyStringBuilder.AppendLine(string.Concat(@"//", assertHasValue));
                }
            }

            if (emptyStringBuilder.Length > 0)
            {
                returnResult.AppendLine("// Empty values");
                returnResult.Append(emptyStringBuilder);
            }

            return returnResult.ToString();
        }

        public static bool GetCondition(this DataTable totalDataTable, int pageNumber, int pageSize)
        {
            return (pageSize * (pageNumber - 1)) < totalDataTable.Rows.Count;
        }

        public static DataTable GetPagedResult(this DataTable dataTable, int pageNumber, int pageSize)
        {
            var skip = (pageNumber - 1) * pageSize;

            DataTable pagedResults;
            if (skip >= dataTable.Rows.Count)
            {
                pagedResults = dataTable.Clone();
            }
            else
            {
                pagedResults = dataTable.AsEnumerable().Skip(skip).Take(pageSize).CopyToDataTable();
            }

            return pagedResults;
        }
    }
}