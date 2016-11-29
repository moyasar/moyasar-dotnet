using System;
using moyasar;
using moyasar.InvoiceArea;
using moyasar.PaymentArea;

namespace Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var p = new Payment
            {
                ApiKey = "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR",
                SourceType = SourceType.CreditCard,
                Currency = "SAR",
                Amount = 1000,
                // SourceReault = new SadadType() { Type = "sadad", Username ="u3042346X" },
                Description = "samer",
                SourceReault = new CreditCard
                {
                    Year = 2019,
                    Month = 10,
                    Company = "visa",
                    Cvc = "111",
                    Name = "any any",
                    Type = "creditcard",
                    Number = "4111111111111111"
                }
            };
            var i = p.GetPaymentById("429f4fde-2bd7-49b9-8fd8-2c6ca76be614");
            var ix = p.Refund("d42aaec1-6997-46ab-a839-55c709bc5f7b", "100");
            var pp = p.GetPaymentsList();

            var qx = p.IniParameters();
            p.CreatePay();
            //p.CreatePayment();
            var inv = new Invoice();
            inv.ApiKey = "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR";
            inv.Amount = "1000";
           
            inv.Currency = "SAR";
            inv.Desciption = "item testig";
            var q = inv.CreateInvoice();
            //List of invoices 
            var invs = inv.GetInvoicesList();
            var idf = p.GetPaymentById("429f4fde-2bd7-49b9-8fd8-2c6ca76be614");
            var res = inv.GetInvoiceById("4a760a95-fca3-4d09-b430-43f1742fb8df");
            Console.WriteLine(q.Id + "" + q.Status);
            Console.Read();
        }
    }
}