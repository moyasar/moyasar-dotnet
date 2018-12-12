using Moyasar.Common;
using System;
using System.Collections.Generic;

namespace Moyasar.Payments
{
    [Serializable]
    public class PaymentListResult : PaymentResultBase, IPaymentResult
    {
        public List<PaymentResult> Payments { get; set; }
        public MetaResult Meta { get; set; }
    }
}
