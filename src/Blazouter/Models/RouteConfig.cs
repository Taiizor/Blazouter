using Blazouter.Enums;

namespace Blazouter.Models
{
    /// <summary>
    /// Represents a route configuration in the Blazouter routing system.
    /// </summary>
    /// <remarks>
    /// <para>
    /// RouteConfig defines how URLs are mapped to components and what behavior should occur during navigation.
    /// Each configuration can include path patterns with parameters, nested child routes, guards for access control,
    /// lazy loading capabilities, and visual transitions.
    /// </para>
    /// <para>
    /// Path patterns support dynamic parameters using the ':' prefix (e.g., "/users/:id" where 'id' is a parameter).
    /// Nested routes allow creating hierarchical route structures where child components render within parent components
    /// using the RouterOutlet component.
    /// </para>
    /// </remarks>
    /// <example>
    /// Basic route configuration:
    /// <code>
    /// new RouteConfig
    /// {
    ///     Path = "/about",
    ///     Component = typeof(AboutPage),
    ///     Title = "About Us",
    ///     Transition = RouteTransition.Fade
    /// }
    /// </code>
    /// </example>
    /// <example>
    /// Route with dynamic parameters:
    /// <code>
    /// new RouteConfig
    /// {
    ///     Path = "/users/:id",
    ///     Component = typeof(UserDetailPage),
    ///     Guards = new List&lt;Type&gt; { typeof(AuthGuard) }
    /// }
    /// </code>
    /// </example>
    public class RouteConfig
    {
        /// <summary>
        /// Gets or sets the path pattern for this route.
        /// </summary>
        /// <value>
        /// A string representing the URL pattern. Can include dynamic parameters prefixed with ':' (e.g., "/users/:id").
        /// Empty string or "/" represents the root path.
        /// </value>
        /// <remarks>
        /// Dynamic parameters in the path (e.g., :id, :slug) will be extracted and made available through
        /// the RouterStateService. Parameter names must be valid identifiers and are case-sensitive.
        /// </remarks>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the component type to render when this route matches.
        /// </summary>
        /// <value>
        /// A Type that inherits from ComponentBase, or null if using ComponentLoader for lazy loading.
        /// </value>
        /// <remarks>
        /// Must be a valid Blazor component type. Either Component or ComponentLoader should be specified, but not both.
        /// If both are specified, Component takes precedence.
        /// </remarks>
        public Type? Component { get; set; } = null;

        /// <summary>
        /// Gets or sets a function that returns the component type asynchronously for lazy loading scenarios.
        /// </summary>
        /// <value>
        /// An async function that returns a component Type when invoked, or null if using direct component loading.
        /// </value>
        /// <remarks>
        /// <para>
        /// Use ComponentLoader for lazy loading to improve initial bundle size and load time. The component will only
        /// be loaded when the route is first accessed. Subsequent navigations to the same route will reuse the loaded component.
        /// </para>
        /// <para>
        /// While the component is loading, the Router's Loading parameter content will be displayed (if specified).
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// new RouteConfig
        /// {
        ///     Path = "/reports",
        ///     ComponentLoader = async () =>
        ///     {
        ///         await Task.Delay(100); // Simulate loading
        ///         return typeof(ReportsPage);
        ///     }
        /// }
        /// </code>
        /// </example>
        public Func<Task<Type>>? ComponentLoader { get; set; } = null;

        /// <summary>
        /// Gets or sets the collection of child/nested route configurations.
        /// </summary>
        /// <value>
        /// A list of RouteConfig objects representing nested routes under this route. Defaults to an empty list.
        /// </value>
        /// <remarks>
        /// <para>
        /// Child routes enable hierarchical URL structures and nested components. When a child route matches,
        /// the parent component is rendered with a RouterOutlet component that displays the matched child.
        /// </para>
        /// <para>
        /// Child route paths are relative to their parent's path. For example, if the parent path is "/users"
        /// and a child path is ":id", the full path becomes "/users/:id".
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// new RouteConfig
        /// {
        ///     Path = "/products",
        ///     Component = typeof(ProductLayout),
        ///     Children = new List&lt;RouteConfig&gt;
        ///     {
        ///         new RouteConfig { Path = "", Component = typeof(ProductList), Exact = true },
        ///         new RouteConfig { Path = ":id", Component = typeof(ProductDetail) }
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<RouteConfig> Children { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of route middleware types to execute during navigation to this route.
        /// </summary>
        /// <value>
        /// A list of Types that implement IRouteMiddleware. Middleware are executed in the order specified. Defaults to an empty list.
        /// </value>
        /// <remarks>
        /// <para>
        /// Route middleware provides a way to execute code before and after route navigation. Unlike guards which
        /// focus solely on access control, middleware can perform any arbitrary logic such as logging, analytics,
        /// data preloading, caching, and more.
        /// </para>
        /// <para>
        /// Middleware executes in a pipeline pattern where each middleware can:
        /// - Execute logic before navigation (code before calling next())
        /// - Execute logic after navigation (code after calling next())
        /// - Short-circuit navigation by not calling next()
        /// - Modify context data that components can access
        /// - Abort navigation or redirect to a different path
        /// </para>
        /// <para>
        /// Middleware are executed before route guards. If middleware aborts navigation, guards are not executed.
        /// Middleware are instantiated either through dependency injection or via Activator.CreateInstance.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// new RouteConfig
        /// {
        ///     Path = "/admin",
        ///     Component = typeof(AdminPage),
        ///     Middleware = new List&lt;Type&gt; 
        ///     { 
        ///         typeof(LoggingMiddleware),
        ///         typeof(TimingMiddleware),
        ///         typeof(AnalyticsMiddleware)
        ///     }
        /// }
        /// </code>
        /// </example>
        public List<Type> Middleware { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of route guard types to execute before allowing navigation to this route.
        /// </summary>
        /// <value>
        /// A list of Types that implement IRouteGuard. Guards are executed in the order specified. Defaults to an empty list.
        /// </value>
        /// <remarks>
        /// <para>
        /// Route guards provide a mechanism to control access to routes based on authentication, authorization,
        /// or custom logic. All guards must pass (return true from CanActivateAsync) for navigation to proceed.
        /// </para>
        /// <para>
        /// If any guard fails, the guard can optionally specify a redirect path through GetRedirectPathAsync.
        /// Guards are instantiated either through dependency injection or via Activator.CreateInstance.
        /// Guards are executed after middleware. If middleware aborts navigation, guards are not executed.
        /// </para>
        /// </remarks>
        public List<Type> Guards { get; set; } = [];

        /// <summary>
        /// Gets or sets the path to redirect to when this route matches.
        /// </summary>
        /// <value>
        /// A string representing the target redirect path, or null if no redirect should occur.
        /// </value>
        /// <remarks>
        /// When specified, navigation to this route will automatically redirect to the specified path
        /// without rendering any component. Useful for route aliases or migrating old URLs.
        /// </remarks>
        /// <example>
        /// <code>
        /// new RouteConfig
        /// {
        ///     Path = "/old-path",
        ///     RedirectTo = "/new-path"
        /// }
        /// </code>
        /// </example>
        public string? RedirectTo { get; set; } = null;

        /// <summary>
        /// Gets or sets custom data to associate with this route.
        /// </summary>
        /// <value>
        /// A dictionary of key-value pairs containing arbitrary data. Defaults to an empty dictionary.
        /// </value>
        /// <remarks>
        /// Route data can be used to pass configuration or metadata to components without using route parameters.
        /// Data is accessible through the RouteMatch object and can be injected as component parameters.
        /// </remarks>
        /// <example>
        /// <code>
        /// new RouteConfig
        /// {
        ///     Path = "/admin",
        ///     Component = typeof(AdminPage),
        ///     Data = new Dictionary&lt;string, object&gt;
        ///     {
        ///         { "RequireAdmin", true },
        ///         { "Section", "Management" }
        ///     }
        /// }
        /// </code>
        /// </example>
        public Dictionary<string, object> Data { get; set; } = [];

        /// <summary>
        /// Gets or sets a value indicating whether this route should match the URL exactly.
        /// </summary>
        /// <value>
        /// true if the route path must match the URL exactly; false to allow partial matches. Defaults to false.
        /// </value>
        /// <remarks>
        /// <para>
        /// When false (default), routes with child routes can match partially. For example, "/users" would match
        /// both "/users" and "/users/123". When true, the path must match exactly.
        /// </para>
        /// <para>
        /// This is particularly useful for routes without children where you want to ensure an exact match,
        /// or for default child routes that should only match when no other child route matches.
        /// </para>
        /// </remarks>
        public bool Exact { get; set; } = false;

        /// <summary>
        /// Gets or sets the transition animation to apply when navigating to this route.
        /// </summary>
        /// <value>
        /// A RouteTransition enum value specifying the animation type. Defaults to RouteTransition.None.
        /// </value>
        /// <remarks>
        /// Transitions provide visual feedback during navigation. Available transitions include Fade, Slide,
        /// SlideUp, and Scale. CSS classes are automatically applied based on the transition type.
        /// Transitions can be disabled at the Router level by setting EnableTransitions to false.
        /// </remarks>
        public RouteTransition Transition { get; set; } = RouteTransition.None;

        /// <summary>
        /// Gets or sets an optional title for the route.
        /// </summary>
        /// <value>
        /// A string representing the route title, or null if no title is specified.
        /// </value>
        /// <remarks>
        /// The title can be used for various purposes such as setting the browser page title, generating breadcrumbs,
        /// displaying in navigation menus, or for SEO purposes. The router doesn't automatically set the page title;
        /// this must be handled in application code if desired.
        /// </remarks>
        public string? Title { get; set; } = null;

        /// <summary>
        /// Gets or sets the layout component type to use for this route.
        /// </summary>
        /// <value>
        /// A Type that inherits from LayoutComponentBase, or null to use the router's default layout or no layout.
        /// </value>
        /// <remarks>
        /// <para>
        /// When specified, the route's component will be rendered inside this layout. The layout must inherit
        /// from LayoutComponentBase and implement the standard Blazor layout pattern with @Body.
        /// </para>
        /// <para>
        /// If explicitly set to null, no layout will be used for this route, even if the router has a DefaultLayout.
        /// If not set at all, the router will use its DefaultLayout parameter if provided.
        /// </para>
        /// <para>
        /// This allows you to have a common layout for most routes while allowing specific routes to use different
        /// layouts or explicitly opt out of any layout.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// new RouteConfig
        /// {
        ///     Path = "/admin",
        ///     Component = typeof(AdminDashboard),
        ///     Layout = typeof(AdminLayout) // Use specific layout for admin pages
        /// }
        /// 
        /// new RouteConfig
        /// {
        ///     Path = "/print",
        ///     Component = typeof(PrintView),
        ///     Layout = null // Explicitly no layout, even if DefaultLayout is set
        /// }
        /// </code>
        /// </example>
        public Type? Layout
        {
            get;
            set
            {
                field = value;
                HasExplicitLayout = true;
            }
        } = null;

        /// <summary>
        /// Gets a value indicating whether the Layout property was explicitly set (even if set to null).
        /// </summary>
        internal bool HasExplicitLayout { get; private set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether caching is enabled for this specific route.
        /// </summary>
        /// <value>
        /// true to enable caching for this route; false to disable; null to use global cache settings. Defaults to null.
        /// </value>
        /// <remarks>
        /// <para>
        /// This property allows fine-grained control over caching on a per-route basis. When set to null (default),
        /// the global cache settings from CacheOptions are used. When explicitly set to true or false, it overrides
        /// the global settings for this specific route.
        /// </para>
        /// <para>
        /// Use cases for disabling caching on specific routes:
        /// </para>
        /// <list type="bullet">
        /// <item><description>Routes with dynamic content that changes frequently</description></item>
        /// <item><description>Admin routes where you always want fresh data</description></item>
        /// <item><description>Routes with user-specific content that shouldn't be cached</description></item>
        /// <item><description>Real-time dashboards or monitoring pages</description></item>
        /// </list>
        /// <para>
        /// Note: This affects route match caching. Component type caching for lazy-loaded components
        /// is controlled separately through CacheOptions.EnableComponentTypeCache.
        /// </para>
        /// </remarks>
        /// <example>
        /// Disable caching for a specific route:
        /// <code>
        /// new RouteConfig
        /// {
        ///     Path = "/admin/dashboard",
        ///     Component = typeof(AdminDashboard),
        ///     EnableCache = false  // This route will never be cached
        /// }
        /// </code>
        /// </example>
        /// <example>
        /// Force caching even if global caching is disabled:
        /// <code>
        /// new RouteConfig
        /// {
        ///     Path = "/static-page",
        ///     Component = typeof(StaticPage),
        ///     EnableCache = true  // This route will always be cached
        /// }
        /// </code>
        /// </example>
        public bool? EnableCache { get; set; } = null;
    }
}