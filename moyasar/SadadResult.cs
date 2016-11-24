

namespace moyasar
{
  public  class SadadResult
    {
        /// <summary>
        /// type of payment creditcard or sadad
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// sadad’s username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// error code from sadad if failure happen.
        /// </summary>
        public string ErrorCode { get; set; }

      public string Message { get; set; }

        /// <summary>
        /// Sadad Transaction ID.
        /// </summary>
        public string Transaction_Id { get; set; }

        /// <summary>
        /// URL given from sadad to continue the payment.
        /// </summary>
        public string transaction_url { get; set; }

    }
}
