using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moyasar.PaymentArea
{
   public class PaymentListResult:PaymentResultBase,IPaymentResult
    {
       public List<PaymentResult> Payments { get; set; }
       public MetaResult Meta { get; set; }
    }
}
