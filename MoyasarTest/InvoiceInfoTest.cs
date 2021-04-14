using System;
using System.Collections.Generic;
using Moyasar.Models;
using Newtonsoft.Json;
using Xunit;

namespace MoyasarTest
{
    public class InvoiceInfoTest
    {
        [Fact]
        public void InvoiceInfoShouldSerializeMetadata()
        {
            var info = new InvoiceInfo
            {
                Amount = 8000,
                Currency = "SAR",
                Description = "Test",
                CallbackUrl = "https://aa.aa/cc",
                ExpiredAt = DateTime.Now,
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