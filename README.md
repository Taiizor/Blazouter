# Blazouter 🚀

[![NuGet Core](https://img.shields.io/nuget/v/Blazouter.svg?label=Blazouter)](https://www.nuget.org/packages/Blazouter/)
[![NuGet Hybrid](https://img.shields.io/nuget/v/Blazouter.Hybrid.svg?label=Blazouter.Hybrid)](https://www.nuget.org/packages/Blazouter.Hybrid/)
[![NuGet Server](https://img.shields.io/nuget/v/Blazouter.Server.svg?label=Blazouter.Server)](https://www.nuget.org/packages/Blazouter.Server/)
[![NuGet WebAssembly](https://img.shields.io/nuget/v/Blazouter.WebAssembly.svg?label=Blazouter.WebAssembly)](https://www.nuget.org/packages/Blazouter.WebAssembly/)
[![CI Build](https://github.com/Taiizor/Blazouter/workflows/CI%20Build/badge.svg)](https://github.com/Taiizor/Blazouter/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/Taiizor/Blazouter/blob/develop/LICENSE)

A powerful React Router-like routing library for Blazor applications. Blazouter brings the best features of React Router to the Blazor ecosystem with dedicated packages for each hosting model.

## 🌟 Why Blazouter?

Blazor's built-in routing is functional but lacks many modern features that developers expect from frameworks like React Router. Blazouter fills this gap by providing:

- ✅ **Type-safe** - Full IntelliSense support
- ✅ **Lazy loading** - Load components on-demand
- ✅ **True nested routing** - Not just @page directives
- ✅ **Built-in route guards** - Protect your routes easily
- ✅ **Beautiful transitions** - Smooth animations between routes
- ✅ **Programmatic navigation** - Navigate imperatively with ease

## ✨ Features

Blazouter addresses the limitations of traditional Blazor routing:

| Feature | React Router | Blazor Router | **Blazouter** |
|---------|--------------|---------------|---------------|
| Active Links | ✔ Built-in NavLink | ⚠️ Manual active class | ✅ Automatic with RouterLink |
| Lazy Loading | ✔ Route-based code splitting | ❌ Still limited in WASM | ✅ Full support with ComponentLoader |
| Route Guards | ✔ Easy with wrappers/hooks | ❌ Manual, component-based | ✅ Built-in IRouteGuard interface |
| Layout System | ✔ Component composition | ⚠️ Static @layout | ✅ Dynamic per-route with priority |
| Nested Routes | ✔ Easy to define child routes | ❌ Limited, single level with `@page` | ✅ Unlimited nesting with RouterOutlet |
| Error Handling | ✔ Error boundaries | ❌ Manual | ✅ Built-in IRouterErrorHandler |
| Attribute Routes | ✔ JSX-based | ❌ @page only | ✅ 9 attribute types with full config |
| Dynamic Params | ✔ Easy route parameters | ✔ Available but basic | ✅ Enhanced with RouterStateService |
| Route Transitions | ✔ Very easy | ❌ No native support | ✅ 14 built-in transition types |
| Route Middleware | ✔ Route-level middleware | ❌ No native support | ✅ Built-in IRouteMiddleware interface |
| Query String Helpers | ✔ URLSearchParams API | ❌ Manual parsing | ✅ Type-safe fluent API with 30+ methods |
| Conditional Rendering | ✔ Direct with `<Route>` | ❌ Manual via state | ✅ Component-based rendering |
| Programmatic Navigation | ✔ `navigate("/path")` | ✔ `NavigationManager.NavigateTo` | ✅ Enhanced RouterNavigationService |

### Key Features

- **📊 Route Parameters**: Easy access to route and query parameters
- **🎯 Nested Routes**: Define complex hierarchical route structures easily
- **⚡ Lazy Loading**: Load components on-demand for better performance
- **🎭 Dynamic Components**: Load components dynamically based on routes
- **🔒 Route Guards**: Protect routes with authentication and authorization logic
- **🏷️ Attribute-Based Routing**: Declarative route configuration using attributes
- **📐 Layout System**: Flexible layout management with default and per-route layouts
- **🔗 Programmatic Navigation**: Navigate imperatively with enhanced navigation service
- **🔧 Route Middleware**: Execute code before/after navigation for logging, analytics, data preloading
- **⚠️ Error Handling**: Comprehensive error handling with custom error handlers and retry mechanisms
- **🔧 Query String Utilities**: Type-safe query string builder and typed parameter parsing with fluent API
- **🎨 Route Transitions**: Beautiful animations when navigating between routes with 14 built-in transition types

## 📦 Available Packages

Blazouter provides specialized packages for each Blazor hosting model:

| Package | Description | Target Frameworks |
|---------|-------------|-------------------|
| **[Blazouter](https://www.nuget.org/packages/Blazouter/)** | Core routing library | net6.0, net7.0, net8.0, net9.0, net10.0 |
| **[Blazouter.Server](https://www.nuget.org/packages/Blazouter.Server/)** | Blazor Server extensions | net8.0, net9.0, net10.0 |
| **[Blazouter.Hybrid](https://www.nuget.org/packages/Blazouter.Hybrid/)** | Blazor Hybrid/MAUI extensions | net9.0, net10.0 (iOS, Android, macOS, Windows) |
| **[Blazouter.WebAssembly](https://www.nuget.org/packages/Blazouter.WebAssembly/)** | Blazor WebAssembly extensions | net6.0, net7.0, net8.0, net9.0, net10.0 |

> **Note:** The `Blazouter.Web` package has been deprecated. For Blazor Web Applications, use `Blazouter.Server` for the server project and `Blazouter.WebAssembly` for the client project.

## 🚀 Quick Start

### Installation

Choose the package(s) based on your hosting model:

**For Blazor Server:**
```bash
dotnet add package Blazouter
dotnet add package Blazouter.Server
```

**For Blazor Hybrid/MAUI:**
```bash
dotnet add package Blazouter
dotnet add package Blazouter.Hybrid
```

**For Blazor WebAssembly:**
```bash
dotnet add package Blazouter
dotnet add package Blazouter.WebAssembly
```

**For Blazor Web Application (.NET 8+):**

*Server project:*
```bash
dotnet add package Blazouter
dotnet add package Blazouter.Server
```

*Client project:*
```bash
dotnet add package Blazouter
dotnet add package Blazouter.WebAssembly
```

### Setup

**1. Register Blazouter services**

```csharp
// In Program.cs (WebAssembly & Server) or MauiProgram.cs (Hybrid)
using Blazouter.Extensions;

builder.Services.AddBlazouter();
```

**2. Platform-specific configuration**

<details>
<summary><b>Blazor Server</b></summary>

Add Blazouter support to routing in `Program.cs`:

```csharp
using Blazouter.Server.Extensions;

app.MapRazorComponents<App>()
    .AddBlazouterSupport()  // Required for Server mode
    .AddInteractiveServerRenderMode();
```

Create a `Routes.razor` component:

```razor
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
        new RouteConfig { Path = "/", Component = typeof(Pages.Home) },
        // Add more routes...
    };
}
```

Use in `App.razor`:

```razor
<Routes @rendermode="InteractiveServer" />
```

**Important**: The `@rendermode="InteractiveServer"` attribute is required to enable SignalR connection and interactivity in Blazor Server applications (.NET 8+).

</details>

<details>
<summary><b>Blazor WebAssembly</b></summary>

Use the `Router` component in `App.razor`:

```razor
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
        new RouteConfig { Path = "/", Component = typeof(Pages.Home) },
        // Add more routes...
    };
}
```

</details>

<details>
<summary><b>Blazor Hybrid (MAUI)</b></summary>

Register in `MauiProgram.cs`:

```csharp
using Blazouter.Hybrid.Extensions;

builder.AddBlazouterSupport();  // Instead of builder.Services.AddBlazouter()
```

Use the `Router` component in your root Blazor component:

```razor
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
        new RouteConfig { Path = "/", Component = typeof(Pages.Home) },
        // Add more routes...
    };
}
```

</details>

<details>
<summary><b>Blazor Web Application (.NET 8+)</b></summary>

**Server project** - Add Blazouter support in `Program.cs`:

```csharp
using Blazouter.Server.Extensions;

app.MapRazorComponents<App>()
    .AddBlazouterSupport()  // Required for Blazor Server/Web
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();
```

**Server project** - Create a `Routes.razor` component:

```razor
@using Blazouter.Models
@using Blazouter.Components
@using ServerPages = YourApp.Server.Components.Pages
@using ClientPages = YourApp.Client.Components.Pages

<Router Routes="@_routes">
    <NotFound>
        <h1>404 - Page Not Found</h1>
    </NotFound>
</Router>

@code {
    private List<RouteConfig> _routes = new()
    {
        // Server-side pages
        new RouteConfig { Path = "/", Component = typeof(ServerPages.Home) },
        // Client-side pages  
        new RouteConfig { Path = "/about", Component = typeof(ClientPages.About) },
        // Add more routes...
    };
}
```

**Server project** - Use in `App.razor` with InteractiveServer render mode:

```razor
<!-- Use InteractiveServer to keep Routes on server -->
<Routes @rendermode="InteractiveServer" />

<!-- Or use InteractiveWebAssembly for client-only -->
<Routes @rendermode="InteractiveWebAssembly" />
```

**InteractiveAuto** provides the best experience by starting with fast server rendering, then automatically switching to WebAssembly once it's downloaded.

</details>

**3. Include the CSS**

```html
<link rel="stylesheet" href="_content/Blazouter/blazouter[.min].css" />
```

**4. Define your routes with optional layout**

```csharp
// Using a default layout for all routes
<Router Routes="@_routes" DefaultLayout="typeof(MainLayout)">
    <NotFound><h1>404</h1></NotFound>
</Router>

@code {
    private List<RouteConfig> _routes = new()
    {
        new RouteConfig
        {
            Path = "/",
            Component = typeof(Pages.Home),
            Transition = RouteTransition.Fade
        },
        new RouteConfig
        {
            Path = "/users",
            Component = typeof(Pages.UserLayout),
            Children = new List<RouteConfig>
            {
                new RouteConfig { Path = ":id", Component = typeof(Pages.UserDetail) }
            }
        },
        new RouteConfig
        {
            Path = "/admin",
            Component = typeof(Pages.Admin),
            Layout = typeof(AdminLayout) // Override default layout
        },
        new RouteConfig
        {
            Path = "/print",
            Component = typeof(Pages.Print),
            Layout = null // No layout for this route
        }
    };
}
```

## 📖 Usage Examples

### Attribute-Based Routing

Define routes declaratively using attributes directly on your components:

```csharp
using Blazouter.Enums;
using Blazouter.Models;
using Blazouter.Attributes;
using Microsoft.AspNetCore.Components;

[Route("/admin")]
[RouteTitle("Admin Panel")]
[RouteGuard(typeof(AuthGuard))]
[RouteTransition(RouteTransition.Fade)]
public class AdminPage : ComponentBase
{
    // Component implementation
}
```

Enable attribute-based routes in your app:

```csharp
// In App.razor.cs or Routes.razor.cs
private List<RouteConfig> _routes = new List<RouteConfig>()
    .AddAttributeRoutes(typeof(App).Assembly);

// Or mix with programmatic routes
private List<RouteConfig> _routes = new List<RouteConfig>
{
    new RouteConfig { Path = "/", Component = typeof(Home) }
}.AddAttributeRoutes(typeof(App).Assembly);
```

[**📚 Learn more about Attribute-Based Routing →**](ATTRIBUTE_ROUTING.md)

### Layouts

Blazouter provides flexible layout management with `DefaultLayout` and per-route `Layout` properties.

```csharp
// Set a default layout for all routes
<Router Routes="@_routes" DefaultLayout="typeof(MainLayout)">
    <NotFound><h1>404</h1></NotFound>
</Router>

@code {
    private List<RouteConfig> _routes = new()
    {
        // Uses default layout (MainLayout)
        new RouteConfig { Path = "/", Component = typeof(Home) },
        
        // Override with different layout
        new RouteConfig 
        { 
            Path = "/admin", 
            Component = typeof(AdminDashboard),
            Layout = typeof(AdminLayout)  // Uses AdminLayout instead
        },
        
        // No layout for this route
        new RouteConfig 
        { 
            Path = "/print", 
            Component = typeof(PrintView),
            Layout = null  // Renders without any layout
        }
    };
}
```

**Layout Priority:** `RouteConfig.Layout` > `Router.DefaultLayout` > No Layout

Your layout component must inherit from `LayoutComponentBase` and use `@Body`:

```razor
@inherits LayoutComponentBase

<div class="app-layout">
    <nav><!-- Navigation --></nav>
    <main>
        @Body  <!-- Page component renders here -->
    </main>
    <footer><!-- Footer --></footer>
</div>
```

### Basic Routing

```csharp
new RouteConfig
{
    Path = "/about",
    Component = typeof(About),
    Title = "About Us",
    Transition = RouteTransition.Slide
}
```

### Nested Routes

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

Use `<RouterOutlet />` in the parent component to render child routes:

```razor
@using Blazouter.Components

<div class="layout">
    <h1>Products</h1>
    <RouterOutlet />
</div>
```

### Route Middleware

Execute code before and after route navigation for logging, analytics, data preloading, and more:

```csharp
new RouteConfig
{
    Path = "/admin",
    Component = typeof(AdminPanel),
    Middleware = new List<Type> 
    { 
        typeof(LoggingMiddleware),
        typeof(TimingMiddleware),
        typeof(AnalyticsMiddleware)
    }
}
```

Create a middleware:

```csharp
using Blazouter.Models;
using Blazouter.Interfaces;

public class LoggingMiddleware : IRouteMiddleware
{
    public async Task InvokeAsync(RouteMiddlewareContext context, Func<Task> next)
    {
        // Before navigation
        Console.WriteLine($"Navigating to: {context.Path}");
        
        // Continue to next middleware or component
        await next();
        
        // After navigation
        Console.WriteLine($"Navigation completed");
    }
}
```

Middleware can share data with components:

```csharp
public class DataPreloadMiddleware : IRouteMiddleware
{
    public async Task InvokeAsync(RouteMiddlewareContext context, Func<Task> next)
    {
        // Store data - will only be passed if component has matching parameter
        context.Data["PreloadedData"] = await LoadDataAsync();
        context.Data["LoadTimestamp"] = DateTime.UtcNow;
        
        await next();
    }
}

// In your component - only define parameters you need
[Parameter]
public object? PreloadedData { get; set; }

// LoadTimestamp is automatically filtered out if not defined
```

**Note:** The Router automatically filters both middleware data and route data to only pass parameters that the component actually has. You can store any data in `context.Data` or use `RouteData` attributes without worrying about parameter mismatch errors.

### Route Guards (Protected Routes)

Control access to routes based on authentication or authorization:

```csharp
new RouteConfig
{
    Path = "/admin",
    Component = typeof(AdminPanel),
    Guards = new List<Type> { typeof(AuthGuard) }
}
```

Create a guard:

```csharp
using Blazouter.Models;
using Blazouter.Interfaces;

public class AuthGuard : IRouteGuard
{
    public async Task<bool> CanActivateAsync(RouteMatch match)
    {
        // Check authentication
        return await IsAuthenticated();
    }

    public Task<string?> GetRedirectPathAsync(RouteMatch match)
    {
        return Task.FromResult<string?>("/login");
    }
}
```

### Lazy Loading

```csharp
new RouteConfig
{
    Path = "/reports",
    ComponentLoader = async () =>
    {
        // Simulate loading delay or dynamic import
        await Task.Delay(100);
        return typeof(ReportsPage);
    }
}
```

### Route Links

```razor
@using Blazouter.Components

<nav>
    <RouterLink Href="/" Exact="true" ActiveClass="active">Home</RouterLink>
    <RouterLink Href="/about" ActiveClass="active">About</RouterLink>
    <RouterLink Href="/users" ActiveClass="active">Users</RouterLink>
</nav>
```

### Programmatic Navigation

```razor
@inject RouterNavigationService NavService

<button @onclick="NavigateToUser">Go to User</button>

@code {
    private void NavigateToUser()
    {
        NavService.NavigateTo("/users/123");
    }
}
```

### Access Route Parameters

```razor
@using Blazouter.Services
@inject RouterStateService RouterState

<h1>User: @_userId</h1>

@code {
    private string? _userId;

    protected override void OnInitialized()
    {
        _userId = RouterState.GetParam("id");
    }
}
```

### Query String Utilities

Blazouter provides comprehensive query string helpers for type-safe parameter handling:

**Type-safe query parameter parsing:**

```razor
@using Blazouter.Extensions
@inject RouterStateService RouterState

@code {
    protected override void OnInitialized()
    {
        // Typed parsing with defaults
        int page = RouterState.GetQueryInt("page", 1);
        bool active = RouterState.GetQueryBool("active", false);
        DateTime? date = RouterState.GetQueryDateTimeOrNull("date");
        
        // Get all query parameters
        var allParams = RouterState.GetAllQueryParams();
    }
}
```

**Fluent query string building:**

```csharp
@using Blazouter.Utilities
@inject RouterNavigationService NavService

@code {
    private void NavigateWithQuery()
    {
        // Build query strings with type safety
        NavService.NavigateToWithQuery("/search", q => q
            .Add("term", "blazor")
            .Add("active", true)
            .Add("page", 2));
    }
}
```

**Update query parameters:**

```csharp
// Replace specific parameters while keeping others
NavService.NavigateToWithUpdatedQuery(RouterState, null, q => q
    .Set("page", currentPage + 1)
    .Set("sort", "name"));

// Remove parameters
NavService.NavigateToWithRemovedQuery(RouterState, "filter", "sort");

// Clear all parameters
NavService.NavigateToWithClearedQuery(RouterState);
```

The `QueryStringBuilder` supports 15 type overloads including: string, int, long, decimal, double, bool, DateTime, Guid, enum, and their nullable variants.

## 🎨 Route Transitions

Blazouter includes 14 built-in transitions for beautiful page navigation:

- `Fade` - Fade in animation
- `Scale` - Scale in animation
- `Flip` - 3D card flip animation
- `Slide` - Slide from left animation
- `Pop` - Bounce effect with elastic easing
- `SlideUp` - Slide from bottom animation
- `None` - No transition animation (instant)
- `Rotate` - Spinning entrance along Z-axis
- `Reveal` - Mask opening from bottom to top
- `SlideFade` - Combined slide and fade effect
- `Blur` - Focus transition from blurred to sharp
- `Swipe` - Mobile-style swipe reveal from right to left
- `Curtain` - Theatrical curtain opening from top to bottom
- `Spotlight` - Dramatic lighting effect with brightness and blur
- `Lift` - Content lifts up with subtle scaling and shadow (iOS-style)

```csharp
new RouteConfig
{
    Path = "/",
    Component = typeof(Home),
    Transition = RouteTransition.Fade
}
```

## ⚠️ Error Handling

Blazouter provides comprehensive error handling capabilities to gracefully manage routing errors:

### Built-in Error Handling

Use the `ErrorContent` parameter in the Router component to display custom error pages:

```razor
<Router Routes="@_routes">
    <ErrorContent Context="errorInfo">
        <div class="error-page">
            <h1>⚠️ Routing Error</h1>
            <p><strong>@errorInfo.ErrorType</strong></p>
            <p>@errorInfo.Message</p>
            @if (errorInfo.Retry != null)
            {
                <button @onclick="@errorInfo.Retry">Try Again</button>
            }
        </div>
    </ErrorContent>
    <NotFound>
        <h1>404 - Page Not Found</h1>
    </NotFound>
</Router>
```

### Custom Error Handlers

Implement `IRouterErrorHandler` for custom error handling logic:

```csharp
public class CustomRouterErrorHandler : IRouterErrorHandler
{
    public Task HandleErrorAsync(RouterErrorContext context)
    {
        // Log error, send telemetry, etc.
        Console.WriteLine($"Route error: {context.ErrorType} - {context.Message}");
        return Task.CompletedTask;
    }
}

// Register in Program.cs
builder.Services.AddBlazouterErrorHandler<CustomRouterErrorHandler>();
```

### Error Types

- `InvalidRoute` - Invalid route configuration
- `GuardRejected` - Route guard denied access
- `NavigationFailed` - Navigation operation failed
- `ComponentLoadFailed` - Component failed to load

## 🚀 Advanced Caching Strategies

Blazouter includes sophisticated caching mechanisms to optimize route matching and component loading performance. The caching layer is transparent, requiring no code changes while significantly improving navigation speed, especially for applications with complex routing structures.

### Features

- **Route Match Caching**: Cached route matching results for faster subsequent navigations
- **Component Type Caching**: Lazily loaded components are cached to avoid repeated async loading
- **LRU Eviction**: Least Recently Used (LRU) policy ensures efficient memory usage
- **TTL Support**: Optional time-to-live for cache entries
- **Thread-Safe**: Concurrent-safe implementation for server-side scenarios
- **Statistics Tracking**: Monitor cache performance with detailed metrics

### Default Configuration

Caching is enabled by default with sensible defaults:

```csharp
// In Program.cs - caching is automatic
builder.Services.AddBlazouter();
```

Default settings:
- Route match cache: **Enabled** (max 100 entries)
- Component type cache: **Enabled** (max 50 entries)
- TTL: **No expiration** (cached indefinitely)
- Statistics: **Disabled** (minimal overhead)

### Custom Cache Configuration

Fine-tune caching behavior for your application's needs:

```csharp
// In Program.cs
builder.Services.AddBlazouter(options =>
{
    // Adjust cache sizes
    options.MaxRouteMatchCacheSize = 200;        // Increase for apps with many routes
    options.MaxComponentTypeCacheSize = 100;     // More lazy-loaded components
    
    // Enable statistics tracking
    options.EnableStatistics = true;             // Monitor cache effectiveness
    
    // Set TTL for development scenarios
    options.RouteMatchCacheTTLSeconds = 300;     // 5 minutes (0 = no expiration)
    
    // Disable specific caches if needed
    options.EnableRouteMatchCache = true;        // Route matching cache
    options.EnableComponentTypeCache = true;     // Component loading cache
});
```

### Per-Route Cache Control

Control caching at the individual route level for fine-grained optimization:

```csharp
new RouteConfig
{
    Path = "/admin/dashboard",
    Component = typeof(AdminDashboard),
    EnableCache = false  // Never cache this route
}

new RouteConfig
{
    Path = "/static-content",
    Component = typeof(StaticPage),
    EnableCache = true   // Always cache (even if global caching disabled)
}

new RouteConfig
{
    Path = "/default-page",
    Component = typeof(DefaultPage),
    EnableCache = null   // Use global cache settings (default)
}
```

**Use cases for disabling cache per route:**
- Admin dashboards with real-time data
- User-specific pages that change frequently
- Routes with middleware that should run every time
- Error pages for testing
- Routes with dynamic content

### Cache Statistics

Monitor cache performance to optimize configuration:

```csharp
@inject IRouteCacheService CacheService

@code {
    private void ShowCacheStats()
    {
        var stats = CacheService.GetStatistics();
        
        Console.WriteLine($"Total Requests: {stats.TotalRequests}");
        Console.WriteLine($"Cache Hits: {stats.CacheHits}");
        Console.WriteLine($"Cache Misses: {stats.CacheMisses}");
        Console.WriteLine($"Hit Rate: {stats.HitRate:F2}%");
        Console.WriteLine($"Route Cache Size: {stats.RouteMatchCacheSize}");
        Console.WriteLine($"Component Cache Size: {stats.ComponentTypeCacheSize}");
    }
}
```

**Note:** Statistics tracking must be enabled in cache options to collect metrics.

### Cache Management

Programmatically manage cache entries when needed:

```csharp
@inject IRouteCacheService CacheService

@code {
    // Clear all cached entries
    private void ClearCache()
    {
        CacheService.Clear();
    }
    
    // Invalidate specific route
    private void InvalidateRoute(string path)
    {
        CacheService.InvalidateRouteMatch(path);
    }
}
```

### Performance Benefits

The caching layer provides significant performance improvements:

- **First Navigation**: Normal route matching (no cache)
- **Subsequent Navigations**: Instant lookup from cache (10-50x faster)
- **Lazy Loading**: Components loaded once, cached for instant reuse
- **Memory Efficient**: LRU eviction keeps memory usage bounded

### When to Adjust Cache Settings

**Increase cache sizes** if you have:
- Many unique routes in your application
- Frequent navigation between many different pages
- High memory availability

**Enable TTL** if you have:
- Dynamic routes that change during runtime
- Development environment with hot reload
- Routes that depend on external configuration

**Disable caching** if you need:
- Real-time route configuration updates
- Debugging route matching logic
- Minimal memory footprint

**[📚 Complete Caching Documentation →](CACHING.md)**

## 🔧 TypeScript Integration

Blazouter includes TypeScript-based JavaScript interop for enhanced browser integration with full type safety.

### Features

- **SEO Support**: Set meta tags, Open Graph tags, and canonical URLs
- **Type Safety**: Full TypeScript definitions with `.d.ts` files for IntelliSense
- **Browser Navigation**: True browser back/forward navigation using the History API
- **Document Manipulation**: Dynamic title updates, meta tags, scrolling, and focus management

### Installation

Enable JavaScript interop by registering the services:

```csharp
builder.Services.AddBlazouter();
builder.Services.AddBlazouterInterop();  // Enable TypeScript interop
```

Add the JavaScript module import to your `index.html`:

```html
<!-- In wwwroot/index.html, add this in the <head> section -->
<script type="module" src="_content/Blazouter/js/index.js"></script>
```

### Browser Navigation Example

```csharp
@inject RouterNavigationService NavService

<button @onclick="GoBack">← Back</button>
<button @onclick="GoForward">Forward →</button>

@code {
    private async Task GoBack()
    {
        await NavService.GoBackAsync();  // Uses browser History API
    }

    private async Task GoForward()
    {
        await NavService.GoForwardAsync();
    }
}
```

### Document Manipulation Example

```csharp
@inject DocumentInterop DocumentInterop

@code {
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            // Update page title
            await DocumentInterop.SetTitleAsync("Home - My App");
            
            // Set meta tags for SEO
            await DocumentInterop.SetMetaTagAsync("description", "Welcome to my app");
            
            // Set Open Graph tags for social sharing
            await DocumentInterop.SetOpenGraphTagAsync("og:title", "My App");
            
            // Scroll to top on navigation
            await DocumentInterop.ScrollToTopAsync();
        }
    }
}
```

**[📚 Full TypeScript Integration Documentation →](TYPESCRIPT_INTEGRATION.md)**

## 🏗️ Project Structure

```
Blazouter/
├── src/
│   ├── Blazouter/                 # Core library (required)
│   │   ├── Attributes/            # Route attribute definitions
│   │   ├── Components/            # Router components (Router, RouterLink, RouterOutlet)
│   │   │   └── Layouts/           # Built-in layout components
│   │   ├── Enums/                 # Enumeration types (RouteTransition, RouterErrorType)
│   │   ├── Extensions/            # Service collection and router extensions (typed query parameters, navigation, transitions)
│   │   ├── Guards/                # Route guard implementations (AuthGuard)
│   │   ├── Handlers/              # Error handler implementations (DefaultRouterErrorHandler)
│   │   ├── Interfaces/            # Interface definitions (IRouteGuard, IRouteMiddleware, IRouteMatcherService, IRouterErrorHandler)
│   │   ├── Interops/              # JavaScript interop services (NavigationInterop, DocumentInterop, StorageInterop, ViewportInterop, ClipboardInterop)
│   │   ├── Models/                # Route models (RouteConfig, RouteMatch, RouterErrorContext, etc.)
│   │   ├── Resources/             # Embedded resources
│   │   ├── Services/              # Routing services (RouterStateService, RouteMatcherService, RouterNavigationService)
│   │   ├── Utilities/             # Query string builder and helper utilities
│   │   └── wwwroot/               # CSS and assets (blazouter.css, blazouter.min.css)
│   │       └── js/                # Compiled JavaScript modules with TypeScript definitions (.js, .d.ts, .js.map)
│   ├── Blazouter.TypeScript/      # TypeScript source files for JavaScript interop
│   │   ├── TypeScript/            # TypeScript source files (navigation.ts, document.ts, storage.ts, viewport.ts, clipboard.ts, index.ts)
│   │   ├── package.json           # NPM dependencies for TypeScript compilation
│   │   └── tsconfig.json          # TypeScript compiler configuration
│   ├── Blazouter.Server/          # Server-specific extensions
│   │   ├── Extensions/            # Server integration (AddBlazouterSupport)
│   │   ├── Pages/                 # Server pages
│   │   └── Resources/             # Embedded resources
│   ├── Blazouter.WebAssembly/     # WebAssembly-specific extensions
│   │   └── Resources/             # Embedded resources
│   ├── Blazouter.Web/             # Web-specific extensions (DEPRECATED - use Server + WebAssembly)
│   │   ├── Extensions/            # Web integration
│   │   ├── Pages/                 # Web pages
│   │   └── Resources/             # Embedded resources
│   └── Blazouter.Hybrid/          # Hybrid/MAUI-specific extensions
│       ├── Extensions/            # MAUI integration (AddBlazouterSupport)
│       └── Resources/             # Embedded resources
└── samples/
    ├── Blazouter.Server.Sample/       # Server sample app
    ├── Blazouter.WebAssembly.Sample/  # WebAssembly sample app
    ├── Blazouter.Hybrid.Sample/       # Hybrid/MAUI sample app
    └── Blazouter.Web.Sample/          # Web (Server + WASM) sample app
        ├── Blazouter.Web.Sample/         # Server project
        └── Blazouter.Web.Client.Sample/  # Client project
```

## 🎮 Running the Samples

Blazouter includes multiple sample applications for different hosting models:

**Blazor Server Sample:**

```bash
cd samples/Blazouter.Server.Sample
dotnet run
```

**Blazor WebAssembly Sample:**

```bash
cd samples/Blazouter.WebAssembly.Sample
dotnet run
```

**Blazor Hybrid Sample (MAUI):**

```bash
cd samples/Blazouter.Hybrid.Sample
dotnet build -t:Run -f net9.0-windows10.0.19041.0
```

**Blazor Web Sample (.NET 8+ with Server + WASM):**

```bash
cd samples/Blazouter.Web.Sample/Blazouter.Web.Sample
dotnet run
```

Then navigate to the URL shown in your terminal (typically `https://localhost:5001` or `http://localhost:5000`).

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## 📄 License

This project is licensed under the MIT License.

## 📊 Project Stats

- **Supported .NET versions**: .NET 6.0, 7.0, 8.0, 9.0, 10.0
- **Platforms**: Blazor WebAssembly, Blazor Server, Blazor Hybrid (MAUI)
- **License**: MIT
- **Packages**:
  - [Blazouter](https://www.nuget.org/packages/Blazouter/) (Core)
  - [Blazouter.Server](https://www.nuget.org/packages/Blazouter.Server/)
  - [Blazouter.Hybrid](https://www.nuget.org/packages/Blazouter.Hybrid/)
  - [Blazouter.WebAssembly](https://www.nuget.org/packages/Blazouter.WebAssembly/)

## 🔗 Links

- [Changelog](https://github.com/Taiizor/Blazouter/blob/develop/CHANGELOG.md)
- [Issue Tracker](https://github.com/Taiizor/Blazouter/issues)
- [Documentation](https://github.com/Taiizor/Blazouter/blob/develop/FEATURES.md)
- [Caching System](https://github.com/Taiizor/Blazouter/blob/develop/CACHING.md)
- [Contributing Guide](https://github.com/Taiizor/Blazouter/blob/develop/CONTRIBUTING.md)
- [Sample Applications](https://github.com/Taiizor/Blazouter/tree/develop/samples)
- [TypeScript Integration](https://github.com/Taiizor/Blazouter/blob/develop/TYPESCRIPT_INTEGRATION.md)

## 🙏 Acknowledgments

Inspired by React Router and built to bring similar capabilities to the Blazor ecosystem.

## 📝 Roadmap

- [x] Route middleware support
- [ ] Performance optimizations
- [x] Advanced caching strategies
- [x] Query string helpers and utilities
- [x] Better TypeScript integration for JS interop

## ⭐ Show Your Support

If you find Blazouter helpful, please consider giving it a star on GitHub! It helps the project grow and reach more developers.

---

Made with ❤️ for the Blazor community