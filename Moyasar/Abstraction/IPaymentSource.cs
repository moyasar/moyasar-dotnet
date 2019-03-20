using System.Collections.Generic;

namespace Moyasar.Abstraction
{
    public interface IPaymentSource
    {
        Dictionary<string, object> ToDictionary();
        void Validate();
    }
}