using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Moyasar.Models
{
    public class PaginationResult<TModel>
    {
        private const string CurrentPageField = "current_page";
        private const string NextPageField = "next_page";
        private const string PreviousPageField = "prev_page";
        private const string TotalPagesField = "total_pages";
        private const string TotalCountField = "total_count";
        
        [JsonProperty(CurrentPageField)]
        public int CurrentPage { get; set; }
        
        [JsonProperty(NextPageField)]
        public int? NextPage { get; set; }
        
        [JsonProperty(PreviousPageField)]
        public int? PreviousPage { get; set; }
        
        [JsonProperty(TotalPagesField)]
        public int TotalPages { get; set; }
        
        [JsonProperty(TotalCountField)]
        public int TotalCount { get; set; }

        [JsonIgnore]
        internal Func<int, PaginationResult<TModel>> Paginator { get; set; }
        
        public PaginationResult<TModel> GetNextPage()
        {
            if (Paginator == null) return null;
            return NextPage.HasValue ? Paginator(NextPage.Value) : null;
        }
        
        public PaginationResult<TModel> GetPreviousPage()
        {
            if (Paginator == null) return null;
            return PreviousPage.HasValue ? Paginator(PreviousPage.Value) : null;   
        }
        
        [JsonIgnore]
        public List<TModel> Items { get; set; }
    }
}