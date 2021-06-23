using System;
using System.Linq;
using System.Threading.Tasks;
using Zoobook.Models;

namespace Zoobook.Core
{
    public abstract class CommandBase<TEntity>
        where TEntity : class, new()
    {
        public async virtual Task<QueryResult<IQueryable<TEntity>>> ExecuteAsync()
        {
            try
            {
                return await PerformCommandAsync();
            }
            catch (Exception exception)
            {
                var error = exception.InnerException?.Message ?? exception.Message;
                return QueryResult<IQueryable<TEntity>>.Error(error);
            }
        }

        protected abstract Task<QueryResult<IQueryable<TEntity>>> PerformCommandAsync();
    }
}
