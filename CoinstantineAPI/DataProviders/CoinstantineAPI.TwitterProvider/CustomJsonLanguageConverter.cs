using System;
using Newtonsoft.Json;
using Tweetinvi.Logic.JsonConverters;
using Tweetinvi.Models;

namespace CoinstantineAPI.DataProvider.TwitterProvider
{
    public class CustomJsonLanguageConverter : JsonLanguageConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            return reader.Value != null
                ? base.ReadJson(reader, objectType, existingValue, serializer)
                : Language.English;
        }
    }
}
