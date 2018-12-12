using System;

namespace Moyasar
{
    [Serializable]
    public class CreditCard : SourceResultBase
    {
        public string Company { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Cvc { get; set; }
        public string TransactionUrl { get; set; }
    }
}
