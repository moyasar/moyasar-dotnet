using System;
using System.Collections.Generic;
using Moyasar.Abstraction;
using Newtonsoft.Json;

namespace Moyasar.Providers
{
    /// <summary>
    /// Converter used by Newtonsoft's json library to deserialize a json payment
    /// type object into an <code>IPaymentMethod</code>
    /// </summary>
    public class MetadataSerializer : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Dictionary<string, string> dict)
            {
                writer.WriteNull();
                
                foreach (var pair in dict)
                {
                    writer.WritePropertyName($"metadata[{pair.Key}]");
                    writer.WriteValue(pair.Value);
                }
            }
            else
            {
                serializer.Serialize(writer, value);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize(reader, objectType);
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IPaymentMethod) == objectType;
        }
    }
}
