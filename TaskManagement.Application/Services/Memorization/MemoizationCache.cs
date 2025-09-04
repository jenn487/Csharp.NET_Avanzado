using System.Collections.Concurrent;

namespace TaskManagement.Application.Services.Memoization
{
    public static class MemoizationCache
    {
        private static readonly ConcurrentDictionary<string, object> _cache = new();

        public static T GetOrAdd<T>(string key, Func<T> factory)
        {
            if (_cache.TryGetValue(key, out var value))
                return (T)value;

            var result = factory();
            _cache[key] = result;
            return result;
        }

        public static void Clear()
        {
            _cache.Clear();
        }


    }
}
