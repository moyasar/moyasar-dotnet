using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Moyasar.Abstraction;
using Moyasar.Exceptions;
using Moyasar.Helpers;

namespace Moyasar.Models
{
    /// <summary>
    /// Model that contains credit card information needed to create a new payment
    /// </summary>
    [DataContract]
    public class CreditCardSource : IPaymentSource
    {
        public const string TypeFieldName = "type";
        public const string NameFieldName = "name";
        public const string NumberFieldName = "number";
        public const string CvcFieldName = "cvc";
        public const string MonthFieldName = "month";
        public const string YearFieldName = "year";
        public const string _3DsFieldName = "3ds";
        
        public string Name { get; set; }
        public string Number { get; set; }        
        public int Cvc { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        
        // By default TRUE according to documentation
        public bool _3Ds { get; set; } = true;

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>()
            {
                { TypeFieldName, "creditcard" },
                { NameFieldName, Name },
                { NumberFieldName, Number },
                { CvcFieldName, Cvc },
                { MonthFieldName, Month },
                { YearFieldName, Year },
                { _3DsFieldName, _3Ds }
            };
        }
        
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