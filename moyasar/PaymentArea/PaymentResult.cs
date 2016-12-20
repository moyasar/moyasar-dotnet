using System;
using System.Collections.Generic;
 
using System.Text;

namespace moyasar.PaymentArea
{
   public class PaymentResult:PaymentResultBase
    {
       public string id { get; set; }
       public string status { get; set; }
       public int amount { get; set; }
       public string fee { get; set; }
       public string currency { get; set; }
       public string refunded { get; set; }
       public string refunded_at { get; set; }
       public string description { get; set; }
       public string amount_format { get; set; }
       public string fee_format { get; set; }
       public string invoice_id { get; set; }
       public string ip { get; set; }
       public string created_at { get; set; }
       public string updated_at { get; set; }
        public SourceReaultBase source { get; set; }
    }
}
