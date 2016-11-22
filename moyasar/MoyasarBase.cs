using System;
using System.Net;


namespace moyasar
{
    public enum SourceType
    {
        CreditCard=1,
        Sadad=2
    }
  public  class MoyasarBase
  {
      protected string MakePaymentUrl = "https://api.moyasar.com/v1/payments";
      protected string MakeInvoiceUrl = "https://api.moyasar.com/v1/invoices";
      protected WebClient WebClient = new WebClient();
        public string ApiKey { get; set; }

      public bool Auth()
      {
          try
          {
                WebClient.BaseAddress = MakePaymentUrl;
                // WebClient.DefaultRequestHeaders.Add("basic", "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR");
                WebClient.UseDefaultCredentials = true;
                WebClient.Credentials = new NetworkCredential("sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR", "");
                var respone = WebClient.DownloadString(MakePaymentUrl);//.UploadString(MakePaymentUrl,"POST","")
                return true;
            }
          catch (Exception e)
          {

              return false;
          }
      }
  }
}
