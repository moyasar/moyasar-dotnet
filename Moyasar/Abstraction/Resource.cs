using System;

namespace Moyasar.Abstraction
{
    /// <summary>
    /// Represents an abstract resource that other Moyasar's resources are built on 
    /// </summary>
    /// <typeparam name="TResource">Resource type that inherents this class</typeparam>
    public abstract class Resource<TResource>
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
