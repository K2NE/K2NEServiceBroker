using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SourceCode.SmartObjects.Services.Tests.Extensions
{
    public static class DataRowExtensions
    {
        public static void AssertAreEqual<U>(this DataRow dataRow, string columnName, U expectedValue, string rowIdentifier = null)
        {
            dataRow.ThrowIfNull("dataRow");
            columnName.ThrowIfNullOrWhiteSpace("columnName");

            Assert.AreEqual<U>(expectedValue, dataRow.Field<U>(columnName)
                , string.Format("{0}.{1} has an incorrect value.{2}", dataRow.Table.TableName, columnName, rowIdentifier ?? string.Empty));
        }

        public static U AssertHasValue<U>(this DataRow dataRow, string columnName, string rowIdentifier = null)
        {
            dataRow.ThrowIfNull("dataRow");
            columnName.ThrowIfNullOrWhiteSpace("columnName");

            var cellObjectValue = (dataRow.Field<object>(columnName));

            Assert.IsFalse(cellObjectValue == null || string.IsNullOrEmpty(cellObjectValue.ToString()),
                string.Format("[{0}].[{1}] must have a '{2}' value. Row Identifier: [{3}]",
                    dataRow.Table.TableName, columnName, typeof(U).ToString(), rowIdentifier));
            try
            {
                var cellValue = dataRow.Field<U>(columnName);
                return cellValue;
            }
            catch (System.Exception ex)
            {
                Assert.Fail(string.Format("[{0}].[{1}] convert to '{2}' error. Value: '{3}' Row Identifier: [{4}] Error: '{5}'",
                    dataRow.Table.TableName, columnName, typeof(U).ToString(), cellObjectValue, rowIdentifier, ex.Message));

                throw;
            }
        }

        public static string AssertHasValue(this DataRow dataRow, string columnName, string rowIdentifier = null)
        {
            return dataRow.AssertHasValue<string>(columnName, rowIdentifier);
        }

        public static string GetFirstValue(this DataRow dataRow, params string[] columnNames)
        {
            if (dataRow == null ||
                dataRow.Table == null)
            {
                return null;
            }

            foreach (var columnName in columnNames)
            {
                if (dataRow.Table.Columns == null ||
                    !dataRow.Table.Columns.Contains(columnName) ||
                    string.IsNullOrWhiteSpace(dataRow[columnName].ToString()))
                {
                    continue;
                }

                return dataRow[columnName].ToString();
            }

            return null;
        }
    }
}