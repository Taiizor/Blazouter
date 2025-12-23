namespace Blazouter.Models
{
    /// <summary>
    /// Configuration options for route caching in Blazouter.
    /// </summary>
    /// <remarks>
    /// <para>
    /// CacheOptions allows fine-tuning of cache behavior including size limits,
    /// time-to-live settings, and feature toggles.
    /// </para>
    /// </remarks>
    public class CacheOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether route match caching is enabled.
        /// </summary>
        /// <value>
        /// true to enable caching of route matches; false to disable. Defaults to true.
        /// </value>
        /// <remarks>
        /// When disabled, route matching will be performed on every navigation without caching.
        /// This can be useful for debugging or in scenarios where routes change dynamically.
        /// </remarks>
        public bool EnableRouteMatchCache { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether component type caching is enabled.
        /// </summary>
        /// <value>
        /// true to enable caching of lazily loaded component types; false to disable. Defaults to true.
        /// </value>
        /// <remarks>
        /// When enabled, component types loaded via ComponentLoader are cached to avoid
        /// repeated async loading operations on subsequent navigations to the same route.
        /// </remarks>
        public bool EnableComponentTypeCache { get; set; } = true;

        /// <summary>
        /// Gets or sets the maximum number of route matches to cache.
        /// </summary>
        /// <value>
        /// The maximum cache size. Defaults to 100 entries.
        /// </value>
        /// <remarks>
        /// <para>
        /// When the cache reaches this limit, the least recently used entries will be evicted.
        /// Setting this to 0 effectively disables caching. Very large values may consume
        /// significant memory.
        /// </para>
        /// <para>
        /// Consider your application's typical navigation patterns when setting this value.
        /// Applications with many unique routes may benefit from a larger cache size.
        /// </para>
        /// </remarks>
        public int MaxRouteMatchCacheSize { get; set; } = 100;

        /// <summary>
        /// Gets or sets the maximum number of component types to cache.
        /// </summary>
        /// <value>
        /// The maximum cache size. Defaults to 50 entries.
        /// </value>
        /// <remarks>
        /// This limit applies to lazily loaded components. Since component types are typically
        /// small objects, this cache has less memory impact than route match caching.
        /// </remarks>
        public int MaxComponentTypeCacheSize { get; set; } = 50;

        /// <summary>
        /// Gets or sets the time-to-live for cached route matches in seconds.
        /// </summary>
        /// <value>
        /// The number of seconds before a cached route match expires. A value of 0 means no expiration.
        /// Defaults to 0 (no expiration).
        /// </value>
        /// <remarks>
        /// <para>
        /// TTL-based expiration is useful in scenarios where route configurations might change
        /// during application lifetime, such as in development environments or applications
        /// with dynamic routing.
        /// </para>
        /// <para>
        /// In most production scenarios, a value of 0 (no expiration) is appropriate since
        /// route configurations are typically static after application startup.
        /// </para>
        /// </remarks>
        public int RouteMatchCacheTTLSeconds { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating whether cache statistics tracking is enabled.
        /// </summary>
        /// <value>
        /// true to track cache statistics; false to disable. Defaults to false.
        /// </value>
        /// <remarks>
        /// Enabling statistics tracking has a small performance overhead but provides
        /// valuable insights into cache effectiveness. Consider enabling this in development
        /// or when tuning cache configuration.
        /// </remarks>
        public bool EnableStatistics { get; set; } = false;
    }
}