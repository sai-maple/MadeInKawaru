using System;
using System.Collections.Generic;
using System.Linq;

namespace MadeInKawaru.Extensions
{
    public static class DictionaryExtension
    {
        public static IOrderedEnumerable<KeyValuePair<TKey, TValue>> RandomSort<TKey, TValue>(
            this Dictionary<TKey, TValue> self)
        {
            return self.OrderBy(_ => Guid.NewGuid());
        }
    }
}