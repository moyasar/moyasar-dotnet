using System;
using System.Collections.Generic;
using Moyasar.Abstraction;
using Newtonsoft.Json;

namespace Moyasar.Models
{
    /// <summary>
    /// Contains meta-data that help with resources pagination 
    /// </summary>
    /// <typeparam name="TModel">Resource Type</typeparam>
    public class PaginationResult<TModel> where TModel : Resource<TModel>
    {
        public int CurrentPage => Meta.CurrentPage;
        public int? NextPage => Meta.NextPage;
        public int? PreviousPage => Meta.PreviousPage;
        public int TotalPages => Meta.TotalPages;
        public int TotalCount => Meta.TotalCount;

        [JsonProperty("meta")]
        public PaginationResultMeta Meta { get; private set; }
        
        [JsonIgnore]
        public List<TModel> Items { get; private set; }

        [JsonProperty("payments")]
        private List<TModel> Payments { set => Items = value; }

        [JsonProperty("invoices")]
        private List<TModel> Invoices { set => Items = value; }
    }
}
