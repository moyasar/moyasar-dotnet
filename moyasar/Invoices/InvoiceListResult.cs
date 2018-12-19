using Moyasar.Common;
using System;
using System.Collections.Generic;

namespace Moyasar.Invoices
{
    [Serializable]
    public class InvoiceListResult
    {
        public List<InvoiceResult> Invoices { get; set; }
        public MetaResult Meta { get; set; }
    }
}
