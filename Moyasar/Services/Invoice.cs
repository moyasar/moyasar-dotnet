using System;
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
        private const string IdField = "id";
        private const string StatusField = "status";
        private const string AmountField = "amount";
        private const string CurrencyField = "currency";
        private const string DescriptionField = "description";
        private const string ExpiredAtField = "expired_at";
        private const string LogoUrlField = "logo_url";
        private const string AmountFormatField = "amount_format";
        private const string UrlField = "url";
        private const string CreatedAtField = "created_at";
        private const string UpdatedAtField = "updated_at";
        private const string PaymentsField = "payments";
        private const string CallbackUrlField = "callback_url";

        [JsonProperty(IdField)]
        public string Id { get; private set; }
        
        [JsonProperty(StatusField)]
        public string Status { get; private set; }
        
        [JsonProperty(AmountField)]
        public int Amount { get; set; }
        
        [JsonProperty(CurrencyField)]
        public string Currency { get; set; }
        
        [JsonProperty(DescriptionField)]
        public string Description { get; set; }
        
        [JsonProperty(ExpiredAtField)]
        public DateTime? ExpiredAt { get; set; }
        
        [JsonProperty(LogoUrlField)]
        public string LogoUrl { get; private set; }
        
        [JsonProperty(AmountFormatField)]
        public string FormattedAmount { get; private set; }
        
        [JsonProperty(UrlField)]
        public string Url { get; private set; }
        
        [JsonProperty(CreatedAtField)]
        public DateTime? CreatedAt { get; private set; }
        
        [JsonProperty(UpdatedAtField)]
        public DateTime? UpdatedAt { get; private set; }
        
        [JsonProperty(PaymentsField)]
        public List<Payment> Payments { get; private set; }
        
        [JsonProperty(CallbackUrlField)]
        public string CallbackUrl { get; set; }

        private static string GetCancelUrl(string id) => $"{ResourceUrl}/{id}/cancel";
        
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
            Validate();
            
            var requestParams = new Dictionary<string, object>()
            {
                { AmountField, Amount },
                { CurrencyField, Currency },
                { DescriptionField, Description }
            };
            
            if(CallbackUrl != null) requestParams.Add(CallbackUrlField, CallbackUrl);
            if(ExpiredAt != null) requestParams.Add(ExpiredAtField, ExpiredAt);
            
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
        public void Validate()
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

            if (ExpiredAt != null && ExpiredAt.Value <= DateTime.Now)
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
            var requestParams = info.ToDictionary();
            var response = MoyasarService.SendRequest("POST", GetCreateUrl(), requestParams);
            return DeserializeInvoice(response);
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

            var invoiceObjects =
                MoyasarService.Serializer.Deserialize<List<object>>(
                    MoyasarService.Serializer.Serialize((object)response.invoices));
            var invoicesList = invoiceObjects
                .Select(i => DeserializeInvoice(MoyasarService.Serializer.Serialize(i))).ToList();

            var pagination = new PaginationResult<Invoice>
            {
                Paginator = page =>
                {
                    var q = query?.Clone() ?? new SearchQuery();
                    q.Page = page;
                    return List(q);
                },
                Items = invoicesList
            };
            
            if (metaJson != null)
            {
                MoyasarService.Serializer.PopulateObject(metaJson, pagination);
            }
            
            return pagination;
        }

        internal static Invoice DeserializeInvoice(string json, Invoice obj = null)
        {
            var invoice = obj ?? new Invoice();
            MoyasarService.Serializer.PopulateObject(json, invoice);
            return invoice;
        }
    }
}