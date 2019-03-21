using System;
using System.Collections.Generic;
using System.Linq;
using Moyasar.Abstraction;
using Moyasar.Exceptions;
using Moyasar.Models;
using Newtonsoft.Json;

namespace Moyasar.Services
{
    public class Payment : Resource<Payment>
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
        public IPaymentMethod Source { get; private set; }
        
        [JsonProperty(DescriptionField)]
        public string Description { get; set; }
        
        internal Payment() { }
        
        protected static string GetRefundUrl(string id) => $"{ResourceUrl}/{id}/refund";

        /// <summary>
        /// Updates the following fields
        /// <list type="bullet">
        /// <item><term>Description</term></item>
        /// </list>
        /// </summary>
        /// <exception cref="ApiException">Thrown when an exception occurs at server</exception>
        /// <exception cref="NetworkException">Thrown when server is unreachable</exception>
        public void Update()
        {
            MoyasarService.SendRequest("PUT", GetUpdateUrl(Id), new Dictionary<string, object>()
            {
                { DescriptionField, Description }
            });           
        }

        /// <summary>
        /// Refunds the current payment
        /// </summary>
        /// <param name="amount">Optional amount to refund, should be equal to or less than current amount</param>
        /// <returns>An updated <code>Payment</code> instance</returns>
        /// <exception cref="ApiException">Thrown when an exception occurs at server</exception>
        /// <exception cref="NetworkException">Thrown when server is unreachable</exception>
        public Payment Refund(int? amount = null)
        {
            if (amount > this.Amount)
            {
                throw new ValidationException("Payment must be equal to or less than current amount");
            }
            
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

        /// <summary>
        /// Get an payment from Moyasar by Id
        /// </summary>
        /// <param name="id">Payment Id</param>
        /// <returns><code>Payment</code> instance representing an payment created at Moyasar</returns>
        /// <exception cref="ApiException">Thrown when an exception occurs at server</exception>
        /// <exception cref="NetworkException">Thrown when server is unreachable</exception>
        public static Payment Fetch(string id)
        {
            return DeserializePayment(MoyasarService.SendRequest(
                "GET",
                GetFetchUrl(id),
                null
            ));
        }

        /// <summary>
        /// Retrieve provisioned payments at Moyasar
        /// </summary>
        /// <param name="query">Used to filter results</param>
        /// <returns>A list of payments</returns>
        /// <exception cref="ApiException">Thrown when an exception occurs at server</exception>
        /// <exception cref="NetworkException">Thrown when server is unreachable</exception>
        public static PaginationResult<Payment> List(SearchQuery query = null)
        {
            var responseJson = MoyasarService.SendRequest(
                "GET",
                GetListUrl(),
                query?.ToDictionary()
            );

            dynamic response = MoyasarService.Serializer.Deserialize<object>(responseJson);

            string metaJson = null;
            try
            {
                metaJson = MoyasarService.Serializer.Serialize((object)response.meta);
            }
            catch
            {
                // ignored
            }

            var paymentObjects =
                MoyasarService.Serializer.Deserialize<List<object>>(
                    MoyasarService.Serializer.Serialize((object)response.payments));
            var paymentsList = paymentObjects
                .Select(po => DeserializePayment(MoyasarService.Serializer.Serialize(po))).ToList();

            var pagination = new PaginationResult<Payment>
            {
                Paginator = page =>
                {
                    var q = query?.Clone() ?? new SearchQuery();
                    q.Page = page;
                    return List(q);
                },
                Items = paymentsList
            };
            
            if (metaJson != null)
            {
                MoyasarService.Serializer.PopulateObject(metaJson, pagination);
            }
            
            return pagination;
        }

        internal static Payment DeserializePayment(string json, Payment obj = null)
        {
            var payment = obj ?? new Payment();
            MoyasarService.Serializer.PopulateObject(json, payment);
            return payment;
        }
    }
}