using moyasar;
using moyasar.InvoiceArea;
using moyasar.PaymentArea;
using System;

namespace Test
{
    public class Examples
    {
        public void CreateCreditCardPayment()
        {
            MoyasarBase.ApiKey = "pk_live_3jtN7Rn3BuweYsUK38Hp5bhkCcYbp7f76DdZPXRU";

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
        }

        public void ListOfPayment()
        {
            MoyasarBase.ApiKey = "pk_live_3jtN7Rn3BuweYsUK38Hp5bhkCcYbp7f76DdZPXRU";

            Payment p = new Payment();
            PaymentListResult rs = p.GetPaymentsList();
        }

        public void PaymentByID()
        {
            MoyasarBase.ApiKey = "pk_live_3jtN7Rn3BuweYsUK38Hp5bhkCcYbp7f76DdZPXRU";

            Payment p = new Payment();
            var py = p.GetPaymentById("d42aaec1-6997-46ab-a839-55c709bc5f7b");
            var amount = py.amount;
            var cur = py.currency;
        }

        public void refund()
        {
            MoyasarBase.ApiKey = "pk_live_3jtN7Rn3BuweYsUK38Hp5bhkCcYbp7f76DdZPXRU";

            Payment p = new Payment();
            var refs = p.Refund("787a9902-0866-4170-af5c-e8f2337624d3", "258900");
        }

        public void CreateInvoice()
        {
            MoyasarBase.ApiKey = "pk_live_3jtN7Rn3BuweYsUK38Hp5bhkCcYbp7f76DdZPXRU";

            Invoice v = new Invoice();
            v.Amount = "200";
            v.Currency = "SAR";
            v.Description = "this invoice for testing";
            var res = v.GetInvoicesList();
        }
    }
}
