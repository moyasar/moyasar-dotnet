using System.Collections.Generic;

namespace Moyasar.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
        {
            if (dict.ContainsKey(key)) return dict[key];
            return defaultValue;
        }
    }
}