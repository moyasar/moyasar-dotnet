
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
      public Sadad SadadType { get; set; }
      public SourceType SourceType { get; set; }

      public string IniParameters()
      {
          StringBuilder sb =new StringBuilder();
            //sb.Append("?amount=");
            //sb.Append(this.Amount);
            //sb.Append("&currency=");
            //sb.Append(this.Currency);
            //sb.Append("&description=");
            //sb.Append(this.Description);

            //  if (this.SourceType == SourceType.CreditCard)
            //  {
            //      //source:
            //      //{
            //      //    type: 'creditcard',
            //      //          name: 'Abdulaziz Nasser',
            //      //          number: '4111111111111111',
            //      //          cvc: 331,
            //      //          month: 12,
            //      //          year: 2017
            //      //        }

            //  }

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
            

           var sm =  JsonConvert.SerializeObject(q);
            return sm;

      }
      public PaymentResult CreatePayment()
      {
         Validation();
          if (Auth())
          {
              var s = IniParameters();
              string result = WebClient.UploadString(MakePaymentUrl, s);
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
