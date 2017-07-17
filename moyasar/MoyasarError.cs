using System;

namespace Moyasar
{
    public class MoyasarException : Exception
    {
        public MoyasarException(string message, string type, string errors) : base(message)
        {
            Type = type;
            Errors = errors;
        }

        public string Type { get; private set; }
        public string Errors { get; private set; }
    }
}
