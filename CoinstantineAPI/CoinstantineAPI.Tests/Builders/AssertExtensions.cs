using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CoinstantineAPI.Tests
{
    public static class AssertExtentions
    {
        public static void Count<T>(IEnumerable<T> enumerable, int count)
        {
            Assert.StrictEqual(count, enumerable.Count());
        }

        public static void Contains<T>(IEnumerable<T> enumerable, T item)
        {
            Assert.True(enumerable.Contains(item));
        }
    }
}
