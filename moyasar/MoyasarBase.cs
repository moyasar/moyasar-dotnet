using System;
using System.IO;
using System.Net;
using System.Web.Script.Serialization;

namespace Moyasar
{
    public enum SourceType
    {
        CreditCard = 1,
        Sadad = 2
    }

    public class MoyasarBase
    {
        protected readonly JavaScriptSerializer js = new JavaScriptSerializer();
        protected string MakePaymentUrl = "https://api.moyasar.com/v1/payments";
        protected string MakeInvoiceUrl = "https://api.moyasar.com/v1/invoices";
        protected WebRequest Request;
        public static string ApiKey { get; set; }

        protected bool Auth()
        {
            // Create a request using a URL that can receive a post.
            this.Request = WebRequest.Create(this.MakePaymentUrl);

            // Set the Method property of the request to POST.
            // request.Method = "POST";
            this.Request.Credentials = new NetworkCredential(ApiKey, ApiKey);

            // Get the response.
            WebResponse response = this.Request.GetResponse();

            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            if (((HttpWebResponse)response).StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected MoyasarException HandleRequestErrors(WebException e)
        {
            using (var streamReader = new StreamReader(e.Response.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var rs = this.js.Deserialize<ErrorHandler>(result); //JObject.Parse(result);
                string msg = rs.Message;//rs["message"];
                var type = (string)rs.Type;//rs["type"];
                var errors = rs.Errors?.ToString(); //rs["errors"].ToString();
                var error = new MoyasarException(msg, type, errors);
                return error;
            }
        }

    }

    public class ErrorHandler
    {
        public string Message { get; set; }
        public string Type { get; set; }
        public object[] Errors { get; set; }
    }
}
