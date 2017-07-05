using Moyasar;
using Moyasar.ExceptionsMap;
using Moyasar.InvoiceArea;
using Moyasar.PaymentArea;
using Moyasar.PaymentArea.RefundMap;
using System;
using System.Linq;

namespace Test
{
    public class Examples
    {
        public void CreateCreditCardPayment()
        {
            MoyasarBase.ApiKey = "pk_test_yTqHr4bcm1eCJkGdpNExsU4f6s1FZkqpHzr7XezG";

            Payment p = new Payment();
            p.Amount = 100;
            p.Currency = "SAR";
            p.Description = "New Books Purchase";
            p.SourceType = SourceType.CreditCard;
            p.SourceReault = new CreditCard()
            {
                Type = "creditcard",  //or master,
                Message = "",
                Company = "visa",
                Number = "4111111111111111",
                Name = "Abdullah Barrak",
                Year = 2018,
                Month = 03,
                Cvc = "111"

            };
            var result = p.CreatePay();

            Console.WriteLine("Payment Id: {0}", result.id);
            Console.WriteLine("Payment Status: {0}", result.status);
            Console.WriteLine("Payment Source Message: {0}", result.source.Message);
            Console.WriteLine();
        }

        public void CreateSadadPayment()
        {
            MoyasarBase.ApiKey = "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR";

            Payment p = new Payment();

            p.Amount = 200;
            p.Currency = "SAR";
            p.Description = "Simple Test Payment";
            p.SourceType = SourceType.Sadad;
            p.SourceReault = new SadadType()
            {
                Type = "sadad",
                Username = "u3043090Xolp",
                SuccessUrl = "https://moyasar.com",
                FaildUrl = "https://moyasar.com/docs"
            };

            var result = p.CreatePay();

            Console.WriteLine("Payment Id: {0}", result.id);
            Console.WriteLine("Payment Status: {0}", result.status);
            Console.WriteLine("Payment Source Message: {0}", result.source.Message);
            Console.WriteLine("Payment Source Transaction Id: {0}", (result.source as SadadType).TransactionId);
            Console.WriteLine("Payment Source Transaction Url: {0}", (result.source as SadadType).TransactionUrl);
            Console.WriteLine();
        }

        public void ListOfPayment()
        {
            MoyasarBase.ApiKey = "sk_test_NpdJByQ5fE9ACfNBvQPEu9jakiFrH36fUA9cSGdP";

            Payment p = new Payment();
            PaymentListResult rs = p.GetPaymentsList();

            Console.WriteLine("Number Of Payments: {0}", rs.Payments.Count);
            Console.WriteLine("Last Payment from List:");
            Console.WriteLine("ID: {0} ---- Status: {1}", rs.Payments.Last().id, rs.Payments.Last().status);
            Console.WriteLine();
        }

        public void PaymentByID()
        {
            MoyasarBase.ApiKey = "sk_test_NpdJByQ5fE9ACfNBvQPEu9jakiFrH36fUA9cSGdP";

            // Getting existing payment ...
            Payment p = new Payment();
            var py = p.GetPaymentById("2eac340c-713d-4556-9d53-9a3f4671be6f");
            var amount = py.amount;
            var cur = py.currency;

            Console.WriteLine("Found Payment with:");
            Console.WriteLine("ID: {0} ---- Amount: {1} ---- Currency: {2}", py.id, py.amount, py.currency);

            // Getting non-existing payment ...
            try
            {
                Payment payment = new Payment();
                var not_py = payment.GetPaymentById("2eac340c-713d-4556-9d53-XXX");
                var py_amount = not_py.amount;
            }
            catch (MoyasarException ex)
            {
                Console.WriteLine("Error While Fetching Payment");
                Console.WriteLine("Error Type: {0} || Error Message: {1}", ex.Type, ex.Message);
            }
            Console.WriteLine();
        }

        public void refund()
        {
            MoyasarBase.ApiKey = "sk_test_NpdJByQ5fE9ACfNBvQPEu9jakiFrH36fUA9cSGdP";

            Payment p = new Payment();
            var refs = p.Refund("a76ffffe-3479-4491-a7e5-64803a055cec", "100");

            if (refs is RefundException) {
                Console.WriteLine("Error While doing refund with Credit Card");
                Console.WriteLine("Error Type: {0} || Error Messag: {1}", ((RefundException)refs).Type, ((RefundException)refs).Message);
            }
            else
            {
                Console.WriteLine("Refunded Payment");
                Console.WriteLine("Id: {0} || Refunded: {1} || Refunded At: {3}", ((RefundResult)refs).Id, ((RefundResult)refs).Refunded, ((RefundResult)refs).RefundedAt);
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
