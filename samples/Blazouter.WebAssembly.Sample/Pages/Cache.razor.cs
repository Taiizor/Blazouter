using Blazouter.Extensions;
using Blazouter.Interfaces;
using Blazouter.Models;
using Blazouter.Services;
using Microsoft.AspNetCore.Components;

namespace Blazouter.WebAssembly.Sample.Pages
{
    /// <summary>
    /// Demonstrates advanced caching features in Blazouter.
    /// </summary>
    public partial class Cache
    {
        [Inject] private IRouteCacheService CacheService { get; set; } = default!;
        [Inject] private RouterNavigationService NavService { get; set; } = default!;

        private CacheStatistics? _stats;
        private string _formattedStats = string.Empty;

        protected override void OnInitialized()
        {
            RefreshStats();
        }

        /// <summary>
        /// Refreshes the cache statistics display.
        /// </summary>
        private void RefreshStats()
        {
            _stats = CacheService.GetStatistics();
            _formattedStats = CacheService.GetFormattedStatistics();
            StateHasChanged();
        }

        /// <summary>
        /// Clears all cache entries and resets statistics.
        /// </summary>
        private void ClearCache()
        {
            CacheService.Clear();
            RefreshStats();
        }
    }
}