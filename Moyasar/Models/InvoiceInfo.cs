using System;
using System.Collections.Generic;
using Moyasar.Exceptions;
using Newtonsoft.Json;

namespace Moyasar.Models
{
    /// <summary>
    /// Model that contains information needed to create a new invoice
    /// </summary>
    public class InvoiceInfo
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }
        
        [JsonProperty("currency")]
        public string Currency { get; set; } = "SAR";
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("callback_url", NullValueHandling = NullValueHandling.Ignore)]
        public string CallbackUrl { get; set; }
        
        [JsonProperty("expired_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? ExpiredAt { get; set; }
        
        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Metadata { get; set; }

        public void Validate()
        {
            var errors = new List<FieldError>();
            
            if (Amount < 1) errors.Add(new FieldError()
            {
                Field = nameof(Amount),
                Error = "Amount must be a positive integer greater than 0"
            });

            if (string.IsNullOrEmpty(Currency)) errors.Add(new FieldError()
            {
                Field = nameof(Currency),
                Error = "Field is required"
            });
            
            if (string.IsNullOrEmpty(Description)) errors.Add(new FieldError()
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
    }
}