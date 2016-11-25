using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using moyasar;
using moyasar.InvoiceArea;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Payment p = new Payment
            //{
            //    ApiKey = "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR",
            //    SourceType = SourceType.Sadad,
            //    Currency = "SAR",
            //    Amount = 100,
            //    SadadType = new SadadType() {fail_url = "##", success_url = "#", type = "sadad", username = "u3042346X"},
            //    Description = "samer"


            //};
            //p.CreatePayment();
            Invoice inv = new Invoice();
            inv.ApiKey = "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR";
            inv.AMOUNT  = "100";
            inv.CURRENCY  = "SAR";
            inv.DESCRIPTION  = "Test invoice";
           var q = inv.CreateInvoice();
            //List of invoices 
            var invs = inv.GetInvoicesList();
            var res = inv.GetInvoiceById("4a760a95-fca3-4d09-b430-43f1742fb8df");
            Console.WriteLine(q.Id+""+q.Status);
            Console.Read();
        }
    }
}
