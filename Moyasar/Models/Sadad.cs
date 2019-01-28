using Moyasar.Abstraction;
using Newtonsoft.Json;

namespace Moyasar.Models
{
    /// <summary>
    /// Represents Sadad payment method for a payment
    /// </summary>
    public class Sadad : IPaymentMethod
    {
        private const string UserNameField = "username";
        private const string ErrorCodeField = "error_code";
        private const string MessageField = "message";
        private const string TransactionIdField = "transaction_id";
        private const string TransactionUrlField = "transaction_url";
        
        [JsonProperty(UserNameField)]
        public string UserName { get; set; }

        [JsonProperty(ErrorCodeField)]
        public string ErrorCode { get; set; }
        
        [JsonProperty(MessageField)]
        public string Message { get; set; }
        
        [JsonProperty(TransactionIdField)]
        public string TransactionId { get; set; }
        
        [JsonProperty(TransactionUrlField)]
        public string TransactionUrl { get; set; }
    }
}