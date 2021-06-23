using System;
using System.Linq.Expressions;

namespace Zoobook.Shared
{
    public static class ExpressionExtension
    {
        /// <summary>
        /// Appends an OrElse to the expression
        /// </summary>
        /// <param name="expression">Expression instance</param>
        /// <param name="expressionOr">Expression to be appended on the OrElse</param>
        public static Expression<Func<TEntity, bool>> OrElse<TEntity>(
            this Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, bool>> expressionOr)
        {
            if (expression == null) return expressionOr;

            return Expression.Lambda<Func<TEntity, bool>>(
                Expression.OrElse(new ExpressionSwapper(expression.Parameters[0], expressionOr.Parameters[0])
                    .Visit(expression.Body), expressionOr.Body),
                expressionOr.Parameters);
        }

        /// <summary>
        /// Appends an AndAlso to the expression
        /// </summary>
        /// <param name="expression">Expression instance</param>
        /// <param name="expressionOr">Expression to be appended on the AndAlso</param>
        public static Expression<Func<TEntity, bool>> AndAlso<TEntity>(
            this Expression<Func<TEntity, bool>> expression,
            Expression<Func<TEntity, bool>> expressionAnd)
        {
            if (expression == null) return expressionAnd;

            return Expression.Lambda<Func<TEntity, bool>>(
                Expression.AndAlso(new ExpressionSwapper(expression.Parameters[0], expressionAnd.Parameters[0])
                    .Visit(expression.Body), expressionAnd.Body),
                expressionAnd.Parameters);
        }
    }

    internal class ExpressionSwapper : ExpressionVisitor
    {
        private readonly Expression _from;
        private readonly Expression _to;

        public ExpressionSwapper(Expression from, Expression to)
        {
            _from = from;
            _to = to;
        }

        public override Expression Visit(Expression node)
        {
            return node == _from ? _to : base.Visit(node);
        }
    }
}
