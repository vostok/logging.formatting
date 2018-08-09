using System;
using System.Text;

namespace Vostok.Logging.Formatting.Helpers
{
    internal static class StringBuilderCache
    {
        private const int MaximumSize = 768;

        [ThreadStatic]
        private static StringBuilder cachedInstance;

        public static StringBuilder Acquire(int capacity)
        {
            if (capacity <= MaximumSize)
            {
                var builder = cachedInstance;
                if (capacity <= builder?.Capacity)
                {
                    cachedInstance = null;
                    builder.Clear();
                    return builder;
                }
            }

            return new StringBuilder(capacity);
        }

        public static void Release(StringBuilder builder)
        {
            if (builder.Capacity <= MaximumSize)
                cachedInstance = builder;
        }
    }
}