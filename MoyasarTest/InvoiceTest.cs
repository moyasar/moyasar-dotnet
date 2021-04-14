using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moyasar.Exceptions;
using Moyasar.Models;
using Moyasar.Services;
using MoyasarTest.Helpers;
using Xunit;

namespace MoyasarTest
{
    public class InvoiceTest
    {
        [Fact]
        public async void TestInvoiceInfoValidation()
        {
            var info = GetValidInvoiceInfo();
            info.Validate();
        
            info.Amount = 0;
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => info.Validate()));
        
            info = GetValidInvoiceInfo();
            info.Currency = "";
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => info.Validate()));
            
            info = GetValidInvoiceInfo();
            info.Description = "";
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => info.Validate()));
            
            info = GetValidInvoiceInfo();
            info.ExpiredAt = DateTime.Now.AddDays(-1);
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => info.Validate()));
            
            info = GetValidInvoiceInfo();
            info.CallbackUrl = "not a valid url";
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => info.Validate()));
        }
        
        [Fact]
        public void TestCreateInvoice()
        {
            ServiceMockHelper.MockJsonResponse("Fixtures/Invoice/Initiated.json");

            var invoice = Invoice.Create(GetValidInvoiceInfo());
            Assert.IsType<Invoice>(invoice);
            
            Assert.Equal(7000, invoice.Amount);
            Assert.Equal("SAR", invoice.Currency);
            Assert.Equal("A 70 SAR invoice just because", invoice.Description);
            Assert.Equal(DateTime.Parse("2016-04-07T06:45:18.866Z").ToUniversalTime(), invoice.ExpiredAt);
            Assert.Equal("http://www.example.com/invoice_callback", invoice.CallbackUrl);
        }
        
        [Fact]
        public void TestFetchInvoice()
        {
            ServiceMockHelper.MockJsonResponse("Fixtures/Invoice/Paid.json");

            var invoice = Invoice.Fetch("f91065f7-d188-4ec8-8fc5-af97841ec14e");
            
            Assert.Equal("f91065f7-d188-4ec8-8fc5-af97841ec14e", invoice.Id);
            Assert.Equal(7000, invoice.Amount);
            Assert.Equal("SAR", invoice.Currency);
            Assert.Equal("A 70 SAR invoice just because", invoice.Description);
            Assert.Equal(DateTime.Parse("2021-04-07T06:45:18.866Z").ToUniversalTime(), invoice.ExpiredAt);
            Assert.Equal("http://www.example.com/invoice_callback", invoice.CallbackUrl);
            Assert.Equal("de92988a-34bd-43a5-963f-b757cf02de7b", invoice.Metadata["order_id"]);
            
            Assert.Single(invoice.Payments);
            
            Assert.Equal("a4a144ba-adc3-43bd-98e8-c80f2925fdc4", invoice.Payments[0].Id);
            Assert.Equal(7000, invoice.Payments[0].Amount);
            Assert.Equal("SAR", invoice.Payments[0].Currency);
            Assert.Equal("A 70 SAR invoice just because", invoice.Payments[0].Description);
        }
        
        [Fact]
        public void TestUpdateInvoice()
        {
            ServiceMockHelper.MockJsonResponse("Fixtures/Invoice/Initiated.json");
            var invoice = Invoice.Fetch("f91065f7-d188-4ec8-8fc5-af97841ec14e");

            ServiceMockHelper.MockJsonResponse("Fixtures/Invoice/Updated.json");
            invoice.Update();
            
            Assert.Equal(8000, invoice.Amount);
            Assert.Equal("USD", invoice.Currency);
            Assert.Equal("An 80 USD invoice just because", invoice.Description);
        }
        
        [Fact]
        public void TestCancelInvoice()
        {
            ServiceMockHelper.MockJsonResponse("Fixtures/Invoice/Initiated.json");
            var invoice = Invoice.Fetch("some-random-id");
            
            ServiceMockHelper.MockJsonResponse("Fixtures/Invoice/Canceled.json");
            invoice.Cancel();
            Assert.Equal(7000, invoice.Amount);
            Assert.Equal("canceled", invoice.Status);
        }
        
        [Fact]
        public void TestInvoiceListing()
        {
            ServiceMockHelper.MockJsonResponse("Fixtures/Invoice/List.json");
            var pagination = Invoice.List();
            
            Assert.Equal(2, pagination.Items.Count);
            
            Assert.Equal(7000, pagination.Items[0].Amount);
            Assert.Equal("SAR", pagination.Items[0].Currency);
            Assert.Equal(DateTime.Parse("2016-04-07T06:45:18.866Z").ToUniversalTime(), pagination.Items[0].ExpiredAt);
            Assert.Equal("9e5c7df4-b796-4c83-9a61-e304c9c8fa51", pagination.Items[0].Metadata["order_id"]);
            
            Assert.Equal(2, pagination.CurrentPage);
            Assert.Equal(3, pagination.NextPage);
            Assert.Equal(1, pagination.PreviousPage);
            Assert.Equal(3, pagination.TotalPages);
        }
        
        internal static InvoiceInfo GetValidInvoiceInfo()
        {
            return new InvoiceInfo
            {
                Amount = 7000,
                Currency = "SAR",
                Description = "A 70 SAR invoice just because",
                ExpiredAt = DateTime.Now.AddDays(3),
                CallbackUrl = "http://www.example.com/invoice_callback"
            };
        }
    }
}