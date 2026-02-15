# Blazouter - Core Library

[![NuGet](https://img.shields.io/nuget/v/Blazouter.svg)](https://www.nuget.org/packages/Blazouter/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Blazouter.svg)](https://www.nuget.org/packages/Blazouter/)

A powerful React Router-like routing library for Blazor applications. This is the core library that provides the foundational routing capabilities for all Blazor hosting models.

## Features

- ✅ **Type-safe** - Full IntelliSense support
- ✅ **True nested routing** - Full hierarchical route structures
- ✅ **Beautiful transitions** - Smooth animations between routes
- ✅ **Programmatic navigation** - Navigate imperatively with ease
- ✅ **Lazy loading** - Load components on-demand for better performance
- ✅ **Built-in route guards** - Protect routes with authentication/authorization
- ✅ **Dynamic route parameters** - Easy access to route and query parameters
- ✅ **Flexible layout system** - Default and per-route layouts with @Body support
- ✅ **Query string utilities** - Type-safe query string builder and typed parameter parsing
- ✅ **TypeScript integration** - Optional JavaScript interop for browser History and Document APIs

## Installation

```bash
dotnet add package Blazouter
```

For hosting-specific implementations, also install:
- **Blazor Server**: `Blazouter.Server`
- **Blazor Hybrid/MAUI**: `Blazouter.Hybrid`
- **Blazor WebAssembly**: `Blazouter.WebAssembly`

## Quick Start

### Blazor WebAssembly

```csharp
// Program.cs
using Blazouter.Extensions;

builder.Services.AddBlazouter();
```

```razor
<!-- App.razor -->
@using Blazouter.Models
@using Blazouter.Components

<Router Routes="@_routes" DefaultLayout="typeof(MainLayout)">
    <NotFound>
        <h1>404 - Page Not Found</h1>
    </NotFound>
</Router>

@code {
    private List<RouteConfig> _routes = new()
    {
        new RouteConfig
        {
            Path = "/",
            Component = typeof(Home),
            Transition = RouteTransition.Fade
        }
    };
}
```

### Blazor Server

```csharp
// Program.cs
using Blazouter.Extensions;
using Blazouter.Server.Extensions;

builder.Services.AddBlazouter();

app.MapRazorComponents<App>()
    .AddBlazouterSupport()
    .AddInteractiveServerRenderMode();
```

### Blazor Hybrid (MAUI)

```csharp
// MauiProgram.cs
using Blazouter.Hybrid.Extensions;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>();

        builder.Services.AddMauiBlazorWebView();
        
        // Add Blazouter support
        builder.AddBlazouterSupport();

        return builder.Build();
    }
}
```

```razor
<!-- Main.razor or your root component -->
@using Blazouter.Models
@using Blazouter.Components

<Router Routes="@_routes">
    <NotFound>
        <h1>404 - Page Not Found</h1>
    </NotFound>
</Router>

@code {
    private List<RouteConfig> _routes = new()
    {
        new RouteConfig
        {
            Path = "/",
            Component = typeof(Home),
            Transition = RouteTransition.Fade
        }
    };
}
```

## Key Components

### Router Component
The main router component that handles route matching and rendering.

### RouterOutlet Component
Used in parent components to render child routes in nested routing scenarios.

### RouterLink Component
Navigation links with automatic active state detection.

```razor
<RouterLink Href="/" Exact="true" ActiveClass="active">Home</RouterLink>
<RouterLink Href="/about" ActiveClass="active">About</RouterLink>
```

## Layouts

Set a default layout for all routes and override per route:

```csharp
<Router Routes="@_routes" DefaultLayout="typeof(MainLayout)">
    <NotFound><h1>404</h1></NotFound>
</Router>

@code {
    private List<RouteConfig> _routes = new()
    {
        // Uses DefaultLayout (MainLayout)
        new RouteConfig { Path = "/", Component = typeof(Home) },
        
        // Override with different layout
        new RouteConfig 
        { 
            Path = "/admin", 
            Component = typeof(Admin),
            Layout = typeof(AdminLayout)
        },
        
        // No layout for this route
        new RouteConfig 
        { 
            Path = "/print", 
            Component = typeof(Print),
            Layout = null
        }
    };
}
```

## Route Configuration

```csharp
new RouteConfig
{
    Path = "/users",
    Component = typeof(UserLayout),
    Title = "Users",
    Layout = typeof(MainLayout),  // Optional: override default layout
    Transition = RouteTransition.Slide,
    Guards = new List<Type> { typeof(AuthGuard) },
    Children = new List<RouteConfig>
    {
        new RouteConfig 
        { 
            Path = ":id", 
            Component = typeof(UserDetail) 
        }
    }
}
```

## Route Guards

Create custom guards by implementing `IRouteGuard`:

```csharp
using Blazouter.Models;
using Blazouter.Interfaces;

public class AuthGuard : IRouteGuard
{
    public async Task<bool> CanActivateAsync(RouteMatch match)
    {
        return await IsAuthenticated();
    }

    public Task<string?> GetRedirectPathAsync(RouteMatch match)
    {
        return Task.FromResult<string?>("/login");
    }
}
```

## Lazy Loading

**ComponentLoader** - Load components on-demand:

```csharp
new RouteConfig
{
    Path = "/reports",
    ComponentLoader = async () =>
    {
        await Task.Delay(100); // Simulated delay
        return typeof(ReportsPage);
    }
}
```

**WASM RCL Assembly Lazy Loading** - Load Razor Class Library assemblies on demand:

```razor
@using System.Reflection
@using Blazouter.Models
@using Microsoft.AspNetCore.Components.WebAssembly.Services

@inject LazyAssemblyLoader AssemblyLoader

<Router Routes="@_routes"
        OnNavigateAsync="@OnNavigateAsync"
        AdditionalAssemblies="@_lazyLoadedAssemblies">
</Router>

@code {
    private readonly List<Assembly> _lazyLoadedAssemblies = [];

    private async Task OnNavigateAsync(BlazouterNavigationContext context)
    {
        if (context.Path.StartsWith("/module", StringComparison.OrdinalIgnoreCase))
        {
            if (_lazyLoadedAssemblies.Count == 0)
            {
                var assemblies = await AssemblyLoader.LoadAssembliesAsync(
                    ["MyModule.wasm"]);
                _lazyLoadedAssemblies.AddRange(assemblies);
            }
        }
    }
}
```

## Programmatic Navigation

```csharp
@inject RouterNavigationService NavService

private void NavigateToUser()
{
    NavService.NavigateTo("/users/123");
}
```

## Route Parameters

```csharp
@inject RouterStateService RouterState

protected override void OnInitialized()
{
    var userId = RouterState.GetParam("id");
}
```

## Query String Utilities

Type-safe query string manipulation with fluent API:

```csharp
@using Blazouter.Utilities
@using Blazouter.Extensions
@inject RouterStateService RouterState
@inject RouterNavigationService NavService

// Typed query parameter parsing
protected override void OnInitialized()
{
    int page = RouterState.GetQueryInt("page", 1);
    bool active = RouterState.GetQueryBool("active", false);
    DateTime? date = RouterState.GetQueryDateTimeOrNull("date");
}

// Fluent query string building
private void SearchWithFilters()
{
    NavService.NavigateToWithQuery("/search", q => q
        .Add("term", "blazor")
        .Add("active", true)
        .Add("page", 2));
}

// Update query parameters
private void NextPage()
{
    NavService.NavigateToWithUpdatedQuery(RouterState, null, q => q
        .Set("page", currentPage + 1));
}
```

**Supported types**: string, int, long, decimal, double, bool, DateTime, Guid, enum, and nullable variants (15 type-safe methods each for `Add()` and `Set()`).

## TypeScript Integration (Optional)

Enhanced browser integration with type-safe JavaScript interop using 5 specialized services:

```csharp
// Enable JavaScript interop (optional)
builder.Services.AddBlazouterInterop();
```

Add to your `index.html`:
```html
<script type="module" src="_content/Blazouter/js/index.js"></script>
```

**Available Services:**

- **ClipboardInterop**: Clipboard operations (copy, read, permissions)
- **StorageInterop**: localStorage & sessionStorage with JSON serialization
- **NavigationInterop**: Browser History API (back/forward, URL info, hash, query params)
- **ViewportInterop**: Viewport/device info (dimensions, device type, orientation, fullscreen)
- **DocumentInterop**: Document manipulation (title, meta tags, scrolling, focus, CSS classes)

```csharp
@using Blazouter.Interops

@inject StorageInterop Storage
@inject DocumentInterop Document
@inject ViewportInterop Viewport
@inject ClipboardInterop Clipboard
@inject NavigationInterop Navigation

// Navigation
await Navigation.GoBackAsync();
string url = await Navigation.GetCurrentUrlAsync();

// Document
await Document.SetTitleAsync("Home - My App");
await Document.ScrollToTopAsync();

// Storage
await Storage.SetLocalStorageAsync("key", myObject);
var data = await Storage.GetLocalStorageAsync<MyType>("key");

// Viewport
string device = await Viewport.GetDeviceTypeAsync();
bool isMobile = await Viewport.IsMobileAsync();

// Clipboard
await Clipboard.CopyTextAsync("text");
```

## Transitions

Built-in transitions: `None`, `Pop`, `Blur`, `Fade`, `Flip`, `Lift`, `Scale`, `Slide`, `Swipe`, `Reveal`, `Rotate`, `Curtain`, `SlideUp`, `SlideFade`, `Spotlight`

```csharp
new RouteConfig
{
    Path = "/",
    Component = typeof(Home),
    Transition = RouteTransition.Fade
}
```

## Multi-Platform Support

- Blazor Server
- Blazor WebAssembly
- Blazor Hybrid (MAUI)
- .NET 6.0, 7.0, 8.0, 9.0, 10.0

## Documentation

- [Features](https://github.com/Taiizor/Blazouter/blob/develop/FEATURES.md)
- [Sample Application](https://github.com/Taiizor/Blazouter/tree/develop/samples)
- [Full Documentation](https://github.com/Taiizor/Blazouter)

## License

MIT License - see [LICENSE](https://github.com/Taiizor/Blazouter/blob/develop/LICENSE) for details.

## Support

- [GitHub Issues](https://github.com/Taiizor/Blazouter/issues)
- [GitHub Discussions](https://github.com/Taiizor/Blazouter/discussions)

Made with ❤️ for the Blazor community