using System.Collections.Concurrent;

namespace TaskManagement.Application.Services.Memoization
{
    public static class MemoizationCache
    {
        private static readonly ConcurrentDictionary<string, object> _cache = new();

        // Para valores normales
        public static T GetOrAdd<T>(string key, Func<T> factory)
        {
            if (_cache.TryGetValue(key, out var value))
                return (T)value;


            var result = factory();
            _cache[key] = result;
            return result;
        }

        // Para valores asincronicos
        public static async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> factory)
        {
            if (_cache.TryGetValue(key, out var value) && value is Task<T> typedTask)
            {
                return await typedTask;
            }

            var newTask = factory();
            _cache[key] = newTask!;
            return await newTask;
        }

        public static void Clear()
        {
            _cache.Clear();
        }
    }
}

