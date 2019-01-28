using System;
using System.Collections.Generic;
using System.Linq;
using Moyasar.Abstraction;
using Moyasar.Exceptions;

namespace Moyasar.Models
{
    /// <summary>
    /// Model that contains information needed to create a new payment
    /// </summary>
    public class PaymentInfo
    {
        public const string AmountFieldName = "amount";
        public const string CurrencyFieldName = "currency";
        public const string DescriptionFieldName = "description";
        public const string SourceFieldName = "source";
        public const string CallbackFieldName = "callback_url";
        
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public IPaymentSource Source { get; set; }
        public string CallbackUrl { get; set; }

        public Dictionary<string, object> ToDictionary()
        {
            var dict = new Dictionary<string, object>()
            {
                { AmountFieldName, Amount },
                { CurrencyFieldName, Currency },
                { SourceFieldName, Source.ToDictionary() },
                { CallbackFieldName, CallbackUrl }
            };

            if (!string.IsNullOrEmpty(Description))
            {
                dict.Add(DescriptionFieldName, Description);
            }
            
            return dict;
        }

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