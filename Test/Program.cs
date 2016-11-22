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
            p.Auth();
           p.SourceType=SourceType.CreditCard;
            p.CreatePayment();
        }
    }
}
