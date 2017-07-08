using System.Collections.Generic;

namespace Moyasar.PaymentArea
{
    public class PaymentListResult : PaymentResultBase, IPaymentResult
    {
        public List<PaymentResult> Payments { get; set; }
        public MetaResult Meta { get; set; }
    }
}
