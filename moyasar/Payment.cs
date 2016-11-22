
using moyasar.ExceptionsMap;
using moyasar.MessagesMap;

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
      public PaymentResult CreatePayment()
      {
         Validation();

          return new PaymentResult();
      }


      private void Validation()
      {
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
                var ex = new MoyasarValidationException(EnMessages.CurrencyEmpty) {ErrorCode = "#1000"};
                throw ex;
            }
        }
    }
}
