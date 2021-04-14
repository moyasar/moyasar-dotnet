using System;
using System.Linq;
using Moyasar.Providers;
using Newtonsoft.Json;
using BasicStringDict = System.Collections.Generic.Dictionary<string, string>;

namespace Moyasar.Models
{
    /// <summary>
    /// Contains search parametes used when searching a resource
    /// </summary>
    public class SearchQuery
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }
        
        [JsonProperty("source", NullValueHandling = NullValueHandling.Ignore)]
        public string Source { get; set; }
        
        [JsonProperty("status", NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }
        
        [JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        public int? Page { get; set; }
        
        [JsonProperty("created[gt]", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreatedAfter { get; set; }
        
        [JsonProperty("created[lt]", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? CreatedBefore { get; set; }

        [JsonProperty("metadata", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(MetadataSerializer))]
        public BasicStringDict Metadata { get; set; }

        public SearchQuery Clone() => new SearchQuery
        {
            Id = Id,
            Source = Source,
            Status = Status,
            Page = Page,
            CreatedAfter = CreatedAfter,
            CreatedBefore = CreatedBefore,
            Metadata = Metadata.ToDictionary(p => p.Key, p => p.Value)
        };
    }
}