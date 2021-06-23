using System;
using System.Linq;
using System.Threading.Tasks;
using Zoobook.Models;
using Zoobook.Shared;
using Microsoft.EntityFrameworkCore;

namespace Zoobook.Core
{
    public abstract class QueryBase<TEntity, TRequest>
        where TEntity : class, new()
        where TRequest : class, IRequestBaseDto
    {
        protected int TotalCount { get; set; } = 0;
        protected TRequest Request { get; }

        public QueryBase(TRequest request)
        {
            Request = request;
        }

        protected abstract Task<IQueryable<TEntity>> PerformQueryAsync();

        public virtual async Task<QueryResult<IQueryable<TEntity>>> ExecuteQueryAsync()
        {
            try
            {
                var result = await PerformQueryAsync();
                var filteredResult = await FilterEntitiesAsync(result);

                return QueryResult<IQueryable<TEntity>>.Success(filteredResult, TotalCount);
            }
            catch (Exception exception)
            {
                var error = exception.InnerException?.Message ?? exception.Message;
                return QueryResult<IQueryable<TEntity>>.Error(error);
            }
        }

        private async Task<IQueryable<TEntity>> FilterEntitiesAsync(IQueryable<TEntity> query)
        {
            TotalCount = query.Count();
            var pagedQuery = query
                .AsNoTracking()
                .ApplyPaging(Request?.Paging);

            return await Task.Run(() => pagedQuery.AsNoTracking());
        }
    }
}
