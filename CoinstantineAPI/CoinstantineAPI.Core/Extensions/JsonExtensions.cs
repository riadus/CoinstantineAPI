using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CoinstantineAPI.Core.Extensions
{
	public static class JsonExtensions
	{
		static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
		{
			ContractResolver = new CamelCasePropertyNamesContractResolver(),
			Converters = { new FlexibleStringEnumConverter() },
			NullValueHandling = NullValueHandling.Ignore
		};

		public static T DeserializeTo<T>(this string jsonString)
		{
			return JsonConvert.DeserializeObject<T>(jsonString, _settings);
		}

		public static string Serialize<T>(this T objectToSerialize)
		{
			return JsonConvert.SerializeObject(objectToSerialize, _settings);
		}
	}
}