using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CoinstantineAPI.Core.Documents
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum FileApplicationType
    {
        [EnumMember(Value = "web")]
        Web,
        [EnumMember(Value = "pdf")]
        PDF,
        [EnumMember(Value = "word")]
        Word,
        [EnumMember(Value = "excel")]
        Excel,
        [EnumMember(Value = "other")]
        Other
    }
}
