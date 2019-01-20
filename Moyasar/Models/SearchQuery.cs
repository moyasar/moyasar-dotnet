using System;
using System.Collections.Generic;

namespace Moyasar.Models
{
    public class SearchQuery
    {
        private const string IdField = "id";
        private const string SourceField = "source";
        private const string StatusField = "status";
        private const string PageField = "page";
        private const string CreatedAfterField = "created[gt]";
        private const string CreatedBeforeField = "created[lt]";
        
        public string Id { get; set; }
        public string Source { get; set; }
        public string Status { get; set; }
        public int? Page { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }

        public Dictionary<string, object> ToDictionary()
        {
            var dict = new Dictionary<string, object>();
            
            if(!string.IsNullOrEmpty(Id)) dict.Add(IdField, Id);
            if(!string.IsNullOrEmpty(Source)) dict.Add(SourceField, Source);
            if(!string.IsNullOrEmpty(Status)) dict.Add(StatusField, Status);
            if(Page.HasValue) dict.Add(PageField, Page.Value);
            if(CreatedAfter.HasValue) dict.Add(CreatedAfterField, CreatedAfter?.ToString("O"));
            if(CreatedBefore.HasValue) dict.Add(CreatedBeforeField, CreatedBefore?.ToString("O"));
            
            return dict.Count > 0 ? dict : null;
        }
        
        public SearchQuery Clone() => new SearchQuery()
        {
            Id = Id,
            Source = Source,
            Status = Status,
            Page = Page,
            CreatedAfter = CreatedAfter,
            CreatedBefore = CreatedBefore
        };
    }
}