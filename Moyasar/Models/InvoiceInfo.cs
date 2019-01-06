using System;
using System.Collections.Generic;
using Moyasar.Exceptions;

namespace Moyasar.Models
{
    public class InvoiceInfo
    {
        private const string AmountField = "amount";
        private const string CurrencyField = "currency";
        private const string DescriptionField = "description";
        private const string CallbackUrlField = "callback_url";
        private const string ExpiredAtField = "expired_at";

        public int Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string CallbackUrl { get; set; }
        public DateTime? ExpiredAt { get; set; }

        public Dictionary<string, object> ToDictionary()
        {
            var dict = new Dictionary<string, object>
            {
                { AmountField, Amount },
                { CurrencyField, Currency },
                { DescriptionField, Description }
            };
            
            if(CallbackUrl != null) dict.Add(CallbackUrlField, CallbackUrl);
            if(ExpiredAt != null) dict.Add(ExpiredAtField, ExpiredAt.Value.ToString("O"));

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