using System;
using System.Collections.Generic;
using Moyasar.Abstraction;
using Moyasar.Models;
using Newtonsoft.Json;

namespace Moyasar.Providers
{
    public class PaymentMethodConverter : JsonConverter<PaymentMethod>
    {
        public override void WriteJson(JsonWriter writer, PaymentMethod value, Newtonsoft.Json.JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override PaymentMethod ReadJson(JsonReader reader, Type objectType, PaymentMethod existingValue, bool hasExistingValue,
            Newtonsoft.Json.JsonSerializer serializer)
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
    }
}