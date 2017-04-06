using System.Data;
using System.Linq;

namespace SourceCode.SmartObjects.Services.Tests.Extensions
{
    public static class DataTableExtensions
    {
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