using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SourceCode.SmartObjects.Services.Tests.Extensions
{
    public static class DataRowExtensions
    {
        public static void AssertAreEqual<U>(this DataRow dataRow, string columnName, U expectedValue, string rowIdentifier = null)
        {
            dataRow.ThrowIfNull(nameof(dataRow));
            columnName.ThrowIfNullOrWhiteSpace(nameof(columnName));

            Assert.AreEqual<U>(expectedValue, dataRow.Field<U>(columnName)
                , string.Concat($"{dataRow.Table.TableName}.{columnName} has an incorrect value.", rowIdentifier ?? string.Empty));
        }

        public static U AssertHasValue<U>(this DataRow dataRow, string columnName, string rowIdentifier = null)
        {
            dataRow.ThrowIfNull(nameof(dataRow));
            columnName.ThrowIfNullOrWhiteSpace(nameof(columnName));

            var cellObjectValue = (dataRow.Field<object>(columnName))?.ToString();

            Assert.IsFalse(string.IsNullOrEmpty(cellObjectValue)
                , $"[{dataRow?.Table?.TableName}].[{columnName}] must have a '{typeof(U).ToString()}' value. Row Identifier: [{rowIdentifier}]");

            try
            {
                var cellValue = dataRow.Field<U>(columnName);
                return cellValue;
            }
            catch (System.Exception ex)
            {
                Assert.Fail($"[{dataRow?.Table?.TableName}].[{columnName}] convert to '{typeof(U).ToString()}' error. Value: '{cellObjectValue}' Row Identifier: [{rowIdentifier}] Error: '{ex.Message}'");
                throw;
            }
        }

        public static string AssertHasValue(this DataRow dataRow, string columnName, string rowIdentifier = null)
        {
            return dataRow.AssertHasValue<string>(columnName, rowIdentifier);
        }

        public static string GetFirstValue(this DataRow dataRow, params string[] columnNames)
        {
            if (dataRow?.Table == null) return null;

            foreach (var columnName in columnNames)
            {
                if (dataRow?.Table?.Columns == null ||
                    !dataRow.Table.Columns.Contains(columnName) ||
                    string.IsNullOrWhiteSpace(dataRow[columnName]?.ToString()))
                {
                    continue;
                }

                return dataRow[columnName].ToString();
            }

            return null;
        }
    }
}