# Attribute-Based Routing in Blazouter

Blazouter supports both programmatic route configuration (using `RouteConfig` objects) and declarative attribute-based routing. Attribute-based routing provides a more intuitive, co-located way to define routes directly on your component classes.

## Overview

Attribute-based routing allows you to decorate your Blazor components with attributes that define their routing configuration. This approach:

-   **✅ Declarative** - Clear, easy-to-read syntax
-   **✅ Flexible** - Can be mixed with programmatic routes as needed
-   **✅ More intuitive** - Route configuration is co-located with the component
-   **✅ Backward compatible** - Works alongside existing programmatic routing

## Available Attributes

### `[Route]` - Define Route Path

Specifies the URL path for a component.

```csharp
[Route("/admin")]
public class AdminPage : ComponentBase
{
    // Component implementation
}
```

**Note:** Named `Route` instead of `Route` to avoid conflicts with Blazor's built-in `RouteAttribute`.

**Parameters:**

-   `path` (string) - The route path pattern. Supports dynamic parameters with `:` prefix (e.g., `/users/:id`)

### `[RouteTransition]` - Set Animation

Specifies the transition animation when navigating to this route.

```csharp
[Route("/about")]
[RouteTransition(RouteTransition.Fade)]
public class AboutPage : ComponentBase
{
    // Component implementation
}
```

**Parameters:**

-   `transition` (RouteTransition enum) - The animation type (Fade, Slide, Scale, etc.)

### `[RouteMiddleware]` - Add Route Middleware

Specifies middleware to execute during navigation. Multiple middleware can be applied.

```csharp
[Route("/admin")]
[RouteMiddleware(typeof(LoggingMiddleware))]
[RouteMiddleware(typeof(TimingMiddleware))]
[RouteMiddleware(typeof(AnalyticsMiddleware))]
public class AdminPage : ComponentBase
{
    // Component implementation
}
```

**Parameters:**

-   `middlewareType` (Type) - The type of the middleware class (must implement `IRouteMiddleware`)

**Note:** Middleware execute in the order they are declared, before guards. Middleware can execute code before and after navigation, share data with components, and abort or redirect navigation.

**Common Use Cases:**

-   Logging and analytics tracking
-   Performance monitoring
-   Data preloading
-   Feature flags
-   Session management

### `[RouteGuard]` - Add Access Control

Specifies route guards for authentication/authorization. Multiple guards can be applied.

```csharp
[Route("/admin")]
[RouteGuard(typeof(AuthGuard))]
[RouteGuard(typeof(AdminRoleGuard))]
public class AdminPage : ComponentBase
{
    // Component implementation
}
```

**Parameters:**

-   `guardType` (Type) - The type of the guard class (must implement `IRouteGuard`)

**Note:** Guards execute in the order they are declared, after middleware.

### `[RouteLayout]` - Set Layout Component

Specifies the layout component for this route.

```csharp
[Route("/admin")]
[RouteLayout(typeof(AdminLayout))]
public class AdminPage : ComponentBase
{
    // Component implementation
}
```

**Parameters:**

-   `layoutType` (Type?) - The layout component type (must inherit from `LayoutComponentBase`), or `null` for no layout

### `[RouteTitle]` - Set Page Title

Specifies the title for the route.

```csharp
[Route("/about")]
[RouteTitle("About Us")]
public class AboutPage : ComponentBase
{
    // Component implementation
}
```

**Parameters:**

-   `title` (string) - The route title

### `[RouteData]` - Add Custom Data

Adds custom key-value data to the route. Multiple data attributes can be applied.

```csharp
[Route("/admin")]
[RouteData("RequireAdmin", true)]
[RouteData("Section", "Management")]
public class AdminPage : ComponentBase
{
    // Only define parameters you need - others are automatically filtered
    [Parameter]
    public string? Section { get; set; }

    // RequireAdmin is not defined, so it's filtered out (no error)
}
```

**Parameters:**

-   `key` (string) - The data key
-   `value` (object) - The data value

**Note:** Route data is automatically filtered based on component parameters. You can use any `[RouteData]` attributes without needing matching parameters in the component - only data with matching `[Parameter]` properties will be passed through.

### `[RouteRedirect]` - Redirect to Another Path

Specifies that this route should redirect to another path without rendering the component.

```csharp
[Route("/old-path")]
[RouteRedirect("/new-path")]
public partial class OldPathRedirect : ComponentBase
{
    // This component won't be rendered; navigation redirects to /new-path
}
```

**Parameters:**

-   `redirectPath` (string) - The target redirect path

**Note:** When using `RouteRedirect`, the component will not be rendered, and other attributes like `RouteTransition` or `RouteGuard` will be ignored.

### `[RouteExact]` - Exact Path Matching

Specifies that this route should match the URL exactly.

```csharp
[Route("/products")]
[RouteExact(true)]
public partial class ProductsPage : ComponentBase
{
    // This route only matches exactly "/products", not "/products/123"
}
```

**Parameters:**

-   `exact` (bool) - Whether the route path must match the URL exactly (defaults to `true`)

**Note:** When `false` (default behavior without the attribute), routes with child routes can match partially. Use `[RouteExact(true)]` for routes that should only match the exact path.

### `[RouteCache]` - Control Route Caching

Specifies whether caching is enabled for this specific route.

```csharp
[Route("/admin/dashboard")]
[RouteCache(false)]
public partial class AdminDashboard : ComponentBase
{
    // This route will never be cached
}

[Route("/static-page")]
[RouteCache(true)]
public partial class StaticPage : ComponentBase
{
    // This route will always be cached, even if global caching is disabled
}
```

**Parameters:**

-   `enableCache` (bool) - Whether to cache this route. `true` = always cache, `false` = never cache. To use global settings, omit the attribute.

**Note:** This affects route match caching. For more information about caching, see [CACHING.md](CACHING.md).

## Usage

### Basic Example

```csharp
using Blazouter.Enums;
using Blazouter.Models;
using Blazouter.Attributes;
using Microsoft.AspNetCore.Components;

namespace MyApp.Pages
{
    [Route("/profile")]
    [RouteTransition(RouteTransition.Fade)]
    [RouteTitle("User Profile")]
    public class ProfilePage : ComponentBase
    {
        // Component implementation
    }
}
```

### Complete Example with All Attributes

```csharp
using Blazouter.Enums;
using Blazouter.Models;
using Blazouter.Attributes;
using Microsoft.AspNetCore.Components;

namespace MyApp.Pages
{
    [Route("/admin/dashboard")]
    [RouteTransition(RouteTransition.Slide)]
    [RouteMiddleware(typeof(LoggingMiddleware))]
    [RouteMiddleware(typeof(TimingMiddleware))]
    [RouteGuard(typeof(AuthGuard))]
    [RouteGuard(typeof(AdminGuard))]
    [RouteLayout(typeof(AdminLayout))]
    [RouteTitle("Admin Dashboard")]
    [RouteData("RequireAdmin", true)]
    [RouteData("Section", "Dashboard")]
    public class AdminDashboard : ComponentBase
    {
        // Access route data via parameters if needed
        [Parameter]
        public string? Section { get; set; }

        // Component implementation
    }
}
```

### Enabling Attribute-Based Routes

After defining components with route attributes, you need to enable route discovery in your application:

#### Option 1: Add to Existing Routes

Mix attribute-based routes with programmatic routes:

```csharp
// In App.razor.cs or Routes.razor.cs
using Blazouter.Enums;
using Blazouter.Models;
using Blazouter.Extensions;

public partial class App
{
    private List<RouteConfig> _routes = new List<RouteConfig>
    {
        // Programmatic routes
        new RouteConfig
        {
            Path = "/",
            Component = typeof(Pages.Home),
            Transition = RouteTransition.Fade
        },
        new RouteConfig
        {
            Path = "/contact",
            Component = typeof(Pages.Contact),
            Transition = RouteTransition.Slide
        }
    }.AddAttributeRoutes(typeof(App).Assembly); // Add routes from attributes
}
```

#### Option 2: Use Only Attribute-Based Routes

Create routes entirely from attributes:

```csharp
// In App.razor.cs or Routes.razor.cs
using Blazouter.Models;
using Blazouter.Extensions;

public partial class App
{
    private List<RouteConfig> _routes =
        RouteConfigExtensions.FromAttributes(typeof(App).Assembly);
}
```

#### Option 3: Scan Multiple Assemblies

If your components are spread across multiple assemblies:

```csharp
private List<RouteConfig> _routes = new List<RouteConfig>()
    .AddAttributeRoutes(
        typeof(App).Assembly,              // Main assembly
        typeof(SharedComponents).Assembly  // Shared library
    );
```

## Dynamic Parameters

Attribute-based routes support dynamic parameters just like programmatic routes:

```csharp
[Route("/users/:id")]
[RouteTransition(RouteTransition.Fade)]
public class UserDetailPage : ComponentBase
{
    [Inject] private RouterStateService RouterState { get; set; } = default!;

    private string? _userId;

    protected override void OnInitialized()
    {
        _userId = RouterState.GetParam("id");
    }
}
```

## Middleware with Attributes

Route middleware allows you to execute code before and after navigation. Middleware can be used for logging, analytics, data preloading, and more.

### Basic Middleware Example

```csharp
[Route("/profile")]
[RouteMiddleware(typeof(LoggingMiddleware))]
[RouteMiddleware(typeof(TimingMiddleware))]
public class ProfilePage : ComponentBase
{
    // Middleware will execute in order: Logging -> Timing -> Component
}
```

### Middleware with Data Sharing

Middleware can pass data to components via the context:

```csharp
// Middleware implementation
public class DataPreloadMiddleware : IRouteMiddleware
{
    public async Task InvokeAsync(RouteMiddlewareContext context, Func<Task> next)
    {
        // Preload data
        var data = await LoadDataAsync();
        context.Data["PreloadedData"] = data;

        await next();
    }
}

// Component using the attribute
[Route("/users/:id")]
[RouteMiddleware(typeof(DataPreloadMiddleware))]
public class UserDetailPage : ComponentBase
{
    [Parameter]
    public object? PreloadedData { get; set; }

    // Only define parameters you need - other middleware data is automatically filtered
}
```

**Note:** The Router automatically filters both middleware data and route data based on component parameters. Middleware can store any data in `context.Data` and you can use any `[RouteData]` attributes without causing errors - only matching `[Parameter]` properties will receive the data.

### Combining Middleware and Guards

Middleware execute before guards, allowing you to set up context for guards:

```csharp
[Route("/admin")]
[RouteMiddleware(typeof(SessionMiddleware))]     // Runs first
[RouteMiddleware(typeof(LoggingMiddleware))]     // Runs second
[RouteGuard(typeof(AuthGuard))]                  // Runs third
[RouteGuard(typeof(AdminRoleGuard))]             // Runs fourth
public class AdminPage : ComponentBase
{
    // Execution order: SessionMiddleware -> LoggingMiddleware -> AuthGuard -> AdminRoleGuard -> Component
}
```

## Nested Routes

For nested routes, you still need to use programmatic configuration since parent-child relationships require more complex setup:

```csharp
// Parent can use attributes
[Route("/products")]
public class ProductsLayout : ComponentBase
{
    // Layout with <RouterOutlet />
}

// But children should be added programmatically
private List<RouteConfig> _routes = new List<RouteConfig>
{
    new RouteConfig
    {
        Path = "/products",
        Component = typeof(ProductsLayout),
        Children = new List<RouteConfig>
        {
            new RouteConfig { Path = "", Component = typeof(ProductList), Exact = true },
            new RouteConfig { Path = ":id", Component = typeof(ProductDetail) }
        }
    }
}.AddAttributeRoutes(typeof(App).Assembly);
```

## Best Practices

### 1. Naming Convention

Use descriptive route paths that reflect your component purpose:

```csharp
[Route("/admin/users")]          // Good
[Route("/au")]                   // Bad - unclear
```

### 2. Group Related Attributes

Keep related attributes together for readability:

```csharp
// Good - grouped by purpose
[Route("/admin")]
[RouteLayout(typeof(AdminLayout))]
[RouteTitle("Admin Panel")]
[RouteMiddleware(typeof(LoggingMiddleware))]
[RouteGuard(typeof(AuthGuard))]
[RouteGuard(typeof(AdminGuard))]
[RouteTransition(RouteTransition.Fade)]
[RouteData("Section", "Admin")]
public class AdminPage : ComponentBase { }
```

### 3. When to Use Attributes vs Programmatic

**Use Attributes When:**

-   Route configuration is simple and self-contained
-   You want configuration co-located with the component
-   The route doesn't have complex nested children
-   You need basic redirects or exact matching

**Use Programmatic Configuration When:**

-   You need nested routes with complex hierarchies
-   Routes need to be generated dynamically
-   You need lazy loading with `ComponentLoader`
-   Route configuration is shared across components

**Note:** As of this version, all `RouteConfig` properties except `ComponentLoader` and `Children` are supported via attributes.

### 4. Mixing Both Approaches

You can freely mix both approaches based on what works best for each route:

```csharp
private List<RouteConfig> _routes = new List<RouteConfig>
{
    // Complex nested route - programmatic
    new RouteConfig
    {
        Path = "/products",
        Component = typeof(ProductsLayout),
        Children = new List<RouteConfig>
        {
            new RouteConfig { Path = "", Component = typeof(ProductList) },
            new RouteConfig { Path = ":id", Component = typeof(ProductDetail) }
        }
    },

    // Lazy-loaded route - programmatic
    new RouteConfig
    {
        Path = "/reports",
        ComponentLoader = async () =>
        {
            await Task.Delay(100);
            return typeof(ReportsPage);
        }
    }
}.AddAttributeRoutes(typeof(App).Assembly); // Simple routes - attributes
```

## Comparison with Programmatic Routing

### Attribute-Based (New)

```csharp
[Route("/admin")]
[RouteTransition(RouteTransition.Fade)]
[RouteMiddleware(typeof(LoggingMiddleware))]
[RouteGuard(typeof(AuthGuard))]
[RouteTitle("Admin Panel")]
public class AdminPage : ComponentBase { }
```

### Programmatic (Traditional)

```csharp
new RouteConfig
{
    Path = "/admin",
    Component = typeof(AdminPage),
    Transition = RouteTransition.Fade,
    Middleware = new List<Type> { typeof(LoggingMiddleware) },
    Guards = new List<Type> { typeof(AuthGuard) },
    Title = "Admin Panel"
}
```

Both approaches produce the exact same result - choose based on your preference and use case!

## Feature Support Matrix

The following table shows which `RouteConfig` properties are supported via attributes:

| RouteConfig Property | Attribute Support | Attribute Name                   | Notes                                             |
| -------------------- | ----------------- | -------------------------------- | ------------------------------------------------- |
| `Path`               | ✅ Yes            | `[Route("/path")]`               | Required for attribute-based routing              |
| `Component`          | ✅ Yes            | (Inferred)                       | Automatically set to the decorated component type |
| `Transition`         | ✅ Yes            | `[RouteTransition(...)]`         | Supports all transition types                     |
| `Middleware`         | ✅ Yes            | `[RouteMiddleware(typeof(...))]` | Can be applied multiple times                     |
| `Guards`             | ✅ Yes            | `[RouteGuard(typeof(...))]`      | Can be applied multiple times                     |
| `Title`              | ✅ Yes            | `[RouteTitle("...")]`            | Sets the route title                              |
| `Layout`             | ✅ Yes            | `[RouteLayout(typeof(...))]`     | Supports null for no layout                       |
| `Data`               | ✅ Yes            | `[RouteData("key", value)]`      | Can be applied multiple times                     |
| `RedirectTo`         | ✅ Yes            | `[RouteRedirect("/path")]`       | Component won't render when redirecting           |
| `Exact`              | ✅ Yes            | `[RouteExact(true)]`             | Controls exact path matching                      |
| `EnableCache`        | ✅ Yes            | `[RouteCache(true/false)]`       | Controls per-route caching                        |
| `ComponentLoader`    | ❌ No             | N/A                              | Requires async lambda - use programmatic config   |
| `Children`           | ❌ No             | N/A                              | Complex hierarchies - use programmatic config     |

**Coverage:** 11 out of 13 `RouteConfig` properties are supported via attributes (92% coverage).

The two unsupported properties (`ComponentLoader` and `Children`) require complex programmatic logic that cannot be expressed declaratively through attributes. For these scenarios, use traditional programmatic `RouteConfig` objects.

## FAQ

### Q: Why is it named `Route` instead of `Route`?

**A:** Blazor has a built-in `RouteAttribute` used for traditional `@page` directives. To avoid naming conflicts and confusion, Blazouter uses `Route` for its attribute-based routing.

### Q: Can I use both `@page` and `[Route]` on the same component?

**A:** While technically possible, it's not recommended. Choose one routing approach:

-   Use `@page` for Blazor's built-in routing
-   Use `[Route]` for Blazouter's enhanced routing

### Q: Do attribute-based routes break existing code?

**A:** No! Attribute-based routing is completely optional and additive. Existing programmatic routes continue to work unchanged.

### Q: Can I use attributes with lazy loading?

**A:** Lazy loading requires using `ComponentLoader` in programmatic configuration, which cannot be expressed as an attribute. For lazy-loaded routes, use programmatic configuration.

### Q: How do I debug which routes are being discovered?

**A:** You can call `RouteAttributeDiscoveryService.DiscoverRoutes()` directly and inspect the returned list:

```csharp
var discoveredRoutes = RouteAttributeDiscoveryService.DiscoverRoutes(typeof(App).Assembly);
foreach (var route in discoveredRoutes)
{
    Console.WriteLine($"Found route: {route.Path} -> {route.Component?.Name}");
}
```

## Examples

Check out the [AttributeRouting.razor](samples/Blazouter.WebAssembly.Sample/Pages/AttributeRouting.razor) in the WebAssembly sample project for a complete working example.

## Learn More

-   [Main README](README.md)
-   [Sample Applications](samples/)
-   [Features Documentation](FEATURES.md)