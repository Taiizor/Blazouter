# Blazouter.Hybrid

[![NuGet](https://img.shields.io/nuget/v/Blazouter.Hybrid.svg)](https://www.nuget.org/packages/Blazouter.Hybrid/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Blazouter.Hybrid.svg)](https://www.nuget.org/packages/Blazouter.Hybrid/)

Hybrid/MAUI-specific extensions for Blazouter - the React Router-like routing library for Blazor applications. This package provides optimized components and extensions for Blazor Hybrid applications running on .NET MAUI.

## Features

- ✅ MAUI integration
- ✅ All core Blazouter features
- ✅ Native mobile app routing
- ✅ Native platform optimizations
- ✅ Cross-platform support (iOS, Android, macOS, Windows)

## Installation

```bash
dotnet add package Blazouter
dotnet add package Blazouter.Hybrid
```

**Note**: This package requires the core `Blazouter` package and is designed for .NET MAUI projects.

## Supported Platforms

- 🍎 iOS (15.0+)
- 📱 Android (API 24+)
- 💻 macOS Catalyst (15.0+)
- 🪟 Windows (10.0.17763+)

## Quick Start

### 1. Configure Services

```csharp
// MauiProgram.cs
using Blazouter.Hybrid.Extensions;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .AddBlazouterSupport() // Add Blazouter support
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

        #if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        #endif

        return builder.Build();
    }
}
```

### 2. Define Routes

```razor
@using Blazouter.Models
@using Blazouter.Components

<Router Routes="@_routes" DefaultLayout="typeof(Layout.MainLayout)">
    <NotFound>
        <div class="not-found">
            <h1>404 - Page Not Found</h1>
        </div>
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
            Path = "/settings",
            Component = typeof(Pages.Settings),
            Title = "Settings",
            Transition = RouteTransition.Slide
        }
    };
}
```

### 3. Include CSS

Add to your main HTML file (typically `index.html` in wwwroot):

```html
<link rel="stylesheet" href="_content/Blazouter/blazouter[.min].css" />
```

## Platform-Specific Features

### Native Navigation Integration
Blazouter.Hybrid integrates seamlessly with native platform navigation:
- Swipe gestures on iOS
- Platform-specific animations
- Back button handling on Android

### Optimized for Mobile
The Hybrid package includes optimizations for:
- Mobile performance
- Touch-based navigation
- Reduced memory footprint
- Native feel and responsiveness

## Layouts

Configure a default layout for all routes and override per route as needed:

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
        
        // Override with different layout for specific sections
        new RouteConfig 
        { 
            Path = "/onboarding", 
            Component = typeof(Pages.Onboarding),
            Layout = typeof(Layout.OnboardingLayout)
        },
        
        // No layout for fullscreen experiences
        new RouteConfig 
        { 
            Path = "/camera", 
            Component = typeof(Pages.Camera),
            Layout = null
        }
    };
}
```

## Nested Routes for Mobile Apps

Use `<RouterOutlet />` for nested routing within components:

Perfect for creating tab-based or hierarchical navigation:

```csharp
new RouteConfig
{
    Path = "/app",
    Component = typeof(MainLayout),
    Children = new List<RouteConfig>
    {
        new RouteConfig 
        { 
            Path = "home", 
            Component = typeof(HomePage) 
        },
        new RouteConfig 
        { 
            Path = "profile", 
            Component = typeof(ProfilePage) 
        },
        new RouteConfig 
        { 
            Path = "settings", 
            Component = typeof(SettingsPage) 
        }
    }
}
```

```razor
<!-- MainLayout.razor -->
@using Blazouter.Components

<div class="main-layout">
    <nav class="bottom-nav">
        <RouterLink Href="/app/home" ActiveClass="active">Home</RouterLink>
        <RouterLink Href="/app/profile" ActiveClass="active">Profile</RouterLink>
        <RouterLink Href="/app/settings" ActiveClass="active">Settings</RouterLink>
    </nav>
    <div class="content">
        <RouterOutlet />
    </div>
</div>
```

**Note**: `<RouterOutlet />` is for nested routing within a component hierarchy, while `Layout` (via `DefaultLayout` or `RouteConfig.Layout`) wraps entire routes with a common layout structure like headers, footers, and navigation.

## Route Guards for Mobile

Implement authentication or platform-specific checks:

```csharp
new RouteConfig
{
    Path = "/premium",
    Component = typeof(PremiumFeatures),
    Guards = new List<Type> { typeof(SubscriptionGuard) }
}

public class SubscriptionGuard : IRouteGuard
{
    private readonly ISubscriptionService _subscriptionService;
    
    public SubscriptionGuard(ISubscriptionService subscriptionService)
    {
        _subscriptionService = subscriptionService;
    }

    public async Task<bool> CanActivateAsync(RouteMatch match)
    {
        return await _subscriptionService.HasActiveSubscriptionAsync();
    }

    public Task<string?> GetRedirectPathAsync(RouteMatch match)
    {
        return Task.FromResult<string?>("/subscribe");
    }
}
```

## Platform-Aware Navigation

```csharp
@inject RouterNavigationService NavService

private async Task OnBackButtonPressed()
{
    // Handle Android back button
    #if ANDROID
    if (CanGoBack())
    {
        NavService.NavigateTo(-1);
    }
    else
    {
        // Exit app or show exit confirmation
    }
    #endif
}
```

## Lazy Loading for Better Performance

Reduce app startup time by loading features on-demand:

```csharp
new RouteConfig
{
    Path = "/reports",
    ComponentLoader = async () =>
    {
        // Load heavy component only when needed
        await Task.Delay(100);
        return typeof(ReportsPage);
    },
    Title = "Reports"
}
```

## Route Transitions

Smooth animations optimized for mobile devices:

```csharp
// Fade - Good for subtle transitions
new RouteConfig
{
    Path = "/home",
    Component = typeof(HomePage),
    Transition = RouteTransition.Fade
}

// Slide - Natural for hierarchical navigation
new RouteConfig
{
    Path = "/details",
    Component = typeof(DetailsPage),
    Transition = RouteTransition.Slide
}

// SlideUp - Great for modal-like screens
new RouteConfig
{
    Path = "/modal",
    Component = typeof(ModalPage),
    Transition = RouteTransition.SlideUp
}
```

## Programmatic Navigation

```csharp
@inject RouterNavigationService NavService

// Navigate to a route
private void GoToDetail(int itemId)
{
    NavService.NavigateTo($"/items/{itemId}");
}

// Go back
private void GoBack()
{
    NavService.NavigateTo(-1);
}

// Navigate with query parameters
private void SearchItems(string query)
{
    NavService.NavigateTo($"/search?q={query}");
}
```

## Access Route Parameters

```csharp
@inject RouterStateService RouterState

@code {
    private string? _itemId;
    private string? _searchQuery;

    protected override void OnInitialized()
    {
        _itemId = RouterState.GetParam("id");
        _searchQuery = RouterState.GetQueryParam("q");
    }
}
```

## Performance Tips for Hybrid Apps

1. **Cache route data** when appropriate
2. **Use lazy loading** for non-essential features
3. **Minimize transitions** on lower-end devices
4. **Optimize images and assets** used in routes
5. **Test on actual devices** for real-world performance
6. **Implement route guards** to prevent unnecessary loading

## Platform-Specific Considerations

### Android
- Test on various screen sizes
- Handle back button navigation
- Consider material design transitions

### iOS
- Respect safe areas
- Test swipe-back gestures
- Follow iOS Human Interface Guidelines

### Windows
- Support window resizing
- Consider keyboard navigation
- Test with mouse and touch input

## Target Frameworks

- .NET 10.0 (iOS, Android, macOS Catalyst, Windows)

## Example Application

See the [MAUI sample application](https://github.com/Taiizor/Blazouter/tree/develop/samples) for a complete working example.

## Documentation

- [Core Features](https://github.com/Taiizor/Blazouter/blob/develop/FEATURES.md)
- [Main Documentation](https://github.com/Taiizor/Blazouter)
- [.NET MAUI Documentation](https://learn.microsoft.com/dotnet/maui/)

## Migration from Shell Navigation

If you're migrating from .NET MAUI Shell navigation:

### Before (Shell)
```xml
<Shell>
    <ShellContent Title="Home" Route="home" ContentTemplate="{DataTemplate pages:HomePage}" />
    <ShellContent Title="Settings" Route="settings" ContentTemplate="{DataTemplate pages:SettingsPage}" />
</Shell>
```

### After (Blazouter)
```csharp
private List<RouteConfig> _routes = new()
{
    new RouteConfig { Path = "/home", Component = typeof(HomePage), Title = "Home" },
    new RouteConfig { Path = "/settings", Component = typeof(SettingsPage), Title = "Settings" }
};
```

## License

MIT License - see [LICENSE](https://github.com/Taiizor/Blazouter/blob/develop/LICENSE) for details.

## Support

- [GitHub Issues](https://github.com/Taiizor/Blazouter/issues)
- [GitHub Discussions](https://github.com/Taiizor/Blazouter/discussions)

Made with ❤️ for the Blazor community