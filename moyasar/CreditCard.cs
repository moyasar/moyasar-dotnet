namespace moyasar
{
    public class CreditCard : SourceReaultBase
    {
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

        public string Cvc { get; set; }
    }
}
