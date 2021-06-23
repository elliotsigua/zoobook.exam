using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Humanizer;

namespace Zoobook.Shared
{
    public static class StringExtension
    {
        /// <summary>
        /// Converts current string to snake-case
        /// </summary>
        /// <param name="input">Input string to be converted</param>
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            var startUnderscores = Regex.Match(input, @"^_+");
            return $"{ startUnderscores }{ Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower() }";
        }

        /// <summary>
        /// Converts current string to camelCase
        /// </summary>
        /// <param name="input">Input string to be converted</param>
        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            input = input.ToPascalCase();
            return $"{ Char.ToLowerInvariant(input[0]) }{ input[1..] }";
        }

        /// <summary>
        /// Converts current string to PascalCase
        /// </summary>
        /// <param name="input">Input string to be converted</param>
        public static string ToPascalCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            StringBuilder builder = new StringBuilder();
            foreach (var word in input.Split(" "))
            {
                foreach (var dashWord in word.Split("-"))
                {
                    if (string.IsNullOrWhiteSpace(dashWord)) continue;

                    var pascalDashWord = $"{ dashWord[0].ToString().ToUpper() }{ dashWord[1..] }";
                    builder.Append(pascalDashWord);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// Convert string to Plural
        /// </summary>
        /// <param name="singularWord"></param>
        /// <param name="input">Input string to pluralized</param>
        public static string ToPlural(this string singularWord)
        {
            return singularWord.Pluralize();
        }

        /// <summary>
        /// Converts current string to enumeration type
        /// </summary>
        /// <param name="input">Input string to be converted</param>
        /// <param name="enumType">Type of enumeration for conversion</param>
        public static object ToEnum(this string input, Type enumType)
        {
            if (string.IsNullOrWhiteSpace(input) || !enumType.IsEnum) return null;

            var enumItems = Enum.GetNames(enumType).ToList();
            var enumNameFound = enumItems.Find(item => ComparisonUtility.IgnoreCaseContains(item, input));

            var successfullyConverted = Enum.TryParse(enumType, enumNameFound, out var enumValue);
            return successfullyConverted ? enumValue : default;
        }

        // TODO: Refactor this
        /// <summary>
        /// Converts string input to a csv safe format
        /// </summary>
        /// <param name="input">Input string to convert</param>
        public static string ConvertToSafeCsvFormat(this string input)
        {
            bool needsEscaping = false;

            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            input = input
                .Replace(Environment.NewLine, " ")
                .Replace("\r", "")
                .Replace("\n", " ")
                .Trim();

            if (input.Contains('"'))
            {
                input = input.Replace("\"", "\"\"");
                needsEscaping = true;
            }

            if (input.Contains(','))
            {
                needsEscaping = true;
            }

            if (DateTime.TryParse(input, out _))
            {
                needsEscaping = true;
            }

            if (input.StartsWith("-") || input.StartsWith("="))
            {
                input = input.Insert(0, "'");
                needsEscaping = false;
            }

            if (needsEscaping)
                return $"\"{input}\"";
            else
                return input;
        }
    }
}
