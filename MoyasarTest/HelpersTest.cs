using Moyasar.Helpers;
using Xunit;

namespace MoyasarTest
{
    public class HelpersTest
    {
        [Fact]
        public void TestVisaValidation()
        {
            var testCards = new[]
            {
                "4111111111111111",
                "4012888888881881",
                "4222222222222"
            };

            foreach (var testCard in testCards)
            {
                Assert.True(CreditCardHelper.IsVisa(testCard),
                    $"{testCard} is not a valid Visa");
            }
        }
        
        [Fact]
        public void TestMasterCardValidation()
        {
            var testCards = new[]
            {
                "5555555555554444",
                "5105105105105100"
            };
            
            foreach (var testCard in testCards)
            {
                Assert.True(CreditCardHelper.IsMasterCard(testCard), 
                    $"{testCard} is not a valid MasterCard");
            }
        }
    }
}