using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moyasar
{
   public class PaymentResult
    {
       public string Id { get; set; }
        /// <summary>
        /// payment status. (default: initiated)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// payment amount in halals.
        /// </summary>
        public double   Amount { get; set; }
        /// <summary>
        /// transaction fee in halals.
        /// </summary>
        public double Fee { get; set; }

        /// <summary>
        /// 3 currency code iso alpha payment currency. (default: SAR)
        /// </summary>
        public string Currency { get; set; }
        /// <summary>
        /// refunded amount in halals. (default: 0)
        /// </summary>
        public double Refunded { get; set; }
        /// <summary>
        /// datetime of refunded. (default: null)
        /// </summary>
        public string Refunded_at { get; set; }
        /// <summary>
        /// payment description
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// ID of the invoice this payment is for if one exists.(default: null)
        /// </summary>
        public string Invoice_Id { get; set; }
        /// <summary>
        /// User IP
        /// </summary>
        public string IP { get; set; }
       public string Created_at { get; set; }
       public string Updated_at { get; set; }
       public CreditCard CreditCardSource { get; set; }
       public Sadad SadadSource { get; set; }

    }
}
