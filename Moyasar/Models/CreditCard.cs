using Moyasar.Abstraction;
using Newtonsoft.Json;

namespace Moyasar.Models
{
    /// <summary>
    /// Represents credit card payment method for a payment
    /// </summary>
    public class CreditCard : IPaymentMethod
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