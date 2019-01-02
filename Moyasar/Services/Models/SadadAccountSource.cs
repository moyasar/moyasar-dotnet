using System;
using System.Collections.Generic;
using System.Linq;
using Moyasar.Core;
using Moyasar.Exceptions;
using Moyasar.Services.Abstraction;

namespace Moyasar.Services.Models
{
    public class SadadAccountSource : IPaymentSource
    {
        public const string TypeFieldName = "type";
        public const string UserNameFieldName = "username";
        
        public string UserName { get; set; }

        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>()
            {
                { TypeFieldName, "sadad" },
                { UserNameFieldName, UserName }
            };
        }

        public void Validate()
        {
            var errors = new List<FieldError>();
            
            if (String.IsNullOrEmpty(UserName)) errors.Add(new FieldError()
            {
                Field = nameof(UserName),
                Error = "User name is required"
            });

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