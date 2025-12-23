namespace Blazouter.Models
{
    /// <summary>
    /// Represents cache performance statistics for route caching operations.
    /// </summary>
    /// <remarks>
    /// <para>
    /// CacheStatistics provides insights into cache effectiveness, helping developers
    /// tune cache configuration and understand routing patterns.
    /// </para>
    /// </remarks>
    public class CacheStatistics
    {
        /// <summary>
        /// Gets or sets the total number of cache hit attempts.
        /// </summary>
        /// <value>
        /// The total number of times a cache lookup was performed.
        /// </value>
        public long TotalRequests { get; set; }

        /// <summary>
        /// Gets or sets the number of successful cache hits.
        /// </summary>
        /// <value>
        /// The number of times a requested item was found in the cache.
        /// </value>
        public long CacheHits { get; set; }

        /// <summary>
        /// Gets or sets the number of cache misses.
        /// </summary>
        /// <value>
        /// The number of times a requested item was not found in the cache.
        /// </value>
        public long CacheMisses { get; set; }

        /// <summary>
        /// Gets the cache hit rate as a percentage.
        /// </summary>
        /// <value>
        /// A value between 0 and 100 representing the percentage of successful cache hits.
        /// Returns 0 if no requests have been made.
        /// </value>
        public double HitRate => TotalRequests > 0 ? CacheHits / (double)TotalRequests * 100 : 0;

        /// <summary>
        /// Gets or sets the number of cached route matches currently in memory.
        /// </summary>
        /// <value>
        /// The current count of cached route match entries.
        /// </value>
        public int RouteMatchCacheSize { get; set; }

        /// <summary>
        /// Gets or sets the number of cached component types currently in memory.
        /// </summary>
        /// <value>
        /// The current count of cached component type entries.
        /// </value>
        public int ComponentTypeCacheSize { get; set; }
    }
}