using System;
using System.Collections.Generic;
using System.Text;

namespace moyasar.InvoiceArea
{
  public  class InvoiceResult
    {
      public string Id { get; set; }
      public string Status { get; set; }
      public string Amount { get; set; }
      public string Currency { get; set; }
      public string Description { get; set; }
      public string LogoUrl { get; set; }
      public string AmountFormat { get; set; }
      public string Url { get; set; }
      public string CreatedAt { get; set; }
      public string UpdatedAt { get; set; }
    }
}
