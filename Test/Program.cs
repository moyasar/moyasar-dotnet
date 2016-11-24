using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using moyasar;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Payment p = new Payment();
            p.ApiKey = "sk_test_73b6rMCw9N1zHz7Ki6foweoqqXTWnoi5GcVmEEhR";
            p.SourceType = SourceType.CreditCard;
            p.Currency = "SAR";
            p.Amount = 100;
            p.CreditCardType = new CreditCard() {name = "Visa",number = "122344444",Company = "Visa", Year = 16,Month = 01,message = "",type = "Visa"};
            
            p.CreatePayment();
        }
    }
}
