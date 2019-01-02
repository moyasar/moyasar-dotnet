using System.Collections.Generic;
using System.Linq;
using System.Net;
using Moyasar.Services.Models;
using MoyasarBase = Moyasar.Moyasar;

namespace MoyasarTest.Helpers
{
    public class PaymentMockHelper
    {
        public static void MockPaymentResponse(
            PaymentInfo info,
            string id = null, 
            string status = "initiated", 
            int refunded = 0,
            string invoiceId = null,
            string ccCompany = "visa")
        {
            var paymentJson = FixturesHelper.GetPaymentJson(info, id, status, refunded, invoiceId, ccCompany);
            HttpMockHelper.MockHttpResponse(HttpStatusCode.OK, paymentJson);
        }

        public static void MockPaymentListResponse(List<PaymentInfo> infoList)
        {
            var paymentDictList = infoList
                .Select(info => MoyasarBase.Serializer.Deserialize<Dictionary<string, object>>(
                    FixturesHelper.GetPaymentJson(info)
                )).ToList();

            var listJson = MoyasarBase.Serializer.Serialize(paymentDictList);
            HttpMockHelper.MockHttpResponse(HttpStatusCode.OK, listJson);
        }
    }
}