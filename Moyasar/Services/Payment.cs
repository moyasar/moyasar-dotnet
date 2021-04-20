using System;
using System.Collections.Generic;
using Moyasar.Abstraction;
using Moyasar.Exceptions;
using Moyasar.Models;
using Moyasar.Providers;
using Newtonsoft.Json;

namespace Moyasar.Services
{
    public class Payment : Resource<Payment>
    {
        [JsonProperty("id")]
        public string Id { get; private set; }
        
        [JsonProperty("status")]
        public string Status { get; private set; }
        
        [JsonProperty("amount")]
        public int Amount { get; private set; }
        
        [JsonProperty("fee")]
        public int Fee { get; private set; }
        
        [JsonProperty("refunded")]
        public int RefundedAmount { get; private set; }
        
        [JsonProperty("refunded_at")]
        public DateTime? RefundedAt { get; private set; }
        
        [JsonProperty("captured")]
        public int CapturedAmount { get; private set; }
        
        [JsonProperty("captured_at")]
        public DateTime? CapturedAt { get; private set; }
        
        [JsonProperty("voided_at")]
        public DateTime? VoidedAt { get; private set; }
        
        [JsonProperty("amount_format")]
        public string FormattedAmount { get; private set; }
        
        [JsonProperty("fee_format")]
        public string FormattedFee { get; private set; }
        
        [JsonProperty("refunded_format")]
        public string FormattedRefundedAmount { get; private set; }
        
        [JsonProperty("captured_format")]
        public string FormattedCapturedAmount { get; private set; }
        
        [JsonProperty("currency")]
        public string Currency { get; private set; }
        
        [JsonProperty("invoice_id")]
        public string InvoiceId { get; private set; }
        
        [JsonProperty("ip")]
        public string Ip { get; private set; }
        
        [JsonProperty("callback_url")]
        public string CallbackUrl { get; private set; }
        
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; private set; }
        
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; private set; }

        [JsonProperty("metadata")]
        public Dictionary<string, string> Metadata { get; private set; }
        
        [JsonProperty("source")]
        [JsonConverter(typeof(PaymentMethodConverter))]
        public IPaymentMethod Source { get; private set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        internal Payment() { }
        
        protected static string GetRefundUrl(string id) => $"{ResourceUrl}/{id}/refund";
        protected static string GetCaptureUrl(string id) => $"{ResourceUrl}/{id}/capture";
        protected static string GetVoidUrl(string id) => $"{ResourceUrl}/{id}/void";

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
                { "description", Description }
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
                throw new ValidationException("Amount to be refunded must be equal to or less than current amount");
            }
            
            var reqParams = amount != null ? new Dictionary<string, object>
            {
                { "amount", amount.Value }
            } : null;

            return DeserializePayment
            (
                MoyasarService.SendRequest("POST", GetRefundUrl(Id), reqParams), 
                this
            );
        }
        
        /// <summary>
        /// Captures the current authorized payment
        /// </summary>
        /// <param name="amount">Optional amount to capture, should be equal to or less than current amount</param>
        /// <returns>An updated <code>Payment</code> instance</returns>
        /// <exception cref="ApiException">Thrown when an exception occurs at server</exception>
        /// <exception cref="NetworkException">Thrown when server is unreachable</exception>
        public Payment Capture(int? amount = null)
        {
            if (amount > this.Amount)
            {
                throw new ValidationException("Amount to be refunded must be equal to or less than current amount");
            }

            if (Status != "authorized")
            {
                throw new ValidationException("Payment is not in authorized status.");
            }
            
            var reqParams = amount != null ? new Dictionary<string, object>
            {
                { "amount", amount.Value }
            } : null;

            return DeserializePayment
            (
                MoyasarService.SendRequest("POST", GetCaptureUrl(Id), reqParams), 
                this
            );
        }
        
        /// <summary>
        /// Voids the current authorized payment
        /// </summary>
        /// <returns>An updated <code>Payment</code> instance</returns>
        /// <exception cref="ApiException">Thrown when an exception occurs at server</exception>
        /// <exception cref="NetworkException">Thrown when server is unreachable</exception>
        public Payment Void()
        {
            if (Status != "authorized")
            {
                throw new ValidationException("Payment is not in authorized status.");
            }
            
            return DeserializePayment
            (
                MoyasarService.SendRequest("POST", GetVoidUrl(Id), null), 
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
                query
            );

            return JsonConvert.DeserializeObject<PaginationResult<Payment>>(responseJson);
        }

        internal static Payment DeserializePayment(string json, Payment obj = null)
        {
            var payment = obj ?? new Payment();
            JsonConvert.PopulateObject(json, payment);
            return payment;
        }
    }
}