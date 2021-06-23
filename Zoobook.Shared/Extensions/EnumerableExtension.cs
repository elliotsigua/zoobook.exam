using System;
using System.Collections.Generic;
using System.Linq;

namespace Zoobook.Shared
{
    public static class EnumerableExtension
    {
        /// <summary>
        /// Returns true when the enumerable items is null or empty
        /// </summary>
        /// <param name="enumerable">Enumerable item list to be checked</param>
        public static bool IsNullOrEmpty<TEntity>(this IEnumerable<TEntity> enumerable, Func<TEntity, bool> predicate = null)
        {
            return Equals(enumerable, null) || !(Equals(predicate, null) ? 
                enumerable.Any() : enumerable.Any(predicate));
        }
    }
}
