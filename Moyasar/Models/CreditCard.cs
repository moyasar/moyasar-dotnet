using Moyasar.Abstraction;
using Newtonsoft.Json;

namespace Moyasar.Models
{
    /// <summary>
    /// Represents credit card payment method for a payment
    /// </summary>
    public class CreditCard : IPaymentMethod
    {
        [JsonProperty("type")]
        public string Type { get; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("transaction_url")]
        public string TransactionUrl { get; set; }

        [JsonProperty("gateway_id")]
        public string GatewayId { get; set; }

        [JsonProperty("reference_number")]
        public string ReferenceNumber { get; set; }
    }
}
