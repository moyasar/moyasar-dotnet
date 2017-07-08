namespace Moyasar.Payments
{
    public class MetaResult : PaymentResultBase
    {
        public string CurrentPage { get; set; }
        public string NextPage { get; set; }
        public string PrevPage { get; set; }
        public string TotalPages { get; set; }
        public string TotalCount { get; set; }
    }
}
