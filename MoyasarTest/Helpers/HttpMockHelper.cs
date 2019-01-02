using System;
using System.IO;
using System.Net;
using System.Text;
using Moq;

namespace MoyasarTest.Helpers
{
    public static class HttpMockHelper
    {
        public static void MockHttpResponse(HttpStatusCode statusCode, string response)
        {
            var httpWebResponse = new Mock<HttpWebResponse>(MockBehavior.Loose);
            httpWebResponse.Setup(r => r.StatusCode).Returns(statusCode);

            if (response == null) response = "";
            
            httpWebResponse.Setup(r => r.GetResponseStream()).Returns(
                new MemoryStream(Encoding.UTF8.GetBytes(response))
            );

            var httpWebRequest = new Mock<HttpWebRequest>(MockBehavior.Loose);
            httpWebRequest.Setup(req => req.GetResponse()).Returns(httpWebResponse.Object);
            httpWebRequest.Setup(req => req.GetRequestStream()).Returns(new MemoryStream());
            
            Moyasar.Moyasar.HttpWebRequestFactory = url =>
            {
                httpWebRequest.Setup(req => req.RequestUri).Returns(new Uri(url));
                return httpWebRequest.Object;
            };
        }

        public static void MockTransportError()
        {
            var httpWebRequest = new Mock<HttpWebRequest>(MockBehavior.Loose);
            httpWebRequest.Setup(req => req.GetResponse()).Throws<WebException>();
            
            Moyasar.Moyasar.HttpWebRequestFactory = url =>
            {
                httpWebRequest.Setup(req => req.RequestUri).Returns(new Uri(url));
                return httpWebRequest.Object;
            };
        }
    }
}