using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moyasar.PaymentArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moyasar.PaymentArea.Tests
{
    [TestClass()]
    public class PaymentTests
    {
        [TestMethod()]
        public void IniParametersTest()
        {
            Payment p = new Payment();
            p.Create();

        }

        [TestMethod()]
        public void CreatePayTest()
        {
           
        }

        [TestMethod()]
        public void GetPaymentByIdTest()
        {

        }

        [TestMethod()]
        public void RefundTest()
        {

        }

        [TestMethod()]
        public void GetPaymentsListTest()
        {

        }
    }
}