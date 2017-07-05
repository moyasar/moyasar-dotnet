using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Moyasar.ExceptionsMap;
using Moyasar.InvoiceArea;
using Moyasar.MessagesMap;
using Moyasar.PaymentArea.RefundMap;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Moyasar.PaymentArea
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
            if (this.SourceType == SourceType.CreditCard)
            {
                CreditCard crd = (CreditCard) SourceReault;
                q = new
                {
                    amount = this.Amount,
                    currency = this.Currency,
                    description = this.Description,
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

        public PaymentResult CreatePay()
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
                        id = (string)rs["id"],
                        status = (string)rs["status"],
                        amount = (int)rs["amount"],
                        description = (string)rs["description"],
                        currency = (string)rs["currency"],
                        amount_format = (string)rs["amount_format"],
                        created_at = (string)rs["created_at"],
                        fee = (string)rs["fee"],
                        fee_format = (string)rs["fee_format"],
                        invoice_id = (string)rs["invoice_id"],
                        ip = (string)rs["ip"],
                        refunded = (string)rs["refunded"],
                        refunded_at = (string)rs["refunded_at"],
                        updated_at = (string)rs["updated_at"]

                    };
                    if (this.SourceType == SourceType.Sadad)
                    {
                        payment.source = new SadadType()
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
                        payment.source = new CreditCard()
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

        public PaymentResult GetPaymentById(string id)
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
                        id = (string)rs["id"],
                        status = (string)rs["status"],
                        amount = (int)rs["amount"],
                        description = (string)rs["description"],
                        currency = (string)rs["currency"],
                        amount_format = (string)rs["amount_format"],
                        created_at = (string)rs["created_at"],
                        fee = (string)rs["fee"],
                        fee_format = (string)rs["fee_format"],
                        invoice_id = (string)rs["invoice_id"],
                        ip = (string)rs["ip"],
                        refunded = (string)rs["refunded"],
                        refunded_at = (string)rs["refunded_at"],
                        updated_at = (string)rs["updated_at"]

                    };
                    if ("sadad" == (string)rs["source"]["type"])
                    {
                        payment.source = new SadadType()
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
                        payment.source = new CreditCard()
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

        public MoyasarRefundBase Refund(string id, string amount)
        {
            var finalUrl = MakePaymentUrl + "/" + id + "/refund?amount=" + amount;
            if (amount.Equals("0"))
            {
                MoyasarValidationException ex = new MoyasarValidationException(EnMessages.AmountNotZero);
                throw ex;
            }

            try
            {
                var httpWebRequest = (HttpWebRequest) WebRequest.Create(finalUrl);
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "POST";
                httpWebRequest.Credentials = new NetworkCredential(ApiKey, ApiKey);

                var httpResponse = (HttpWebResponse) httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var rs = JObject.Parse(result);
                    RefundResult refundResult = new RefundResult();
                    var type = (string) rs["type"];
                    if (type == null)
                    {
                        refundResult = new RefundResult()
                        {
                            Currency = (string) rs["currency"],
                            Amount = (string) rs["amount"],
                            Id = (string) rs["id"],
                            Fee = (string) rs["fee"],
                            Refunded = (string) rs["refunded"],
                            RefundedAt = (string) rs["refunded_at"]
                        };
                        if ((string) rs["source"]["type"] == "creditcard")
                        {
                            refundResult.Source = new CreditCard()
                            {
                                Type = (string) rs["source"]["type"],
                                Company = (string) rs["source"]["company"],
                                Name = (string) rs["source"]["name"],
                                Number = (string) rs["source"]["number"],
                                Message = (string) rs["source"]["message"]
                            };
                        }
                        return refundResult;
                    }
                    else
                    {
                        RefundException exception = new RefundException
                        {
                            Type = (string) rs["type"],
                            Message = (string) rs["message"],
                            Error = (string) rs["errors"]

                        };
                        return exception;
                    }
                }
            }
            catch (Exception ex)
            {
                return new RefundException() { Message = ex.Message, Error = null, Type = ex.Source};
            }
        }

        public PaymentListResult GetPaymentsList()
        {
            var finalUrl = MakePaymentUrl;
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
                            id = (string)item["id"],
                            status = (string)item["status"],
                            amount = (int)item["amount"],
                            description = (string)item["description"],
                            currency = (string)item["currency"],
                            amount_format = (string)item["amount_format"],
                            created_at = (string)item["created_at"],
                            fee = (string)item["fee"],
                            fee_format = (string)item["fee_format"],
                            invoice_id = (string)item["invoice_id"],
                            ip = (string)item["ip"],
                            refunded = (string)item["refunded"],
                            refunded_at = (string)item["refunded_at"],
                            updated_at = (string)item["updated_at"]

                        };
                        if ("sadad" == (string)item["source"]["type"])
                        {
                            payment.source = new SadadType()
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
                            payment.source = new CreditCard()
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
    }
}
