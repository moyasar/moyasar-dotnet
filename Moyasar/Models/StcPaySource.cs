using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Moyasar.Abstraction;
using Moyasar.Exceptions;
using Newtonsoft.Json;

namespace Moyasar.Models
{
    public class StcPaySource : IPaymentSource
    {
        [JsonProperty("type")]
        public string Type { get; } = "stcpay";

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("branch", NullValueHandling = NullValueHandling.Ignore)]
        public string Branch { get; set; }

        [JsonProperty("cashier", NullValueHandling = NullValueHandling.Ignore)]
        public string Cashier { get; set; }
        
        public void Validate()
        {
            var errors = new List<FieldError>();

            if (String.IsNullOrEmpty(Mobile))
            {
                errors.Add(new FieldError
                {
                    Field = nameof(Mobile),
                    Error = "Mobile number is required."
                });
            }
            
            if (!Regex.IsMatch(Mobile, @"^05[503649187][0-9]{7}$"))
            {
                errors.Add(new FieldError
                {
                    Field = nameof(Mobile),
                    Error = "Number is not a valid Saudi mobile."
                });
            }

            if (errors.Any())
            {
                throw new ValidationException("stc pay information are incorrect")
                {
                    FieldErrors = errors
                };
            }
        }
    }
}
