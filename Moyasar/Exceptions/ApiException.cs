using System;
using System.Collections.Generic;

namespace Moyasar.Exceptions
{
    public class ApiException : Exception
    {
        public int HttpStatusCode { get; set; }
        public string ResponsePayload { get; set; }

        public string Type { get; set; }
        public string Errors { get; set; }
        public Dictionary<string, List<string>> ErrorsDictionary { get; set; }
        
        public ApiException(string message) : base(message)
        {
        }
    }
}