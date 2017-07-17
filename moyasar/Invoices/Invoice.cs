using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Moyasar.Invoices
{
    public class Invoice : MoyasarBase
    {
        public string Amount { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }

        private string IniParam()
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

        public InvoiceResult Create()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(IniParam());
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

        public List<InvoiceResult> List()
        {
            var ls = new List<InvoiceResult>();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(MakeInvoiceUrl);
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
                            Id = (string)i["id"],
                            Status = (string)i["status"],
                            Amount = (string)i["amount"],
                            Currency = (string)i["currency"],
                            Description = (string)i["description"],
                            AmountFormat = (string)i["amount_format"],
                            Url = (string)i["url"],
                            CreatedAt = (string)i["created_at"],
                            UpdatedAt = (string)i["updated_at"]
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

        public InvoiceResult Fetch(string id)
        {
            var finalUrl = MakeInvoiceUrl + "/" + id;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(finalUrl);
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
                    var invoice = new InvoiceResult()
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

        /// <summary>
        /// Aliases for old names
        /// </summary>

        public InvoiceResult GetInvoiceById(string id)
        {
            return Fetch(id);
        }

        public List<InvoiceResult> GetInvoicesList()
        {
            return List();
        }

        public InvoiceResult CreateInvoice()
        {
            return Create();
        }

    }
}