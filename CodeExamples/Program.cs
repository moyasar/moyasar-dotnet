using System;

namespace Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ///
            /// Uncomment one of the examples below to watch its output.
            /// Inspect the methods to learn about API endpoints and returns ..
            /// 
            /// For more:
            ///     htttps://moyasar.com/docs/api/?csharp
            ///

            var examples = new Examples();

            // Credit Card Experiement 
            examples.CreateCreditCardPayment();

            // Sadad Experiment
            // examples.CreateSadadPayment();

            // List Payments 
            // examples.ListOfPayment();

            // List All Payments
            // examples.ListAllPayments();

            // Fetch Payment
            // examples.PaymentByID();

            // Refund Payment
            // examples.RefundPayment();

            // Create Invoice
            // examples.CreateInvoice();

            // List Invoices
            // examples.ListInvoices();

            // List All Invoices
            // examples.ListAllInvoices();

            // Fetch Invoice
            // examples.FetchInvoice();

            Console.Read();
        }
    }
}