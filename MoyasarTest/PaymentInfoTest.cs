using System;
using System.Collections.Generic;
using Moyasar.Models;
using Newtonsoft.Json;
using Xunit;

namespace MoyasarTest
{
    public class PaymentInfoTest
    {
        [Fact]
        public void PaymentInfoShouldSerializeMetadata()
        {
            var info = new PaymentInfo
            {
                Amount = 8000,
                Currency = "SAR",
                Description = "Test",
                CallbackUrl = "https://aa.aa/cc",
                Source = new CreditCardSource(),
                Metadata = new Dictionary<string, string>
                {
                    {"order_id", "123"}
                }
            };

            var json = JsonConvert.SerializeObject(info);
            
            Assert.Contains(@"""metadata"":{""order_id"":""123""}", json);
        }
    }
}