using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace MadeInKawaru.Extensions
{
    public static class ListExtension
    {
        public static T RandomOne<T>(this List<T> self)
        {
            return self[Random.Range(0, self.Count)];
        }

        public static IEnumerable<T> RandomSort<T>(this List<T> self)
        {
            return self.OrderBy(_ => Guid.NewGuid());
        }
    }
}