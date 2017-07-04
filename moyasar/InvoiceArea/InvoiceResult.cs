using Newtonsoft.Json;

namespace moyasar.InvoiceArea
{
    public class InvoiceResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("logo_url")]
        public string LogoUrl { get; set; }

        [JsonProperty("amount_format")]
        public string AmountFormat { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }


        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }
}
