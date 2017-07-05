using System;
using moyasar;
using moyasar.InvoiceArea;
using moyasar.PaymentArea;
using moyasar.ExceptionsMap;

namespace Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var examples = new Examples();

            // Credit Card Experiement 
            examples.CreateCreditCardPayment();

            // Sadad Experiment
            examples.CreateSadadPayment();

            // List Payments 
            examples.ListOfPayment();

            // Fetch Payment
            examples.PaymentByID();

            // Refund Payment
            examples.refund();

            // List Invoices
            examples.ListInvoices();

            Console.Read();
        }
    }
}