using System.Collections.Generic;
using System.Net;

namespace Zoobook.Models
{
    public class Response<TData>
    {
        public HttpStatusCode StatusCode { get; set; }

        public TData Data { get; set; }

        public int TotalCount { get; set; }

        public string ErrorMessage { get; set; }

        public bool Failed => StatusCode != HttpStatusCode.OK;

        public static Response<TData> Success(TData result, int? totalCount = null)
        {
            var recordCount = typeof(TData).IsArray ?
                (result as List<TData>).Count :
                result == null ? 0 : 1;

            return new Response<TData>()
            {
                StatusCode = HttpStatusCode.OK,
                Data = result,
                TotalCount = totalCount ?? recordCount,
                ErrorMessage = null
            };
        }

        public static Response<TData> Error(HttpStatusCode code, string error)
        {
            return new Response<TData>()
            {
                StatusCode = code,
                Data = default,
                TotalCount = 0,
                ErrorMessage = error
            };
        }

        public static Response<TData> UnprocessableEntityError(string error)
        {
            return Error(HttpStatusCode.UnprocessableEntity, error);
        }

        public static Response<TData> BadRequestError(string error)
        {
            return Error(HttpStatusCode.BadRequest, error);
        }
    }
}
