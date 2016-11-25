
namespace moyasar
{
   public class CreditCard
    {
        /// <summary>
        /// type of payment creditcard or sadad
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// credit card’s company
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// credit card’s holder name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// credit card’s masked number
        /// </summary>
        public string Number { get; set; }

       public int Month { get; set; }
       public int Year { get; set; }

        /// <summary>
        /// payment gateway message
        /// </summary>
        public string Message { get; set; }

       public string Cvc { get; set; }
    }
}
