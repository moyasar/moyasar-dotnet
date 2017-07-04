using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace moyasar.InvoiceArea
{
    public class Invoice : MoyasarBase
    {
        public string Amount { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }

        private string iniParam()
        {
            var q = new
            {
                Amount,
                Desciption = Currency,
                Currency = Description
            };

            var finalUrl = MakeInvoiceUrl + "?amount=" + Amount + "&currency=" + Currency + "&description=" +
                           Description;
            return finalUrl;
        }

        public InvoiceResult CreateInvoice()
        {
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(iniParam());
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";
            httpWebRequest.Credentials = new NetworkCredential(ApiKey, ApiKey);

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var rs = JObject.Parse(result);
                    var invoice = new InvoiceResult
                    {
                        Id = (string)rs["id"],
                        Status = (string)rs["status"],
                        Amount = (string)rs["amount"],
                        Currency = (string)rs["currency"],
                        Description = (string)rs["description"],
                        LogoUrl = (string)rs["logo_url"],
                        AmountFormat = (string)rs["amount_format"],
                        Url = (string)rs["url"],
                        CreatedAt = (string)rs["created_at"],
                        UpdatedAt = (string)rs["updated_at"]
                    };
                    return invoice;
                }
            }
            catch (WebException webEx)
            {
                throw HandleRequestErrors(webEx);
            }
        }

        public List<InvoiceResult> GetInvoicesList()
        {
            var ls = new List<InvoiceResult>();
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(MakeInvoiceUrl);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "GET";
            httpWebRequest.Credentials = new NetworkCredential(ApiKey, ApiKey);

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var j = JObject.Parse(result);

                    var inv = j["invoices"].Children();
                    foreach (var i in inv)
                    {
                        var invoiceResult = new InvoiceResult
                        {
                            Currency = (string)i["Currency"],
                            Amount = (string)i["Amount"],
                            Description = (string)i["Desciption"],
                            Id = (string)i["id"],
                            Status = (string)i["status"],
                            AmountFormat = (string)i["Amount_format"],
                            LogoUrl = (string)i["url"],
                            CreatedAt = (string)i["created_at"],
                            UpdatedAt = (string)i["updated_at"],
                            Url = (string)i["url"]
                        };
                        ls.Add(invoiceResult);
                    }
                    return ls;
                }
            }
            catch (WebException webEx)
            {
                throw HandleRequestErrors(webEx);
            }
        }

        public InvoiceResult GetInvoiceById(string id)
        {
            var finalUrl = MakeInvoiceUrl + "/" + id;
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(finalUrl);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "GET";
            httpWebRequest.Credentials = new NetworkCredential(ApiKey, ApiKey);

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var rs = JObject.Parse(result);
                    var invoice = new InvoiceResult();
                    invoice.Id = (string)rs["id"];
                    invoice.Status = (string)rs["status"];
                    invoice.Amount = (string)rs["Amount"];

                    invoice.Currency = (string)rs["Currency"];
                    invoice.Description = (string)rs["Desciption"];

                    invoice.LogoUrl = (string)rs["logo_url"];
                    invoice.AmountFormat = (string)rs["Amount_format"];

                    invoice.Url = (string)rs["url"];
                    invoice.CreatedAt = (string)rs["created_at"];
                    invoice.UpdatedAt = (string)rs["updated_at"];
                    return invoice;
                }
            }
            catch (WebException webEx)
            {
                throw HandleRequestErrors(webEx);
            }
        }
    }
}