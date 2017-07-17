using System.Collections.Generic;

namespace Moyasar.Payments
{
    public interface IPaymentResult
    {
        List<PaymentResult> Payments { get; set; }
        MetaResult Meta { get; set; }
    }
}
