using System.Text.RegularExpressions;

namespace Moyasar.Helpers
{
    /// <summary>
    /// Contains a number of methods that helps with credit card validation
    /// </summary>
    public static class CreditCardHelper
    {
        private static readonly Regex VisaNumberPattern;
        private static readonly Regex MasterCardNumberPattern;

        static CreditCardHelper()
        {
            VisaNumberPattern = new Regex(@"^4[0-9]{12}(?:[0-9]{3})?$");
            MasterCardNumberPattern =
                new Regex(@"^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$");
        }

        /// <summary>
        /// Check whether a given number is a valid Visa number 
        /// </summary>
        /// <param name="number">Number to check</param>
        /// <returns>True when the number given is a valid Visa number</returns>
        public static bool IsVisa(string number)
        {
            return VisaNumberPattern.IsMatch(number);
        }

        /// <summary>
        /// Check whether a given number is a valid MasterCard number
        /// </summary>
        /// <param name="number">Number to check</param>
        /// <returns>True when the number given is a valid MasterCard number</returns>
        public static bool IsMasterCard(string number)
        {
            return MasterCardNumberPattern.IsMatch(number);
        }

        /// <summary>
        /// Get credit card type for a given number
        /// </summary>
        /// <param name="number">Credit card number</param>
        /// <returns>Credit card type</returns>
        public static CreditCardType? GetCreditCardType(string number)
        {
            if (number == null)
            {
                return null;
            }

            if (IsVisa(number))
            {
                return CreditCardType.Visa;
            }

            if (IsMasterCard(number))
            {
                return CreditCardType.MasterCard;
            }

            return null;
        }
    }

    /// <summary>
    /// List of known credit card types
    /// </summary>
    public enum CreditCardType
    {
        Visa = 0,
        MasterCard = 1
    }
}
