using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using moyasar;
using moyasar.InvoiceArea;
using moyasar.PaymentArea;

namespace Test
{
  public  class Class1
    {
      public void CreatePayment()
      {
          Payment p = new Payment();
          p.ApiKey = "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR";
          p.Amount = 200;
          p.Currency = "SAR";
          p.Description = "Payment testing onlny";
          p.SourceType = SourceType.Sadad;
          p.SourceReault = new SadadType() {Type = "sadad", Username = "u3042346X",SuccessUrl ="#",FaildUrl = "#"};

        var result =  p.CreatePay();
         
          Console.WriteLine(result.id);
          Console.Read();

      }

      public void ListOfPayment()
      {
          Payment p = new Payment();
            p.ApiKey = "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR";
            PaymentListResult rs = p.GetPaymentsList();
          var amount = rs.Payments[0].amount;


      }

      public void PaymentByID()
      {
            Payment p = new Payment();
            p.ApiKey = "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR";
          var py = p.GetPaymentById("d42aaec1-6997-46ab-a839-55c709bc5f7b");
         var amount =  py.amount;
          var cur = py.currency;
      }

      public void refund()
      {
            Payment p = new Payment();
            p.ApiKey = "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR";
         var refs =  p.Refund("787a9902-0866-4170-af5c-e8f2337624d3", "258900");
      }


      public void CreateInvoice()
      {
          Invoice v = new Invoice();
            v.ApiKey = "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR";
            v.Amount = "200";
          v.Currency = "SAR";
          v.Description = "this invoice for testing";
        var res =   v.CreateInvoice();

      }


        
    }
}
