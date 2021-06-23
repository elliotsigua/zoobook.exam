using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zoobook.Models;

namespace Zoobook.Core
{
    class UpdateEntitiesCommand<TEntity> : CommandBase<TEntity>
        where TEntity : class, new()
    {
        private readonly IEnumerable<TEntity> _payload;
        private readonly ZoobookContextBase _context;

        public UpdateEntitiesCommand(
            ZoobookContextBase context,
            params TEntity[] payload)
        {
            _payload = payload;
            _context = context;
        }

        protected async override Task<QueryResult<IQueryable<TEntity>>> PerformCommandAsync()
        {
            var entityTable = _context.Set<TEntity>();

            entityTable.UpdateRange(_payload);
            var saveStatus = await _context.SaveChangesAsync();
            if (saveStatus < 0)
            {
                var errorMessage = "Error during updating of entity.";
                return QueryResult<IQueryable<TEntity>>.Error(errorMessage);
            }

            return QueryResult<IQueryable<TEntity>>.Success(_payload.AsQueryable(), _payload.Count());
        }
    }
}
