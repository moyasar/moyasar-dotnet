using Moyasar.Common;
using Moyasar.ExceptionsMap;
using Moyasar.MessagesMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Moyasar.Payments
{
    [Serializable]
    public class Payment : MoyasarBase
    {
        public int Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public string CallbackUrl { get; set; }

        public SourceResultBase SourceResult { get; set; }
        public SourceType SourceType { get; set; }

        public string IniParameters()
        {
            var q = new object();
            if (this.SourceType == SourceType.CreditCard)
            {
                CreditCard crd = (CreditCard)this.SourceResult;
                q = new
                {
                    amount = this.Amount,
                    currency = this.Currency,
                    description = this.Description,
                    callback_url = this.CallbackUrl,
                    source = new
                    {
                        type = crd.Type,
                        name = crd.Name,
                        number = crd.Number,
                        cvc = crd.Cvc,
                        month = crd.Month,
                        year = crd.Year
                    },
                };
            }
            if (this.SourceType == SourceType.Sadad)
            {
                SadadType Sd = (SadadType)this.SourceResult;
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

            var sm = this.js.Serialize(q); //JsonConvert.SerializeObject(q);
            return sm;
        }

        private void Validation()
        {
            if (ApiKey == "" || ApiKey == string.Empty)
            {
                var ex = new MoyasarValidationException(EnMessages.ApiKeyNotFound) { ErrorCode = "#1559" };
                throw ex;
            }
            if (this.SourceType == 0)
            {
                var ex = new MoyasarValidationException(EnMessages.SelectType) { ErrorCode = "#1550" };
                throw ex;
            }

            if (this.SourceResult == null && this.SourceType == SourceType.CreditCard)
            {
                var ex = new MoyasarValidationException(EnMessages.SelectCreditCardType) { ErrorCode = "#1555" };
                throw ex;
            }

            if (this.SourceResult == null)
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
                if (this.SourceResult != null)
                {
                    var credit = (CreditCard)this.SourceResult;
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
            this.Validation();
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(this.MakePaymentUrl);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Credentials = new NetworkCredential(ApiKey, ApiKey);
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = this.IniParameters();

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var rs = this.js.Deserialize<PaymentResult>(result); //JObject.Parse(result);

                    PaymentResult payment = rs; /*new PaymentResult
                    {
                        Id = (string)rs["id"],
                        Status = (string)rs["status"],
                        Amount = (int)rs["amount"],
                        Description = (string)rs["description"],
                        Currency = (string)rs["currency"],
                        CallbackUrl = (string)rs["callback_url"],
                        AmountFormat = (string)rs["amount_format"],
                        CreatedAt = (string)rs["created_at"],
                        Fee = (string)rs["fee"],
                        FeeFormat = (string)rs["fee_format"],
                        InvoiceId = (string)rs["invoice_id"],
                        Ip = (string)rs["ip"],
                        Refunded = (string)rs["refunded"],
                        RefundedAt = (string)rs["refunded_at"],
                        UpdatedAt = (string)rs["updated_at"]

                    };      */
                    if (this.SourceType == SourceType.Sadad)
                    {
                        //payment.Source = new SadadType()
                        //{
                        //    Type = (string)rs["source"]["type"],
                        //    Username = (string)rs["source"]["username"],
                        //    TransactionUrl = (string)rs["source"]["transaction_url"],
                        //    ErrorCode = (string)rs["source"]["error_code"],
                        //    TransactionId = (string)rs["source"]["transaction_id"],
                        //    Message = (string)rs["source"]["message"]
                        //};  
                        payment.Source = new SadadType()
                        {
                            Type = rs.Source.Type, //(string)rs["source"]["type"],
                            Username = rs.Source.UserName, //(string)rs["source"]["username"],
                            TransactionUrl = rs.Source.Transaction_Url, //(string)rs["source"]["transaction_url"],
                            ErrorCode = rs.Source.Error_Code,//(string)rs["source"]["error_code"],
                            TransactionId = rs.Source.Transaction_Id, //(string)rs["source"]["transaction_id"],
                            Message = rs.Source.Message //(string)rs["source"]["message"]
                        };
                    }
                    if (this.SourceType == SourceType.CreditCard)
                    {
                        /*  payment.Source = rs.Source; new CreditCard()
                          {
                              Type = (string)rs["source"]["type"],
                              Company = (string)rs["source"]["company"],
                              Name = (string)rs["source"]["name"],
                              Number = (string)rs["source"]["number"],
                              Message = (string)rs["source"]["message"],
                              TransactionUrl = (string)rs["source"]["transaction_url"],
                          };    */
                        payment.Source = new CreditCard()
                        {
                            Type = rs.Source.Type, // (string)rs["source"]["type"],
                            Company = rs.Source.Company,//(string)rs["source"]["company"],
                            Name = rs.Source.Name,//(string)rs["source"]["name"],
                            Number = rs.Source.Number, //(string)rs["source"]["number"],
                            Message = rs.Source.Message, //(string)rs["source"]["message"],
                            TransactionUrl = rs.Source.Transaction_Url //(string)rs["source"]["transaction_url"],
                        };
                    }
                    return payment;
                }
            }
            catch (WebException webEx)
            {
                throw this.HandleRequestErrors(webEx);
            }
        }

        public PaymentResult Fetch(string id)
        {
            var finalUrl = this.MakePaymentUrl + "/" + id;
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
                    var rs = this.js.Deserialize<PaymentResult>(result); //JObject.Parse(result);

                    PaymentResult payment = rs; /*new PaymentResult
                    {
                        Id = (string)rs["id"],
                        Status = (string)rs["status"],
                        Amount = (int)rs["amount"],
                        Description = (string)rs["description"],
                        Currency = (string)rs["currency"],
                        AmountFormat = (string)rs["amount_format"],
                        CallbackUrl = (string)rs["callback_url"],
                        CreatedAt = (string)rs["created_at"],
                        Fee = (string)rs["fee"],
                        FeeFormat = (string)rs["fee_format"],
                        InvoiceId = (string)rs["invoice_id"],
                        Ip = (string)rs["ip"],
                        Refunded = (string)rs["refunded"],
                        RefundedAt = (string)rs["refunded_at"],
                        UpdatedAt = (string)rs["updated_at"]

                    };        */
                    if ("sadad" == rs.Source.Type) //(string)rs["source"]["type"]
                    {
                        payment.Source = new SadadType()
                        {
                            Type = rs.Source.Type, //(string)rs["source"]["type"],
                            Username = rs.Source.UserName, //(string)rs["source"]["username"],
                            TransactionUrl = rs.Source.Transaction_Url, //(string)rs["source"]["transaction_url"],
                            ErrorCode = rs.Source.Error_Code,//(string)rs["source"]["error_code"],
                            TransactionId = rs.Source.Transaction_Id, //(string)rs["source"]["transaction_id"],
                            Message = rs.Source.Message //(string)rs["source"]["message"]
                        };
                    }
                    if ("creditcard" == rs.Source.Type)     //(string)rs["source"]["type"]
                    {
                        payment.Source = new CreditCard()
                        {
                            Type = rs.Source.Type, // (string)rs["source"]["type"],
                            Company = rs.Source.Company,//(string)rs["source"]["company"],
                            Name = rs.Source.Name,//(string)rs["source"]["name"],
                            Number = rs.Source.Number, //(string)rs["source"]["number"],
                            Message = rs.Source.Message, //(string)rs["source"]["message"],
                            TransactionUrl = rs.Source.Transaction_Url //(string)rs["source"]["transaction_url"],
                        };
                    }
                    return payment;
                }
            }
            catch (WebException webEx)
            {
                throw this.HandleRequestErrors(webEx);
            }
        }

        public PaymentResult Refund(string id, string amount = null)
        {
            var finalUrl = this.MakePaymentUrl + "/" + id + "/refund";
            finalUrl = amount == null ? finalUrl : (finalUrl + "? amount = " + amount);
            if (amount != null && amount.Equals("0"))
            {
                MoyasarValidationException ex = new MoyasarValidationException(EnMessages.AmountNotZero);
                throw ex;
            }

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(finalUrl);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";
            httpWebRequest.Credentials = new NetworkCredential(ApiKey, ApiKey);

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    var rs = this.js.Deserialize<PaymentResult>(result); //JObject.Parse(result);

                    PaymentResult payment = rs; /*new PaymentResult
                    {
                        Id = (string)rs["id"],
                        Status = (string)rs["status"],
                        Amount = (int)rs["amount"],
                        Description = (string)rs["description"],
                        CallbackUrl = (string)rs["callback_url"],
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

                    }; */
                    if ("sadad" == rs.Source.Type)  //(string)rs["source"]["type"]
                    {
                        //payment.Source = new SadadType()
                        //{
                        //    Type = (string)rs["source"]["type"],
                        //    Username = (string)rs["source"]["username"],
                        //    TransactionUrl = (string)rs["source"]["transaction_url"],
                        //    ErrorCode = (string)rs["source"]["error_code"],
                        //    TransactionId = (string)rs["source"]["transaction_id"],
                        //    Message = (string)rs["source"]["message"]
                        //};
                        payment.Source = new SadadType()
                        {
                            Type = rs.Source.Type, //(string)rs["source"]["type"],
                            Username = rs.Source.UserName, //(string)rs["source"]["username"],
                            TransactionUrl = rs.Source.Transaction_Url, //(string)rs["source"]["transaction_url"],
                            ErrorCode = rs.Source.Error_Code,//(string)rs["source"]["error_code"],
                            TransactionId = rs.Source.Transaction_Id, //(string)rs["source"]["transaction_id"],
                            Message = rs.Source.Message //(string)rs["source"]["message"]
                        };
                    }
                    if ("creditcard" == rs.Source.Type)  //(string)rs["source"]["type"]
                    {
                        //payment.Source = new CreditCard()
                        //{
                        //    Type = (string)rs["source"]["type"],
                        //    Company = (string)rs["source"]["company"],
                        //    Name = (string)rs["source"]["name"],
                        //    Number = (string)rs["source"]["number"],
                        //    Message = (string)rs["source"]["message"],
                        //    TransactionUrl = (string)rs["source"]["transaction_url"],
                        //};
                        payment.Source = new CreditCard()
                        {
                            Type = rs.Source.Type, // (string)rs["source"]["type"],
                            Company = rs.Source.Company,//(string)rs["source"]["company"],
                            Name = rs.Source.Name,//(string)rs["source"]["name"],
                            Number = rs.Source.Number, //(string)rs["source"]["number"],
                            Message = rs.Source.Message, //(string)rs["source"]["message"],
                            TransactionUrl = rs.Source.Transaction_Url //(string)rs["source"]["transaction_url"],
                        };
                    }
                    return payment;
                }
            }
            catch (WebException webEx)
            {
                throw this.HandleRequestErrors(webEx);
            }
        }

        public PaymentListResult List(int? page = null)
        {
            var finalUrl = page == null ? this.MakePaymentUrl : this.MakePaymentUrl + "?page=" + page.ToString();

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
                    var rs = this.js.Deserialize<PaymentListResult>(result); //JObject.Parse(result);
                    var listResult = new PaymentListResult()
                    {
                        Payments = new List<PaymentResult>(),
                        Meta = new MetaResult()
                    };

                    var paymentList = rs.Payments;//rs["payments"];
                    foreach (var item in paymentList)
                    {
                        PaymentResult payment = new PaymentResult
                        {
                            Id = item.Id, // (string)item["id"],
                            Status = item.Status, //(string)item["status"],
                            Amount = item.Amount, //(int)item["amount"],
                            Description = item.Description, //(string)item["description"],
                            Currency = item.Currency,// (string)item["currency"],
                            CallbackUrl = item.CallbackUrl,// (string)rs["callback_url"],
                            AmountFormat = item.AmountFormat, //(string)item["amount_format"],
                            CreatedAt = item.CreatedAt,// (string)item["created_at"],
                            Fee = item.Fee, //(string)item["fee"],
                            FeeFormat = item.FeeFormat,// (string)item["fee_format"],
                            InvoiceId = item.InvoiceId,//(string)item["invoice_id"],
                            Ip = item.Ip, //(string)item["ip"],
                            Refunded = item.Refunded, //(string)item["refunded"],
                            RefundedAt = item.RefundedAt, //(string)item["refunded_at"],
                            UpdatedAt = item.UpdatedAt //(string)item["updated_at"]
                        };
                        if ("sadad" == item.Source.Type) //(string)item["source"]["type"]
                        {
                            //payment.Source = new SadadType()
                            //{
                            //    Type = (string)item["source"]["type"],
                            //    Username = (string)item["source"]["username"],
                            //    TransactionUrl = (string)item["source"]["transaction_url"],
                            //    ErrorCode = (string)item["source"]["error_code"],
                            //    TransactionId = (string)item["source"]["transaction_id"],
                            //    Message = (string)item["source"]["message"]
                            //};
                            payment.Source = new SadadType()
                            {
                                Type = item.Source.Type, //(string)rs["source"]["type"],
                                Username = item.Source.UserName, //(string)rs["source"]["username"],
                                TransactionUrl = item.Source.Transaction_Url, //(string)rs["source"]["transaction_url"],
                                ErrorCode = item.Source.Error_Code,//(string)rs["source"]["error_code"],
                                TransactionId = item.Source.Transaction_Id, //(string)rs["source"]["transaction_id"],
                                Message = item.Source.Message //(string)rs["source"]["message"]
                            };
                        }
                        if ("creditcard" == item.Source.Type)//(string)item["source"]["type"]
                        {
                            //payment.Source = new CreditCard()
                            //{
                            //    Type = (string)item["source"]["type"],
                            //    Company = (string)item["source"]["company"],
                            //    Name = (string)item["source"]["name"],
                            //    Number = (string)item["source"]["number"],
                            //    Message = (string)item["source"]["message"],
                            //    TransactionUrl = (string)item["source"]["transaction_url"],
                            //};
                            payment.Source = new CreditCard()
                            {
                                Type = item.Source.Type, // (string)rs["source"]["type"],
                                Company = item.Source.Company,//(string)rs["source"]["company"],
                                Name = item.Source.Name,//(string)rs["source"]["name"],
                                Number = item.Source.Number, //(string)rs["source"]["number"],
                                Message = item.Source.Message, //(string)rs["source"]["message"],
                                TransactionUrl = item.Source.Transaction_Url //(string)rs["source"]["transaction_url"],
                            };
                        }
                        listResult.Payments.Add(payment);
                    }

                    //rs
                    listResult.Meta.CurrentPage = rs.Meta.CurrentPage; //(string)rs["meta"]["current_page"];
                    listResult.Meta.NextPage = rs.Meta.NextPage; //(string)rs["meta"]["next_page"];
                    listResult.Meta.PrevPage = rs.Meta.PrevPage;//(string)rs["meta"]["prev_page"];
                    listResult.Meta.TotalCount = rs.Meta.TotalCount; //(string)rs["meta"]["total_pages"];
                    listResult.Meta.TotalPages = rs.Meta.TotalPages; //(string)rs["meta"]["total_count"];
                    return listResult;
                }
            }
            catch (WebException webEx)
            {
                throw this.HandleRequestErrors(webEx);
            }
        }

        public IEnumerable<PaymentListResult> ListAll()
        {
            var allList = new PaymentListResult();
            int? nextPage = null;
            do
            {
                allList = this.List(nextPage);
                nextPage = Int32.Parse(allList.Meta.CurrentPage) + 1;
                yield return allList;
            } while (allList.Meta.NextPage != null);
        }

        /// <summary>
        /// Aliases for old names
        /// </summary>

        public PaymentResult CreatePay()
        {
            return this.Create();
        }

        public PaymentResult GetPaymentById(string id)
        {
            return this.Fetch(id);
        }

        public PaymentListResult GetPaymentsList()
        {
            return this.List();
        }
    }
}
