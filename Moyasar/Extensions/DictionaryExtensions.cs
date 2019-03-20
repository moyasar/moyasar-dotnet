using System.Collections.Generic;

namespace Moyasar.Extensions
{
    public static class DictionaryExtensions
    {
        /// <summary>
        /// An extension method to retrieve some key's value or return a default one when the key doesn't exists
        /// </summary>
        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
        {
            return dict.ContainsKey(key) ? dict[key] : defaultValue;
        }
    }
}