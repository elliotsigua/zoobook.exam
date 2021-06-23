using System;
using System.Collections.Generic;
using System.Linq;

namespace Zoobook.Shared
{
    public static class ComparisonUtility
    {
        /// <summary>
        /// Returns true when the enumerable strings contains case insensitive value
        /// </summary>
        /// <param name="fieldValues">List of fields to be compared</param>
        /// <param name="containValue">The target value to be compared</param>
        public static bool IgnoreCaseContains(IEnumerable<string> fieldValues, string containValue)
        {
            return fieldValues.Select(item => item?.Trim().ToLowerInvariant()).Contains(containValue?.Trim().ToLowerInvariant());
        }

        /// <summary>
        /// Returns true when the source string contains the target string
        /// </summary>
        /// <param name="source">Source string to be compared</param>
        /// <param name="target">Target string to check</param>
        public static bool IgnoreCaseContains(string source, string target)
        {
            return source.Trim().ToLowerInvariant().Contains(target?.Trim().ToLowerInvariant());
        }

        /// <summary>
        /// Returns true when both strings are equal
        /// </summary>
        /// <param name="firstString">First string to compare</param>
        /// <param name="secondString">Second string to be compared</param>
        public static bool IgnoreCaseEquals(string firstString, string secondString)
        {
            return firstString?.Trim().ToLowerInvariant() == secondString?.Trim().ToLowerInvariant();
        }
    }
}
