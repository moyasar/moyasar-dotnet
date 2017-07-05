using System;
using System.Collections.Generic;
using System.Text;

namespace Moyasar
{
  public  class SadadType:SourceReaultBase
    {
      
      public string Username { get; set; }
      
      public string ErrorCode { get; set; }
      public string TransactionId { get; set; }
      public string TransactionUrl { get; set; }
      public string SuccessUrl { get; set; }
      public string FaildUrl { get; set; }
    }
}
