using Blazouter.Enums;
using Blazouter.Extensions;
using Blazouter.Models;
using Blazouter.Services;
using Blazouter.WebAssembly.Sample.Guards;
using Blazouter.WebAssembly.Sample.Middlewares;
using Blazouter.WebAssembly.Sample.Pages;
using Blazouter.WebAssembly.Sample.Pages.Users;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Services;
using System.Reflection;

namespace Blazouter.WebAssembly.Sample
{
    public partial class App
    {
        [Inject] private LazyAssemblyLoader _assemblyLoader { get; set; } = default!;
        [Inject] private RouterNavigationService _navService { get; set; } = default!;

        private readonly List<Assembly> _lazyLoadedAssemblies = [];

        /// <summary>
        /// Ensures the lazy module assembly is loaded exactly once, shared between
        /// OnNavigateAsync (for /support, /help) and ComponentLoader (for /faq).
        /// </summary>
        private async Task EnsureLazyModuleLoadedAsync()
        {
            if (_lazyLoadedAssemblies.Count == 0)
            {
                IEnumerable<Assembly> assemblies = await _assemblyLoader.LoadAssembliesAsync(["Blazouter.LazyModule.Sample.wasm"]);
                _lazyLoadedAssemblies.AddRange(assemblies);
            }
        }

        private async Task OnNavigateAsync(BlazouterNavigationContext context)
        {
            // Load the lazy module assembly when navigating to /support or /help
            if (context.Path.StartsWith("/support", StringComparison.OrdinalIgnoreCase) || context.Path.StartsWith("/help", StringComparison.OrdinalIgnoreCase))
            {
                await EnsureLazyModuleLoadedAsync();
            }
        }

        private List<RouteConfig> _routes = [];

        protected override void OnInitialized()
        {
            _routes = new List<RouteConfig>
            {
                new() {
                    Path = "/",
                    Component = typeof(Home),
                    Title = "Home",
                    Transition = RouteTransition.Blur
                },
                new() {
                    Path = "/about",
                    Component = typeof(About),
                    Title = "About",
                    Transition = RouteTransition.Fade
                },
                new() {
                    Path = "/navigation",
                    Component = typeof(Navigation),
                    Title = "Navigation Demo",
                    Transition = RouteTransition.Flip
                },
                new() {
                    Path = "/transitions",
                    Component = typeof(Transitions),
                    Title = "Transitions Demo",
                    Transition = RouteTransition.Lift
                },
                new() {
                    Path = "/users",
                    Component = typeof(UserLayout),
                    Title = "Users",
                    Transition = RouteTransition.Swipe,
                    Children =
                    [
                        new RouteConfig
                        {
                            Path = "",
                            Component = typeof(UserList),
                            Title = "User List",
                            Exact = true
                        },
                        new RouteConfig
                        {
                            Path = ":id",
                            Component = typeof(UserDetail),
                            Title = "User Details",
                            Middleware = [typeof(DataPreloadMiddleware)]
                        }
                    ]
                },
                new() {
                    Path = "/protected",
                    Component = typeof(Protected),
                    Title = "Protected Page",
                    Guards = [typeof(AuthenticationGuard)],
                    Transition = RouteTransition.Rotate
                },
                new() {
                    Path = "/cache",
                    Component = typeof(Cache),
                    Title = "Cache Page",
                    Transition = RouteTransition.Fade,
                    // Cache page itself uses caching (default behavior)
                    EnableCache = null  // null means use global settings
                },
                new() {
                    Path = "/lazy",
                    ComponentLoader = async () =>
                    {
                        await Task.Delay(1000); // Simulate loading
                        return typeof(LazyPage);
                    },
                    Title = "Lazy Loaded Page",
                    Transition = RouteTransition.Curtain
                },
                new() {
                    Path = "/typescript",
                    Component = typeof(TypeScript),
                    Title = "TypeScript Integration",
                    Transition = RouteTransition.Reveal
                },
                new() {
                    Path = "/middleware",
                    Component = typeof(RouteMiddleware),
                    Title = "Middleware Example",
                    Middleware = [
                        typeof(TimingMiddleware),
                        typeof(LoggingMiddleware),
                        typeof(AnalyticsMiddleware)
                    ],
                    Transition = RouteTransition.Slide,
                    // Disable caching for this route to always execute middleware fresh
                    EnableCache = false
                },
                new() {
                    Path = "/faq",
                    ComponentLoader = async () =>
                    {
                        // Load the lazy module assembly using ComponentLoader approach
                        // (alternative to OnNavigateAsync + AdditionalAssemblies)
                        // Uses shared EnsureLazyModuleLoadedAsync to avoid duplicate loads —
                        // LoadAssembliesAsync returns empty if the assembly is already loaded,
                        // and calling it twice causes "body stream already read" errors.
                        await EnsureLazyModuleLoadedAsync();

                        // Note: typeof() cannot be used here because the assembly isn't loaded yet at JIT time.
                        // Instead, use reflection to resolve the type after loading the assembly.
                        return _lazyLoadedAssemblies
                            .First(a => a.GetName().Name == "Blazouter.LazyModule.Sample")
                            .GetType("Blazouter.LazyModule.Sample.Pages.FaqPage")!;
                    },
                    Title = "FAQ (Lazy ComponentLoader)",
                    Transition = RouteTransition.Pop
                },
                new() {
                    Path = "/error-example",
                    Component = typeof(ErrorExample),
                    Title = "Error Example",
                    Transition = RouteTransition.Spotlight,
                    // Disable caching for error example to test errors each time
                    EnableCache = false
                },
                new() {
                    Path = "/test-error",
                    ComponentLoader = async () =>
                    {
                        if (new Random().Next(2) == 1)
                        {
                            try
                            {
                                // Simulated failure
                                throw new InvalidOperationException("Component failed to load due to an unexpected condition.");
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(
                                    message: $"Blazouter failed to load component for route '{"/test-error"}'. " +
                                             $"Reason: {ex.Message}",
                                    innerException: ex
                                );
                            }
                        }
                        else{
                            return typeof(ErrorExample);
                        }
                    },
                    Title = "Test Error"
                }
            }.AddAttributeRoutes(typeof(App).Assembly);
        }
    }
}