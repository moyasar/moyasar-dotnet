using System.Reflection;

namespace Moyasar.Abstraction
{
    public class Resource
    {
        protected static string Name => MethodBase.GetCurrentMethod().DeclaringType.Name.ToLower();
        protected static string PluralName => $"{Name}s";
        protected static string ResourceUrl => $"{MoyasarService.CurrentVersionUrl}/{PluralName}";
        
        protected static string GetCreateUrl() => ResourceUrl;
        protected static string GetFetchUrl(string id) => $"{ResourceUrl}/{id}";
        protected static string GetUpdateUrl(string id) => $"{ResourceUrl}/{id}";
        protected static string GetListUrl() => ResourceUrl;
    }
}