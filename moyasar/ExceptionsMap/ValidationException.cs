using System;

namespace Moyasar.ExceptionsMap
{
    [Serializable]
    public class MoyasarValidationException : Exception
    {
        public string ErrorCode { get; set; }
        public MoyasarValidationException(string messages) : base(messages) { }
    }
}
