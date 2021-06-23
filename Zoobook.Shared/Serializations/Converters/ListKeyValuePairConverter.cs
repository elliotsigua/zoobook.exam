using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Zoobook.Shared
{
    public class ListKeyValuePairConverter : JsonConverter<List<KeyValuePair<string, object>>>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert == typeof(List<KeyValuePair<string, object>>);
        }

        public override List<KeyValuePair<string, object>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // We need to deserialize manually the List<KeyValuePair> because its not totally a list of array
            // its an object of operators that needs to manually deserialized individually
            if (reader.TokenType == JsonTokenType.StartArray ||
                reader.TokenType == JsonTokenType.StartObject)
            {
                var itemList = new List<KeyValuePair<string, object>>();
                var propertyName = string.Empty;

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        return itemList;
                    }

                    if (reader.TokenType == JsonTokenType.PropertyName)
                    {
                        propertyName = reader.GetString();
                        continue;
                    }
                    itemList.AddRange(DeserializeListOfKeyValuePair(ref reader, options, propertyName));
                }
                return itemList;
            }
            return null;
        }

        private List<KeyValuePair<string, object>> DeserializeListOfKeyValuePair(ref Utf8JsonReader reader, JsonSerializerOptions options, string propertyName)
        {
            var itemList = new List<KeyValuePair<string, object>>();
            if (reader.TokenType != JsonTokenType.StartArray) { return itemList; }

            var convertedKeyValuePairs = new List<KeyValuePair<string, object>>();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }

                // This part will read all the objects inside the array individually
                if (reader.TokenType == JsonTokenType.StartObject)
                {
                    var propertyKey = string.Empty;
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject)
                        {
                            break;
                        }

                        if (reader.TokenType == JsonTokenType.PropertyName)
                        {
                            propertyKey = reader.GetString();
                            continue;
                        }

                        var propertyValue = JsonSerializer.Deserialize<object>(ref reader, options);
                        convertedKeyValuePairs.Add(new KeyValuePair<string, object>(propertyKey, propertyValue));
                    }
                }
            }

            itemList.Add(new KeyValuePair<string, object>(propertyName, convertedKeyValuePairs));
            return itemList;
        }

        public override void Write(Utf8JsonWriter writer, List<KeyValuePair<string, object>> value, JsonSerializerOptions options)
        {
            var isMainItem = !Equals(options, null);
            List<KeyValuePair<string, object>> mainDataValues = value as List<KeyValuePair<string, object>>;

            // Consider options to be null for recursive objects
            if (isMainItem) writer.WriteStartObject();
            foreach (var item in mainDataValues)
            {
                if (!isMainItem) writer.WriteStartObject();
                writer.WritePropertyName(item.Key);
                WriteObjectValue(writer, item.Value);
                if (!isMainItem) writer.WriteEndObject();
            }
            if (isMainItem) writer.WriteEndObject();
        }

        private void WriteObjectValue(Utf8JsonWriter writer, object value)
        {
            var valueType = value.GetType();
            if (valueType.IsArray || valueType.IsAssignableFrom(typeof(List<KeyValuePair<string, object>>)))
            {
                var itemList = value as List<KeyValuePair<string, object>>;
                writer.WriteStartArray();
                Write(writer, itemList, null);
                writer.WriteEndArray();
                return;
            }
            else if (valueType.IsAssignableFrom(typeof(KeyValuePair<string, object>)))
            {
                var itemKeyValuePair = (KeyValuePair<string, object>)value;
                writer.WriteStartObject();
                writer.WritePropertyName(itemKeyValuePair.Key);
                WriteObjectValue(writer, itemKeyValuePair.Value);
                writer.WriteEndObject();
            }

            var successfullyConverted = int.TryParse(Convert.ToString(value), out var intValue);
            if (successfullyConverted)
            {
                writer.WriteNumberValue(intValue);
                return;
            }

            successfullyConverted = bool.TryParse(Convert.ToString(value), out var booleanValue);
            if (successfullyConverted)
            {
                writer.WriteBooleanValue(booleanValue);
                return;
            }

            writer.WriteStringValue(Convert.ToString(value));
        }
    }
}
