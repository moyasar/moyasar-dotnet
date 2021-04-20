using System;
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
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            dynamic dict = serializer.Deserialize(reader);
            string json = JsonConvert.SerializeObject(dict);
            
            string type;
            try
            {
                type = dict.type;
            }
            catch
            {
                return null;
            }

            return type.ToLower() switch
            {
                "creditcard" => JsonConvert.DeserializeObject<CreditCard>(json),
                "applepay" => JsonConvert.DeserializeObject<ApplePayMethod>(json),
                "stcpay" => JsonConvert.DeserializeObject<StcPayMethod>(json),
                _ => null
            };
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IPaymentMethod) == objectType;
        }
    }
}
