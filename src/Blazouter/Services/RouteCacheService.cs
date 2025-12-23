using System.Collections.Concurrent;
using Blazouter.Interfaces;
using Blazouter.Models;

namespace Blazouter.Services
{
    /// <summary>
    /// Provides in-memory caching for route matches and component types with LRU eviction.
    /// </summary>
    /// <remarks>
    /// <para>
    /// RouteCacheService implements a thread-safe caching layer that improves routing
    /// performance by storing previously matched routes and lazily loaded component types.
    /// </para>
    /// <para>
    /// The cache uses a Least Recently Used (LRU) eviction policy when size limits are reached,
    /// ensuring that frequently accessed routes remain in cache while rarely used ones are evicted.
    /// </para>
    /// <para>
    /// This service is registered as a singleton, making the cache shared across all users
    /// in server-side scenarios. Cache entries are thread-safe and can be accessed concurrently.
    /// </para>
    /// </remarks>
    public class RouteCacheService : IRouteCacheService
    {
        private readonly ConcurrentDictionary<string, CacheEntry<RouteMatch>> _routeMatchCache = new();
        private readonly ConcurrentDictionary<string, Type> _componentTypeCache = new();
        private readonly ConcurrentQueue<string> _componentTypeInsertionOrder = new();
        private readonly CacheOptions _options;
        private long _totalRequests = 0;
        private long _cacheHits = 0;
        private long _cacheMisses = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteCacheService"/> class with specified options.
        /// </summary>
        /// <param name="options">The cache configuration options.</param>
        public RouteCacheService(CacheOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Attempts to retrieve a cached route match for the specified path.
        /// </summary>
        /// <param name="path">The URL path to look up in the cache.</param>
        /// <returns>
        /// The cached <see cref="RouteMatch"/> if found and not expired, or null otherwise.
        /// </returns>
        /// <remarks>
        /// This method is thread-safe and can be called concurrently from multiple threads.
        /// Cache hits and misses are tracked if statistics are enabled.
        /// </remarks>
        public RouteMatch? GetCachedRouteMatch(string path)
        {
            if (!_options.EnableRouteMatchCache)
            {
                return null;
            }

            if (_options.EnableStatistics)
            {
                Interlocked.Increment(ref _totalRequests);
            }

            if (_routeMatchCache.TryGetValue(path, out CacheEntry<RouteMatch>? entry))
            {
                // Check if entry has expired
                if (_options.RouteMatchCacheTTLSeconds > 0)
                {
                    if (DateTime.UtcNow - entry.Timestamp > TimeSpan.FromSeconds(_options.RouteMatchCacheTTLSeconds))
                    {
                        // Entry expired, remove it
                        _routeMatchCache.TryRemove(path, out _);
                        if (_options.EnableStatistics)
                        {
                            Interlocked.Increment(ref _cacheMisses);
                        }
                        return null;
                    }
                }

                // Update last accessed time for LRU using Interlocked for thread safety
                Interlocked.Exchange(ref entry.LastAccessedTicks, DateTime.UtcNow.Ticks);

                if (_options.EnableStatistics)
                {
                    Interlocked.Increment(ref _cacheHits);
                }

                return entry.Value;
            }

            if (_options.EnableStatistics)
            {
                Interlocked.Increment(ref _cacheMisses);
            }

            return null;
        }

        /// <summary>
        /// Stores a route match in the cache for the specified path.
        /// </summary>
        /// <param name="path">The URL path to use as the cache key.</param>
        /// <param name="match">The route match to cache.</param>
        /// <remarks>
        /// <para>
        /// If the cache has reached its maximum size, the least recently used entry
        /// will be evicted before adding the new entry.
        /// </para>
        /// <para>
        /// This method is thread-safe and can be called concurrently from multiple threads.
        /// </para>
        /// </remarks>
        public void CacheRouteMatch(string path, RouteMatch match)
        {
            if (!_options.EnableRouteMatchCache || match == null)
            {
                return;
            }

            // Check cache size and evict LRU if needed
            if (_routeMatchCache.Count >= _options.MaxRouteMatchCacheSize)
            {
                EvictLeastRecentlyUsed(_routeMatchCache);
            }

            CacheEntry<RouteMatch> entry = new()
            {
                Value = match,
                Timestamp = DateTime.UtcNow,
                LastAccessedTicks = DateTime.UtcNow.Ticks
            };

            _routeMatchCache[path] = entry;
        }

        /// <summary>
        /// Attempts to retrieve a cached component type for lazy loading.
        /// </summary>
        /// <param name="routePath">The route path used to identify the component.</param>
        /// <returns>
        /// The cached component Type if found, or null if not in the cache.
        /// </returns>
        /// <remarks>
        /// Component types are cached indefinitely once loaded, as they do not expire.
        /// This method is thread-safe.
        /// </remarks>
        public Type? GetCachedComponentType(string routePath)
        {
            if (!_options.EnableComponentTypeCache)
            {
                return null;
            }

            if (_componentTypeCache.TryGetValue(routePath, out Type? componentType))
            {
                return componentType;
            }

            return null;
        }

        /// <summary>
        /// Stores a component type in the cache for lazy loading.
        /// </summary>
        /// <param name="routePath">The route path to use as the cache key.</param>
        /// <param name="componentType">The component type to cache.</param>
        /// <remarks>
        /// <para>
        /// If the cache has reached its maximum size, the oldest entry (first inserted)
        /// will be evicted before adding the new entry using FIFO policy.
        /// </para>
        /// <para>
        /// This method is thread-safe and can be called concurrently from multiple threads.
        /// </para>
        /// </remarks>
        public void CacheComponentType(string routePath, Type componentType)
        {
            if (!_options.EnableComponentTypeCache || componentType == null)
            {
                return;
            }

            // Check cache size and evict oldest if needed (FIFO)
            while (_componentTypeCache.Count >= _options.MaxComponentTypeCacheSize)
            {
                if (_componentTypeInsertionOrder.TryDequeue(out string? oldKey))
                {
                    _componentTypeCache.TryRemove(oldKey, out _);
                }
                else
                {
                    // Queue is empty but dictionary is not, shouldn't happen but break to avoid infinite loop
                    break;
                }
            }

            // Add to cache
            if (_componentTypeCache.TryAdd(routePath, componentType))
            {
                _componentTypeInsertionOrder.Enqueue(routePath);
            }
        }

        /// <summary>
        /// Clears all cached route matches and component types.
        /// </summary>
        /// <remarks>
        /// This method also resets cache statistics if enabled. Use with caution in production
        /// environments as it will cause performance degradation until the cache is repopulated.
        /// </remarks>
        public void Clear()
        {
            _routeMatchCache.Clear();
            _componentTypeCache.Clear();

            // Clear the insertion order queue
            while (_componentTypeInsertionOrder.TryDequeue(out _))
            {
                // Empty the queue
            }

            if (_options.EnableStatistics)
            {
                Interlocked.Exchange(ref _totalRequests, 0);
                Interlocked.Exchange(ref _cacheHits, 0);
                Interlocked.Exchange(ref _cacheMisses, 0);
            }
        }

        /// <summary>
        /// Removes a specific cached route match by path.
        /// </summary>
        /// <param name="path">The URL path to remove from the cache.</param>
        /// <remarks>
        /// This is useful when a route configuration changes dynamically and you need to
        /// invalidate specific cache entries.
        /// </remarks>
        public void InvalidateRouteMatch(string path)
        {
            _routeMatchCache.TryRemove(path, out _);
        }

        /// <summary>
        /// Gets cache statistics including hit rate and entry counts.
        /// </summary>
        /// <returns>
        /// A <see cref="CacheStatistics"/> object containing current cache performance metrics.
        /// </returns>
        /// <remarks>
        /// Statistics are only tracked if <see cref="CacheOptions.EnableStatistics"/> is true.
        /// Otherwise, most statistics will return zero values.
        /// </remarks>
        public CacheStatistics GetStatistics()
        {
            return new CacheStatistics
            {
                TotalRequests = _totalRequests,
                CacheHits = _cacheHits,
                CacheMisses = _cacheMisses,
                RouteMatchCacheSize = _routeMatchCache.Count,
                ComponentTypeCacheSize = _componentTypeCache.Count
            };
        }

        /// <summary>
        /// Evicts the least recently used entry from the cache.
        /// </summary>
        /// <typeparam name="T">The type of value stored in the cache entry.</typeparam>
        /// <param name="cache">The cache dictionary to evict from.</param>
        /// <remarks>
        /// Note: This implementation has O(n) complexity. For very large caches (>1000 entries),
        /// consider using a more sophisticated LRU implementation with O(1) operations.
        /// </remarks>
        private void EvictLeastRecentlyUsed<T>(ConcurrentDictionary<string, CacheEntry<T>> cache)
        {
            if (cache.IsEmpty)
            {
                return;
            }

            // Find the least recently used entry
            string? lruKey = null;
            long oldestAccessTicks = long.MaxValue;

            foreach (KeyValuePair<string, CacheEntry<T>> entry in cache)
            {
                long accessTicks = Interlocked.Read(ref entry.Value.LastAccessedTicks);
                if (accessTicks < oldestAccessTicks)
                {
                    oldestAccessTicks = accessTicks;
                    lruKey = entry.Key;
                }
            }

            if (lruKey != null)
            {
                cache.TryRemove(lruKey, out _);
            }
        }

        /// <summary>
        /// Represents a cache entry with timestamp and last accessed tracking for LRU eviction.
        /// </summary>
        /// <typeparam name="T">The type of value stored in the cache entry.</typeparam>
        private class CacheEntry<T>
        {
            /// <summary>
            /// Gets or sets the cached value.
            /// </summary>
            public T Value { get; set; } = default!;

            /// <summary>
            /// Gets or sets the timestamp when the entry was created.
            /// </summary>
            public DateTime Timestamp { get; set; }

            /// <summary>
            /// Gets or sets the timestamp when the entry was last accessed, stored as ticks for thread-safe updates.
            /// </summary>
            public long LastAccessedTicks;
        }
    }
}