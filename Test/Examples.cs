using Moyasar;
using Moyasar.InvoiceArea;
using Moyasar.PaymentArea;
using Moyasar.PaymentArea.RefundMap;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Moyasar.ExceptionsMap;

namespace Test
{
    public class Examples
    {
        public void CreateCreditCardPayment()
        {
            MoyasarBase.ApiKey = "pk_test_yTqHr4bcm1eCJkGdpNExsU4f6s1FZkqpHzr7XezG";

            Payment payment = new Payment()
            {
                Amount = 100,
                Currency = "SAR",
                Description = "New Suitcase Purchase",
                SourceType = SourceType.CreditCard,
                SourceReault = new CreditCard()
                {
                    Type = "creditcard",
                    Message = "",
                    Company = "visa",
                    Number = "4111111111111111",
                    Name = "Abdullah Barrak",
                    Year = 2018,
                    Month = 03,
                    Cvc = "111"
                }
            };
            var result = payment.Create();

            Console.WriteLine("Payment Id: {0}", result.Id);
            Console.WriteLine("Payment Status: {0}", result.Status);
            Console.WriteLine("Payment Source Message: {0}", result.Source.Message);
            Console.WriteLine();
        }

        public void CreateSadadPayment()
        {
            MoyasarBase.ApiKey = "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR";

            Payment payment = new Payment()
            {
                Amount = 200,
                Currency = "SAR",
                Description = "Simple Test Payment",
                SourceType = SourceType.Sadad,
                SourceReault = new SadadType()
                {
                    Type = "sadad",
                    Username = "u3043090Xolp",
                    SuccessUrl = "https://moyasar.com",
                    FaildUrl = "https://moyasar.com/docs"
                }
            };
            var result = payment.Create();

            Console.WriteLine("Payment Id: {0}", result.Id);
            Console.WriteLine("Payment Status: {0}", result.Status);
            Console.WriteLine("Payment Source Message: {0}", result.Source.Message);
            Console.WriteLine("Payment Source Transaction Id: {0}", (result.Source as SadadType).TransactionId);
            Console.WriteLine("Payment Source Transaction Url: {0}", (result.Source as SadadType).TransactionUrl);
            Console.WriteLine();
        }

        public void ListOfPayment()
        {
            MoyasarBase.ApiKey = "sk_test_NpdJByQ5fE9ACfNBvQPEu9jakiFrH36fUA9cSGdP";

            Payment p = new Payment();
            PaymentListResult rs = p.List();

            Console.WriteLine("Number Of Payments: {0}", rs.Payments.Count);
            Console.WriteLine("Last Payment from List:");
            Console.WriteLine("ID: {0} ---- Status: {1}", rs.Payments.Last().Id, rs.Payments.Last().Status);
            Console.WriteLine();
        }

        public void PaymentByID()
        {
            MoyasarBase.ApiKey = "sk_test_NpdJByQ5fE9ACfNBvQPEu9jakiFrH36fUA9cSGdP";

            // Getting existing payment ...

            var payment = new Payment().Fetch("2eac340c-713d-4556-9d53-9a3f4671be6f");
            var amount = payment.Amount;
            var cur = payment.Currency;

            Console.WriteLine("Found Payment with:");
            Console.WriteLine("ID: {0} ---- Amount: {1} ---- Currency: {2}", payment.Id, payment.Amount, payment.Currency);

            // Getting non-existing payment ...
            try
            {
                payment = new Payment().GetPaymentById("2eac340c-713d-4556-9d53-XXX");
                amount = payment.Amount;
            }
            catch (MoyasarException ex)
            {
                Console.WriteLine("Error While Fetching Payment");
                Console.WriteLine("Error Type: {0} || Error Message: {1}", ex.Type, ex.Message);
            }
            Console.WriteLine();
        }

        public void RefundPayment()
        {
            MoyasarBase.ApiKey = "sk_test_NpdJByQ5fE9ACfNBvQPEu9jakiFrH36fUA9cSGdP";

            Payment p = new Payment();
            var refs = p.Refund("a76ffffe-3479-4491-a7e5-64803a055cec", "100");

            if (refs is RefundException)
            {
                Console.WriteLine("Error While doing refund with Credit Card");
                Console.WriteLine("Error Type: {0} || Error Messag: {1}", ((RefundException)refs).Type, ((RefundException)refs).Message);
            }
            else
            {
                Console.WriteLine("Refunded Payment");
                Console.WriteLine("Id: {0} || Refunded: {" +
                    "1} || Refunded At: {3}", ((RefundResult)refs).Id, ((RefundResult)refs).Refunded, ((RefundResult)refs).RefundedAt);
            }
            Console.WriteLine();
        }

        public void ListInvoices()
        {
            MoyasarBase.ApiKey = "sk_test_NpdJByQ5fE9ACfNBvQPEu9jakiFrH36fUA9cSGdP";

            Invoice v = new Invoice();
            var all = v.GetInvoicesList();

            Console.WriteLine("Number Of Invoices: {0}", all.Count);
            Console.WriteLine("First Invoice from List:");
            Console.WriteLine("ID: {0} ---- Status: {1}", all.First().Id, all.Last().Status);
            Console.WriteLine();
        }

        public void CreateInvoice()
        {

        }

        public void FetchInvoice()
        {

        }
    }

}
