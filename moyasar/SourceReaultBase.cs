using System;

namespace Moyasar
{
    [Serializable]
    public class SourceResultBase
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string UserName { get; internal set; }
        public string Transaction_Url { get; internal set; }
        public string Error_Code { get; internal set; }
        public string Transaction_Id { get; internal set; }
        public string Company { get; internal set; }
        public string Name { get; internal set; }
        public string Number { get; internal set; }
    }
}
