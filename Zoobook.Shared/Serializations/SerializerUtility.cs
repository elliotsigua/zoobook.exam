using System.Text.Json;

namespace Zoobook.Shared
{
    public class SerializerUtility
    {
        /// <summary>
        /// Gets the JSON Serializer options
        /// </summary>
        /// <param name="existingSettings">Existing settings to be used or extend if provided</param>
        public static JsonSerializerOptions GetJsonSerializerOptions(JsonSerializerOptions existingOptions = null)
        {
            var options = Equals(existingOptions, null) ?
                new JsonSerializerOptions() : existingOptions;

            options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.IgnoreNullValues = true;

            // Add object converters other than declared types
            options.Converters.Add(new ObjectConverter());
            options.Converters.Add(new ListKeyValuePairConverter());
            options.Converters.Add(new PagingConverter());

            return options;
        }
    }
}
