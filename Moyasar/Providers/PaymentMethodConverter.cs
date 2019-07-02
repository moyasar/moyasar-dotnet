using System;
using System.Collections.Generic;
using Moyasar.Abstraction;
using Moyasar.Models;
using Newtonsoft.Json;

namespace Moyasar.Providers
{
    /// <summary>
    /// Converter used by Newtonsoft's json library to deserialize a json payment
    /// type object into an <code>IPaymentMethod</code>
    /// </summary>
    public class PaymentMethodConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var dict = serializer.Deserialize<Dictionary<string, object>>(reader);

            if (!dict.ContainsKey("type")) return null;
            
            if (dict["type"].ToString().Equals("creditcard", StringComparison.InvariantCultureIgnoreCase))
            {
                return JsonConvert.DeserializeObject<CreditCard>(JsonConvert.SerializeObject(dict));
            }
            else
            {
                return JsonConvert.DeserializeObject<Sadad>(JsonConvert.SerializeObject(dict));
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IPaymentMethod) == objectType;
        }
    }
}