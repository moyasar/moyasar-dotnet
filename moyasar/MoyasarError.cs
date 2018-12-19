using System;

namespace Moyasar
{
    [Serializable]
    public class MoyasarException : Exception
    {
        public MoyasarException(string message, string type, string errors) : base(message)
        {
            this.Type = type;
            this.Errors = errors;
        }

        public string Type { get; private set; }
        public string Errors { get; private set; }
    }
}
