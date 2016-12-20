using moyasar;
using moyasar.InvoiceArea;
using moyasar.PaymentArea;
using System;

namespace Test
{
    public class Class1
    {
        public void CreatePayment()
        {
            Payment p = new Payment();
             
            p.Amount = 200;
            p.Currency = "SAR";
            p.Description = "";
            p.SourceType = SourceType.CreditCard;
            p.SourceReault = new SadadType()
            {
                Type = "sadad",
                Username = "u3042346X"
                ,
                SuccessUrl = "#",
                FaildUrl = "#"
            };

            p.SourceReault = new CreditCard()
            {
                Type = "visa",//or master,
                Message = "",
                Company = "",
                Number = "",
                Name = "",
                Year = 0,
                Month = 0,
                Cvc = ""

            };
            var result = p.CreatePay();

            Console.WriteLine(result.id);
            Console.Read();

        }

        public void ListOfPayment()
        {

            Payment p = new Payment();
            MoyasarBase.ApiKey = "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR";
            PaymentListResult rs = p.GetPaymentsList();



        }

        public void PaymentByID()
        {
            Payment p = new Payment();
            MoyasarBase.ApiKey = "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR";
            var py = p.GetPaymentById("d42aaec1-6997-46ab-a839-55c709bc5f7b");
            var amount = py.amount;
            var cur = py.currency;
        }

        public void refund()
        {
            MoyasarBase.ApiKey = "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR";
            Payment p = new Payment();

            var refs = p.Refund("787a9902-0866-4170-af5c-e8f2337624d3", "258900");
        }


        public void CreateInvoice()
        {
            Invoice v = new Invoice();
            MoyasarBase.ApiKey = "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR";
            v.Amount = "200";
            v.Currency = "SAR";
            v.Description = "this invoice for testing";
            var res = v.GetInvoicesList();


        }



    }
}
