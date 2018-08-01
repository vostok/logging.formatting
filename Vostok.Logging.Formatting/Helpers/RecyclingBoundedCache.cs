using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Vostok.Logging.Formatting.Helpers
{
    internal class RecyclingBoundedCache<TKey, TValue>
    {
        private readonly int capacity;
        private RecyclingBoundedCacheState state;

        public RecyclingBoundedCache(int capacity)
        {
            this.capacity = capacity;

            state = new RecyclingBoundedCacheState();
        }

        public TValue Obtain(TKey key, Func<TKey, TValue> factory)
        {
            var currentState = state;
            if (currentState.Items.TryGetValue(key, out var value))
                return value;

            if (currentState.Items.TryAdd(key, value = factory(key)))
            {
                var newCount = Interlocked.Increment(ref currentState.Count);
                if (newCount == capacity)
                    Interlocked.Exchange(ref state, new RecyclingBoundedCacheState());
            }

            return value;
        }

        private class RecyclingBoundedCacheState
        {
            public readonly ConcurrentDictionary<TKey, TValue> Items = new ConcurrentDictionary<TKey, TValue>();

            public int Count;
        }
    }
}
