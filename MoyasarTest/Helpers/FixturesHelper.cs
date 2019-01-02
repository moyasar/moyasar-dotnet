using System;
using System.IO;
using Moyasar.Services.Models;
using MoyasarBase = Moyasar.Moyasar;

namespace MoyasarTest.Helpers
{   
    public static class FixturesHelper
    {        
        public static string GetPaymentJson(PaymentInfo info, 
            string id = null, 
            string status = "initiated", 
            int refunded = 0,
            string invoiceId = null,
            string ccCompany = "visa"
        )
        {
            if (id == null) id = Guid.NewGuid().ToString().ToLower();
            var paymentJson = File.ReadAllText("Fixtures/Payment.json");

            var source = "";
            if (info.Source is CreditCardSource)
            {
                source = String.Format(
                    File.ReadAllText("Fixtures/CcSourceResult.json"),
                    ccCompany,
                    (info.Source as CreditCardSource).Name
                );
            }
            else
            {
                source = String.Format(
                    File.ReadAllText("Fixtures/SadadSourceResult.json")
                );
            }

            return String.Format(
                paymentJson,
                id,
                status,
                info.Amount,
                info.Currency,
                refunded,
                info.Description,
                info.Amount / 100.0,
                invoiceId ?? "null",
                source,
                info.CallbackUrl
            );
        }
    }
}