using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moyasar.PaymentArea
{
   public interface IPaymentResult
    {
      List<PaymentResult> Payments { get; set; }
       MetaResult Meta { get; set; }
    }
}
