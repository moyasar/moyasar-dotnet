using Moyasar.Services.Abstraction;
using Newtonsoft.Json;

namespace Moyasar.Services.Models
{
    public class SadadAccount : PaymentMethod
    {
        private const string UserNameFieldName = "username";
        
        [JsonProperty(UserNameFieldName)]
        public string UserName { get; set; }
    }
}