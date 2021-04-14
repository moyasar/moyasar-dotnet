using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using BasicDict = System.Collections.Generic.Dictionary<string, object>;

namespace MoyasarTest.Helpers
{
    public class ServiceMockHelper
    {
        public static void MockJsonResponse(string filename)
        {
            HttpMockHelper.MockHttpResponse(HttpStatusCode.OK, FixtureHelper.GetJsonObjectFixture(filename));
        }
    }
}
