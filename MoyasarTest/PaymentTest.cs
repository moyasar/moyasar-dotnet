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
        public void TestCreatePayment2()
        {
            var paymentInfo = GetValidPaymentInfoSadadAccount();
            const string sadadMessage = "Paid Successfully";
            ServicesMockHelper.MockPaymentResponse(paymentInfo, sadadMessage: sadadMessage);
            
            var payment = Payment.Create(paymentInfo);
            Assert.Equal(paymentInfo.Amount, payment.Amount);
            Assert.Equal(paymentInfo.Currency, payment.Currency);
            Assert.Equal(paymentInfo.Description, payment.Description);
            Assert.Equal(paymentInfo.CallbackUrl, payment.CallbackUrl);
            
            Assert.IsType<Sadad>(payment.Source);
            Assert.Equal(((SadadSource) paymentInfo.Source).UserName, ((Sadad) payment.Source).UserName);
            Assert.Equal(sadadMessage, ((Sadad) payment.Source).Message);
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
            var infoList = new List<PaymentInfo>
            {
                GetValidPaymentInfoVisa(),
                GetValidPaymentInfoSadadAccount()
            };
            
            ServicesMockHelper.MockPaymentListResponse(infoList, nextPage: 2, totalPages: 2, totalCount: 4);
            var pagination = Payment.List();

            Assert.IsType<CreditCard>(pagination.Items[0].Source);
            Assert.IsType<Sadad>(pagination.Items[1].Source);
            
            Assert.Equal(1, pagination.CurrentPage);
            Assert.Equal(2, pagination.NextPage);
            Assert.Equal(2, pagination.TotalPages);
            Assert.Equal(4, pagination.TotalCount);
            
            ServicesMockHelper.MockPaymentListResponse(infoList, 2, prevPage: 1, totalPages: 2, totalCount: 4);
            pagination = pagination.GetNextPage();
            
            Assert.Equal(2, pagination.CurrentPage);
            Assert.Equal(1, pagination.PreviousPage);
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

        internal static SadadSource GetValidSadadSource()
        {
            return new SadadSource()
            {
                UserName = "johndoe123"
            };
        }
    }
}