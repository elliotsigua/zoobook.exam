using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Zoobook.Shared
{
    public class PagingConverter : JsonConverter<IPaging>
    {
        public override IPaging Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
            {
                var paging = new Paging();
                var pagingValues = new List<int>();

                while (reader.Read())
                {
                    if (reader.TokenType == JsonTokenType.EndObject)
                    {
                        break;
                    }

                    var propertyName = reader.GetString()?.ToPascalCase();
                    var propertyInfo = paging.GetType().GetProperty(propertyName);
                    if (Equals(propertyInfo, null)) { continue; }

                    reader.Read();
                    reader.TryGetInt32(out var propertyValue);
                    pagingValues.Add(propertyValue);
                }
                return new Paging(pagingValues.First(), pagingValues.Last());
            }

            return JsonSerializer.Deserialize<Paging>(ref reader, options);
        }

        public override void Write(
            Utf8JsonWriter writer,
            IPaging value,
            JsonSerializerOptions options)
        {
            switch (value)
            {
                case null:
                    JsonSerializer.Serialize(writer, (IPaging)null, options);
                    break;
                default:
                    JsonSerializer.Serialize(writer, value, value.GetType(), options);
                    break;
            }
        }
    }
}
