using Blazouter.Attributes;
using Blazouter.Interfaces;
using Blazouter.Models;
using Blazouter.Services;
using Microsoft.AspNetCore.Components;
using RouteAttribute = Blazouter.Attributes.RouteAttribute;

namespace Blazouter.WebAssembly.Sample.Pages.AttributeRoutingExamples
{
    /// <summary>
    /// Demonstrates attribute-based cache control for routes.
    /// </summary>
    [RouteTitle("Cache Example")]
    [Route("/attribute-examples/cache")]
    [RouteCache(false)]  // This route will never be cached (for demonstration)
    public partial class CacheExample : ComponentBase
    {
        [Inject]
        private IRouteCacheService CacheService { get; set; } = default!;

        [Inject]
        private RouterNavigationService NavigationService { get; set; } = default!;

        private CacheStatistics? _stats;

        protected override void OnInitialized()
        {
            RefreshStats();
        }

        private void RefreshStats()
        {
            _stats = CacheService.GetStatistics();
            StateHasChanged();
        }
    }
}