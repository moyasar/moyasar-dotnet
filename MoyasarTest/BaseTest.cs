using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Moq;
using Moyasar.Exceptions;
using MoyasarTest.Helpers;
using Xunit;

namespace MoyasarTest
{
    public class BaseTest
    {
        [Fact]
        public async void TestSendRequestMustThrowApiExceptionOnErrorCode()
        {
            HttpMockHelper.MockHttpResponse(HttpStatusCode.NotFound, File.ReadAllText("Fixtures/ObjectNotFound.json"));
            
            await Assert.ThrowsAsync<ApiException>(async () =>
            {
                Moyasar.Moyasar.SendRequest(
                    "GET",
                    "http://someurl/",
                    null
                );
            });
        }
        
        [Fact]
        public async void TestSendRequestMustThrowTransportExceptionWhenCantConnect()
        {
            HttpMockHelper.MockTransportError();
            
            await Assert.ThrowsAsync<TransportException>(async () =>
            {
                Moyasar.Moyasar.SendRequest(
                    "GET",
                    "http://someurl/",
                    null
                );
            });
        }

        [Fact]
        public void TestUrlParamsBuilder()
        {
            var url = "https://api.moyasar.com/v1/payments";
            var urlParams = new Dictionary<string, object>()
            {
                { "id", "81c0fc10-9424-476d-b2c3-67e7aae1088a" },
                { "created[gt]", "13/12/2017" }
            };
            
            Assert.Equal(
                "https://api.moyasar.com/v1/payments?id=81c0fc10-9424-476d-b2c3-67e7aae1088a&created[gt]=13/12/2017",
                Moyasar.Moyasar.AppendUrlParameters(url, urlParams));
        }
    }
}