
using System;
using System.IO;
using System.Net;
using System.Text;
using moyasar.ExceptionsMap;
using moyasar.MessagesMap;
using Newtonsoft.Json;

namespace moyasar
{
  public  class Payment:MoyasarBase
    {
      
      public double Amount { get; set; }
      public string Currency { get; set; }
      public string Description { get; set; }
      public CreditCard CreditCardType { get; set; }
      public SadadType SadadType { get; set; }
      public SourceType SourceType { get; set; }

      public string IniParameters()
      {
          StringBuilder sb =new StringBuilder();
             

          var q =new object();
          if (this.SourceType== SourceType.CreditCard)
          {
               
                  q = new {amount =this. Amount, currency = this. Currency,
                      description = this. Description, source = new
                      {
                          type = this.CreditCardType.type,
                          name = this.CreditCardType.name,
                          number = this.CreditCardType.number,
                          cvc = this.CreditCardType.cvc,
                          month= this.CreditCardType.Month,
                          year = this.CreditCardType.Year
                      }
                  };
          }
          if (this.SourceType == SourceType.Sadad)
          {
              q = new
              {
                  amount = this.Amount,
                  currency = this.Currency,
                  description = this.Description,
                  source = new
                  {
                      type = this.SadadType.type,
                      username = this.SadadType.username,
                      success_url = this.SadadType.success_url,
                      fail_url = this.SadadType.fail_url,
                      
                  }
              };
            }
            

           var sm =  JsonConvert.SerializeObject(q);
            return sm;

      }
      public PaymentResult CreatePayment()
      {
         Validation();
          if (Auth())
          {
              var s = IniParameters();
                JavaScriptSerializer js = new JavaScriptSerializer();

                  // Create a request using a URL that can receive a post. 
                  Request = WebRequest.Create(MakePaymentUrl);
                Request.Credentials = new NetworkCredential(ApiKey, ApiKey);
                // Set the Method property of the request to POST.
                Request.Method = "POST";

                // Create POST data and convert it to a byte array.
                string postData = s;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                // Set the ContentType property of the WebRequest.
                Request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                Request.ContentLength = byteArray.Length;


                // Get the request stream.
                Stream dataStream = Request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();

                // Get the response.
                WebResponse response = Request.GetResponse();

                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);


                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                // Display the content.
                Console.WriteLine(responseFromServer);
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();

            }


          return new PaymentResult();
        }


      private void Validation()
      {

            if (ApiKey == ""|| ApiKey==string.Empty)
            {
                var ex = new MoyasarValidationException(EnMessages.ApiKeyNotFound) { ErrorCode = "#1559" };
                throw ex;
            }
            if (SourceType == 0 )
            {
                var ex = new MoyasarValidationException(EnMessages.SelectType) { ErrorCode = "#1550" };
                throw ex;
            }

            if (this.CreditCardType == null && this.SourceType == SourceType.CreditCard)
            {
                var ex = new MoyasarValidationException(EnMessages.SelectCreditCardType) { ErrorCode = "#1555" };
                throw ex;
            }

            if (this.CreditCardType==null && this.SadadType == null)
            {
                var ex = new MoyasarValidationException(EnMessages.TypeEmpty) {ErrorCode = "#1500"};
                throw ex;
            }
            if (string.IsNullOrEmpty(this.Currency) || this.Currency == string.Empty)
            {
                var ex = new MoyasarValidationException(EnMessages.CurrencyEmpty) { ErrorCode = "#1000" };
                throw ex;
            }
            //check if this creditCard Type
            if (this.SourceType ==SourceType.CreditCard)
            {
                if (this.CreditCardType!=null)
                {
                    if (this.CreditCardType.Company== string.Empty)
                    {
                        var ex = new MoyasarValidationException(EnMessages.CreatedCardCompanyNotFound) { ErrorCode = "#1110" };
                        throw ex;
                    }
                    if (this.CreditCardType.name == string.Empty)
                    {
                        var ex = new MoyasarValidationException(EnMessages.CreatedCardNameNotFound) { ErrorCode = "#1111" };
                        throw ex;
                    }
                    if (this.CreditCardType.name == string.Empty)
                    {
                        var ex = new MoyasarValidationException(EnMessages.CreatedCardNameNotFound) { ErrorCode = "#1112" };
                        throw ex;
                    }
                    if (this.CreditCardType.number == string.Empty)
                    {
                        var ex = new MoyasarValidationException(EnMessages.CreatedCardNumberNotFound) { ErrorCode = "#1113" };
                        throw ex;
                    }
                    if (this.CreditCardType.Month == 0)
                    {
                        var ex = new MoyasarValidationException(EnMessages.CreatedCardNumberNotFound) { ErrorCode = "#1114" };
                        throw ex;
                    }
                    if (this.CreditCardType.Year == 0)
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
    }
}
