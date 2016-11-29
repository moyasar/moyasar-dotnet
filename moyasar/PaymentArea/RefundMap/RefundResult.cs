using System;
using System.Collections.Generic;
using System.Text;

namespace moyasar.PaymentArea.RefundMap
{
  public  class RefundResult:MoyasarRefundBase
    {
      public string Id { get; set; }
      public string Currency { get; set; }
      public string Amount { get; set; }
      public string Refunded { get; set; }
      public string RefundedAt { get; set; }
      public string Fee { get; set; }
      public SourceReaultBase Source { get; set; }
    }
}
