using System;
using System.Collections.Generic;

namespace Moyasar.Models
{
    public class PaginationResult<TModel>
    {
        public int CurrentPage { get; set; }
        public int? NextPage { get; set; }
        public int? PreviousPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

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
        
        public List<TModel> Items { get; set; }
    }
}