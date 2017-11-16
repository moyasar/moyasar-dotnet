using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Moyasar.ExceptionsMap;
using Moyasar.MessagesMap;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Moyasar.Payments
{
    [Serializable]
    public class Payment : MoyasarBase
    {
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public SourceReaultBase SourceReault { get; set; }
        public SourceType SourceType { get; set; }

        public string IniParameters()
        {
            var q = new object();
            if (SourceType == SourceType.CreditCard)
            {
                CreditCard crd = (CreditCard) SourceReault;
                q = new
                {
                    amount = Amount,
                    currency = Currency,
                    description = Description,
                    source = new
                    {
                        type = crd.Type,
                        name = crd.Name,
                        number = crd.Number,
                        cvc = crd.Cvc,
                        month = crd.Month,
                        year = crd.Year

                    }
                };
            }
            if (this.SourceType == SourceType.Sadad)
            {
                SadadType Sd = (SadadType) this.SourceReault;
                q = new
                {
                    amount = this.Amount,
                    currency = this.Currency,
                    description = this.Description,
                    source = new
                    {
                        type = Sd.Type,
                        username = Sd.Username,
                        success_url = Sd.SuccessUrl,
                        fail_url = Sd.FaildUrl
                    }
                };
            }

            var sm = JsonConvert.SerializeObject(q);
            return sm;
        }

        private void Validation()
        {
            if (ApiKey == "" || ApiKey == string.Empty)
            {
                var ex = new MoyasarValidationException(EnMessages.ApiKeyNotFound) { ErrorCode = "#1559" };
                throw ex;
            }
            if (SourceType == 0)
            {
                var ex = new MoyasarValidationException(EnMessages.SelectType) { ErrorCode = "#1550" };
                throw ex;
            }

            if (this.SourceReault == null && this.SourceType == SourceType.CreditCard)
            {
                var ex = new MoyasarValidationException(EnMessages.SelectCreditCardType) { ErrorCode = "#1555" };
                throw ex;
            }

            if (this.SourceReault == null)
            {
                var ex = new MoyasarValidationException(EnMessages.TypeEmpty) { ErrorCode = "#1500" };
                throw ex;
            }
            if (string.IsNullOrEmpty(this.Currency) || this.Currency == string.Empty)
            {
                var ex = new MoyasarValidationException(EnMessages.CurrencyEmpty) { ErrorCode = "#1000" };
                throw ex;
            }

            // Check if this creditCard Type
            if (this.SourceType == SourceType.CreditCard)
            {
                if (this.SourceReault != null)
                {
                    var credit = (CreditCard) SourceReault;
                    if (credit.Company == string.Empty)
                    {
                        var ex = new MoyasarValidationException(EnMessages.CreatedCardCompanyNotFound) { ErrorCode = "#1110" };
                        throw ex;
                    }
                    if (credit.Name == string.Empty)
                    {
                        var ex = new MoyasarValidationException(EnMessages.CreatedCardNameNotFound) { ErrorCode = "#1111" };
                        throw ex;
                    }
                    if (credit.Name == string.Empty)
                    {
                        var ex = new MoyasarValidationException(EnMessages.CreatedCardNameNotFound) { ErrorCode = "#1112" };
                        throw ex;
                    }
                    if (credit.Number == string.Empty)
                    {
                        var ex = new MoyasarValidationException(EnMessages.CreatedCardNumberNotFound) { ErrorCode = "#1113" };
                        throw ex;
                    }
                    if (credit.Month == 0)
                    {
                        var ex = new MoyasarValidationException(EnMessages.CreatedCardNumberNotFound) { ErrorCode = "#1114" };
                        throw ex;
                    }
                    if (credit.Year == 0)
                    {
                        var ex = new MoyasarValidationException(EnMessages.CreatedCardNumberNotFound) { ErrorCode = "#1115" };
                        throw ex;
                    }
                }
                else
                {
                    var ex = new MoyasarValidationException(EnMessages.CreatedCardNotReady) { ErrorCode = "#1110" };
                    throw ex;
                }
            }
        }

        public PaymentResult Create()
        {
            Validation();
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(MakePaymentUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Credentials = new NetworkCredential(ApiKey, ApiKey);
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = IniParameters();

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            try
            { 
                var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var rs = JObject.Parse(result);

                    PaymentResult payment = new PaymentResult
                    {
                        Id = (string)rs["id"],
                        Status = (string)rs["status"],
                        Amount = (int)rs["amount"],
                        Description = (string)rs["description"],
                        Currency = (string)rs["currency"],
                        AmountFormat = (string)rs["amount_format"],
                        CreatedAt = (string)rs["created_at"],
                        Fee = (string)rs["fee"],
                        FeeFormat = (string)rs["fee_format"],
                        InvoiceId = (string)rs["invoice_id"],
                        Ip = (string)rs["ip"],
                        Refunded = (string)rs["refunded"],
                        RefundedAt = (string)rs["refunded_at"],
                        UpdatedAt = (string)rs["updated_at"]

                    };
                    if (this.SourceType == SourceType.Sadad)
                    {
                        payment.Source = new SadadType()
                        {
                            Type = (string)rs["source"]["type"],
                            Username = (string)rs["source"]["username"],
                            TransactionUrl = (string)rs["source"]["transaction_url"],
                            ErrorCode = (string)rs["source"]["error_code"],
                            TransactionId = (string)rs["source"]["transaction_id"],
                            Message = (string)rs["source"]["message"]
                        };
                    }
                    if (this.SourceType == SourceType.CreditCard)
                    {
                        payment.Source = new CreditCard()
                        {
                            Type = (string)rs["source"]["type"],
                            Company = (string)rs["source"]["company"],
                            Name = (string)rs["source"]["name"],
                            Number = (string)rs["source"]["number"],
                            Message = (string)rs["source"]["message"]
                        };
                    }
                    return payment;
                }
            }
            catch (WebException webEx)
            {
                throw HandleRequestErrors(webEx);
            }
        }

        public PaymentResult Fetch(string id)
        {
            var finalUrl = MakePaymentUrl + "/" + id;
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(finalUrl);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "GET";
            httpWebRequest.Credentials = new NetworkCredential(ApiKey, ApiKey);

            try
            {
                var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var rs = JObject.Parse(result);

                    PaymentResult payment = new PaymentResult
                    {
                        Id = (string)rs["id"],
                        Status = (string)rs["status"],
                        Amount = (int)rs["amount"],
                        Description = (string)rs["description"],
                        Currency = (string)rs["currency"],
                        AmountFormat = (string)rs["amount_format"],
                        CreatedAt = (string)rs["created_at"],
                        Fee = (string)rs["fee"],
                        FeeFormat = (string)rs["fee_format"],
                        InvoiceId = (string)rs["invoice_id"],
                        Ip = (string)rs["ip"],
                        Refunded = (string)rs["refunded"],
                        RefundedAt = (string)rs["refunded_at"],
                        UpdatedAt = (string)rs["updated_at"]

                    };
                    if ("sadad" == (string)rs["source"]["type"])
                    {
                        payment.Source = new SadadType()
                        {
                            Type = (string)rs["source"]["type"],
                            Username = (string)rs["source"]["username"],
                            TransactionUrl = (string)rs["source"]["transaction_url"],
                            ErrorCode = (string)rs["source"]["error_code"],
                            TransactionId = (string)rs["source"]["transaction_id"],
                            Message = (string)rs["source"]["message"]
                        };
                    }
                    if ("creditcard" == (string)rs["source"]["type"])
                    {
                        payment.Source = new CreditCard()
                        {
                            Type = (string)rs["source"]["type"],
                            Company = (string)rs["source"]["company"],
                            Name = (string)rs["source"]["name"],
                            Number = (string)rs["source"]["number"],
                            Message = (string)rs["source"]["message"],
                        };
                    }
                    return payment;
                }
            }
            catch (WebException webEx)
            {
                throw HandleRequestErrors(webEx);
            }
        }

        public PaymentResult Refund(string id, string amount = null)
        {
            var finalUrl = MakePaymentUrl + "/" + id + "/refund";
            finalUrl = amount == null ? finalUrl : (finalUrl + "? amount = " + amount);
            if (amount != null && amount.Equals("0"))
            {
                MoyasarValidationException ex = new MoyasarValidationException(EnMessages.AmountNotZero);
                throw ex;
            }

            var httpWebRequest = (HttpWebRequest) WebRequest.Create(finalUrl);
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

                    PaymentResult payment = new PaymentResult
                    {
                        Id = (string)rs["id"],
                        Status = (string)rs["status"],
                        Amount = (int)rs["amount"],
                        Description = (string)rs["description"],
                        Currency = (string)rs["currency"],
                        AmountFormat = (string)rs["amount_format"],
                        CreatedAt = (string)rs["created_at"],
                        Fee = (string)rs["fee"],
                        FeeFormat = (string)rs["fee_format"],
                        InvoiceId = (string)rs["invoice_id"],
                        Ip = (string)rs["ip"],
                        Refunded = (string)rs["refunded"],
                        RefundedAt = (string)rs["refunded_at"],
                        UpdatedAt = (string)rs["updated_at"]

                    };
                    if ("sadad" == (string)rs["source"]["type"])
                    {
                        payment.Source = new SadadType()
                        {
                            Type = (string)rs["source"]["type"],
                            Username = (string)rs["source"]["username"],
                            TransactionUrl = (string)rs["source"]["transaction_url"],
                            ErrorCode = (string)rs["source"]["error_code"],
                            TransactionId = (string)rs["source"]["transaction_id"],
                            Message = (string)rs["source"]["message"]
                        };
                    }
                    if ("creditcard" == (string)rs["source"]["type"])
                    {
                        payment.Source = new CreditCard()
                        {
                            Type = (string)rs["source"]["type"],
                            Company = (string)rs["source"]["company"],
                            Name = (string)rs["source"]["name"],
                            Number = (string)rs["source"]["number"],
                            Message = (string)rs["source"]["message"],
                        };
                    }
                    return payment;
                }
            }
            catch (WebException webEx)
            {
                throw HandleRequestErrors(webEx);
            }
        }

        public PaymentListResult List(int? page = null)
        {
            var finalUrl = page == null ? MakePaymentUrl : MakePaymentUrl + "?page=" + page.ToString();

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
                    var listResult = new PaymentListResult()
                    {
                        Payments = new List<PaymentResult>(),
                        Meta = new MetaResult()
                    };

                    var paymentList = rs["payments"];
                    foreach (var item in paymentList)
                    {
                        PaymentResult payment = new PaymentResult
                        {
                            Id = (string)item["id"],
                            Status = (string)item["status"],
                            Amount = (int)item["amount"],
                            Description = (string)item["description"],
                            Currency = (string)item["currency"],
                            AmountFormat = (string)item["amount_format"],
                            CreatedAt = (string)item["created_at"],
                            Fee = (string)item["fee"],
                            FeeFormat = (string)item["fee_format"],
                            InvoiceId = (string)item["invoice_id"],
                            Ip = (string)item["ip"],
                            Refunded = (string)item["refunded"],
                            RefundedAt = (string)item["refunded_at"],
                            UpdatedAt = (string)item["updated_at"]

                        };
                        if ("sadad" == (string)item["source"]["type"])
                        {
                            payment.Source = new SadadType()
                            {
                                Type = (string)item["source"]["type"],
                                Username = (string)item["source"]["username"],
                                TransactionUrl = (string)item["source"]["transaction_url"],
                                ErrorCode = (string)item["source"]["error_code"],
                                TransactionId = (string)item["source"]["transaction_id"],
                                Message = (string)item["source"]["message"]
                            };
                        }
                        if ("creditcard" == (string)item["source"]["type"])
                        {
                            payment.Source = new CreditCard()
                            {
                                Type = (string)item["source"]["type"],
                                Company = (string)item["source"]["company"],
                                Name = (string)item["source"]["name"],
                                Number = (string)item["source"]["number"],
                                Message = (string)item["source"]["message"]
                            };
                        }
                        listResult.Payments.Add(payment);
                    }

                    //rs
                    listResult.Meta.CurrentPage = (string)rs["meta"]["current_page"];
                    listResult.Meta.NextPage = (string)rs["meta"]["next_page"];
                    listResult.Meta.PrevPage = (string)rs["meta"]["prev_page"];
                    listResult.Meta.TotalCount = (string)rs["meta"]["total_pages"];
                    listResult.Meta.TotalPages = (string)rs["meta"]["total_count"];
                    return listResult;
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

        public PaymentResult CreatePay()
        {
            return Create();
        }

        public PaymentResult GetPaymentById(string id)
        {
            return Fetch(id);
        }

        public PaymentListResult GetPaymentsList()
        {
            return List();
        }
    }
}
