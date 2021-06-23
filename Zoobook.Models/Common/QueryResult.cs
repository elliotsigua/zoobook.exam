using System.Collections.Generic;

namespace Zoobook.Models
{
    public class QueryResult<TData>
    {
        public TData Data { get; set; }

        public int TotalCount { get; set; }

        public string ErrorMessage { get; set; }

        public bool Failed { get; set; }

        public static QueryResult<TData> Success(TData result, int? totalCount = null)
        {
            var recordCount = typeof(TData).IsArray ?
                (result as List<TData>).Count :
                result == null ? 0 : 1;

            return new QueryResult<TData>()
            {
                Failed = false,
                Data = result,
                TotalCount = totalCount ?? recordCount,
                ErrorMessage = null
            };
        }

        public static QueryResult<TData> Error(string message)
        {
            return new QueryResult<TData>()
            {
                Failed = true,
                Data = default,
                TotalCount = 0,
                ErrorMessage = message
            };
        }
    }
}
