using Moyasar.Abstraction;
using Newtonsoft.Json;

namespace Moyasar.Models
{
    public class SadadAccount : PaymentMethod
    {
        private const string UserNameFieldName = "username";
        
        [JsonProperty(UserNameFieldName)]
        public string UserName { get; set; }
    }
}