using Blazouter.Interfaces;
using Blazouter.Models;

namespace Blazouter.Services
{
    /// <summary>
    /// Provides cached route matching logic that wraps RouteMatcherService with caching layer.
    /// </summary>
    /// <remarks>
    /// <para>
    /// CachedRouteMatcherService decorates the standard RouteMatcherService with a caching layer
    /// to improve performance for repeated route lookups. It delegates to the underlying matcher
    /// on cache misses and stores results for subsequent requests.
    /// </para>
    /// <para>
    /// This implementation is thread-safe and registered as a singleton. Cache behavior is
    /// controlled by CacheOptions configured during service registration.
    /// </para>
    /// </remarks>
    public class CachedRouteMatcherService : IRouteMatcherService
    {
        private readonly RouteMatcherService _innerMatcher;
        private readonly IRouteCacheService _cacheService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CachedRouteMatcherService"/> class.
        /// </summary>
        /// <param name="cacheService">The cache service for storing and retrieving route matches.</param>
        public CachedRouteMatcherService(IRouteCacheService cacheService)
        {
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _innerMatcher = new RouteMatcherService();
        }

        /// <summary>
        /// Matches a URL path against a collection of route configurations with caching.
        /// </summary>
        /// <param name="path">
        /// The URL path to match, optionally including a query string (e.g., "/users/123?tab=profile").
        /// </param>
        /// <param name="routes">
        /// The collection of route configurations representing the application's route tree.
        /// </param>
        /// <returns>
        /// A <see cref="RouteMatch"/> object containing the matched route configuration, extracted parameters,
        /// and query string values, or null if no route matches the path.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method first checks the cache for a previously matched route. On a cache hit,
        /// it returns the cached result immediately. On a cache miss, it delegates to the
        /// underlying RouteMatcherService and caches the result if successful.
        /// </para>
        /// <para>
        /// Note: Only successful route matches are cached. Failed matches (null results) are
        /// not cached to allow for dynamic route additions or configuration changes.
        /// </para>
        /// <para>
        /// Query parameters are part of the cache key, so different query strings
        /// for the same path will result in separate cache entries.
        /// </para>
        /// <para>
        /// Per-route caching can be controlled via RouteConfig.EnableCache property. When set to false,
        /// that specific route will never be cached regardless of global cache settings.
        /// </para>
        /// </remarks>
        public RouteMatch? MatchRoute(string path, List<RouteConfig> routes)
        {
            // Try to get from cache
            RouteMatch? cachedMatch = _cacheService.GetCachedRouteMatch(path);
            if (cachedMatch != null)
            {
                // Check if the matched route has caching disabled
                if (cachedMatch.Route?.EnableCache == false)
                {
                    // Route has caching explicitly disabled, invalidate and re-match
                    _cacheService.InvalidateRouteMatch(path);
                }
                else
                {
                    return cachedMatch;
                }
            }

            // Not in cache, perform actual matching
            RouteMatch? match = _innerMatcher.MatchRoute(path, routes);

            // Cache only successful matches and respect per-route cache settings
            if (match != null)
            {
                // Check if this specific route allows caching
                // null means use global settings (cache), false means never cache, true means always cache
                bool shouldCache = match.Route?.EnableCache ?? true;

                if (shouldCache)
                {
                    _cacheService.CacheRouteMatch(path, match);
                }
            }

            return match;
        }
    }
}