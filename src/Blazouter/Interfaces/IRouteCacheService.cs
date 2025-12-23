using Blazouter.Models;

namespace Blazouter.Interfaces
{
    /// <summary>
    /// Defines caching operations for route matching and component loading in Blazouter.
    /// </summary>
    /// <remarks>
    /// <para>
    /// IRouteCacheService provides a caching layer to improve route matching performance
    /// and reduce redundant component loading operations. Implementations can use in-memory
    /// caching with configurable eviction policies.
    /// </para>
    /// </remarks>
    public interface IRouteCacheService
    {
        /// <summary>
        /// Attempts to retrieve a cached route match for the specified path.
        /// </summary>
        /// <param name="path">The URL path to look up in the cache.</param>
        /// <returns>
        /// The cached <see cref="RouteMatch"/> if found, or null if the path is not in the cache.
        /// </returns>
        RouteMatch? GetCachedRouteMatch(string path);

        /// <summary>
        /// Stores a route match in the cache for the specified path.
        /// </summary>
        /// <param name="path">The URL path to use as the cache key.</param>
        /// <param name="match">The route match to cache.</param>
        void CacheRouteMatch(string path, RouteMatch match);

        /// <summary>
        /// Attempts to retrieve a cached component type for lazy loading.
        /// </summary>
        /// <param name="routePath">The route path used to identify the component.</param>
        /// <returns>
        /// The cached component Type if found, or null if not in the cache.
        /// </returns>
        Type? GetCachedComponentType(string routePath);

        /// <summary>
        /// Stores a component type in the cache for lazy loading.
        /// </summary>
        /// <param name="routePath">The route path to use as the cache key.</param>
        /// <param name="componentType">The component type to cache.</param>
        void CacheComponentType(string routePath, Type componentType);

        /// <summary>
        /// Clears all cached route matches and component types.
        /// </summary>
        void Clear();

        /// <summary>
        /// Removes a specific cached route match by path.
        /// </summary>
        /// <param name="path">The URL path to remove from the cache.</param>
        void InvalidateRouteMatch(string path);

        /// <summary>
        /// Gets cache statistics including hit rate and entry counts.
        /// </summary>
        /// <returns>
        /// A <see cref="CacheStatistics"/> object containing cache performance metrics.
        /// </returns>
        CacheStatistics GetStatistics();
    }
}