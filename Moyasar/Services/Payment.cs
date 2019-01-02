using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Moyasar.Core;
using Moyasar.Extensions;
using Moyasar.Services.Abstraction;
using Moyasar.Services.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Moyasar.Services
{
    public class Payment : Resource
    {
        // Field Names
        private const string IdFieldName = "id";
        private const string StatusFieldName = "status";
        private const string AmountFieldName = "amount";
        private const string FeeFieldName = "fee";
        private const string RefundedAmountFieldName = "refunded";
        private const string RefundedAtFieldName = "refunded_at";
        private const string FormattedAmountFieldName = "amount_format";
        private const string FormattedFeeFieldName = "fee_format";
        private const string FormattedRefundedAmountFieldName = "refunded_format";
        private const string CurrencyFieldName = "currency";
        private const string InvoiceIdFieldName = "invoice_id";
        private const string IpFieldName = "ip";
        private const string CallbackUrlFieldName = "callback_url";
        private const string CreatedAtFieldName = "created_at";
        private const string UpdatedAtFieldName = "updated_at";
        private const string SourceFieldName = "source";
        private const string DescriptionFieldName = "description";
        
        [JsonProperty(IdFieldName)]
        public string Id { get; private set; }
        
        [JsonProperty(StatusFieldName)]
        public string Status { get; private set; }
        
        [JsonProperty(AmountFieldName)]
        public int Amount { get; private set; }
        
        [JsonProperty(FeeFieldName)]
        public int Fee { get; private set; }
        
        [JsonProperty(RefundedAmountFieldName)]
        public int RefundedAmount { get; private set; }
        
        [JsonProperty(RefundedAtFieldName)]
        public DateTime? RefundedAt { get; private set; }
        
        [JsonProperty(FormattedAmountFieldName)]
        public string FormattedAmount { get; private set; }
        
        [JsonProperty(FormattedFeeFieldName)]
        public string FormattedFee { get; private set; }
        
        [JsonProperty(FormattedRefundedAmountFieldName)]
        public string FormattedRefundedAmount { get; private set; }
        
        [JsonProperty(CurrencyFieldName)]
        public string Currency { get; private set; }
        
        [JsonProperty(InvoiceIdFieldName)]
        public string InvoiceId { get; private set; }
        
        [JsonProperty(IpFieldName)]
        public string Ip { get; private set; }
        
        [JsonProperty(CallbackUrlFieldName)]
        public string CallbackUrl { get; private set; }
        
        [JsonProperty(CreatedAtFieldName)]
        public DateTime? CreatedAt { get; private set; }
        
        [JsonProperty(UpdatedAtFieldName)]
        public DateTime? UpdatedAt { get; private set; }
        
        [JsonProperty(SourceFieldName)]
        public PaymentMethod Source { get; private set; }
        
        [JsonProperty(DescriptionFieldName)]
        public string Description { get; set; }
        
        private Payment() { }
        
        protected static string GetRefundUrl(string id) => $"{ResourceUrl}/{id}/refund";

        public void Update()
        {
            Moyasar.SendRequest("PUT", GetUpdateUrl(Id), new Dictionary<string, object>()
            {
                { nameof(Description).ToLower(), Description }
            });           
        }

        public Payment Refund(int? amount = null)
        {
            var reqParams = amount != null ? new Dictionary<string, Object>()
            {
                { AmountFieldName, amount.Value }
            } : null;

            return DeserializePayment
            (
                Moyasar.SendRequest("POST", GetRefundUrl(Id), reqParams), 
                this
            );
        }
        
        public static Payment Create(PaymentInfo info)
        {
            info.Validate();
            var requestParams = info.ToDictionary();
            var response = Moyasar.SendRequest("POST", GetCreateUrl(), requestParams);
            return DeserializePayment(response);
        }

        public static Payment Fetch(string id)
        {
            return DeserializePayment(Moyasar.SendRequest(
                "GET",
                GetFetchUrl(id),
                null
            ));
        }

        public static List<Payment> List(dynamic queryParams = null)
        {
            string result = Moyasar.SendRequest(
                "GET",
                GetListUrl(),
                queryParams != null ? queryParams as Dictionary<string, object> : null
            );

            var paymentObjects = Moyasar.Serializer.Deserialize<List<object>>(result);
            return paymentObjects.Select(po => DeserializePayment(Moyasar.Serializer.Serialize(po))).ToList();
        }

        private static Payment DeserializePayment(string json, Payment obj = null)
        {
            var payment = obj ?? new Payment();
            Moyasar.Serializer.PopulateObject(json, payment);
            return payment;
        }
    }
}