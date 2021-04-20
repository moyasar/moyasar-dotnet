using Moyasar.Abstraction;
using Newtonsoft.Json;

namespace Moyasar.Models
{
    /// <summary>
    /// Represents an stc pay payment method. Used for new payments
    /// </summary>
    public class StcPayMethod : IPaymentMethod
    {
        [JsonProperty("type")]
        public string Type { get; } = "stcpay";

        [JsonProperty("mobile")]
        public string Mobile { get; set; }
        
        [JsonProperty("reference_number")]
        public string ReferenceNumber { get; set; }

        [JsonProperty("branch")]
        public string Branch { get; set; }

        [JsonProperty("cashier")]
        public string Cashier { get; set; }

        [JsonProperty("transaction_url")]
        public string TransactionUrl { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}