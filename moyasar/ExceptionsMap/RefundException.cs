using moyasar.PaymentArea.RefundMap;

namespace moyasar.ExceptionsMap
{
    public class RefundException : MoyasarRefundBase
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
    }
}