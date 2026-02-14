namespace Blazouter.Models
{
    /// <summary>
    /// Provides context information for the <see cref="Components.Router.OnNavigateAsync"/> callback,
    /// enabling lazy assembly loading and dynamic route configuration during navigation.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This context is passed to the OnNavigateAsync callback before route matching occurs, allowing
    /// applications to load assemblies on demand (e.g., via LazyAssemblyLoader in Blazor WebAssembly)
    /// and update the router's AdditionalAssemblies parameter before the route is resolved.
    /// </para>
    /// <para>
    /// The CancellationToken is automatically cancelled when a new navigation occurs before the current
    /// OnNavigateAsync callback completes, preventing stale navigations from completing.
    /// </para>
    /// </remarks>
    /// <example>
    /// Lazy-load assemblies based on the navigation path:
    /// <code>
    /// private async Task OnNavigateAsync(BlazouterNavigationContext context)
    /// {
    ///     if (context.Path.StartsWith("/admin", StringComparison.OrdinalIgnoreCase))
    ///     {
    ///         var assemblies = await AssemblyLoader.LoadAssembliesAsync(
    ///             new[] { "MyApp.Admin.wasm" });
    ///         _lazyLoadedAssemblies.AddRange(assemblies);
    ///     }
    /// }
    /// </code>
    /// </example>
    public class BlazouterNavigationContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlazouterNavigationContext"/> class.
        /// </summary>
        /// <param name="path">The URL path being navigated to.</param>
        /// <param name="cancellationToken">A token that is cancelled when a new navigation supersedes this one.</param>
        public BlazouterNavigationContext(string path, CancellationToken cancellationToken)
        {
            Path = path;
            CancellationToken = cancellationToken;
        }

        /// <summary>
        /// Gets the URL path being navigated to.
        /// </summary>
        /// <value>
        /// The absolute path portion of the URL (e.g., "/admin/users").
        /// </value>
        public string Path { get; }

        /// <summary>
        /// Gets a cancellation token that is triggered when a subsequent navigation occurs
        /// before this navigation's OnNavigateAsync callback completes.
        /// </summary>
        /// <value>
        /// A <see cref="CancellationToken"/> that signals cancellation when the navigation is superseded.
        /// </value>
        /// <remarks>
        /// Use this token to cancel long-running operations (such as assembly loading) when the user
        /// navigates away before the operation completes.
        /// </remarks>
        public CancellationToken CancellationToken { get; }
    }
}