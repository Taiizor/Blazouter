using Blazouter.Attributes;
using Blazouter.Models;
using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using RouteAttribute = Blazouter.Attributes.RouteAttribute;

namespace Blazouter.Services
{
    /// <summary>
    /// Provides services for discovering routes defined via attributes and converting them to RouteConfig objects.
    /// </summary>
    /// <remarks>
    /// <para>
    /// RouteAttributeDiscoveryService enables declarative route definition using attributes on component classes
    /// as an alternative to programmatic route configuration. It scans assemblies for components decorated with
    /// Blazouter route attributes and automatically generates RouteConfig objects.
    /// </para>
    /// <para>
    /// This attribute-based approach offers several advantages:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Route definitions live alongside component code, improving maintainability</description></item>
    /// <item><description>Reduces boilerplate code for simple routing scenarios</description></item>
    /// <item><description>Supports all routing features (guards, transitions, layouts, etc.) via attributes</description></item>
    /// <item><description>Can be mixed with programmatic route configuration</description></item>
    /// <item><description>Automatic discovery eliminates manual route registration</description></item>
    /// </list>
    /// <para>
    /// Supported attributes include: [Route], [RouteMiddleware], [RouteGuard], [RouteTransition], [RouteLayout], 
    /// [RouteTitle], [RouteData], [RouteRedirect], and [RouteExact]. Multiple attributes can be combined on a 
    /// single component.
    /// </para>
    /// <para>
    /// <strong>Important:</strong> Route discovery uses reflection to scan assemblies. This may not work correctly
    /// with trimmed or ahead-of-time (AOT) compiled applications unless proper trim warnings are preserved.
    /// The RequiresUnreferencedCode attribute warns about this limitation.
    /// </para>
    /// </remarks>
    /// <example>
    /// Discover routes from application assembly:
    /// <code>
    /// // In Program.cs or configuration
    /// var routes = RouteAttributeDiscoveryService.DiscoverRoutes(typeof(Program).Assembly);
    /// 
    /// // In Router component
    /// &lt;Router Routes="@routes"&gt;
    ///     &lt;NotFound&gt;&lt;h1&gt;404&lt;/h1&gt;&lt;/NotFound&gt;
    /// &lt;/Router&gt;
    /// </code>
    /// </example>
    /// <example>
    /// Discover routes from multiple assemblies:
    /// <code>
    /// // Scan both main app and shared component library
    /// var routes = RouteAttributeDiscoveryService.DiscoverRoutes(
    ///     typeof(App).Assembly,
    ///     typeof(SharedComponents.Base).Assembly
    /// );
    /// </code>
    /// </example>
    /// <example>
    /// Example component with route attributes:
    /// <code>
    /// [Route("/admin/users")]
    /// [RouteGuard(typeof(AuthGuard))]
    /// [RouteGuard(typeof(AdminGuard))]
    /// [RouteTransition(RouteTransition.Fade)]
    /// [RouteLayout(typeof(AdminLayout))]
    /// [RouteTitle("User Management")]
    /// [RouteData("RequireAdmin", true)]
    /// [RouteData("Section", "Users")]
    /// public class AdminUsersPage : ComponentBase
    /// {
    ///     // Component implementation
    /// }
    /// </code>
    /// </example>
    public static class RouteAttributeDiscoveryService
    {
        /// <summary>
        /// Discovers all components with route attributes in the specified assemblies and converts them to RouteConfig objects.
        /// </summary>
        /// <param name="assemblies">
        /// One or more assemblies to scan for components with route attributes. Each assembly is fully scanned
        /// for all types that inherit from ComponentBase and have a [Route] attribute.
        /// </param>
        /// <returns>
        /// A list of <see cref="RouteConfig"/> objects, one for each component found with a [Route] attribute.
        /// Returns an empty list if no components with route attributes are found.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method performs a comprehensive scan of all types in the specified assemblies, filtering for:
        /// </para>
        /// <list type="number">
        /// <item><description>Types that are classes (not interfaces, structs, or enums)</description></item>
        /// <item><description>Types that are not abstract</description></item>
        /// <item><description>Types that inherit from ComponentBase</description></item>
        /// <item><description>Types decorated with the [Route] attribute</description></item>
        /// </list>
        /// <para>
        /// For each matching component, the method extracts all supported route attributes and creates a
        /// corresponding RouteConfig object with all properties populated from the attributes.
        /// </para>
        /// <para>
        /// <strong>Performance Consideration:</strong> Assembly scanning can be expensive for large assemblies.
        /// Consider calling this once at startup and caching the results rather than repeatedly discovering
        /// routes. The returned list can be stored and reused.
        /// </para>
        /// <para>
        /// <strong>Trimming Warning:</strong> In trimmed or AOT-compiled applications, types and attributes
        /// may be removed by the trimmer if they're not directly referenced. Ensure route attribute usage
        /// is preserved in the trim configuration.
        /// </para>
        /// </remarks>
        /// <example>
        /// Common usage pattern in application startup:
        /// <code>
        /// // In Program.cs, App.razor, or Routes.razor
        /// public class RoutesConfig
        /// {
        ///     private static List&lt;RouteConfig&gt;? _cachedRoutes;
        ///     
        ///     public static List&lt;RouteConfig&gt; GetRoutes()
        ///     {
        ///         if (_cachedRoutes == null)
        ///         {
        ///             _cachedRoutes = RouteAttributeDiscoveryService.DiscoverRoutes(
        ///                 typeof(Program).Assembly
        ///             );
        ///         }
        ///         return _cachedRoutes;
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">Thrown if assemblies parameter is null.</exception>
        /// <seealso cref="FromType(Type)"/>
        [RequiresUnreferencedCode("Scanning for route attributes requires unreferenced code. Types might be removed during trimming.")]
        public static List<RouteConfig> DiscoverRoutes(params Assembly[] assemblies)
        {
            List<RouteConfig> routes = [];

            foreach (Assembly assembly in assemblies)
            {
                IEnumerable<Type> componentTypes = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && typeof(ComponentBase).IsAssignableFrom(t));

                foreach (Type componentType in componentTypes)
                {
                    RouteAttribute? routeAttr = componentType.GetCustomAttribute<RouteAttribute>(inherit: true);
                    if (routeAttr != null)
                    {
                        RouteConfig config = CreateRouteConfig(componentType, routeAttr);
                        routes.Add(config);
                    }
                }
            }

            return routes;
        }

        /// <summary>
        /// Creates a RouteConfig object from a component type and its route attributes.
        /// </summary>
        /// <param name="componentType">The component type to extract attributes from.</param>
        /// <param name="routeAttr">The primary Route attribute defining the path.</param>
        /// <returns>
        /// A fully populated <see cref="RouteConfig"/> object with all properties set from the component's attributes.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This internal method handles the conversion from attribute-based configuration to the RouteConfig object
        /// model. It systematically checks for each supported route attribute type and populates the corresponding
        /// RouteConfig properties.
        /// </para>
        /// <para>
        /// The method processes attributes in the following order:
        /// </para>
        /// <list type="number">
        /// <item><description>Route path from [Route] attribute</description></item>
        /// <item><description>Transitions from [RouteTransition] attribute</description></item>
        /// <item><description>Middleware from [RouteMiddleware] attributes (supports multiple)</description></item>
        /// <item><description>Guards from [RouteGuard] attributes (supports multiple)</description></item>
        /// <item><description>Layout from [RouteLayout] attribute</description></item>
        /// <item><description>Title from [RouteTitle] attribute</description></item>
        /// <item><description>Data from [RouteData] attributes (supports multiple)</description></item>
        /// <item><description>Redirect from [RouteRedirect] attribute</description></item>
        /// <item><description>Exact matching from [RouteExact] attribute</description></item>
        /// </list>
        /// <para>
        /// Multiple instances of attributes that support AllowMultiple (RouteMiddleware, RouteGuard, RouteData) 
        /// are all collected and included in the configuration.
        /// </para>
        /// </remarks>
        private static RouteConfig CreateRouteConfig(Type componentType, RouteAttribute routeAttr)
        {
            RouteConfig config = new()
            {
                Path = routeAttr.Path,
                Component = componentType
            };

            // Route Transition
            RouteTransitionAttribute? transitionAttr = componentType.GetCustomAttribute<RouteTransitionAttribute>(inherit: true);
            if (transitionAttr != null)
            {
                config.Transition = transitionAttr.Transition;
            }

            // Route Middleware - supports multiple middleware attributes
            RouteMiddlewareAttribute[] middlewareAttrs = [.. componentType.GetCustomAttributes<RouteMiddlewareAttribute>(inherit: true)];
            if (middlewareAttrs.Length > 0)
            {
                config.Middleware = [.. middlewareAttrs.Select(m => m.MiddlewareType)];
            }

            // Route Guards - supports multiple guard attributes
            RouteGuardAttribute[] guardAttrs = [.. componentType.GetCustomAttributes<RouteGuardAttribute>(inherit: true)];
            if (guardAttrs.Length > 0)
            {
                config.Guards = [.. guardAttrs.Select(g => g.GuardType)];
            }

            // Route Layout
            RouteLayoutAttribute? layoutAttr = componentType.GetCustomAttribute<RouteLayoutAttribute>(inherit: true);
            if (layoutAttr != null)
            {
                config.Layout = layoutAttr.LayoutType;
            }

            // Route Title
            RouteTitleAttribute? titleAttr = componentType.GetCustomAttribute<RouteTitleAttribute>(inherit: true);
            if (titleAttr != null)
            {
                config.Title = titleAttr.Title;
            }

            // Route Data - supports multiple data attributes
            RouteDataAttribute[] dataAttrs = [.. componentType.GetCustomAttributes<RouteDataAttribute>(inherit: true)];
            if (dataAttrs.Length > 0)
            {
                config.Data = dataAttrs.ToDictionary(d => d.Key, d => d.Value);
            }

            // Route Redirect
            RouteRedirectAttribute? redirectAttr = componentType.GetCustomAttribute<RouteRedirectAttribute>(inherit: true);
            if (redirectAttr != null)
            {
                config.RedirectTo = redirectAttr.RedirectPath;
            }

            // Route Exact
            RouteExactAttribute? exactAttr = componentType.GetCustomAttribute<RouteExactAttribute>(inherit: true);
            if (exactAttr != null)
            {
                config.Exact = exactAttr.Exact;
            }

            // Route Cache
            RouteCacheAttribute? cacheAttr = componentType.GetCustomAttribute<RouteCacheAttribute>(inherit: true);
            if (cacheAttr != null)
            {
                config.EnableCache = cacheAttr.EnableCache;
            }

            return config;
        }

        /// <summary>
        /// Converts a single component type with route attributes to a RouteConfig object.
        /// </summary>
        /// <param name="componentType">
        /// The component type to convert. Must be a class that inherits from ComponentBase and is decorated
        /// with the [Route] attribute.
        /// </param>
        /// <returns>
        /// A <see cref="RouteConfig"/> object populated from the component's route attributes, or null if the
        /// type doesn't have a [Route] attribute.
        /// </returns>
        /// <remarks>
        /// <para>
        /// This method provides a way to convert a single known component type to RouteConfig without scanning
        /// entire assemblies. It's useful when you have a specific component and want to generate its route
        /// configuration programmatically.
        /// </para>
        /// <para>
        /// Unlike DiscoverRoutes which scans assemblies, this method works with a specific type, making it
        /// suitable for:
        /// </para>
        /// <list type="bullet">
        /// <item><description>Dynamic route generation based on runtime conditions</description></item>
        /// <item><description>Converting specific components without full assembly scanning</description></item>
        /// <item><description>Unit testing route attribute configuration</description></item>
        /// <item><description>Validating that a component has proper route attributes</description></item>
        /// </list>
        /// <para>
        /// If the component type doesn't have a [Route] attribute, the method returns null rather than throwing
        /// an exception, allowing it to be used in conditional scenarios.
        /// </para>
        /// </remarks>
        /// <example>
        /// Convert specific component types to routes:
        /// <code>
        /// var routes = new List&lt;RouteConfig&gt;();
        /// 
        /// // Try to add specific components
        /// var homeRoute = RouteAttributeDiscoveryService.FromType(typeof(HomePage));
        /// if (homeRoute != null)
        ///     routes.Add(homeRoute);
        ///     
        /// var aboutRoute = RouteAttributeDiscoveryService.FromType(typeof(AboutPage));
        /// if (aboutRoute != null)
        ///     routes.Add(aboutRoute);
        /// </code>
        /// </example>
        /// <example>
        /// Validate component has route attributes:
        /// <code>
        /// public bool HasValidRouteAttributes(Type componentType)
        /// {
        ///     var config = RouteAttributeDiscoveryService.FromType(componentType);
        ///     return config != null &amp;&amp; !string.IsNullOrEmpty(config.Path);
        /// }
        /// </code>
        /// </example>
        /// <exception cref="ArgumentNullException">Thrown if componentType is null.</exception>
        public static RouteConfig? FromType(Type componentType)
        {
            ArgumentNullException.ThrowIfNull(componentType);

            RouteAttribute? routeAttr = componentType.GetCustomAttribute<RouteAttribute>(inherit: true);
            if (routeAttr == null)
            {
                return null;
            }

            return CreateRouteConfig(componentType, routeAttr);
        }
    }
}