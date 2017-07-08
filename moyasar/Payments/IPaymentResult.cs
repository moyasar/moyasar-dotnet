using System.Collections.Generic;

namespace Moyasar.PaymentArea
{
    public interface IPaymentResult
    {
        List<PaymentResult> Payments { get; set; }
        MetaResult Meta { get; set; }
    }
}
