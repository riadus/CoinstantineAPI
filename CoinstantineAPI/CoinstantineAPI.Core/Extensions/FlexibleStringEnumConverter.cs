using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CoinstantineAPI.Core.Extensions
{
	public class FlexibleStringEnumConverter : StringEnumConverter
	{
		public FlexibleStringEnumConverter()
			: base()
		{

		}

		public FlexibleStringEnumConverter(bool camelCase)
			: base(camelCase)
		{

		}
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			try
			{
				return base.ReadJson(reader, objectType, existingValue, serializer);
			}
			catch
			{
				try
				{
					return ((string)reader.Value).ToEnum(objectType);
				}
				catch (Exception e)
				{
					Debug.WriteLine($"Cannot map {reader.Value} to {objectType} due to an {e.GetType()}");
				}
				throw;
			}
		}
	}
}
