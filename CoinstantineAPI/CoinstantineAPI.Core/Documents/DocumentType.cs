using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CoinstantineAPI.Core.Documents
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DocumentType
    {
        NotSet,
        [EnumMember(Value = "white-paper")]
        WhitePaper,
        [EnumMember(Value = "terms-and-services")]
        TermsAndServices,
        [EnumMember(Value = "privacy-policy")]
        PrivacyPolicy
    }
}
