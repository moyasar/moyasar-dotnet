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
            var c = new Examples();
            c.CreatePayment();

            Console.Read();
        }
    }
}