using Moyasar.Common;
using System.Collections.Generic;
using System.IO;
using System.Net;
namespace Moyasar.Invoices
{
    public class Invoice : MoyasarBase
    {

        public string Amount { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public string CallbackUrl { get; set; }

        private string InitParam()
        {
            var q = new
            {
                amount = this.Amount,
                description = this.Description,
                currency = this.Currency,
                callback_url = this.CallbackUrl,
            };

            return this.js.Serialize(q); //JsonConvert.SerializeObject(q);
        }

        public InvoiceResult Create()
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(this.MakeInvoiceUrl);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";
            httpWebRequest.Credentials = new NetworkCredential(ApiKey, ApiKey);

            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(this.InitParam());
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    InvoiceResult rs = this.js.Deserialize<InvoiceResult>(result);  // JObject.Parse(result);
                    InvoiceResult invoice = new InvoiceResult
                    {
                        Id = rs.Id, //(string)rs["id"],
                        Status = rs.Status, //(string)rs["status"],
                        Amount = rs.Amount, //(string)rs["amount"],
                        Currency = rs.Currency, //(string)rs["currency"],
                        Description = rs.Description, //(string)rs["description"],
                        CallbackUrl = rs.CallbackUrl, //(string)rs["callback_url"],
                        LogoUrl = rs.LogoUrl, //(string)rs["logo_url"],
                        AmountFormat = rs.AmountFormat, //(string)rs["amount_format"],
                        Url = rs.Url, //(string)rs["url"],
                        CreatedAt = rs.CreatedAt, //(string)rs["created_at"],
                        UpdatedAt = rs.UpdatedAt //(string)rs["updated_at"]
                    };
                    return invoice;
                }
            }
            catch (WebException webEx)
            {
                throw this.HandleRequestErrors(webEx);
            }
        }

        public InvoiceListResult List(int? page = null)
        {
            string finalUrl = page == null ? this.MakeInvoiceUrl : this.MakeInvoiceUrl + "?page=" + page.ToString();

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(finalUrl);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "GET";
            httpWebRequest.Credentials = new NetworkCredential(ApiKey, ApiKey);

            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    InvoiceListResult j = this.js.Deserialize<InvoiceListResult>(result); // JObject.Parse(result);
                    InvoiceListResult list = new InvoiceListResult
                    {
                        Invoices = new List<InvoiceResult>(),
                        Meta = new MetaResult()
                    };

                    List<InvoiceResult> inv = j.Invoices; //j["invoices"].Children();
                    foreach (InvoiceResult i in inv)
                    {
                        InvoiceResult invoiceResult = new InvoiceResult
                        {
                            Id = i.Id, //(string)i["id"],
                            Status = i.Status,//(string)i["status"],
                            Amount = i.Amount, //(string)i["amount"],
                            Currency = i.Currency, //(string)i["currency"],
                            Description = i.Description,// //(string)i["description"],
                            CallbackUrl = i.CallbackUrl,// (string)i["callback_url"],
                            AmountFormat = i.AmountFormat,// (string)i["amount_format"],
                            Url = i.Url, //(string)i["url"],
                            CreatedAt = i.CreatedAt,//(string)i["created_at"],
                            UpdatedAt = i.UpdatedAt //(string)i["updated_at"]
                        };
                        list.Invoices.Add(invoiceResult);
                    }

                    list.Meta.CurrentPage = j.Meta.CurrentPage; //(string)j["meta"]["current_page"];
                    list.Meta.NextPage = j.Meta.NextPage; // (string)j["meta"]["next_page"];
                    list.Meta.PrevPage = j.Meta.PrevPage; //(string)j["meta"]["prev_page"];
                    list.Meta.TotalCount = j.Meta.TotalCount; //(string)j["meta"]["total_pages"];
                    list.Meta.TotalPages = j.Meta.TotalPages; //(string)j["meta"]["total_count"];

                    return list;
                }
            }
            catch (WebException webEx)
            {
                throw this.HandleRequestErrors(webEx);
            }
        }

        public InvoiceResult Fetch(string id)
        {
            string finalUrl = this.MakeInvoiceUrl + "/" + id;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(finalUrl);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "GET";
            httpWebRequest.Credentials = new NetworkCredential(ApiKey, ApiKey);

            try
            {
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    InvoiceResult rs = this.js.Deserialize<InvoiceResult>(result); //JObject.Parse(result);
                    InvoiceResult invoice = new InvoiceResult()
                    {
                        Id = rs.Id, //(string)rs["id"],
                        Status = rs.Status,//(string)rs["status"],
                        Amount = rs.Amount, //(string)rs["amount"],
                        Currency = rs.Currency, //(string)rs["currency"],
                        Description = rs.Description, //(string)rs["description"],
                        CallbackUrl = rs.CallbackUrl,// (string)rs["callback_url"],
                        LogoUrl = rs.LogoUrl, //(string)rs["logo_url"],
                        AmountFormat = rs.AmountFormat, //(string)rs["amount_format"],
                        Url = rs.Url,//(string)rs["url"],
                        CreatedAt = rs.CreatedAt, //(string)rs["created_at"],
                        UpdatedAt = rs.UpdatedAt //(string)rs["updated_at"]
                    };
                    return invoice;
                }
            }
            catch (WebException webEx)
            {
                throw this.HandleRequestErrors(webEx);
            }
        }

        public IEnumerable<InvoiceListResult> ListAll()
        {
            InvoiceListResult allList = new InvoiceListResult();
            int? nextPage = null;
            do
            {
                allList = this.List(nextPage);
                nextPage = int.Parse(allList.Meta.CurrentPage) + 1;
                yield return allList;
            } while (allList.Meta.NextPage != null);
        }

        /// <summary>
        /// Aliases for old names
        /// </summary>

        public InvoiceResult GetInvoiceById(string id)
        {
            return this.Fetch(id);
        }

        public InvoiceListResult GetInvoicesList()
        {
            return this.List();
        }

        public InvoiceResult CreateInvoice()
        {
            return this.Create();
        }
    }
}