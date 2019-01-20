using System.Collections.Generic;
using System.Reflection;
using Moyasar.Models;
using Moyasar.Services;

namespace Moyasar.Abstraction
{
    public class Resource<TResource>
    {
        protected static string Name => typeof(TResource).Name.ToLower();
        protected static string PluralName => $"{Name}s";
        protected static string ResourceUrl => $"{MoyasarService.CurrentVersionUrl}/{PluralName}";
        
        protected static string GetCreateUrl() => ResourceUrl;
        protected static string GetFetchUrl(string id) => $"{ResourceUrl}/{id}";
        protected static string GetUpdateUrl(string id) => $"{ResourceUrl}/{id}";
        protected static string GetListUrl() => ResourceUrl;
    }
}