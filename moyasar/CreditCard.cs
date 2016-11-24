
namespace moyasar
{
   public class CreditCard
    {
        /// <summary>
        /// type of payment creditcard or sadad
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// credit card’s company
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// credit card’s holder name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// credit card’s masked number
        /// </summary>
        public string number { get; set; }

       public int Month { get; set; }
       public int Year { get; set; }

        /// <summary>
        /// payment gateway message
        /// </summary>
        public string message { get; set; }

       public string cvc { get; set; }
    }
}
