# Blazouter.WebAssembly

[![NuGet](https://img.shields.io/nuget/v/Blazouter.WebAssembly.svg)](https://www.nuget.org/packages/Blazouter.WebAssembly/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Blazouter.WebAssembly.svg)](https://www.nuget.org/packages/Blazouter.WebAssembly/)

WebAssembly-specific extensions for Blazouter - the React Router-like routing library for Blazor applications. This package provides optimized components and extensions for Blazor WebAssembly applications.

## Features

- ✅ All core Blazouter features
- ✅ Browser-specific enhancements
- ✅ Blazor WebAssembly integration
- ✅ Client-side routing optimizations
- ✅ Optimized bundle size for WASM

## Installation

```bash
dotnet add package Blazouter
dotnet add package Blazouter.WebAssembly
```

**Note**: This package requires the core `Blazouter` package.

## Quick Start

### 1. Configure Services

```csharp
// Program.cs
using Blazouter.Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazouter(); // Add Blazouter services

await builder.Build().RunAsync();
```

### 2. Define Routes in App.razor

```razor
@using Blazouter.Models
@using Blazouter.Components

<Router Routes="@_routes" DefaultLayout="typeof(Layout.MainLayout)">
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
            Component = typeof(Pages.Home),
            Title = "Home",
            Transition = RouteTransition.Fade
        },
        new RouteConfig
        {
            Path = "/about",
            Component = typeof(Pages.About),
            Title = "About",
            Transition = RouteTransition.Slide
        }
    };
}
```

### 3. Include CSS

Add to your `index.html`:

```html
<link rel="stylesheet" href="_content/Blazouter/blazouter[.min].css" />
```

## WebAssembly-Specific Features

### Optimized for Client-Side
The WebAssembly package is specifically optimized for:
- Reduced bundle size
- Browser-based navigation
- Client-side route matching
- Local storage integration possibilities

### Router Component
The standard `Router` component works seamlessly in WebAssembly mode with:
- SPA-style navigation
- Browser history integration
- Fast client-side route resolution

## Layouts

Configure a default layout for all routes and override per route:

```csharp
<Router Routes="@_routes" DefaultLayout="typeof(Layout.MainLayout)">
    <NotFound><h1>404</h1></NotFound>
</Router>

@code {
    private List<RouteConfig> _routes = new()
    {
        // Uses DefaultLayout (MainLayout)
        new RouteConfig 
        { 
            Path = "/", 
            Component = typeof(Pages.Home) 
        },
        
        // Override with different layout
        new RouteConfig 
        { 
            Path = "/account", 
            Component = typeof(Pages.Account),
            Layout = typeof(Layout.AccountLayout)
        },
        
        // No layout for fullscreen pages
        new RouteConfig 
        { 
            Path = "/presentation", 
            Component = typeof(Pages.Presentation),
            Layout = null
        }
    };
}
```

## Lazy Loading for Better Performance

In WebAssembly, lazy loading is crucial for reducing initial load time.

### ComponentLoader

Load individual components on-demand:

```csharp
new RouteConfig
{
    Path = "/reports",
    ComponentLoader = async () =>
    {
        // Simulate dynamic loading
        await Task.Delay(100);
        return typeof(ReportsPage);
    },
    Title = "Reports"
}
```

### WASM RCL Assembly Lazy Loading

Load entire Razor Class Library (RCL) assemblies on demand using `OnNavigateAsync` and `AdditionalAssemblies`. This is ideal for large applications where separate modules are packaged as RCL projects:

```razor
@using System.Reflection
@using Blazouter.Models
@using Blazouter.Components
@using Microsoft.AspNetCore.Components.WebAssembly.Services

@inject LazyAssemblyLoader AssemblyLoader

<Router Routes="@_routes"
        DefaultLayout="typeof(MainLayout)"
        OnNavigateAsync="@OnNavigateAsync"
        AdditionalAssemblies="@_lazyLoadedAssemblies">
    <Loading>
        <p>Loading module...</p>
    </Loading>
    <NotFound>
        <h1>404 - Page Not Found</h1>
    </NotFound>
</Router>

@code {
    private readonly List<Assembly> _lazyLoadedAssemblies = [];

    private async Task OnNavigateAsync(BlazouterNavigationContext context)
    {
        if (context.Path.StartsWith("/support", StringComparison.OrdinalIgnoreCase) ||
            context.Path.StartsWith("/help", StringComparison.OrdinalIgnoreCase))
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

Configure the assemblies to lazy-load in your `.csproj`:

```xml
<ItemGroup>
    <BlazorWebAssemblyLazyLoad Include="MyModule.wasm" />
</ItemGroup>
```

**Key parameters:**
- `OnNavigateAsync`: Callback invoked before each navigation. Use it to load assemblies based on the target path.
- `AdditionalAssemblies`: List of dynamically loaded assemblies whose `[Route]` attributes are scanned for route discovery.
- `BlazouterNavigationContext`: Provides `Path` (target URL) and `CancellationToken` for the callback.

## Nested Routes

Use `<RouterOutlet />` for nested routing within components:

```csharp
new RouteConfig
{
    Path = "/products",
    Component = typeof(ProductLayout),
    Children = new List<RouteConfig>
    {
        new RouteConfig 
        { 
            Path = "", 
            Component = typeof(ProductList),
            Exact = true 
        },
        new RouteConfig 
        { 
            Path = ":id", 
            Component = typeof(ProductDetail) 
        }
    }
}
```

```razor
<!-- ProductLayout.razor -->
@using Blazouter.Components

<div class="product-layout">
    <h1>Products</h1>
    <RouterOutlet />
</div>
```

**Note**: `<RouterOutlet />` is for nested routing within a component hierarchy, while `Layout` (via `DefaultLayout` or `RouteConfig.Layout`) wraps entire routes with a common layout structure like headers, footers, and navigation.

## Navigation Links

```razor
@using Blazouter.Components

<nav class="menu">
    <RouterLink Href="/" Exact="true" ActiveClass="active">
        Home
    </RouterLink>
    <RouterLink Href="/products" ActiveClass="active">
        Products
    </RouterLink>
    <RouterLink Href="/about" ActiveClass="active">
        About
    </RouterLink>
</nav>
```

## Route Guards

Protect routes with authentication logic:

```csharp
new RouteConfig
{
    Path = "/admin",
    Component = typeof(AdminDashboard),
    Guards = new List<Type> { typeof(AuthGuard) }
}

public class AuthGuard : IRouteGuard
{
    private readonly ILocalStorageService _localStorage;
    
    public AuthGuard(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<bool> CanActivateAsync(RouteMatch match)
    {
        var token = await _localStorage.GetItemAsync<string>("auth_token");
        return !string.IsNullOrEmpty(token);
    }

    public Task<string?> GetRedirectPathAsync(RouteMatch match)
    {
        return Task.FromResult<string?>("/login");
    }
}
```

## Programmatic Navigation

```csharp
@inject RouterNavigationService NavService

private void NavigateToProduct(int productId)
{
    NavService.NavigateTo($"/products/{productId}");
}

private void GoBack()
{
    NavService.NavigateTo(-1); // Browser back
}
```

## Access Route Parameters

```csharp
@inject RouterStateService RouterState

@code {
    private string? _productId;

    protected override void OnInitialized()
    {
        _productId = RouterState.GetParam("id");
        var category = RouterState.GetQueryParam("category");
    }
}
```

## Route Transitions

Built-in CSS transitions optimized for smooth animations:

```csharp
new RouteConfig
{
    Path = "/",
    Component = typeof(Home),
    Transition = RouteTransition.Fade  // Smooth fade transition
}

new RouteConfig
{
    Path = "/products",
    Component = typeof(Products),
    Transition = RouteTransition.Slide  // Slide from left
}
```

Available transitions:
- `Fade` - Fade in animation
- `Scale` - Scale in animation
- `Flip` - 3D card flip animation
- `Slide` - Slide from left animation
- `Pop` - Bounce effect with elastic easing
- `None` - No transition animation (instant)
- `SlideUp` - Slide from bottom animation
- `Rotate` - Spinning entrance along Z-axis
- `Reveal` - Mask opening from bottom to top
- `SlideFade` - Combined slide and fade effect
- `Blur` - Focus transition from blurred to sharp
- `Swipe` - Mobile-style swipe reveal from right to left
- `Curtain` - Theatrical curtain opening from top to bottom
- `Spotlight` - Dramatic lighting effect with brightness and blur
- `Lift` - Content lifts up with subtle scaling and shadow (iOS-style)

## Performance Tips for WebAssembly

1. **Minimize route configuration** in the initial load
2. **Use lazy loading** extensively to reduce initial bundle size
3. **Use transitions** sparingly on mobile for better performance
4. **Consider route-based code splitting** for large applications
5. **Leverage route guards** to prevent unnecessary component initialization

## Target Frameworks

- .NET 6.0
- .NET 7.0
- .NET 8.0
- .NET 9.0
- .NET 10.0

## Browser Support

Blazouter.WebAssembly supports all modern browsers that support WebAssembly:
- Safari (latest)
- Firefox (latest)
- Chrome/Edge (latest)

## Example Application

See the [sample application](https://github.com/Taiizor/Blazouter/tree/develop/samples) for a complete working example of Blazor WebAssembly with Blazouter.

## Documentation

- [Core Features](https://github.com/Taiizor/Blazouter/blob/develop/FEATURES.md)
- [API Reference](https://github.com/Taiizor/Blazouter)
- [Main Documentation](https://github.com/Taiizor/Blazouter)

## Migration Guide

Moving from standard Blazor WebAssembly routing to Blazouter:

### Before (Standard Blazor)
```razor
@page "/products"
@page "/products/{Id:int}"

<h1>Product @Id</h1>
```

### After (Blazouter)
```csharp
// Route configuration
new RouteConfig
{
    Path = "/products/:id",
    Component = typeof(ProductDetail)
}
```

```razor
<!-- ProductDetail.razor -->
@inject RouterStateService RouterState

<h1>Product @_productId</h1>

@code {
    private string? _productId;
    
    protected override void OnInitialized()
    {
        _productId = RouterState.GetParam("id");
    }
}
```

## License

MIT License - see [LICENSE](https://github.com/Taiizor/Blazouter/blob/develop/LICENSE) for details.

## Support

- [GitHub Issues](https://github.com/Taiizor/Blazouter/issues)
- [GitHub Discussions](https://github.com/Taiizor/Blazouter/discussions)

Made with ❤️ for the Blazor community