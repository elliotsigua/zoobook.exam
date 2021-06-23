using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Zoobook.Shared
{
    public static class QueryableExtension
    {

        /// <summary>
        /// Applies the paging on the query
        /// </summary>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="query">Existing Query Parameters</param>
        /// <param name="paging">Paging to be applied</param>
        /// <returns>Paged data records</returns>
        public static IQueryable<TEntity> ApplyPaging<TEntity>(this IQueryable<TEntity> query, IPaging paging)
        {
            if (Equals(paging, null)) return query;

            return paging.Page < 1 ?
                query.AsQueryable() :
                query.Skip((paging.Page - 1) * paging.PageSize).Take(paging.PageSize);
        }

        /// <summary>
        /// Adds the provided clause when the predicate condition is true
        /// </summary>
        /// <param name="query">Query where to append the where clause</param>
        /// <param name="predicateCondition">Predicate condition to be checked</param>
        /// <param name="clause">Where Clause to be appended</param>
        public static IQueryable<TEntity> ConditionalWhere<TEntity>(
            this IQueryable<TEntity> query,
            Func<bool> predicateCondition,
            Expression<Func<TEntity, bool>> clause)
        {
            return predicateCondition() ? query.Where(clause) : query;
        }

        /// <summary>
        /// Adds the provided include clause when the predicate condition is true
        /// </summary>
        /// <param name="query">Query where to append the where clause</param>
        /// <param name="predicateCondition">Predicate condition to be checked</param>
        /// <param name="includeClause">Include Clause to be appended</param>
        public static IQueryable<TEntity> ConditionalInclude<TEntity, TProperty>(
            this IQueryable<TEntity> query,
            Func<bool> predicateCondition,
            Expression<Func<TEntity, TProperty>> includeClause)
            where TEntity : class
        {
            if (!predicateCondition()) { return query; }
            return query.Include(includeClause);
        }
    }
}
