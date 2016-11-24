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

      internal bool Auth()
      {
          try
          {
                WebClient.BaseAddress = MakePaymentUrl;
                 
                WebClient.UseDefaultCredentials = true;
               WebClient. Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                WebClient.Credentials = new NetworkCredential(this.ApiKey, "");
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
