using Blazouter.Handlers;
using Blazouter.Interfaces;
using Blazouter.Interops;
using Blazouter.Models;
using Blazouter.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Blazouter.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring Blazouter services in the dependency injection container.
    /// </summary>
    /// <remarks>
    /// This class contains extension methods that simplify the registration of all required Blazouter services.
    /// These methods should be called during application startup in the service configuration phase.
    /// </remarks>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all required Blazouter services to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection to add Blazouter services to.</param>
        /// <returns>
        /// The same service collection so that multiple calls can be chained.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method registers the following services:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description><see cref="RouterStateService"/> as a singleton - Manages global router state.</description>
        /// </item>
        /// <item>
        /// <description><see cref="RouterNavigationService"/> as scoped - Provides programmatic navigation per request/circuit.</description>
        /// </item>
        /// <item>
        /// <description><see cref="IRouteMatcherService"/> as a singleton - Provides route matching logic.</description>
        /// </item>
        /// </list>
        /// <para>
        /// The singleton services ensure consistent route matching and state management across the application.
        /// The scoped navigation service ensures proper isolation in server-side scenarios where multiple
        /// users may be using the application simultaneously.
        /// </para>
        /// </remarks>
        /// <example>
        /// Register Blazouter services in Program.cs:
        /// <code>
        /// // Blazor WebAssembly
        /// builder.Services.AddBlazouter();
        /// 
        /// // Blazor Server
        /// builder.Services.AddBlazouter();
        /// 
        /// // .NET MAUI Blazor Hybrid
        /// builder.Services.AddBlazouter();
        /// </code>
        /// </example>
        public static IServiceCollection AddBlazouter(this IServiceCollection services)
        {
            services.AddSingleton<RouterStateService>();
            services.AddScoped<RouterNavigationService>();

            // Add caching with default options
            services.AddSingleton(new CacheOptions());
            services.AddSingleton<IRouteCacheService, RouteCacheService>();
            services.AddSingleton<IRouteMatcherService, CachedRouteMatcherService>();

            return services;
        }

        /// <summary>
        /// Registers a custom error handler for Blazouter routing errors.
        /// </summary>
        /// <typeparam name="THandler">
        /// The type of the custom error handler that implements <see cref="IRouterErrorHandler"/>.
        /// </typeparam>
        /// <param name="services">The service collection to add the error handler to.</param>
        /// <returns>
        /// The same service collection so that multiple calls can be chained.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method registers a custom implementation of <see cref="IRouterErrorHandler"/> as a scoped service.
        /// The error handler will be called whenever routing errors occur, such as component loading failures,
        /// route guard exceptions, or navigation errors.
        /// </para>
        /// <para>
        /// The error handler is registered as scoped to ensure proper isolation in server-side scenarios and
        /// to allow access to scoped services like logging or authentication state.
        /// </para>
        /// <para>
        /// If no custom error handler is registered, Blazouter will use its default error handling behavior,
        /// which displays the error UI defined in the Router component's ErrorContent parameter.
        /// </para>
        /// <para>
        /// For the error handler to display custom error UI, you must also define an ErrorContent section
        /// in your Router component. The error handler primarily controls logging and whether errors should
        /// be handled gracefully (return true) or rethrown (return false).
        /// </para>
        /// </remarks>
        /// <example>
        /// Register a custom error handler in Program.cs:
        /// <code>
        /// // Blazor WebAssembly
        /// builder.Services.AddBlazouter();
        /// builder.Services.AddBlazouterErrorHandler&lt;CustomRouterErrorHandler&gt;();
        /// 
        /// // Blazor Server
        /// builder.Services.AddBlazouter();
        /// builder.Services.AddBlazouterErrorHandler&lt;CustomRouterErrorHandler&gt;();
        /// 
        /// // Custom error handler implementation:
        /// public class CustomRouterErrorHandler : IRouterErrorHandler
        /// {
        ///     private readonly ILogger&lt;CustomRouterErrorHandler&gt; _logger;
        ///     
        ///     public CustomRouterErrorHandler(ILogger&lt;CustomRouterErrorHandler&gt; logger)
        ///     {
        ///         _logger = logger;
        ///     }
        ///     
        ///     public Task&lt;bool&gt; HandleErrorAsync(Exception exception, RouterErrorContext context)
        ///     {
        ///         _logger.LogError(exception, "Routing error: {ErrorType}", context.ErrorType);
        ///         return Task.FromResult(true); // Handle gracefully
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <seealso cref="IRouterErrorHandler"/>
        /// <seealso cref="RouterErrorContext"/>
        /// <seealso cref="DefaultRouterErrorHandler"/>
        public static IServiceCollection AddBlazouterErrorHandler<THandler>(this IServiceCollection services) where THandler : class, IRouterErrorHandler
        {
            services.AddScoped<IRouterErrorHandler, THandler>();

            return services;
        }

        /// <summary>
        /// Adds Blazouter services with custom cache configuration.
        /// </summary>
        /// <param name="services">The service collection to add Blazouter services to.</param>
        /// <param name="configureOptions">An action to configure cache options.</param>
        /// <returns>
        /// The same service collection so that multiple calls can be chained.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method allows customization of caching behavior including:
        /// </para>
        /// <list type="bullet">
        /// <item><description>Cache size limits for route matches and component types</description></item>
        /// <item><description>Time-to-live (TTL) settings for cached entries</description></item>
        /// <item><description>Enabling/disabling specific cache features</description></item>
        /// <item><description>Statistics tracking for monitoring cache performance</description></item>
        /// </list>
        /// <para>
        /// Cache configuration affects performance and memory usage. Default settings are
        /// optimized for typical applications, but you may need to adjust them based on
        /// your specific routing patterns and memory constraints.
        /// </para>
        /// </remarks>
        /// <example>
        /// Configure caching with custom settings:
        /// <code>
        /// builder.Services.AddBlazouter(options =>
        /// {
        ///     // Increase cache size for applications with many routes
        ///     options.MaxRouteMatchCacheSize = 200;
        ///     
        ///     // Enable statistics tracking for monitoring
        ///     options.EnableStatistics = true;
        ///     
        ///     // Set TTL for development scenarios with changing routes
        ///     options.RouteMatchCacheTTLSeconds = 300; // 5 minutes
        /// });
        /// 
        /// // Disable caching entirely (useful for debugging)
        /// builder.Services.AddBlazouter(options =>
        /// {
        ///     options.EnableRouteMatchCache = false;
        ///     options.EnableComponentTypeCache = false;
        /// });
        /// </code>
        /// </example>
        /// <seealso cref="CacheOptions"/>
        /// <seealso cref="IRouteCacheService"/>
        public static IServiceCollection AddBlazouter(this IServiceCollection services, Action<CacheOptions> configureOptions)
        {
            CacheOptions options = new();
            configureOptions?.Invoke(options);

            services.AddSingleton<RouterStateService>();
            services.AddScoped<RouterNavigationService>();

            // Add caching with custom options
            services.AddSingleton(options);
            services.AddSingleton<IRouteCacheService, RouteCacheService>();
            services.AddSingleton<IRouteMatcherService, CachedRouteMatcherService>();

            return services;
        }

        /// <summary>
        /// Adds JavaScript interop services for Blazouter to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection to add interop services to.</param>
        /// <returns>
        /// The same service collection so that multiple calls can be chained.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method registers TypeScript-based JavaScript interop services for enhanced browser integration.
        /// The services provide type-safe access to browser APIs such as:
        /// </para>
        /// <list type="bullet">
        /// <item>
        /// <description><see cref="NavigationInterop"/> - Browser history navigation (back, forward, state management)</description>
        /// </item>
        /// <item>
        /// <description><see cref="DocumentInterop"/> - Document manipulation (title, meta tags, scrolling, focus)</description>
        /// </item>
        /// </list>
        /// <para>
        /// These services are optional but highly recommended for production applications that need:
        /// </para>
        /// <list type="bullet">
        /// <item><description>Proper browser back/forward button support</description></item>
        /// <item><description>Dynamic document title and meta tag updates for SEO</description></item>
        /// <item><description>Scroll restoration and element focusing</description></item>
        /// <item><description>Open Graph tag management for social media sharing</description></item>
        /// </list>
        /// <para>
        /// The interop services require the compiled JavaScript modules to be included in your application.
        /// These are automatically included when you reference the Blazouter package.
        /// </para>
        /// </remarks>
        /// <example>
        /// Register interop services in Program.cs:
        /// <code>
        /// // Add core Blazouter services
        /// builder.Services.AddBlazouter();
        /// 
        /// // Add JavaScript interop services for enhanced functionality
        /// builder.Services.AddBlazouterInterop();
        /// 
        /// // Now you can use the async navigation methods:
        /// @inject RouterNavigationService NavService
        /// 
        /// private async Task HandleBackButton()
        /// {
        ///     await NavService.GoBackAsync();
        /// }
        /// 
        /// // And document manipulation:
        /// @inject DocumentInterop DocumentInterop
        /// 
        /// protected override async Task OnInitializedAsync()
        /// {
        ///     await DocumentInterop.SetTitleAsync("Home - My App");
        ///     await DocumentInterop.ScrollToTopAsync();
        /// }
        /// 
        /// // Storage operations:
        /// @inject StorageInterop Storage
        /// 
        /// await Storage.SetLocalStorageAsync("theme", "dark");
        /// var theme = await Storage.GetLocalStorageAsync&lt;string&gt;("theme");
        /// 
        /// // Viewport and device detection:
        /// @inject ViewportInterop Viewport
        /// 
        /// var deviceType = await Viewport.GetDeviceTypeAsync(); // "mobile", "tablet", or "desktop"
        /// 
        /// // Clipboard operations:
        /// @inject ClipboardInterop Clipboard
        /// 
        /// await Clipboard.CopyTextAsync("Hello, World!");
        /// </code>
        /// </example>
        /// <seealso cref="StorageInterop"/>
        /// <seealso cref="DocumentInterop"/>
        /// <seealso cref="ViewportInterop"/>
        /// <seealso cref="ClipboardInterop"/>
        /// <seealso cref="NavigationInterop"/>
        public static IServiceCollection AddBlazouterInterop(this IServiceCollection services)
        {
            services.AddScoped<StorageInterop>();
            services.AddScoped<DocumentInterop>();
            services.AddScoped<ViewportInterop>();
            services.AddScoped<ClipboardInterop>();
            services.AddScoped<NavigationInterop>();

            return services;
        }
    }
}