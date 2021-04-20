using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moyasar.Abstraction;
using Moyasar.Exceptions;
using Moyasar.Models;
using Moyasar.Services;
using MoyasarTest.Helpers;
using Xunit;
using BasicDict = System.Collections.Generic.Dictionary<string, object>;

namespace MoyasarTest
{
    public class PaymentTest
    {
        [Fact(DisplayName = "Method must return with no exceptions thrown if data are valid")]
        public void TestValidatePaymentInfo()
        {
            GetValidPaymentInfo().Validate();
            GetValidPaymentInfo(GetValidApplePaySource()).Validate();
            GetValidPaymentInfo(GetValidStcPaySource()).Validate();
        }

        [Fact]
        public async void TestPaymentInfoValidationRules()
        {
            var info = GetValidPaymentInfo();
            info.Validate();
            info.Amount = -1;
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => info.Validate()));
            
            info = GetValidPaymentInfo();
            info.Validate();
            info.Source = null;
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => info.Validate()));
            
            info = GetValidPaymentInfo();
            info.Validate();
            info.CallbackUrl = "hey";
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => info.Validate()));
        }

        [Fact]
        public async void TestCcSourceValidationRules()
        {
            var source = GetValidCcSource();
            source.Validate();
            source.Name = "";
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => source.Validate()));
            
            source = GetValidCcSource();
            source.Validate();
            source.Number = "";
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => source.Validate()));
            
            source = GetValidCcSource();
            source.Validate();
            source.Cvc = 0;
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => source.Validate()));
            source.Cvc = 1000;
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => source.Validate()));
            
            source = GetValidCcSource();
            source.Validate();
            source.Month = 0;
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => source.Validate()));
            source.Month = 13;
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => source.Validate()));
            
            source = GetValidCcSource();
            source.Validate();
            source.Year = -1;
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => source.Validate()));
        }

        [Fact]
        public void TestDeserializingPayment()
        {
            ServiceMockHelper.MockJsonResponse("Fixtures/CreditCard/Paid.json");
            
            var payment = Payment.Fetch("b6c01c90-a091-45a4-9358-71668ecbf7ea");
            Assert.Equal("b6c01c90-a091-45a4-9358-71668ecbf7ea", payment.Id);
            Assert.Equal(1000, payment.Amount);
            Assert.Equal("SAR", payment.Currency);
            Assert.Equal("Test Payment", payment.Description);
            Assert.Equal("https://mystore.com/order/redirect-back", payment.CallbackUrl);
            Assert.Equal("5c02ba44-7fd1-444c-b82b-d3993b87d4b0", payment.Metadata["order_id"]);
            Assert.Equal("50", payment.Metadata["tax"]);
            
            Assert.IsType<CreditCard>(payment.Source);
            var ccSource = (CreditCard) payment.Source;
            
            Assert.Equal("Long John", ccSource.Name);
            Assert.Equal("XXXX-XXXX-XXXX-1111", ccSource.Number);
        }
        
        [Fact]
        public void TestDeserializingApplePayPayment()
        {
            ServiceMockHelper.MockJsonResponse("Fixtures/ApplePay/Paid.json");
            
            var payment = Payment.Fetch("a4a144ba-adc3-43bd-98e8-c80f2925fdc4");
            Assert.Equal(1000, payment.Amount);
            Assert.Equal("SAR", payment.Currency);
            Assert.Equal("Test Payment", payment.Description);
            Assert.Null(payment.CallbackUrl);
            
            Assert.IsType<ApplePayMethod>(payment.Source);
            var applePaySource = (ApplePayMethod) payment.Source;
            
            Assert.Equal("applepay", applePaySource.Type);
            Assert.Equal("XXXX-XXXX-XXXX-1111", applePaySource.Number);
            Assert.Equal("APPROVED", applePaySource.Message);
        }
        
        [Fact]
        public void TestDeserializingStcPayPayment()
        {
            ServiceMockHelper.MockJsonResponse("Fixtures/StcPay/Paid.json");
            
            var payment = Payment.Fetch("50559d3b-e67f-4b3a-8df8-509dde19fe38");
            Assert.Equal(1000, payment.Amount);
            Assert.Equal("SAR", payment.Currency);
            Assert.Equal("Test Payment", payment.Description);
            Assert.Null(payment.CallbackUrl);
            
            Assert.IsType<StcPayMethod>(payment.Source);
            var method = (StcPayMethod) payment.Source;
            Assert.Equal("stcpay", method.Type);
            Assert.Equal("0555555555", method.Mobile);
            Assert.Equal("Paid", method.Message);
        }

        [Fact]
        public void TestRefundPayment()
        {
            ServiceMockHelper.MockJsonResponse("Fixtures/CreditCard/Paid.json");
            
            var payment = Payment.Fetch("b6c01c90-a091-45a4-9358-71668ecbf7ea");
            var id = payment.Id;
            var amount = payment.Amount;
            
            ServiceMockHelper.MockJsonResponse("Fixtures/CreditCard/Refunded.json");

            payment.Refund();
            
            Assert.Equal(id, payment.Id);
            Assert.Equal("refunded", payment.Status);
            Assert.Equal(amount, payment.RefundedAmount);
            Assert.Equal(DateTime.Parse("2019-01-03T10:14:14.414Z").ToUniversalTime(), payment.RefundedAt);
        }
        
        [Fact]
        public async void RefundHigherAmountMustThrowException()
        {
            ServiceMockHelper.MockJsonResponse("Fixtures/CreditCard/Paid.json");
            
            var payment = Payment.Fetch("b6c01c90-a091-45a4-9358-71668ecbf7ea");
            var id = payment.Id;
            var amount = payment.Amount;
            
            ServiceMockHelper.MockJsonResponse("Fixtures/CreditCard/Refunded.json");

            await Assert.ThrowsAsync<ValidationException>
            (
                async () => await Task.Run(() => payment.Refund(amount + 1))
            );
        }

        [Fact]
        public void TestPaymentListing()
        {
            ServiceMockHelper.MockJsonResponse("Fixtures/PaymentList.json");
            var pagination = Payment.List();

            Assert.IsType<CreditCard>(pagination.Items[0].Source);
            Assert.IsType<ApplePayMethod>(pagination.Items[1].Source);
            Assert.IsType<StcPayMethod>(pagination.Items[2].Source);
            
            Assert.Equal(2, pagination.CurrentPage);
            Assert.Equal(3, pagination.NextPage);
            Assert.Equal(1, pagination.PreviousPage);
            Assert.Equal(3, pagination.TotalPages);
            Assert.Equal(9, pagination.TotalCount);
        }
        
        internal static PaymentInfo GetValidPaymentInfo(IPaymentSource source = null)
        {
            return new PaymentInfo
            {
                Amount = 3500,
                Currency = "SAR",
                Description = "Chinese Noodles Meal",
                Source = source ?? GetValidCcSource(),
                CallbackUrl = "http://mysite.test/payment_callback",
                Metadata = new Dictionary<string, string>
                {
                    {"order_id", "1232141"},
                    {"store_note", "okay"}
                }
            };
        }

        private static CreditCardSource GetValidCcSource()
        {
            return new CreditCardSource
            {
                Name = "John Doe",
                Number = "4111111111111111",
                Cvc = 141,
                Month = 3,
                Year = 2021,
            };
        }
        
        private static ApplePaySource GetValidApplePaySource()
        {
            return new ApplePaySource
            {
                Token = @"{""stuff"":""foobar""}"
            };
        }

        private static StcPaySource GetValidStcPaySource()
        {
            return new StcPaySource
            {
                Branch = "1",
                Cashier = "1",
                Mobile = "0555555555"
            };
        }
    }
}
