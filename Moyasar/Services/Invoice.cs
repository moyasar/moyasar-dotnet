using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Moyasar.Abstraction;
using Moyasar.Exceptions;
using Moyasar.Models;
using Newtonsoft.Json;

namespace Moyasar.Services
{
    /// <summary>
    /// Represents a Moyasar Invoice
    /// </summary>
    public class Invoice : Resource<Invoice>
    {
        [JsonProperty("id")]
        public string Id { get; private set; }
        
        [JsonProperty("status")]
        public string Status { get; private set; }
        
        [JsonProperty("amount")]
        public int Amount { get; set; }
        
        [JsonProperty("currency")]
        public string Currency { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("expired_at")]
        public DateTime? ExpiredAt { get; set; }
        
        [JsonProperty("logo_url")]
        public string LogoUrl { get; private set; }
        
        [JsonProperty("amount_format")]
        public string FormattedAmount { get; private set; }
        
        [JsonProperty("url")]
        public string Url { get; private set; }
        
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; private set; }
        
        [JsonProperty("updated_at")]
        public DateTime? UpdatedAt { get; private set; }
        
        [JsonProperty("payments")]
        public List<Payment> Payments { get; private set; }
        
        [JsonProperty("callback_url")]
        public string CallbackUrl { get; set; }
        
        [JsonProperty("metadata")]
        public Dictionary<string, string> Metadata { get; private set; }

        private static string GetCancelUrl(string id) => $"{ResourceUrl}/{id}/cancel";
        private static string GetCreateBulkUrl() => $"{ResourceUrl}/bulk";
        
        [JsonConstructor]
        internal Invoice() { }
        
        /// <summary>
        /// Updates the following fields
        /// <list type="bullet">
        /// <item><term>Amount</term></item>
        /// <item><term>Currency</term></item>
        /// <item><term>Description</term></item>
        /// </list>
        /// </summary>
        /// <exception cref="ApiException">Thrown when an exception occurs at server</exception>
        /// <exception cref="NetworkException">Thrown when server is unreachable</exception>
        public void Update()
        {
            Validate(true);
            
            var requestParams = new Dictionary<string, object>()
            {
                { "amount", Amount },
                { "currency", Currency },
                { "description", Description }
            };

            if (CallbackUrl != null)
            {
                requestParams.Add("callback_url", CallbackUrl);
            }

            if (ExpiredAt != null)
            {
                requestParams.Add("expired_at", ExpiredAt);
            }

            if (Metadata != null)
            {
                requestParams.Add("metadata", Metadata);
            }
            
            var response = MoyasarService.SendRequest("PUT", GetUpdateUrl(Id), requestParams);
            DeserializeInvoice(response, this);
        }

        /// <summary>
        /// Changes invoice state to canceled
        /// </summary>
        /// <exception cref="ApiException">Thrown when an exception occurs at server</exception>
        /// <exception cref="NetworkException">Thrown when server is unreachable</exception>
        public void Cancel()
        {
            var response = MoyasarService.SendRequest("PUT", GetCancelUrl(Id), null);
            DeserializeInvoice(response, this);
        }
        
        /// <summary>
        /// Throws a <code>ValidationException</code> when one or more fields are invalid
        /// </summary>
        /// <exception cref="ValidationException"></exception>
        public void Validate(bool isUpdating = false)
        {
            var errors = new List<FieldError>();
            
            if (Amount < 1) errors.Add(new FieldError()
            {
                Field = nameof(Amount),
                Error = "Amount must be a positive integer greater than 0"
            });

            if (Currency == null) errors.Add(new FieldError()
            {
                Field = nameof(Currency),
                Error = "Field is required"
            });
            
            if (Description == null) errors.Add(new FieldError()
            {
                Field = nameof(Description),
                Error = "Field is required"
            });

            if (CallbackUrl != null)
            {
                try
                {
                    new Uri(CallbackUrl);
                }
                catch
                {
                    errors.Add(new FieldError()
                    {
                        Field = nameof(CallbackUrl),
                        Error = "CallbackUrl must be a valid URI"
                    });
                }
            }

            if (!isUpdating && ExpiredAt != null && ExpiredAt.Value <= DateTime.Now)
            {
                errors.Add(new FieldError()
                {
                    Field = nameof(ExpiredAt),
                    Error = "ExpiredAt must be a future date and time"
                });
            }
            
            if (errors.Count > 0)
            {
                throw new ValidationException
                {
                    FieldErrors = errors
                };
            }
        }

        /// <summary>
        /// Creates a new invoice at Moyasar and returns an <code>Invoice</code> instance for it
        /// </summary>
        /// <param name="info">Information needed to create a new invoice</param>
        /// <returns><code>Invoice</code> instance representing an invoice created at Moyasar</returns>
        /// <exception cref="ApiException">Thrown when an exception occurs at server</exception>
        /// <exception cref="NetworkException">Thrown when server is unreachable</exception>
        public static Invoice Create(InvoiceInfo info)
        {
            info.Validate();
            var response = MoyasarService.SendRequest("POST", GetCreateUrl(), info);
            
            return DeserializeInvoice(response);
        }
        
        /// <summary>
        /// Creates a new invoice at Moyasar and returns an <code>Invoice</code> instance for it
        /// </summary>
        /// <param name="info">Information needed to create a new invoice</param>
        /// <returns><code>Invoice</code> instance representing an invoice created at Moyasar</returns>
        /// <exception cref="ApiException">Thrown when an exception occurs at server</exception>
        /// <exception cref="NetworkException">Thrown when server is unreachable</exception>
        public static List<Invoice> CreateBulk(List<InvoiceInfo> info)
        {
            if (info.Count == 0)
            {
                throw new ValidationException("At least one invoice is required.");
            }
            
            if (info.Count > 50)
            {
                throw new ValidationException("No more than 50 invoices is allowed at once.");
            }

            var data = new Dictionary<string, List<InvoiceInfo>>
            {
                {"invoices", info}
            };

            var response = MoyasarService.SendRequest("POST", GetCreateBulkUrl(), data);
            
            return JsonConvert.DeserializeObject<Dictionary<string, List<Invoice>>>(response).First().Value;
        }

        /// <summary>
        /// Get an invoice from Moyasar by Id
        /// </summary>
        /// <param name="id">Invoice Id</param>
        /// <returns><code>Invoice</code> instance representing an invoice created at Moyasar</returns>
        /// <exception cref="ApiException">Thrown when an exception occurs at server</exception>
        /// <exception cref="NetworkException">Thrown when server is unreachable</exception>
        public static Invoice Fetch(string id)
        {
            return DeserializeInvoice(MoyasarService.SendRequest("GET", GetFetchUrl(id), null));
        }

        /// <summary>
        /// Retrieve provisioned invoices at Moyasar
        /// </summary>
        /// <param name="query">Used to filter results</param>
        /// <returns>A list of invoices</returns>
        /// <exception cref="ApiException">Thrown when an exception occurs at server</exception>
        /// <exception cref="NetworkException">Thrown when server is unreachable</exception>
        public static PaginationResult<Invoice> List(SearchQuery query = null)
        {
            var responseJson = MoyasarService.SendRequest
            (
                "GET",
                GetListUrl(),
                query
            );

            return JsonConvert.DeserializeObject<PaginationResult<Invoice>>(responseJson);
        }

        internal static Invoice DeserializeInvoice(string json, Invoice obj = null)
        {
            var invoice = obj ?? new Invoice();
            JsonConvert.PopulateObject(json, invoice);
            return invoice;
        }
    }
}
