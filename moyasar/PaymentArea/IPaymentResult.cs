using System.Collections.Generic;

namespace moyasar.PaymentArea
{
    public interface IPaymentResult
    {
        List<PaymentResult> Payments { get; set; }
        MetaResult Meta { get; set; }
    }
}
