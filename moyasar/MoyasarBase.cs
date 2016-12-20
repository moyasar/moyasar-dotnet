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
      protected WebRequest Request;
        public static string ApiKey { get; set; }

      protected bool Auth()
      {

            // Create a request using a URL that can receive a post. 
              Request = WebRequest.Create(MakePaymentUrl);
            // Set the Method property of the request to POST.
           // request.Method = "POST";
            Request.Credentials = new NetworkCredential(ApiKey, ApiKey);
            // Get the response.
            WebResponse response = Request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
          if (((HttpWebResponse) response).StatusCode == HttpStatusCode.OK)
              return true;
          else
              return false;
      }
  }
}
