using Blazouter.Components.Layouts;
using Blazouter.Enums;
using Blazouter.Extensions;
using Blazouter.Handlers;
using Blazouter.Interfaces;
using Blazouter.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using System.Reflection;

namespace Blazouter.Components
{
    /// <summary>
    /// The main routing component that handles URL matching and component rendering for Blazouter.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The Router component is the core of Blazouter's routing system. It monitors browser URL changes,
    /// matches them against configured routes, executes route guards, handles lazy loading, and renders
    /// the appropriate components with transitions.
    /// </para>
    /// <para>
    /// Key responsibilities:
    /// </para>
    /// <list type="bullet">
    /// <item><description>Route matching using the IRouteMatcherService</description></item>
    /// <item><description>Route guard execution for access control</description></item>
    /// <item><description>Lazy loading of route components</description></item>
    /// <item><description>Route transition animations</description></item>
    /// <item><description>Handling redirects</description></item>
    /// <item><description>Managing router state through RouterStateService</description></item>
    /// <item><description>Error handling and recovery for routing failures</description></item>
    /// </list>
    /// <para>
    /// The Router must be placed at the root level of your application and configured with a collection
    /// of RouteConfig objects that define your application's routes.
    /// </para>
    /// </remarks>
    /// <example>
    /// Basic router setup in App.razor:
    /// <code>
    /// @using Blazouter.Components
    /// @using Blazouter.Models
    /// 
    /// &lt;Router Routes="@_routes"&gt;
    ///     &lt;NotFound&gt;
    ///         &lt;h1&gt;404 - Page Not Found&lt;/h1&gt;
    ///     &lt;/NotFound&gt;
    ///     &lt;Loading&gt;
    ///         &lt;p&gt;Loading...&lt;/p&gt;
    ///     &lt;/Loading&gt;
    ///     &lt;ErrorContent Context="errorInfo"&gt;
    ///         &lt;h1&gt;Error&lt;/h1&gt;
    ///         &lt;p&gt;@errorInfo.Message&lt;/p&gt;
    ///     &lt;/ErrorContent&gt;
    /// &lt;/Router&gt;
    /// 
    /// @code {
    ///     private List&lt;RouteConfig&gt; _routes = new()
    ///     {
    ///         new RouteConfig
    ///         {
    ///             Path = "/",
    ///             Component = typeof(Home),
    ///             Transition = RouteTransition.Fade
    ///         },
    ///         new RouteConfig
    ///         {
    ///             Path = "/users/:id",
    ///             Component = typeof(UserDetail),
    ///             Guards = new List&lt;Type&gt; { typeof(AuthGuard) }
    ///         }
    ///     };
    /// }
    /// </code>
    /// </example>
    public partial class Router
    {
        /// <summary>
        /// Gets or sets the collection of route configurations that define the application's routing structure.
        /// </summary>
        /// <value>
        /// A list of <see cref="RouteConfig"/> objects. This parameter is required and must be provided.
        /// </value>
        /// <remarks>
        /// <para>
        /// This is the primary configuration for the router. Routes are processed in order, and the first
        /// matching route is used. Routes can contain nested children, dynamic parameters, guards, and
        /// other configuration options.
        /// </para>
        /// <para>
        /// The EditorRequired attribute ensures that this parameter must be explicitly set, preventing
        /// configuration errors.
        /// </para>
        /// </remarks>
        [Parameter, EditorRequired]
        public List<RouteConfig> Routes { get; set; } = [];

        /// <summary>
        /// Gets or sets the content to display when no route matches the current URL.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment"/> containing the 404/not found UI, or null to display nothing.
        /// </value>
        /// <remarks>
        /// <para>
        /// This content is displayed when the URL doesn't match any configured route. It's typically used
        /// to show a "404 Page Not Found" message with navigation options.
        /// </para>
        /// <para>
        /// Use as child content in the Router component: &lt;NotFound&gt;...&lt;/NotFound&gt;
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// &lt;Router Routes="@_routes"&gt;
        ///     &lt;NotFound&gt;
        ///         &lt;div class="not-found"&gt;
        ///             &lt;h1&gt;404 - Page Not Found&lt;/h1&gt;
        ///             &lt;p&gt;The page you're looking for doesn't exist.&lt;/p&gt;
        ///             &lt;RouterLink Href="/"&gt;Go Home&lt;/RouterLink&gt;
        ///         &lt;/div&gt;
        ///     &lt;/NotFound&gt;
        /// &lt;/Router&gt;
        /// </code>
        /// </example>
        [Parameter]
        public RenderFragment? NotFound { get; set; }

        /// <summary>
        /// Gets or sets the content to display while lazy-loaded route components are being loaded.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment"/> containing the loading UI, or null to display nothing during loading.
        /// </value>
        /// <remarks>
        /// <para>
        /// This content is shown temporarily when navigating to a route that uses the ComponentLoader
        /// property for lazy loading. It provides visual feedback that the component is being loaded.
        /// </para>
        /// <para>
        /// Use as child content in the Router component: &lt;Loading&gt;...&lt;/Loading&gt;
        /// </para>
        /// <para>
        /// If not specified, the router will show nothing while loading, which may appear as a brief
        /// flash of blank content.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// &lt;Router Routes="@_routes"&gt;
        ///     &lt;Loading&gt;
        ///         &lt;div class="loading-spinner"&gt;
        ///             &lt;p&gt;Loading page...&lt;/p&gt;
        ///             &lt;div class="spinner"&gt;&lt;/div&gt;
        ///         &lt;/div&gt;
        ///     &lt;/Loading&gt;
        /// &lt;/Router&gt;
        /// </code>
        /// </example>
        [Parameter]
        public RenderFragment? Loading { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether route transitions and animations are enabled.
        /// </summary>
        /// <value>
        /// true to enable transitions (default); false to disable all route transitions.
        /// </value>
        /// <remarks>
        /// <para>
        /// When enabled, the router applies CSS transition classes based on each route's Transition property.
        /// This creates smooth visual effects when navigating between routes.
        /// </para>
        /// <para>
        /// You can disable transitions globally by setting this to false, or disable them per-route by
        /// setting the route's Transition property to RouteTransition.None.
        /// </para>
        /// <para>
        /// Transitions require the Blazouter CSS stylesheet to be included in your application for the
        /// transition classes to have any effect.
        /// </para>
        /// </remarks>
        [Parameter]
        public bool EnableTransitions { get; set; } = true;

        /// <summary>
        /// Gets or sets the default layout component type to use for routes that don't specify their own layout.
        /// </summary>
        /// <value>
        /// A Type that inherits from LayoutComponentBase, or null to not use a default layout.
        /// </value>
        /// <remarks>
        /// <para>
        /// This provides a centralized way to apply a common layout to all routes. Individual routes can
        /// override this by specifying their own Layout property in RouteConfig.
        /// </para>
        /// <para>
        /// The layout component must inherit from LayoutComponentBase and use @Body to render the route component.
        /// If a route explicitly sets Layout to null, no layout will be used for that route regardless of this setting.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// &lt;Router Routes="@_routes" DefaultLayout="typeof(MainLayout)"&gt;
        ///     &lt;NotFound&gt;&lt;h1&gt;404&lt;/h1&gt;&lt;/NotFound&gt;
        /// &lt;/Router&gt;
        /// </code>
        /// </example>
        [Parameter]
        public Type? DefaultLayout { get; set; } = typeof(BlankLayout);

        /// <summary>
        /// Gets or sets the content to display when a routing error occurs.
        /// </summary>
        /// <value>
        /// A <see cref="RenderFragment{RouterErrorInfo}"/> that receives error information, or null to display a default error message.
        /// </value>
        /// <remarks>
        /// <para>
        /// This content is displayed when an error occurs during routing operations such as component loading,
        /// guard execution, or component rendering. The error information is passed as context to the render fragment.
        /// </para>
        /// <para>
        /// Use as child content in the Router component: &lt;ErrorContent Context="errorInfo"&gt;...&lt;/ErrorContent&gt;
        /// </para>
        /// <para>
        /// If not specified, a default error message will be displayed showing the error type and message.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// &lt;Router Routes="@_routes"&gt;
        ///     &lt;ErrorContent Context="errorInfo"&gt;
        ///         &lt;div class="error-container"&gt;
        ///             &lt;h1&gt;Oops! Something went wrong&lt;/h1&gt;
        ///             &lt;p&gt;Error: @errorInfo.Message&lt;/p&gt;
        ///             &lt;button @onclick="errorInfo.Retry"&gt;Try Again&lt;/button&gt;
        ///         &lt;/div&gt;
        ///     &lt;/ErrorContent&gt;
        /// &lt;/Router&gt;
        /// </code>
        /// </example>
        [Parameter]
        public RenderFragment<RouterErrorInfo>? ErrorContent { get; set; }

        /// <summary>
        /// Gets or sets the event callback that is invoked when a routing error occurs.
        /// </summary>
        /// <value>
        /// An <see cref="EventCallback{RouterErrorEventArgs}"/> that receives error event data.
        /// </value>
        /// <remarks>
        /// <para>
        /// This event is invoked before the error UI is displayed, allowing you to perform custom
        /// error handling logic such as logging to external services, showing notifications, or
        /// implementing custom recovery strategies.
        /// </para>
        /// <para>
        /// The event provides access to the exception, error context, and allows you to cancel
        /// the default error UI display if you handle the error differently.
        /// </para>
        /// </remarks>
        /// <example>
        /// <code>
        /// &lt;Router Routes="@_routes" OnError="@HandleError"&gt;
        ///     &lt;NotFound&gt;&lt;h1&gt;404&lt;/h1&gt;&lt;/NotFound&gt;
        /// &lt;/Router&gt;
        /// 
        /// @code {
        ///     private async Task HandleError(RouterErrorEventArgs args)
        ///     {
        ///         // Log to external service
        ///         await MyLogger.LogErrorAsync(args.Exception, args.Context);
        ///         
        ///         // Show toast notification
        ///         await ToastService.ShowError("Navigation failed");
        ///     }
        /// }
        /// </code>
        /// </example>
        [Parameter]
        public EventCallback<RouterErrorEventArgs> OnError { get; set; }

        /// <summary>
        /// The current matched route
        /// </summary>
        private RouteMatch? _currentMatch;

        /// <summary>
        /// The type of component to render for the current route
        /// </summary>
        private Type? _componentType;

        /// <summary>
        /// The type of layout component to use for the current route
        /// </summary>
        private Type? _layoutType;

        /// <summary>
        /// Parameters to pass to the rendered component
        /// </summary>
        private Dictionary<string, object> _componentParameters = [];

        /// <summary>
        /// The current URL path
        /// </summary>
        private string _currentPath = "/";

        /// <summary>
        /// CSS class for the current transition animation
        /// </summary>
        private string _transitionClass = "";

        /// <summary>
        /// Indicates whether a lazy-loaded component is currently being loaded
        /// </summary>
        private bool _isLoading = false;

        /// <summary>
        /// Indicates whether an error has occurred during routing
        /// </summary>
        private bool _hasError = false;

        /// <summary>
        /// Information about the current routing error
        /// </summary>
        private RouterErrorInfo? _errorInfo = null;

        /// <summary>
        /// Indicates whether this is the first render (pre-rendering phase in Blazor Server)
        /// </summary>
        private bool _isFirstRender = true;

        /// <summary>
        /// Initializes the router component and performs the initial route matching.
        /// </summary>
        /// <returns>A task representing the asynchronous initialization operation.</returns>
        /// <remarks>
        /// This method subscribes to navigation events and performs the initial route match
        /// based on the current browser URL. It is called once when the component is first rendered.
        /// </remarks>
        protected override async Task OnInitializedAsync()
        {
            NavigationManager.LocationChanged += OnLocationChanged;
            await UpdateRoute();
        }

        /// <summary>
        /// Called after the component has been rendered.
        /// </summary>
        /// <param name="firstRender">True if this is the first render; otherwise false.</param>
        /// <remarks>
        /// This method is used to detect when we've moved from pre-rendering to interactive rendering
        /// in Blazor Server scenarios. After the first render, we enable lazy loading.
        /// </remarks>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && _isFirstRender)
            {
                _isFirstRender = false;
                // If we have a lazy route that wasn't loaded during pre-rendering, load it now
                await InvokeAsync(async () =>
                {
                    await UpdateRoute();
                    StateHasChanged();
                });
            }
        }

        /// <summary>
        /// Responds to parameter changes by re-evaluating the current route.
        /// </summary>
        /// <returns>A task representing the asynchronous parameter processing operation.</returns>
        /// <remarks>
        /// This method is called whenever the component's parameters change, such as when the
        /// Routes collection is updated. It ensures the displayed component stays synchronized
        /// with the route configuration.
        /// </remarks>
        protected override async Task OnParametersSetAsync()
        {
            await UpdateRoute();
        }

        /// <summary>
        /// Handles browser location/URL changes and updates the displayed component.
        /// </summary>
        /// <param name="sender">The event sender (typically the NavigationManager).</param>
        /// <param name="e">Event arguments containing information about the navigation event.</param>
        /// <remarks>
        /// <para>
        /// This event handler is triggered whenever the browser URL changes, whether through user
        /// interaction (clicking links, browser back/forward buttons) or programmatic navigation.
        /// </para>
        /// <para>
        /// After updating the route, StateHasChanged is called to ensure the component re-renders
        /// with the new route's component. InvokeAsync is used to marshal the call to the dispatcher
        /// thread, which is required in Blazor Server scenarios to avoid threading issues.
        /// </para>
        /// </remarks>
        private async void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            await InvokeAsync(async () =>
            {
                await UpdateRoute();
                StateHasChanged();
            });
        }

        /// <summary>
        /// Updates the current route based on the browser URL, handling all routing logic.
        /// </summary>
        /// <returns>A task representing the asynchronous route update operation.</returns>
        /// <remarks>
        /// <para>
        /// This method performs the complete routing pipeline:
        /// </para>
        /// <list type="number">
        /// <item><description>Extracts the path from the current URL</description></item>
        /// <item><description>Matches the path against configured routes</description></item>
        /// <item><description>Handles redirects if specified</description></item>
        /// <item><description>Executes route guards for access control</description></item>
        /// <item><description>Loads components lazily if using ComponentLoader</description></item>
        /// <item><description>Applies transition classes for animations</description></item>
        /// <item><description>Updates the global router state</description></item>
        /// </list>
        /// <para>
        /// If any step fails (guard denies access, lazy loading fails, etc.), navigation is aborted
        /// and the user remains on the current route. If an error occurs, the error handling system
        /// is invoked to display appropriate error UI.
        /// </para>
        /// </remarks>
        private async Task UpdateRoute()
        {
            try
            {
                await UpdateRouteCore();
            }
            catch (Exception ex)
            {
                await HandleRoutingError(ex, RouterErrorType.Unknown, null, null, null);
            }
        }

        /// <summary>
        /// Core routing logic that can throw exceptions to be caught by UpdateRoute.
        /// </summary>
        private async Task UpdateRouteCore()
        {
            Uri uri = new(NavigationManager.Uri);
            string path = uri.AbsolutePath;
            _currentPath = path;

            // Clear any previous error state when navigating to a new route
            _hasError = false;
            _errorInfo = null;

            // Match the route
            RouteMatch? match = RouteMatcher.MatchRoute(path + uri.Query, Routes);

            // Handle redirect
            if (match?.Route.RedirectTo != null)
            {
                NavigationManager.NavigateTo(match.Route.RedirectTo);
                return;
            }

            // Execute middleware - use the deepest child route's middleware if present
            RouteMatch? middlewareMatch = GetDeepestMatch(match);
            if (middlewareMatch != null && middlewareMatch.Route.Middleware.Count != 0)
            {
                try
                {
                    bool canContinue = await ExecuteMiddleware(middlewareMatch);
                    if (!canContinue)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    await HandleRoutingError(ex, RouterErrorType.MiddlewareExecution,
                        _currentPath, middlewareMatch.Route.Path, null);
                    return;
                }
            }

            // Execute guards - use the deepest child route's guards if present
            RouteMatch? guardMatch = GetDeepestMatch(match);
            if (guardMatch != null && guardMatch.Route.Guards.Count != 0)
            {
                try
                {
                    bool canActivate = await ExecuteGuards(guardMatch);
                    if (!canActivate)
                    {
                        return;
                    }
                }
                catch (Exception ex)
                {
                    await HandleRoutingError(ex, RouterErrorType.GuardExecution,
                        _currentPath, guardMatch.Route.Path, null);
                    return;
                }
            }

            // Load component if using lazy loading
            // Check both parent route and child route for ComponentLoader
            RouteMatch? matchToLoad = null;
            if (match != null)
            {
                // Check if parent route needs lazy loading
                if (match.Route.ComponentLoader != null && match.ComponentType == null)
                {
                    matchToLoad = match;
                }
                // Check if child route needs lazy loading
                else if (match.Child != null && match.Child.Route.ComponentLoader != null && match.Child.ComponentType == null)
                {
                    matchToLoad = match.Child;
                }
            }

            if (matchToLoad != null)
            {
                // Skip lazy loading during pre-rendering (first render) to avoid blocking the initial page load
                // The component will be loaded after the first render completes
                if (_isFirstRender)
                {
                    // During pre-rendering, show loading state but don't actually load the component
                    // It will be loaded in OnAfterRenderAsync after interactive rendering starts
                    _isLoading = true;
                    return;
                }

                _isLoading = true;

                // The layout will stay rendered with its current state

                StateHasChanged(); // Force re-render to show loading state

                try
                {
                    await LoadComponentWithCacheAsync(matchToLoad);
                }
                catch (Exception ex)
                {
                    _isLoading = false;
                    await HandleRoutingError(ex, RouterErrorType.ComponentLoading,
                        _currentPath, matchToLoad.Route.Path, null);
                    return;
                }
                finally
                {
                    _isLoading = false;
                }
            }

            _currentMatch = match;

            if (match != null)
            {
                _componentType = match.ComponentType;

                // Determine layout type with proper handling of explicit null
                Type? newLayoutType;
                if (match.Route.HasExplicitLayout)
                {
                    // Route explicitly set Layout (even if null), use it
                    newLayoutType = match.Route.Layout;
                }
                else
                {
                    // Route didn't set Layout, use default
                    newLayoutType = DefaultLayout;
                }

                // Update layout type - @key directive in Router.razor will preserve instance
                _layoutType = newLayoutType;

                // Build parameters for the component - include route data and middleware context data
                _componentParameters = [];

                // Add route data as parameters (only if component has matching properties)
                if (_componentType != null)
                {
                    foreach (KeyValuePair<string, object> data in match.Route.Data)
                    {
                        // Only add parameter if component has a matching [Parameter] property
                        if (ComponentHasParameter(_componentType, data.Key))
                        {
                            _componentParameters[data.Key] = data.Value;
                        }
                    }
                }

                // Add middleware context data as parameters (only if component has matching properties)
                if (_middlewareContextData != null && _componentType != null)
                {
                    foreach (KeyValuePair<string, object> data in _middlewareContextData)
                    {
                        // Only add parameter if component has a matching [Parameter] property
                        if (ComponentHasParameter(_componentType, data.Key))
                        {
                            _componentParameters[data.Key] = data.Value;
                        }
                    }
                }

                // Set transition class
                if (EnableTransitions && match.Route.Transition != RouteTransition.None)
                {
                    string transitionName = match.Route.Transition.ToCssClass();
                    _transitionClass = $"transition-{transitionName}";
                }
            }
            else
            {
                _layoutType = null;
                _componentType = null;
                _transitionClass = "";
            }

            // Update router state
            RouterState.SetCurrentRoute(_currentMatch, _currentPath);
        }

        /// <summary>
        /// Executes all route guards for the given route match sequentially.
        /// </summary>
        /// <param name="match">The route match containing the guards to execute.</param>
        /// <returns>
        /// A task that resolves to true if all guards pass and navigation should proceed,
        /// or false if any guard fails and navigation should be blocked.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Guards are executed in the order they appear in the route configuration. All guards
        /// must pass (return true from CanActivateAsync) for navigation to proceed.
        /// </para>
        /// <para>
        /// If a guard fails, its GetRedirectPathAsync method is called to determine where to
        /// redirect. If a redirect path is provided, navigation occurs to that path instead.
        /// </para>
        /// <para>
        /// Guards are obtained from the dependency injection container first. If not registered,
        /// the router attempts to create an instance using Activator.CreateInstance. If both
        /// fail, the guard is skipped (allowing navigation to proceed).
        /// </para>
        /// </remarks>
        private async Task<bool> ExecuteGuards(RouteMatch match)
        {
            foreach (Type guardType in match.Route.Guards)
            {
                IRouteGuard? guard = ServiceProvider.GetService(guardType) as IRouteGuard;
                if (guard == null)
                {
                    // Try to create instance if not registered
                    try
                    {
                        guard = Activator.CreateInstance(guardType) as IRouteGuard;
                    }
                    catch
                    {
                        continue;
                    }
                }

                if (guard != null)
                {
                    bool canActivate = await guard.CanActivateAsync(match);
                    if (!canActivate)
                    {
                        string? redirectPath = await guard.GetRedirectPathAsync(match);
                        if (redirectPath != null)
                        {
                            NavigationManager.NavigateTo(redirectPath);
                        }
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Stores middleware context data for the current navigation.
        /// </summary>
        private Dictionary<string, object>? _middlewareContextData = null;

        /// <summary>
        /// Executes all route middleware for the given route match in a pipeline.
        /// </summary>
        /// <param name="match">The route match containing the middleware to execute.</param>
        /// <returns>
        /// A task that resolves to true if navigation should proceed,
        /// or false if any middleware aborts navigation.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Middleware are executed in the order they appear in the route configuration.
        /// Each middleware can execute logic before and after navigation by placing code
        /// before and after the next() delegate call.
        /// </para>
        /// <para>
        /// If middleware sets context.Abort to true, the pipeline stops executing and
        /// navigation is cancelled. If context.RedirectPath is also set, navigation
        /// occurs to that path instead.
        /// </para>
        /// <para>
        /// Middleware are obtained from the dependency injection container first. If not registered,
        /// the router attempts to create an instance using Activator.CreateInstance. If both
        /// fail, the middleware is skipped (allowing navigation to proceed).
        /// </para>
        /// </remarks>
        private async Task<bool> ExecuteMiddleware(RouteMatch match)
        {
            RouteMiddlewareContext context = new()
            {
                Match = match,
                Path = _currentPath
            };

            // Build the middleware pipeline
            //Func<Task> pipeline = () => Task.CompletedTask;
            Func<Task> pipeline = async () => await Task.CompletedTask;

            // Build pipeline in reverse order so they execute in forward order
            for (int i = match.Route.Middleware.Count - 1; i >= 0; i--)
            {
                Type middlewareType = match.Route.Middleware[i];
                IRouteMiddleware? middleware = ServiceProvider.GetService(middlewareType) as IRouteMiddleware;

                if (middleware == null)
                {
                    // Try to create instance if not registered
                    try
                    {
                        middleware = Activator.CreateInstance(middlewareType) as IRouteMiddleware;
                    }
                    catch (Exception ex) when (ex is MissingMethodException or ArgumentException or
                                                NotSupportedException or TargetInvocationException or
                                                MethodAccessException or InvalidOperationException)
                    {
                        // Skip this middleware if we can't create it
                        // Common exceptions from Activator.CreateInstance for invalid types
                        continue;
                    }
                }

                if (middleware != null)
                {
                    Func<Task> next = pipeline;
                    pipeline = async () =>
                    {
                        await middleware.InvokeAsync(context, next);
                    };
                }
            }

            // Execute the pipeline
            await pipeline();

            // Store middleware context data for components to access
            _middlewareContextData = context.Data.Count > 0 ? context.Data : null;

            // Check if navigation should be aborted
            if (context.Abort)
            {
                if (context.RedirectPath != null)
                {
                    NavigationManager.NavigateTo(context.RedirectPath);
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the deepest child match in a route match hierarchy.
        /// </summary>
        /// <param name="match">The root route match to search.</param>
        /// <returns>The deepest child match, or the provided match if no children exist.</returns>
        /// <remarks>
        /// For nested routes, this method traverses the Child chain to find the innermost matched route.
        /// This is useful for accessing middleware, guards, and other properties defined on the most
        /// specific matched route rather than the parent.
        /// </remarks>
        private static RouteMatch? GetDeepestMatch(RouteMatch? match)
        {
            if (match == null)
            {
                return null;
            }

            RouteMatch current = match;
            while (current.Child != null)
            {
                current = current.Child;
            }

            return current;
        }

        /// <summary>
        /// Checks if a component type has a property with the [Parameter] attribute matching the given name.
        /// </summary>
        /// <param name="componentType">The component type to check.</param>
        /// <param name="parameterName">The name of the parameter to look for.</param>
        /// <returns>True if the component has a matching parameter property; otherwise, false.</returns>
        /// <remarks>
        /// This method uses reflection to check if the component has a public property with the
        /// specified name that is decorated with the [Parameter] or [CascadingParameter] attribute.
        /// This allows middleware to safely pass data to components without causing errors when
        /// components don't have matching parameters.
        /// </remarks>
        private static bool ComponentHasParameter(Type componentType, string parameterName)
        {
            // Get all public instance properties
            PropertyInfo? property = componentType.GetProperty(
                parameterName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (property == null)
            {
                return false;
            }

            // Check if property has [Parameter] or [CascadingParameter] attribute
            return property.GetCustomAttribute<ParameterAttribute>() != null ||
                   property.GetCustomAttribute<CascadingParameterAttribute>() != null;
        }

        /// <summary>
        /// Creates a dictionary of layout parameters for rendering the current view, including a 'Body' render fragment
        /// representing the appropriate content or loading state.
        /// </summary>
        /// <remarks>The returned dictionary is intended for use with Blazor layouts that accept a 'Body'
        /// parameter. The 'Body' render fragment will display a loading indicator if content is loading, the matched
        /// component if available, or a not found message if no match is present.</remarks>
        /// <returns>A dictionary containing layout parameters, where the 'Body' key maps to a <see cref="RenderFragment"/> that
        /// renders the current content, loading indicator, or not found message as appropriate.</returns>
        private Dictionary<string, object> GetLayoutParameters()
        {
            // Return a new dictionary each time, but with the Body RenderFragment
            // that renders the current content
            return new Dictionary<string, object>
            {
                ["Body"] = (RenderFragment)(builder =>
                {
                    if (_isLoading)
                    {
                        // Show loading inside layout
                        builder.OpenElement(0, "div");
                        builder.AddAttribute(1, "class", "blazouter-view blazouter-loading");
                        if (Loading != null)
                        {
                            builder.AddContent(2, Loading);
                        }
                        else
                        {
                            builder.OpenElement(3, "div");
                            builder.AddAttribute(4, "class", "blazouter-default-loading");
                            builder.OpenElement(5, "div");
                            builder.AddAttribute(6, "class", "blazouter-spinner");
                            builder.CloseElement();
                            builder.OpenElement(7, "p");
                            builder.AddContent(8, "Loading...");
                            builder.CloseElement();
                            builder.CloseElement();
                        }
                        builder.CloseElement();
                    }
                    else if (_currentMatch != null && _componentType != null)
                    {
                        // Show component with transition
                        builder.OpenElement(0, "div");
                        builder.AddAttribute(1, "class", $"blazouter-view {_transitionClass}");
                        builder.OpenComponent(2, _componentType);
                        int attrIndex = 3;
                        foreach (KeyValuePair<string, object> param in _componentParameters)
                        {
                            builder.AddAttribute(attrIndex++, param.Key, param.Value);
                        }
                        builder.CloseComponent();
                        builder.CloseElement();
                    }
                    else if (NotFound != null)
                    {
                        // Show not found
                        builder.OpenElement(0, "div");
                        builder.AddAttribute(1, "class", "blazouter-view");
                        builder.AddContent(2, NotFound);
                        builder.CloseElement();
                    }
                })
            };
        }

        /// <summary>
        /// Handles a routing error by invoking error handlers and displaying error UI.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <param name="errorType">The type of routing error.</param>
        /// <param name="url">The URL being navigated to.</param>
        /// <param name="routePath">The route path being processed.</param>
        /// <param name="componentType">The component type that failed.</param>
        private async Task HandleRoutingError(
            Exception exception,
            RouterErrorType errorType,
            string? url,
            string? routePath,
            Type? componentType)
        {
            RouterErrorContext context = new()
            {
                ErrorType = errorType,
                Url = url ?? _currentPath,
                RoutePath = routePath,
                ComponentType = componentType
            };

            // Try to get error handler from DI
            IRouterErrorHandler? errorHandler = ServiceProvider.GetService(typeof(IRouterErrorHandler)) as IRouterErrorHandler;
            errorHandler ??= new DefaultRouterErrorHandler();

            // Let error handler decide if we should show error UI
            bool shouldShowError = await errorHandler.HandleErrorAsync(exception, context);

            // Invoke OnError event if provided
            if (OnError.HasDelegate)
            {
                RouterErrorEventArgs args = new()
                {
                    Exception = exception,
                    Context = context,
                    Handled = false
                };

                await OnError.InvokeAsync(args);

                // If event handler marked it as handled, don't show error UI
                if (args.Handled)
                {
                    shouldShowError = false;
                }
            }

            if (shouldShowError)
            {
                _hasError = true;
                _errorInfo = new RouterErrorInfo
                {
                    Exception = exception,
                    ErrorType = errorType,
                    Url = url ?? _currentPath,
                    RoutePath = routePath,
                    ComponentType = componentType,
                    Retry = async () =>
                    {
                        _hasError = false;
                        _errorInfo = null;
                        await UpdateRoute();
                        StateHasChanged();
                    }
                };
            }
        }

        /// <summary>
        /// Performs cleanup when the router component is disposed.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method unsubscribes from the NavigationManager's LocationChanged event to prevent
        /// memory leaks and ensure the component is properly garbage collected.
        /// </para>
        /// <para>
        /// This is called automatically by the Blazor framework when the component is removed from
        /// the render tree.
        /// </para>
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Loads a component with caching support for lazy-loaded components.
        /// </summary>
        /// <param name="matchToLoad">The route match containing the component to load.</param>
        /// <returns>A task representing the asynchronous component loading operation.</returns>
        /// <remarks>
        /// <para>
        /// This method implements a caching strategy for lazy-loaded components:
        /// </para>
        /// <list type="number">
        /// <item><description>First checks the component type cache for a previously loaded component</description></item>
        /// <item><description>If found (cache hit), uses the cached type immediately without invoking ComponentLoader</description></item>
        /// <item><description>If not found (cache miss), invokes the ComponentLoader to load the component asynchronously</description></item>
        /// <item><description>After successful loading, stores the component type in cache for future use</description></item>
        /// </list>
        /// <para>
        /// The caching behavior is controlled by the EnableComponentTypeCache option in CacheOptions.
        /// When disabled, the cache always returns null and components are loaded on every navigation.
        /// </para>
        /// </remarks>
        private async Task LoadComponentWithCacheAsync(RouteMatch matchToLoad)
        {
            // Check cache first before loading component
            Type? cachedComponentType = CacheService.GetCachedComponentType(matchToLoad.Route.Path);

            if (cachedComponentType != null)
            {
                // Cache hit - use cached component type (instant!)
                matchToLoad.ComponentType = cachedComponentType;
            }
            else
            {
                // Cache miss - load component using ComponentLoader
                matchToLoad.ComponentType = await matchToLoad.Route.ComponentLoader!();

                // Cache the loaded component type for future use
                if (matchToLoad.ComponentType != null)
                {
                    CacheService.CacheComponentType(matchToLoad.Route.Path, matchToLoad.ComponentType);
                }
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the component and optionally releases the managed resources.
        /// </summary>
        /// <remarks>This method is called by both the public Dispose() method and the finalizer. When
        /// disposing is true, this method should release all managed resources. Override this method to provide custom
        /// disposal logic for derived classes.</remarks>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                NavigationManager.LocationChanged -= OnLocationChanged;
            }
        }
    }
}