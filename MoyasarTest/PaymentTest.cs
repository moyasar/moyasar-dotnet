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
    public class PaymentTest
    {
        [Fact(DisplayName = "Method must return with no exceptions thrown if data are valid")]
        public void TestValidatePaymentInfo()
        {
            GetValidPaymentInfoVisa().Validate();
            GetValidPaymentInfoSadadAccount().Validate();
        }

        [Fact]
        public async void TestPaymentInfoValidationRules()
        {
            var info = GetValidPaymentInfoVisa();
            info.Validate();
            info.Amount = -1;
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => info.Validate()));
            
            info = GetValidPaymentInfoVisa();
            info.Validate();
            info.Source = null;
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => info.Validate()));
            
            info = GetValidPaymentInfoVisa();
            info.Validate();
            info.CallbackUrl = "hey";
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => info.Validate()));
            
            // Callback url is not required for Sadad Account
            info = GetValidPaymentInfoSadadAccount();
            info.Validate();
            info.CallbackUrl = "hey";
            info.Validate();
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
        public async void TestSadadSourceValidationRules()
        {
            var source = GetValidSadadSource();
            source.Validate();
            source.UserName = "";
            await Assert.ThrowsAsync<ValidationException>(async () => await Task.Run(() => source.Validate()));
        }

        [Fact]
        public void TestDeserializingPayment()
        {
            var paymentInfo = GetValidPaymentInfoVisa();
            ServicesMockHelper.MockPaymentResponse(paymentInfo);
            
            var payment = Payment.Fetch("some-random-id");
            Assert.Equal(paymentInfo.Amount, payment.Amount);
            Assert.Equal(paymentInfo.Currency, payment.Currency);
            Assert.Equal(paymentInfo.Description, payment.Description);
            Assert.Equal(paymentInfo.CallbackUrl, payment.CallbackUrl);
            
            Assert.IsType<CreditCard>(payment.Source);
            Assert.Equal(((CreditCardSource)paymentInfo.Source).Name, ((CreditCard)payment.Source).Name);
        }

        [Fact]
        public void TestCreatePayment()
        {
            var paymentInfo = GetValidPaymentInfoVisa();
            ServicesMockHelper.MockPaymentResponse(paymentInfo);
            
            var payment = Payment.Create(paymentInfo);
            Assert.Equal(paymentInfo.Amount, payment.Amount);
            Assert.Equal(paymentInfo.Currency, payment.Currency);
            Assert.Equal(paymentInfo.Description, payment.Description);
            Assert.Equal(paymentInfo.CallbackUrl, payment.CallbackUrl);
            
            Assert.IsType<CreditCard>(payment.Source);
            Assert.Equal(((CreditCardSource)paymentInfo.Source).Name, ((CreditCard)payment.Source).Name);
        }

        [Fact]
        public void TestRefundPayment()
        {
            var paymentInfo = GetValidPaymentInfoVisa();
            ServicesMockHelper.MockPaymentResponse(paymentInfo);
            
            var payment = Payment.Create(paymentInfo);
            var id = payment.Id;
            var amount = payment.Amount;
            
            ServicesMockHelper.MockPaymentResponse
            (
                paymentInfo, 
                id: id,
                status: "refunded",
                refunded: amount
            );

            payment.Refund();
            
            Assert.Equal(id, payment.Id);
            Assert.Equal("refunded", payment.Status);
            Assert.Equal(amount, payment.RefundedAmount);
        }

        [Fact]
        public void TestPaymentListing()
        {
            var infoList = new List<PaymentInfo>()
            {
                GetValidPaymentInfoVisa(),
                GetValidPaymentInfoSadadAccount()
            };
            
            ServicesMockHelper.MockPaymentListResponse(infoList);
            var payments = Payment.List();

            Assert.IsType<CreditCard>(payments[0].Source);
            Assert.IsType<SadadAccount>(payments[1].Source);
        }
        
        internal static PaymentInfo GetValidPaymentInfoVisa()
        {
            return new PaymentInfo()
            {
                Amount = 3500,
                Currency = "SAR",
                Description = "Chinese Noodles Meal",
                Source = GetValidCcSource(),
                CallbackUrl = "http://mysite.test/payment_callback"
            };
        }
        
        internal static PaymentInfo GetValidPaymentInfoSadadAccount()
        {
            return new PaymentInfo()
            {
                Amount = 3500,
                Currency = "SAR",
                Description = "Chinese Noodle Meal",
                Source = GetValidSadadSource(),
                CallbackUrl = "http://mysite.test/payment_callback"
            };
        }

        internal static CreditCardSource GetValidCcSource()
        {
            return new CreditCardSource()
            {
                Name = "John Doe",
                Number = "4111111111111111",
                Cvc = 141,
                Month = 3,
                Year = 2021,
            };
        }

        internal static SadadAccountSource GetValidSadadSource()
        {
            return new SadadAccountSource()
            {
                UserName = "johndoe123"
            };
        }
    }
}