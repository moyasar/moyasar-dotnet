namespace Moyasar.PaymentArea
{
    public class PaymentResult : PaymentResultBase
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public int Amount { get; set; }
        public string Fee { get; set; }
        public string Currency { get; set; }
        public string Refunded { get; set; }
        public string RefundedAt { get; set; }
        public string Description { get; set; }
        public string AmountFormat { get; set; }
        public string FeeFormat { get; set; }
        public string InvoiceId { get; set; }
        public string Ip { get; set; }
        public string CreatedAt { get; set; }
        public string UpdatedAt { get; set; }
        public SourceReaultBase Source { get; set; }
    }
}
