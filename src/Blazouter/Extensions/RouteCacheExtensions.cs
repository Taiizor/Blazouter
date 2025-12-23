using Blazouter.Interfaces;
using Blazouter.Models;

namespace Blazouter.Extensions
{
    /// <summary>
    /// Provides extension methods for working with route caching.
    /// </summary>
    public static class RouteCacheExtensions
    {
        /// <summary>
        /// Gets a formatted string representation of cache statistics.
        /// </summary>
        /// <param name="cacheService">The cache service instance.</param>
        /// <returns>A formatted multi-line string with cache statistics.</returns>
        /// <remarks>
        /// This extension method provides a convenient way to display cache performance
        /// metrics for monitoring and debugging purposes.
        /// </remarks>
        /// <example>
        /// Display cache statistics:
        /// <code>
        /// @inject IRouteCacheService CacheService
        /// 
        /// @code {
        ///     private void LogCacheStats()
        ///     {
        ///         Console.WriteLine(CacheService.GetFormattedStatistics());
        ///     }
        /// }
        /// </code>
        /// </example>
        public static string GetFormattedStatistics(this IRouteCacheService cacheService)
        {
            CacheStatistics stats = cacheService.GetStatistics();

            return $@"Cache Statistics:
  Total Requests: {stats.TotalRequests:N0}
  Cache Hits: {stats.CacheHits:N0}
  Cache Misses: {stats.CacheMisses:N0}
  Hit Rate: {stats.HitRate:F2}%
  Route Cache Size: {stats.RouteMatchCacheSize}
  Component Cache Size: {stats.ComponentTypeCacheSize}";
        }
    }
}