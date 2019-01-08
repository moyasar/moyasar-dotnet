using System.Collections.Generic;
using System.Linq;
using System.Net;
using Moyasar;
using Moyasar.Models;
using Moyasar.Services;

namespace MoyasarTest.Helpers
{
    public class ServicesMockHelper
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
                .Select(info => MoyasarService.Serializer.Deserialize<Dictionary<string, object>>(
                    FixturesHelper.GetPaymentJson(info)
                )).ToList();

            var listJson = MoyasarService.Serializer.Serialize(new
            {
                payments = paymentDictList
            });
            HttpMockHelper.MockHttpResponse(HttpStatusCode.OK, listJson);
        }

        public static void MockInvoiceResponse(InvoiceInfo info,
            List<PaymentInfo> paymentsInfo = null, string id = null, string status = "initiated")
        {
            var listJson = "null";
            if (paymentsInfo != null)
            {
                var paymentDictList = paymentsInfo
                    .Select(i => MoyasarService.Serializer.Deserialize<Dictionary<string, object>>(
                        FixturesHelper.GetPaymentJson(i)
                    )).ToList();

                listJson = MoyasarService.Serializer.Serialize(paymentDictList);
            }
            
            var invoiceJson = FixturesHelper.GetInvoiceJson(info, listJson, id, status);
            HttpMockHelper.MockHttpResponse(HttpStatusCode.OK, invoiceJson);
        }
        
        public static void MockInvoiceListResponse(List<InvoiceInfo> infoList)
        {
            var invoiceDictList = infoList
                .Select(info => MoyasarService.Serializer.Deserialize<Dictionary<string, object>>(
                    FixturesHelper.GetInvoiceJson(info)
                )).ToList();

            var listJson = MoyasarService.Serializer.Serialize(invoiceDictList);
            HttpMockHelper.MockHttpResponse(HttpStatusCode.OK, listJson);
        }
    }
}