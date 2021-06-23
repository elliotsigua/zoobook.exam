using System;
using System.Linq;
using System.Threading.Tasks;
using Zoobook.Models;
using Zoobook.Shared;

namespace Zoobook.Core
{
    public class GetEntitiesQuery<TEntity, TRequest> : QueryBase<TEntity, TRequest>
        where TEntity : EntityBase, new()
        where TRequest : class, IRequestBaseDto
    {
        private readonly ZoobookContextBase _context;

        public GetEntitiesQuery(
            ZoobookContextBase context,
            TRequest request)
                : base(request)
        {
            _context = context;
        }

        protected async override Task<IQueryable<TEntity>> PerformQueryAsync()
        {
            var entityTable = _context.Set<TEntity>()
                .ConditionalWhere(() => !Request.Ids.IsNullOrEmpty(),
                    entity => Request.Ids.Contains(entity.Id))
                .AsQueryable();
            return await Task.Run(() => entityTable);
        }
    }
}
