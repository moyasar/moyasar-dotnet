using System;
using System.Collections.Generic;
using Moyasar.Core;

namespace Moyasar.Exceptions
{
    public class ValidationException : Exception
    {
        public List<FieldError> FieldErrors { get; set; }
        
        public ValidationException()
        {
        }

        public ValidationException(string message) : base(message)
        {
        }

        public ValidationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}