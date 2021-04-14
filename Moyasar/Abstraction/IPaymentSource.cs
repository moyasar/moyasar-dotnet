using System;

namespace Moyasar.Abstraction
{
    public interface IPaymentSource
    {
        string Type { get; }
        
        void Validate();
    }
}
