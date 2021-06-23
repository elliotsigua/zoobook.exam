using System.Text.Json.Serialization;

namespace Zoobook.Shared
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EmployeeStatus
    {
        None,
        Inactive,
        Active
    }
}
