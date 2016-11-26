using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
 
using moyasar.InvoiceArea;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
 

namespace moyasar.PaymentArea
{
    [Serializable]
    public class Payment:MoyasarBase
    {
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public SourceReaultBase  SourceReault { get; set; }
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
                        name =  crd.Name,
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
                         

                    }
                };
            }


            var sm = JsonConvert.SerializeObject(q);
            return sm;

        }

       public PaymentResult CreatePay()
       {
 
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(MakePaymentUrl);
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

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var rs = JObject.Parse(result);
                 
                PaymentResult payment = new PaymentResult
                {
                    id = (string) rs["id"],
                    status = (string) rs["status"],
                    amount = (string) rs["amount"],
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
                if (this.SourceType==SourceType.CreditCard)
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

        public PaymentResult GetPaymentById(string id)
        {
            var finalUrl = MakePaymentUrl + "/" + id;
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(finalUrl);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "GET";
            httpWebRequest.Credentials = new NetworkCredential(ApiKey, ApiKey);

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                var rs = JObject.Parse(result);

                PaymentResult payment = new PaymentResult
                {
                    id = (string)rs["id"],
                    status = (string)rs["status"],
                    amount = (string)rs["amount"],
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

            return null;
        }

         
    }
}
