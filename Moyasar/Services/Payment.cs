using System;
using System.Collections.Generic;
using System.Linq;
using Moyasar.Abstraction;
using Moyasar.Models;
using Newtonsoft.Json;

namespace Moyasar.Services
{
    public class Payment : Resource
    {
        // Field Names
        private const string IdField = "id";
        private const string StatusField = "status";
        private const string AmountField = "amount";
        private const string FeeField = "fee";
        private const string RefundedAmountField = "refunded";
        private const string RefundedAtField = "refunded_at";
        private const string FormattedAmountField = "amount_format";
        private const string FormattedFeeField = "fee_format";
        private const string FormattedRefundedAmountField = "refunded_format";
        private const string CurrencyField = "currency";
        private const string InvoiceIdField = "invoice_id";
        private const string IpField = "ip";
        private const string CallbackUrlField = "callback_url";
        private const string CreatedAtField = "created_at";
        private const string UpdatedAtField = "updated_at";
        private const string SourceField = "source";
        private const string DescriptionField = "description";
        
        [JsonProperty(IdField)]
        public string Id { get; private set; }
        
        [JsonProperty(StatusField)]
        public string Status { get; private set; }
        
        [JsonProperty(AmountField)]
        public int Amount { get; private set; }
        
        [JsonProperty(FeeField)]
        public int Fee { get; private set; }
        
        [JsonProperty(RefundedAmountField)]
        public int RefundedAmount { get; private set; }
        
        [JsonProperty(RefundedAtField)]
        public DateTime? RefundedAt { get; private set; }
        
        [JsonProperty(FormattedAmountField)]
        public string FormattedAmount { get; private set; }
        
        [JsonProperty(FormattedFeeField)]
        public string FormattedFee { get; private set; }
        
        [JsonProperty(FormattedRefundedAmountField)]
        public string FormattedRefundedAmount { get; private set; }
        
        [JsonProperty(CurrencyField)]
        public string Currency { get; private set; }
        
        [JsonProperty(InvoiceIdField)]
        public string InvoiceId { get; private set; }
        
        [JsonProperty(IpField)]
        public string Ip { get; private set; }
        
        [JsonProperty(CallbackUrlField)]
        public string CallbackUrl { get; private set; }
        
        [JsonProperty(CreatedAtField)]
        public DateTime? CreatedAt { get; private set; }
        
        [JsonProperty(UpdatedAtField)]
        public DateTime? UpdatedAt { get; private set; }
        
        [JsonProperty(SourceField)]
        public PaymentMethod Source { get; private set; }
        
        [JsonProperty(DescriptionField)]
        public string Description { get; set; }
        
        internal Payment() { }
        
        protected static string GetRefundUrl(string id) => $"{ResourceUrl}/{id}/refund";

        public void Update()
        {
            MoyasarService.SendRequest("PUT", GetUpdateUrl(Id), new Dictionary<string, object>()
            {
                { DescriptionField, Description }
            });           
        }

        public Payment Refund(int? amount = null)
        {
            var reqParams = amount != null ? new Dictionary<string, Object>()
            {
                { AmountField, amount.Value }
            } : null;

            return DeserializePayment
            (
                MoyasarService.SendRequest("POST", GetRefundUrl(Id), reqParams), 
                this
            );
        }
        
        public static Payment Create(PaymentInfo info)
        {
            info.Validate();
            var requestParams = info.ToDictionary();
            var response = MoyasarService.SendRequest("POST", GetCreateUrl(), requestParams);
            return DeserializePayment(response);
        }

        public static Payment Fetch(string id)
        {
            return DeserializePayment(MoyasarService.SendRequest(
                "GET",
                GetFetchUrl(id),
                null
            ));
        }

        public static List<Payment> List(dynamic queryParams = null)
        {
            string result = MoyasarService.SendRequest(
                "GET",
                GetListUrl(),
                queryParams != null ? queryParams as Dictionary<string, object> : null
            );

            var paymentObjects = MoyasarService.Serializer.Deserialize<List<object>>(result);
            return paymentObjects.Select(po => DeserializePayment(MoyasarService.Serializer.Serialize(po))).ToList();
        }

        internal static Payment DeserializePayment(string json, Payment obj = null)
        {
            var payment = obj ?? new Payment();
            MoyasarService.Serializer.PopulateObject(json, payment);
            return payment;
        }
    }
}