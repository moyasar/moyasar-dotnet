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
            var info = GetValidInvoiceInfo();
            var payInfo = PaymentTest.GetValidPaymentInfoVisa();
            ServicesMockHelper.MockInvoiceResponse(info, new List<PaymentInfo>
            {
                payInfo,
                payInfo
            });

            var invoice = Invoice.Create(info);
            
            Assert.Equal(info.Amount, invoice.Amount);
            Assert.Equal(info.Currency, invoice.Currency);
            Assert.Equal(info.Description, invoice.Description);
            Assert.Equal(info.ExpiredAt, invoice.ExpiredAt);
            Assert.Equal(info.CallbackUrl, invoice.CallbackUrl);
            
            Assert.Equal(2, invoice.Payments.Count);
            
            Assert.Equal(payInfo.Amount, invoice.Payments[0].Amount);
            Assert.Equal(payInfo.Currency, invoice.Payments[0].Currency);
            Assert.Equal(payInfo.Description, invoice.Payments[0].Description);
        }

        [Fact]
        public void TestFetchInvoice()
        {
            var info = GetValidInvoiceInfo();
            var payInfo = PaymentTest.GetValidPaymentInfoVisa();
            ServicesMockHelper.MockInvoiceResponse(info, new List<PaymentInfo>
            {
                payInfo,
                payInfo
            });

            var invoice = Invoice.Fetch("some-random-id");
            
            Assert.Equal(info.Amount, invoice.Amount);
            Assert.Equal(info.Currency, invoice.Currency);
            Assert.Equal(info.Description, invoice.Description);
            Assert.Equal(info.ExpiredAt, invoice.ExpiredAt);
            Assert.Equal(info.CallbackUrl, invoice.CallbackUrl);
            
            Assert.Equal(2, invoice.Payments.Count);
            
            Assert.Equal(payInfo.Amount, invoice.Payments[0].Amount);
            Assert.Equal(payInfo.Currency, invoice.Payments[0].Currency);
            Assert.Equal(payInfo.Description, invoice.Payments[0].Description);
        }

        [Fact]
        public void TestUpdateInvoice()
        {
            var info = GetValidInvoiceInfo();
            var payInfo = PaymentTest.GetValidPaymentInfoVisa();
            ServicesMockHelper.MockInvoiceResponse(info, new List<PaymentInfo>
            {
                payInfo,
                payInfo
            });

            var invoice = Invoice.Fetch("some-random-id");

            info.Amount = 1;
            info.Currency = "USD";
            info.Description = "NEW";
            ServicesMockHelper.MockInvoiceResponse(info, new List<PaymentInfo>
            {
                payInfo,
                payInfo
            });
            
            invoice.Update();
            
            Assert.Equal(info.Amount, invoice.Amount);
            Assert.Equal(info.Currency, invoice.Currency);
            Assert.Equal(info.Description, invoice.Description);
        }
        
        [Fact]
        public void TestCancelInvoice()
        {
            var info = GetValidInvoiceInfo();
            var payInfo = PaymentTest.GetValidPaymentInfoVisa();
            ServicesMockHelper.MockInvoiceResponse(info, new List<PaymentInfo>
            {
                payInfo,
                payInfo
            });

            var invoice = Invoice.Fetch("some-random-id");

            info.Amount = 1;
            ServicesMockHelper.MockInvoiceResponse(info, new List<PaymentInfo>
            {
                payInfo,
                payInfo
            });
            
            invoice.Cancel();
            Assert.Equal(info.Amount, invoice.Amount);
        }

        [Fact]
        public void TestInvoiceListing()
        {
            var infoList = new List<InvoiceInfo>
            {
                GetValidInvoiceInfo(),
                GetValidInvoiceInfo()
            };
            
            ServicesMockHelper.MockInvoiceListResponse(infoList);
            var invoices = Invoice.List();
            Assert.Equal(2, invoices.Count);
            
            Assert.Equal(infoList[0].Amount, invoices[0].Amount);
            Assert.Equal(infoList[0].Currency, invoices[0].Currency);
            Assert.Equal(infoList[0].ExpiredAt, invoices[0].ExpiredAt);
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