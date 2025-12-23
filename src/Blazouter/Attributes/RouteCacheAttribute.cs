namespace Blazouter.Attributes
{
    /// <summary>
    /// Specifies whether caching is enabled for this specific route.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This attribute allows declarative configuration of route caching on component classes.
    /// When applied, it overrides the global cache settings for the specific route, allowing
    /// fine-grained control over which routes are cached.
    /// </para>
    /// <para>
    /// Route caching significantly improves navigation performance by storing route matching
    /// results. Subsequent navigations to cached routes are 10-100x faster than cache misses.
    /// </para>
    /// <para>
    /// Use cases for controlling cache per route:
    /// </para>
    /// <list type="bullet">
    /// <item><description><strong>Disable caching:</strong> Admin dashboards, real-time data, user-specific content</description></item>
    /// <item><description><strong>Force caching:</strong> Static pages, public content that rarely changes</description></item>
    /// <item><description><strong>Default behavior:</strong> Omit attribute to use global cache settings</description></item>
    /// </list>
    /// <para>
    /// <strong>Note:</strong> This affects route match caching. Component type caching for lazy-loaded
    /// components is controlled separately through CacheOptions.EnableComponentTypeCache.
    /// </para>
    /// </remarks>
    /// <example>
    /// Disable caching for an admin dashboard:
    /// <code>
    /// [Route("/admin/dashboard")]
    /// [RouteCache(false)]
    /// public partial class AdminDashboard : ComponentBase
    /// {
    ///     // This route will never be cached
    /// }
    /// </code>
    /// </example>
    /// <example>
    /// Force caching for a static page:
    /// <code>
    /// [Route("/static-page")]
    /// [RouteCache(true)]
    /// public partial class StaticPage : ComponentBase
    /// {
    ///     // This route will always be cached, even if global caching is disabled
    /// }
    /// </code>
    /// </example>
    /// <example>
    /// Use global settings (default - attribute not needed):
    /// <code>
    /// [Route("/default")]
    /// public class DefaultPage : ComponentBase
    /// {
    ///     // Uses global cache settings from CacheOptions
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RouteCacheAttribute"/> class.
    /// </remarks>
    /// <param name="enableCache">
    /// Whether to enable caching for this route.
    /// true = always cache, false = never cache.
    /// To use global settings, simply do not apply this attribute.
    /// </param>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class RouteCacheAttribute(bool enableCache) : Attribute
    {
        /// <summary>
        /// Gets a value indicating whether caching is enabled for this route.
        /// </summary>
        /// <value>
        /// true to always cache this route; false to never cache.
        /// </value>
        public bool EnableCache { get; } = enableCache;
    }
}