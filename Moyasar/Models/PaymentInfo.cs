using System;
using System.Collections.Generic;
using System.Linq;
using Moyasar.Abstraction;
using Moyasar.Exceptions;
using Newtonsoft.Json;

namespace Moyasar.Models
{
    /// <summary>
    /// Model that contains information needed to create a new payment
    /// </summary>
    public class PaymentInfo
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }
        
        [JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        public string Currency { get; set; } = "SAR";
        
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        
        [JsonProperty("source")]
        public IPaymentSource Source { get; set; }
        
        [JsonProperty("callback_url")]
        public string CallbackUrl { get; set; }
        
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

            if (Source == null)
            {
                errors.Add(new FieldError()
                {
                    Field = nameof(Source),
                    Error = "A source of payment must be provided"
                });
            }
            else
            {
                Source.Validate();
            }

            if (Source is CreditCardSource)
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
                        Error = $"The value ({CallbackUrl}) is not a valid url"
                    });
                }
            }

            if (errors.Any())
            {
                throw new ValidationException()
                {
                    FieldErrors = errors
                };
            }
        }
    }
}