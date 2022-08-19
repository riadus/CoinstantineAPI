using System;
using System.Collections.Generic;
using System.Linq;

namespace CoinstantineAPI.Core.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach(var item in self)
            {
                action(item);
            }
        }

        public static IEnumerable<T> ForeachChangeValue<T>(this IEnumerable<T> self, Action<T> action)
        {
            var result = new List<T>();
            for (var i = 0; i < self.Count(); i++)
            {
                var item = self.ElementAt(i);
                action(item);
                result.Add(item);
            }
            return result;
        }
    }
}
