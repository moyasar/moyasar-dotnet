using Newtonsoft.Json;

namespace Moyasar.Payments
{
    public class PaymentResult : PaymentResultBase
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("fee")]
        public string Fee { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("refunded")]
        public string Refunded { get; set; }

        [JsonProperty("refunded_at")]
        public string RefundedAt { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("amount_format")]
        public string AmountFormat { get; set; }

        [JsonProperty("fee_format")]
        public string FeeFormat { get; set; }

        [JsonProperty("invoice_id")]
        public string InvoiceId { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        public SourceReaultBase Source { get; set; }
    }
}
