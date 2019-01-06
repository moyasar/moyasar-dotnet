using System.Collections.Generic;
using Moyasar.Abstraction;
using Newtonsoft.Json;

namespace Moyasar.Providers
{
    public class JsonSerializer : ISerializer
    {
        private JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            Converters = new List<JsonConverter>()
            {
                new PaymentMethodConverter()
            }
        };
        
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public TType Deserialize<TType>(string json) where TType : class, new()
        {
            return JsonConvert.DeserializeObject<TType>(json, settings);
        }

        public void PopulateObject(string json, object obj)
        {
            JsonConvert.PopulateObject(json, obj, settings);
        }
    }
}