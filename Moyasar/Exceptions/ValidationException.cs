using System;
using System.Collections.Generic;

namespace Moyasar.Exceptions
{
    /// <summary>
    /// Thrown when supplied information are incorrect
    /// </summary>
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