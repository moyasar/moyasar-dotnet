using Moyasar.Services.Abstraction;
using Newtonsoft.Json;

namespace Moyasar.Services.Models
{
    public class CreditCard : PaymentMethod
    {
        private const string CompanyFieldName = "company";
        private const string NameFieldName = "name";
        private const string NumberFieldName = "number";
        private const string MessageFieldName = "message";
        private const string TransactionUrlFieldName = "transaction_url";
        
        [JsonProperty(CompanyFieldName)]
        public string Company { get; set; }
        
        [JsonProperty(NameFieldName)]
        public string Name { get; set; }
        
        [JsonProperty(NumberFieldName)]
        public string Number { get; set; }
        
        [JsonProperty(MessageFieldName)]
        public string Message { get; set; }
        
        [JsonProperty(TransactionUrlFieldName)]
        public string TransactionUrl { get; set; }        
    }
}