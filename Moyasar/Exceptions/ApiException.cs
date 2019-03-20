using System;
using System.Collections.Generic;
using System.Text;

namespace Moyasar.Exceptions
{
    /// <summary>
    /// Represents a server side error (400 - 599)
    /// </summary>
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

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append($"Status Code: {HttpStatusCode.ToString()}\n");
            builder.Append($"Error Type: {Type}\n");
            builder.Append($"Message: {Message}\n");

            if (ErrorsDictionary != null)
            {
                foreach (var error in ErrorsDictionary)
                {
                    builder.Append($"Error [{error.Key}]:\n");
                    foreach (var s in error.Value)
                    {
                        builder.Append($"\t- {s}\n");
                    }
                }
            }
            
            builder.Append($"Payload:\n{ResponsePayload}");
            
            return builder.ToString();
        }
    }
}