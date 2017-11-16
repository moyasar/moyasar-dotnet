using System.Collections.Generic;

namespace Moyasar.Invoices
{
    public class InvoiceListResult
    {
        public List<InvoiceResult> Invoices { get; set; }
        public MetaResult Meta { get; set; }
    }
}
