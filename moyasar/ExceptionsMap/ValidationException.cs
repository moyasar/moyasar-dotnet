using System;

namespace moyasar.ExceptionsMap
{
    public class MoyasarValidationException : Exception
    {
        public string ErrorCode { get; set; }
        public MoyasarValidationException(string messages) : base(messages)
        {
        }
    }
}
