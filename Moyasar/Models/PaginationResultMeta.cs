using System;
using Newtonsoft.Json;

namespace Moyasar.Models
{
    public class PaginationResultMeta
    {
        [JsonProperty("current_page")]
        public int CurrentPage { get; set; }
        
        [JsonProperty("next_page")]
        public int? NextPage { get; set; }
        
        [JsonProperty("prev_page")]
        public int? PreviousPage { get; set; }
        
        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }
        
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
    }
}