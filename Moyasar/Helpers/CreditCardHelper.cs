using System.Text.RegularExpressions;

namespace Moyasar.Helpers
{
    public static class CreditCardHelper
    {
        private static readonly Regex VisaNumberPattern;
        private static readonly Regex MasterCardNumberPattern;

        static CreditCardHelper()
        {
            VisaNumberPattern = new Regex(@"^4[0-9]{12}(?:[0-9]{3})?$");
            MasterCardNumberPattern = new Regex(@"^(?:5[1-5][0-9]{2}|222[1-9]|22[3-9][0-9]|2[3-6][0-9]{2}|27[01][0-9]|2720)[0-9]{12}$");
        }

        public static bool IsVisa(string number)
        {
            return VisaNumberPattern.IsMatch(number);
        }

        public static bool IsMasterCard(string number)
        {
            return MasterCardNumberPattern.IsMatch(number);
        }

        public static CreditCardType? GetCreditCardType(string number)
        {
            if (number == null) return null;
            
            if (IsVisa(number)) return CreditCardType.Visa;
            if (IsMasterCard(number)) return CreditCardType.MasterCard;
            
            return null;
        }
    }

    public enum CreditCardType
    {
        Visa = 0,
        MasterCard = 1
    }
}