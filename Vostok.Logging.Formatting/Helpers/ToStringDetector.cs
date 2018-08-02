using System;

// ReSharper disable ConvertClosureToMethodGroup

namespace Vostok.Logging.Formatting.Helpers
{
    internal static class ToStringDetector
    {
        private const int CacheCapacity = 1000;

        private static readonly RecyclingBoundedCache<Type, bool> Cache
            = new RecyclingBoundedCache<Type, bool>(CacheCapacity);

        public static bool HasCustomToString(Type type) =>
            Cache.Obtain(type, t => HasCustomToStringInternal(t));

        private static bool HasCustomToStringInternal(Type type)
        {
            var toStringMethod = type.GetMethod("ToString", Array.Empty<Type>());
            if (toStringMethod == null)
                return false;

            // (iloktionov): Reject anonymous types:
            if (type.Name.StartsWith("<>"))
                return false;

            var declaringType = toStringMethod.DeclaringType;

            return declaringType != typeof(object) &&
                   declaringType != typeof(ValueType);
        }
    }
}