using System;
using Moyasar;
using Moyasar.InvoiceArea;
using Moyasar.PaymentArea;
using Moyasar.ExceptionsMap;

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

            // Create Invoices

            // List Invoices
            examples.ListInvoices();

            // Update Invoices

            Console.Read();
        }
    }
}