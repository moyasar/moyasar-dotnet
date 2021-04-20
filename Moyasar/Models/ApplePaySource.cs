using Moyasar.Abstraction;
using Moyasar.Exceptions;
using Newtonsoft.Json;

namespace Moyasar.Models
{
    public class ApplePaySource : IPaymentSource
    {
        [JsonProperty("type")]
        public string Type { get; } = "applepay";

        [JsonProperty("token")]
        public string Token { get; set; }

        public void SetTokenFromObject(object token)
        {
            Token = JsonConvert.SerializeObject(token);
        }
        
        public void Validate()
        {
            try
            {
                JsonConvert.DeserializeObject(Token);
            }
            catch
            {
                throw new ValidationException("Apple Pay token is not a valid JSON string");
            }
        }
    }
}
