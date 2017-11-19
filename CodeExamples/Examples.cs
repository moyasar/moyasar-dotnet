using System;
using System.Linq;
using Moyasar;
using Moyasar.Invoices;
using Moyasar.Payments;

namespace Test
{
    public class Examples
    {
        public void CreateCreditCardPayment()
        {
            // Replace '<Your API Key>' with your account API Key.
            // Always keep your secret keys saved in secure place and not exposed publicly.
            MoyasarBase.ApiKey = "<Your API Key>";

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

            Console.WriteLine(ObjectDumper.Dump(result));

            Console.WriteLine("Payment Id: {0}", result.Id);
            Console.WriteLine("Payment Status: {0}", result.Status);
            Console.WriteLine("Payment Source Message: {0}", result.Source.Message);
            Console.WriteLine();
        }

        public void CreateSadadPayment()
        {
            // Replace '<Your API Key>' with your account API Key.
            // Always keep your secret keys saved in secure place and not exposed publicly.
            MoyasarBase.ApiKey = "<Your API Key>";

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

            Console.WriteLine(ObjectDumper.Dump(result));

            Console.WriteLine("Payment Id: {0}", result.Id);
            Console.WriteLine("Payment Status: {0}", result.Status);
            Console.WriteLine("Payment Source Message: {0}", result.Source.Message);
            Console.WriteLine("Payment Source Transaction Id: {0}", (result.Source as SadadType).TransactionId);
            Console.WriteLine("Payment Source Transaction Url: {0}", (result.Source as SadadType).TransactionUrl);
            Console.WriteLine();
        }

        public void ListOfPayment()
        {
            // Replace '<Your API Key>' with your account API Key.
            // Always keep your secret keys saved in secure place and not exposed publicly.
            MoyasarBase.ApiKey = "<Your API Key>";

            var results = new Payment().List();

            Console.WriteLine(ObjectDumper.Dump(results));

            Console.WriteLine("Number Of Payments: {0}", results.Payments.Count);
            Console.WriteLine("Last Payment from List:");
            Console.WriteLine("ID: {0} ---- Status: {1}", results.Payments.Last().Id, results.Payments.Last().Status);
            Console.WriteLine();

            Console.WriteLine("\nMeta Data");
            Console.WriteLine(results.Meta.TotalCount);
            Console.WriteLine(results.Meta.TotalPages);
            Console.WriteLine(results.Meta.CurrentPage);
            Console.WriteLine(results.Meta.NextPage, "\n");

            // With Specific Page
            results = new Payment().List(2);

            Console.WriteLine(ObjectDumper.Dump(results));

            Console.WriteLine("Number Of Payments in Page 2: {0}", results.Payments.Count);
            Console.WriteLine("Last Payment from List:");
            Console.WriteLine("ID: {0} ---- Status: {1}", results.Payments.Last().Id, results.Payments.Last().Status);
            Console.WriteLine();
        }

        public void PaymentByID()
        {
            // Replace '<Your API Key>' with your account API Key.
            // Always keep your secret keys saved in secure place and not exposed publicly.
            MoyasarBase.ApiKey = "<Your API Key>";

            // Getting existing payment ...

            var payment = new Payment().Fetch("2eac340c-713d-4556-9d53-9a3f4671be6f");

            Console.WriteLine(ObjectDumper.Dump(payment));

            Console.WriteLine("Found Payment with:");
            Console.WriteLine("ID: {0} ---- Amount: {1} ---- Currency: {2}", payment.Id, payment.Amount, payment.Currency);

            // Getting non-existing payment ...
            try
            {
                payment = new Payment().GetPaymentById("2eac340c-713d-4556-9d53-XXX");
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
            // Replace '<Your API Key>' with your account API Key.
            // Always keep your secret keys saved in secure place and not exposed publicly.
            MoyasarBase.ApiKey = "<Your API Key>";

            try
            {
                var refund = new Payment().Refund("dc3c16ac-579b-455a-adda-892357155df1");

                Console.WriteLine(ObjectDumper.Dump(refund));

                Console.WriteLine("Refunded Payment");
                Console.WriteLine("Id: {0} || Refunded: {1} || Refunded At: {2}", refund.Id, refund.Refunded, refund.RefundedAt);
            }
            catch (MoyasarException ex)
            {
                Console.WriteLine("Error While Fetching Payment");
                Console.WriteLine("Error Type: {0} || Error Message: {1}", ex.Type, ex.Message);
            }
            Console.WriteLine();
        }

        public void ListAllPayments()
        {
            // Replace '<Your API Key>' with your account API Key.
            // Always keep your secret keys saved in secure place and not exposed publicly.
            MoyasarBase.ApiKey = "<Your API Key>";

            var paymentAPI = new Payment();

            foreach (var paymentBatch in paymentAPI.ListAll())
            {
                Console.WriteLine("---------------- *** -----------------");
                Console.WriteLine("Total number Of payments: {0} in Page: {1} : \n", paymentBatch.Payments.Count, paymentBatch.Meta.CurrentPage);
                Console.WriteLine(ObjectDumper.Dump(paymentBatch.Payments));
                Console.WriteLine("Last Payment from List:");
                Console.WriteLine("ID: {0} ---- Status: {1} \n", paymentBatch.Payments.Last().Id, paymentBatch.Payments.Last().Status);
                Console.WriteLine("---------------- *** -----------------");
            }
        }

        public void ListInvoices()
        {
            // Replace '<Your API Key>' with your account API Key.
            // Always keep your secret keys saved in secure place and not exposed publicly.
            MoyasarBase.ApiKey = "sk_test_NpdJByQ5fE9ACfNBvQPEu9jakiFrH36fUA9cSGdP";

            Invoice v = new Invoice();
            var results = v.List();

            Console.WriteLine(ObjectDumper.Dump(results.Invoices));

            Console.WriteLine("Number Of Invoices: {0}", results.Meta.TotalCount);
            Console.WriteLine("First Invoice from List:");
            Console.WriteLine("ID: {0} ---- Status: {1}", results.Invoices.First().Id, results.Invoices.First().Status);
            Console.WriteLine();

            Console.WriteLine("\nMeta Data");
            Console.WriteLine(results.Meta.TotalCount);
            Console.WriteLine(results.Meta.TotalPages);
            Console.WriteLine(results.Meta.CurrentPage);

            // list invoices from next page if exists ..
            if (results.Meta.NextPage != null)
            {

                Console.WriteLine(results.Meta.NextPage, "\n");

                // With Specific Page
                results = v.List(2);

                Console.WriteLine(ObjectDumper.Dump(results));

                Console.WriteLine("Number Of Invoice in Page 2: {0}", results.Invoices.Count);
                Console.WriteLine("Last Invoice from List:");
                Console.WriteLine("ID: {0} ---- Status: {1}", results.Invoices.Last().Id, results.Invoices.Last().Status);
                Console.WriteLine();
            }
        }

        public void CreateInvoice()
        {
            // Replace '<Your API Key>' with your account API Key.
            // Always keep your secret keys saved in secure place and not exposed publicly.
            MoyasarBase.ApiKey = "<Your API Key>";

            Invoice inv = new Invoice()
            {
                Amount = "320",
                Currency = "USD",
                Description = "Sample Pharmacy Invoice",
            };
            var invoice = inv.Create();

            Console.WriteLine(ObjectDumper.Dump(invoice));

            Console.WriteLine("Invoice Id: {0}", invoice.Id);
            Console.WriteLine("Invoice Status: {0}", invoice.Status);
            Console.WriteLine("Invoice Amount: {0}", invoice.Amount);
            Console.WriteLine("Invoice Description: {0}", invoice.Description);
            Console.WriteLine();
        }

        public void FetchInvoice()
        {
            // Replace '<Your API Key>' with your account API Key.
            // Always keep your secret keys saved in secure place and not exposed publicly.
            MoyasarBase.ApiKey = "<Your API Key>";

            var invoice = new Invoice().Fetch("50b88f06-3ef5-4a36-9af8-8cb473b7ba88");

            Console.WriteLine(ObjectDumper.Dump(invoice));

            Console.WriteLine("Invoice Id: {0}", invoice.Id);
            Console.WriteLine("Invoice Status: {0}", invoice.Status);
            Console.WriteLine("Invoice Amount: {0}", invoice.Amount);
            Console.WriteLine("Invoice Description: {0}", invoice.Description);
            Console.WriteLine();
        }
    }
}
