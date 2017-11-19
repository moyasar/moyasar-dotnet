using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Moyasar.Common;

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

        public InvoiceListResult List(int? page = null)
        {
            var finalUrl = page == null ? MakeInvoiceUrl : MakeInvoiceUrl + "?page=" + page.ToString();

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
                    var j = JObject.Parse(result);
                    var list = new InvoiceListResult
                    {
                        Invoices = new List<InvoiceResult>(),
                        Meta = new MetaResult()
                    };

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
                        list.Invoices.Add(invoiceResult);
                    }

                    list.Meta.CurrentPage = (string)j["meta"]["current_page"];
                    list.Meta.NextPage = (string)j["meta"]["next_page"];
                    list.Meta.PrevPage = (string)j["meta"]["prev_page"];
                    list.Meta.TotalCount = (string)j["meta"]["total_pages"];
                    list.Meta.TotalPages = (string)j["meta"]["total_count"];

                    return list;
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

        public IEnumerable<InvoiceListResult> ListAll()
        {
            var allList = new InvoiceListResult();
            int? nextPage = null;
            do
            {
                allList = List(nextPage);
                nextPage = Int32.Parse(allList.Meta.CurrentPage) + 1;
                yield return allList;
            } while (allList.Meta.NextPage != null);
        }

        /// <summary>
        /// Aliases for old names
        /// </summary>

        public InvoiceResult GetInvoiceById(string id)
        {
            return Fetch(id);
        }

        public InvoiceListResult GetInvoicesList()
        {
            return List();
        }

        public InvoiceResult CreateInvoice()
        {
            return Create();
        }

    }
}