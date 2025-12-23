using Blazouter.Extensions;
using Blazouter.WebAssembly.Sample.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Blazouter.WebAssembly.Sample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // Add Blazouter services with cache configuration
            builder.Services.AddBlazouter(options =>
            {
                // Enable route match caching for improved performance
                options.EnableRouteMatchCache = true;

                // Enable component type caching to speed up lazy loading (instant after first load!)
                options.EnableComponentTypeCache = true;

                // Enable statistics tracking to see cache performance
                options.EnableStatistics = true;

                // Configure cache sizes (optional - defaults are 100/50)
                options.MaxRouteMatchCacheSize = 100;
                options.MaxComponentTypeCacheSize = 50;
            });

            // Add TypeScript-based JavaScript interop services
            builder.Services.AddBlazouterInterop();

            // Register sample services
            builder.Services.AddSingleton<AuthenticationService>();

            // Register custom error handler for routing errors
            builder.Services.AddBlazouterErrorHandler<CustomRouterErrorHandler>();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            await builder.Build().RunAsync();
        }
    }
}