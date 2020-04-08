using System;
using System.Collections.Generic;
using System.Text;

namespace Moyasar.Exceptions
{
    /// <summary>
    /// Represents 429 Too Many Requests HTTP Response
    /// </summary>
    public class TooManyRequestsException : ApiException
    {
        public TooManyRequestsException(string message) : base(message)
        {
        }
    }
}
