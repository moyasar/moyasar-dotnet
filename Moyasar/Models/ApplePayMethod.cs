using System;
using Moyasar.Abstraction;
using Newtonsoft.Json;

namespace Moyasar.Models
{
    /// <summary>
    /// Represents an Apple Pay payment method. Used for new payments
    /// </summary>
    public class ApplePayMethod : IPaymentMethod
    {
        [JsonProperty("type")]
        public string Type { get; } = "applepay";

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
