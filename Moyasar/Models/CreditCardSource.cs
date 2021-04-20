using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Moyasar.Abstraction;
using Moyasar.Exceptions;
using Moyasar.Helpers;
using Newtonsoft.Json;

namespace Moyasar.Models
{
    /// <summary>
    /// Model that contains credit card information needed to create a new payment
    /// </summary>
    [DataContract]
    public class CreditCardSource : IPaymentSource
    {
        [JsonProperty("type")]
        public string Type { get; } = "creditcard";
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("number")]
        public string Number { get; set; }
        
        [JsonProperty("cvc")]
        public int Cvc { get; set; }
        
        [JsonProperty("month")]
        public int Month { get; set; }
        
        [JsonProperty("year")]
        public int Year { get; set; }
        
        // By default TRUE according to documentation
        [JsonProperty("3ds", NullValueHandling = NullValueHandling.Ignore)]
        public bool _3Ds { get; set; } = true;

        [JsonProperty("manual", NullValueHandling = NullValueHandling.Ignore)]
        public bool Manual { get; set; } = false;

        public void Validate()
        {
            var errors = new List<FieldError>();

            if (String.IsNullOrEmpty(Name)) errors.Add(new FieldError()
            {
                Field = nameof(Name),
                Error = "Credit card holder name is required"
            });
            
            if(CreditCardHelper.GetCreditCardType(Number) == null) errors.Add(new FieldError()
            {
                Field = nameof(Number),
                Error = $"The number {Number} is not a valid credit card number"
            });
            
            if(!(Cvc >= 1 && Cvc <= 999)) errors.Add(new FieldError()
            {
                Field = nameof(Month),
                Error = "Cvc must be a three digit number"
            });

            if(!(Month >= 1 && Month <= 12)) errors.Add(new FieldError()
            {
                Field = nameof(Month),
                Error = "Month must be an integer between 1 and 12"
            });
            
            if(Year < 1) errors.Add(new FieldError()
            {
                Field = nameof(Month),
                Error = "Year must be a positive integer greater than or equals to 1"
            });

            if (errors.Any())
            {
                throw new ValidationException("Credit card information is incorrect")
                {
                    FieldErrors = errors
                };
            }
        }
    }
}