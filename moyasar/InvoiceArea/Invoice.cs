using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace moyasar.InvoiceArea
{
    public class Invoice : MoyasarBase
    {
        public string AMOUNT { get; set; }
        public string DESCRIPTION { get; set; }
        public string CURRENCY { get; set; }

        public string iniParam()
        {
            var q = new
            {
                AMOUNT,
                DESCRIPTION = CURRENCY,
                CURRENCY = DESCRIPTION
            };

            var finalUrl = MakeInvoiceUrl + "?amount=" + AMOUNT + "&currency=" + CURRENCY + "&description=" +
                           DESCRIPTION;
            return finalUrl;
        }


        public InvoiceResult CreateInvoice()
        {
            
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(iniParam());
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";
            httpWebRequest.Credentials = new NetworkCredential(ApiKey, ApiKey);

            var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var rs = JObject.Parse(result);
                InvoiceResult invoice = new InvoiceResult();
                invoice.Id = (string)rs["id"];
                invoice.Status = (string)rs["status"];
                invoice.Amount = (string)rs["amount"];

                invoice.Currency = (string)rs["currency"];
                invoice.Description = (string)rs["description"];

                invoice.LogoUrl = (string)rs["logo_url"];
                invoice.AmountFormat = (string)rs["amount_format"];

                invoice.Url = (string)rs["url"];
                invoice.CreatedAt = (string)rs["created_at"];
                invoice.UpdatedAt  = (string)rs["updated_at"];
                return invoice;

            }

            return null;
        }
    }
}