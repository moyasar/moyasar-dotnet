using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Moyasar.Services.Abstraction
{
    public interface IPaymentSource
    {
        Dictionary<string, object> ToDictionary();
        void Validate();
    }
}