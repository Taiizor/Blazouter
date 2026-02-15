# Blazouter Features

Blazouter is a comprehensive routing library for Blazor applications, available as multiple specialized packages to support different hosting models.

## 📦 Package Overview

| Package | Purpose | Platforms |
|---------|---------|-----------|
| **Blazouter** | Core routing library with all base features | All Blazor platforms |
| **Blazouter.Server** | Server-side rendering optimizations | Blazor Server / Blazor Web App (server project) |
| **Blazouter.Hybrid** | Native mobile/desktop support | .NET MAUI (iOS, Android, macOS, Windows) |
| **Blazouter.WebAssembly** | Client-side optimizations | Blazor WebAssembly / Blazor Web App (client project) |

> **Note:** The `Blazouter.Web` package has been deprecated. For Blazor Web Applications, use `Blazouter.Server` for the server project and `Blazouter.WebAssembly` for the client project.

## Implemented Features

### ✅ 1. Nested Routes
- Tested and verified working
- Full hierarchical route structure support
- Example: `/users` (parent) → `/users/:id` (child)
- Unlimited nesting depth for complex route hierarchies
- Parent routes with child routes using `RouterOutlet` component

### ✅ 2. Route Middleware
- Tested and verified working
- Can abort navigation or redirect to different paths
- `IRouteMiddleware` interface for custom middleware
- `InvokeAsync` method with context and next delegate
- Pipeline execution pattern for before/after navigation logic
- Middleware execute before guards in the navigation pipeline
- Support for multiple middleware per route (executed in order)
- Data sharing between middleware and components via context.Data
- Middleware can be registered via dependency injection or created via Activator
- Common use cases: logging, analytics, performance monitoring, data preloading, feature flags
- Example middleware: LoggingMiddleware, TimingMiddleware, AnalyticsMiddleware, DataPreloadMiddleware

### ✅ 3. Route Guards (Protected Routes)
- Tested and verified working
- `CanActivateAsync` method to control route access
- `GetRedirectPathAsync` for redirect on denied access
- Example: `SampleAuthGuard` in the sample application
- Support for multiple guards per route (executed in order)
- Guards execute after middleware in the navigation pipeline
- `IRouteGuard` interface for custom authentication/authorization logic
- Guards can be registered via dependency injection or created via Activator

### ✅ 4. Lazy Loading
- Reduces initial bundle size
- Tested and verified working
- Async component loading with `Task<Type>`
- Example: Lazy page with simulated 1-second delay
- Component caching after first load for performance
- `ComponentLoader` function property on `RouteConfig`
- Loading state support with `<Loading>` parameter in Router
- **WASM RCL Assembly Lazy Loading**: Load entire Razor Class Library assemblies on demand
  - `OnNavigateAsync` callback on Router for intercepting navigation and loading assemblies
  - `AdditionalAssemblies` parameter for registering dynamically loaded assemblies for route discovery
  - `BlazouterNavigationContext` model with `Path` and `CancellationToken` properties
  - Cancellation support for in-flight callbacks during rapid navigation
  - Path-based deduplication preventing redundant invocations
  - Works with `LazyAssemblyLoader` and `<BlazorWebAssemblyLazyLoad>` in project files

### ✅ 5. Route Transitions/Animations
- 14 built-in transition types
- Tested and verified working
- Custom animations can be added via CSS
- Configurable per-route via `Transition` property
- Automatic animation application on route change
- GPU-accelerated animations for smooth performance
- Respects prefers-reduced-motion accessibility preference
- Built-in transitions: `None`, `Pop`, `Blur`, `Fade`, `Flip`, `Lift`, `Scale`, `Slide`, `Swipe`, `Reveal`, `Rotate`, `Curtain`, `SlideUp`, `SlideFade`, `Spotlight`

### ✅ 6. Programmatic Navigation
- Query parameter support
- Browser history integration
- `NavigateTo(path)` method
- Tested and verified working
- Support for relative and absolute navigation
- Integration with Blazor's `NavigationManager`
- `RouterNavigationService` for imperative navigation

### ✅ 7. Dynamic Route Parameters
- Tested and verified working
- Parameter change notifications
- Type-safe parameter extraction
- Query string parameter support
- Support for nested route parameters
- Path parameters using `:paramName` syntax
- Easy access via `RouterStateService.GetParam(key)`

### ✅ 8. Active Link State
- `Exact` matching option
- Tested and verified working
- Visual feedback for current route
- Supports nested route active states
- Automatic update on route changes
- `ActiveClass` property for custom styling
- `RouterLink` component with automatic active class

### ✅ 9. Layout System
- Tested and verified working
- Layout state preservation during navigation
- Seamless layout switching during navigation
- Support for no-layout routes by setting Layout to null
- `RouteConfig.Layout` property for route-specific layouts
- `Router.DefaultLayout` parameter for application-wide layout
- Layout priority: Route.Layout > Router.DefaultLayout > No Layout
- Automatic layout wrapping using @Body from LayoutComponentBase

### ✅ 10. Attribute-Based Routing
- Tested and verified working
- Mix programmatic and attribute-based routes
- Declarative route configuration using attributes
- Full documentation in [ATTRIBUTE_ROUTING.md](https://github.com/Taiizor/Blazouter/blob/develop/ATTRIBUTE_ROUTING.md)
- Automatic route discovery via assembly scanning
- `AddAttributeRoutes()` extension for easy integration
- `FromAttributes()` for pure attribute-based configuration
- 9 attribute types: `[Route]`, `[RouteMiddleware]`, `[RouteGuard]`, `[RouteTransition]`, `[RouteLayout]`, `[RouteTitle]`, `[RouteData]`, `[RouteRedirect]`, `[RouteExact]`

### ✅ 11. Error Handling
- Error event notifications
- Comprehensive error handling system
- Retry mechanism for failed operations
- `DefaultRouterErrorHandler` with built-in logging
- `RouterErrorContext` with detailed error information
- `ErrorContent` parameter in Router for custom error UI
- `IRouterErrorHandler` interface for custom error handlers
- Custom error handler registration via `AddBlazouterErrorHandler<T>()`
- Error types: `ComponentLoadFailed`, `GuardRejected`, `NavigationFailed`, `InvalidRoute`, `MiddlewareExecution`

### ✅ 12. Query String Helpers and Utilities
- Tested and verified working
- Type-safe query string manipulation with fluent API
- Automatic URL encoding via `Uri.EscapeDataString`
- `QueryStringBuilder` class for building query strings
- Safe parsing with `TryParse` and default value support
- Comprehensive XML documentation with code examples
- `RouterStateExtensions` for typed query parameter parsing
  - `HasQuery()` to check parameter existence
  - `GetAllQueryParams()` to retrieve all parameters as dictionary
  - Nullable variants: `GetQueryIntOrNull()`, `GetQueryBoolOrNull()`, etc.
  - `GetQueryBool()` supporting "true", "false", "1", "0", "yes", "no", "on", "off"
  - `GetQueryDateTime()`, `GetQueryGuid()`, `GetQueryEnum<T>()` for complex types
  - `GetQueryInt()`, `GetQueryLong()`, `GetQueryDecimal()`, `GetQueryDouble()` with default values
- 15 `Set()` method overloads for replacing values without duplicates
- `RouterNavigationExtensions` for enhanced navigation with query strings
  - `NavigateToWithQuery()` - Fluent query builder integration
  - `NavigateToWithClearedQuery()` - Clear all query parameters
  - `NavigateToWithRemovedQuery()` - Remove specific parameters
  - `NavigateToWithReplacedQuery()` - Replace all query parameters
  - `NavigateToWithSingleQuery()` - Convenience for single parameter
  - `NavigateToWithUpdatedQuery()` - Update specific parameters, preserve others
- Sample application demonstrates all features in Navigation.razor and UserList.razor
- 15 `Add()` method overloads for all common types (string, int, long, decimal, double, bool, DateTime, Guid, enum, and nullable variants)

### ✅ 13. TypeScript Integration for JavaScript Interop
- Tested and verified working
- Type-safe JavaScript modules with `.d.ts` definition files for IntelliSense
- Comprehensive browser API integration with 5 specialized interop services
- Automatic TypeScript compilation to JavaScript with source maps
- Global namespace exposure pattern (`window.blazouterNavigation`, `window.blazouterDocument`, `window.blazouterStorage`, `window.blazouterViewport`, `window.blazouterClipboard`)
- Optional service registration via `AddBlazouterInterop()`
- **`NavigationInterop`** service for History API access
  - Navigate back/forward/go in browser history
  - URL and path information (getCurrentUrl, getPathname)
  - Hash navigation (getHash, setHash)
  - Query parameter helpers (getQueryString, getQueryParam, getAllQueryParams)
  - Page reload functionality
  - Check navigation availability
  - Manage history state (push and replace entries)
- **`DocumentInterop`** service for document manipulation
  - Dynamic title updates
  - Meta tag management (description, keywords, Open Graph tags)
  - Canonical URL support for SEO
  - Scroll position tracking (getScrollPosition, setScrollPosition)
  - Scroll to top/element with smooth scrolling
  - Element visibility detection (isElementVisible)
  - CSS class manipulation (addClass, removeClass, toggleClass)
  - Document ready state checks
  - Element focus management
- **`StorageInterop`** service for browser storage
  - Complete localStorage operations (set, get, remove, clear, keys, has)
  - Complete sessionStorage operations (set, get, remove, clear, keys, has)
  - Generic type support with JSON serialization
  - Type-safe storage access for complex objects
- **`ViewportInterop`** service for viewport and device info
  - Viewport dimensions (getViewportSize, width, height)
  - Screen information (getScreenSize)
  - Device pixel ratio detection
  - Orientation detection (isPortrait, isLandscape, getOrientation)
  - Device type detection (isMobile, isTablet, isDesktop, getDeviceType)
  - Fullscreen API (isFullscreen, requestFullscreen, exitFullscreen)
- **`ClipboardInterop`** service for clipboard operations
  - Copy text to clipboard with fallback for older browsers
  - Read text from clipboard
  - Feature detection (isClipboardSupported)
  - Permission checks (hasClipboardReadPermission, hasClipboardWritePermission)
- Integration with `RouterNavigationService`
  - `GoBackAsync()` - True browser back navigation
  - `GoForwardAsync()` - Browser forward navigation
  - `CanGoBackAsync()` - Check if back navigation is available
- Requires JavaScript module import: `<script type="module" src="_content/Blazouter/js/index.js"></script>`
- Full documentation in [TYPESCRIPT_INTEGRATION.md](https://github.com/Taiizor/Blazouter/blob/develop/TYPESCRIPT_INTEGRATION.md)
- Comprehensive demo page in WebAssembly sample at `/typescript-demo` showcasing all 5 interop services

## Components

### Router
Main routing component that:
- Applies transitions
- Handles lazy loading
- Executes route guards
- Matches current URL to route configuration
- Renders matched components with optional layouts
- Manages layout wrapping via DefaultLayout parameter
- Supports `OnNavigateAsync` callback for WASM assembly lazy loading
- Supports `AdditionalAssemblies` for dynamically loaded assembly route discovery

### RouterLink
Navigation link component that:
- Supports exact matching
- Applies active class to current route
- Renders anchor tags with proper href
- Handles click events for SPA navigation

### RouterOutlet
Nested route renderer that:
- Supports transitions
- Maintains route hierarchy
- Displays child route components

## Services

### RouteMatcherService (IRouteMatcherService)
- Registered as scoped service
- Route priority and exact match handling
- Dynamic parameter extraction from URL paths
- Query string parsing and parameter extraction
- Pattern matching for routes with wildcard support
- Nested route matching with parent-child relationships

### RouterStateService
- Query parameter retrieval
- Registered as scoped service
- Component lifecycle integration
- Event notifications for route changes
- Current route tracking with RouteMatch
- Parameter access via GetParam(key) method
- Central state management for routing context

### RouterNavigationService
- Browser history integration
- Registered as scoped service
- Support for relative and absolute navigation
- Query parameter handling and URL building
- Programmatic navigation API with NavigateTo(path)
- Wrapper around NavigationManager for enhanced functionality
- Async methods for browser navigation: GoBackAsync(), GoForwardAsync(), CanGoBackAsync()

### NavigationInterop (Optional)
- Registered as scoped service via AddBlazouterInterop()
- Type-safe access to browser History API
- Navigate back and forward in history
- Check history length and navigation availability
- Manage history state (push, replace, get)
- Requires JavaScript module import for functionality
- Used by RouterNavigationService async navigation methods

### DocumentInterop (Optional)
- Registered as scoped service via AddBlazouterInterop()
- Type-safe document and DOM manipulation
- Dynamic page title updates
- Meta tag management (description, keywords, Open Graph)
- Canonical URL support for SEO
- Scroll management (scroll to top, scroll to element)
- Element focus management
- Requires JavaScript module import for functionality

### RouteAttributeDiscoveryService
- Supports all 8 route attributes
- Reflection-based with trimming warnings
- Converts attributes to RouteConfig objects
- Static service for route discovery at startup
- Discovers components with [Route] attribute
- Assembly scanning for attribute-based routes

### IRouterErrorHandler / DefaultRouterErrorHandler
- Error type categorization
- Retry mechanism support
- Default handler with console logging
- Error context with detailed information
- Error handling interface for custom implementations
- Registered as scoped service via AddBlazouterErrorHandler<T>()

## Utilities

### QueryStringBuilder
- Fluent API for building query strings
- Support for nullable variants of all types
- `AddIf()` for conditional parameter addition
- `AddRange()` for multiple values per parameter
- `Build()` returns query string without `?` prefix
- `Remove()` and `Clear()` for parameter removal
- 15 type-safe `Set()` methods for replacing values
- Automatic URL encoding with Uri.EscapeDataString
- 15 type-safe `Add()` methods for appending values
- `BuildWithPrefix()` returns query string with `?` prefix
- `ToDictionary()` converts to Dictionary<string, string>
- Support for string, int, long, decimal, double, bool, DateTime, Guid, enum types

## Extension Methods

### RouterStateExtensions
- `HasQuery()` - Check if parameter exists
- `GetQueryBoolOrNull()` - Parse nullable boolean
- All methods use InvariantCulture for consistent parsing
- `GetAllQueryParams()` - Get all parameters as dictionary
- `GetQueryArray()` - Get multiple values for same parameter
- `GetQueryGuid()`, `GetQueryGuidOrNull()` - Parse Guid parameters
- `GetQueryEnum<T>()`, `GetQueryEnumOrNull<T>()` - Parse enum parameters
- `GetQueryDateTime()`, `GetQueryDateTimeOrNull()` - Parse DateTime in ISO 8601 format
- `GetQueryBool()` - Parse boolean with support for "true", "false", "1", "0", "yes", "no", "on", "off"
- `GetQueryInt()`, `GetQueryLong()`, `GetQueryDecimal()`, `GetQueryDouble()` - Parse numeric parameters with defaults
- `GetQueryIntOrNull()`, `GetQueryLongOrNull()`, `GetQueryDecimalOrNull()`, `GetQueryDoubleOrNull()` - Parse nullable numeric parameters

### RouterNavigationExtensions
- `NavigateToWithClearedQuery()` - Clear all query parameters
- `NavigateToWithQuery()` - Navigate with fluent query builder
- `NavigateToWithReplacedQuery()` - Replace all query parameters
- `NavigateToWithRemovedQuery()` - Remove specific parameters by key
- `NavigateToWithSingleQuery()` - Convenience method for single string parameter
- `NavigateToWithSingleQuery<T>()` - Convenience method for single typed parameter
- `NavigateToWithUpdatedQuery()` - Update specific parameters while preserving others

## Configuration

### RouteConfig Properties
- `Exact`: Exact path matching flag - When true, requires exact URL match
- `Data`: Custom data dictionary - Arbitrary metadata passed to components
- `Transition`: Animation type - One of 14 built-in RouteTransition enum values
- `HasExplicitLayout`: Internal flag - Tracks if Layout was explicitly set (even if null)
- `Layout`: Layout component type - Overrides DefaultLayout, set to null for no layout
- `Path`: Route pattern (e.g., "/users/:id") - Supports dynamic parameters with `:` prefix
- `Title`: Route title for metadata - Used for page title, breadcrumbs, or navigation labels
- `Children`: Nested route configurations - List of child RouteConfig for hierarchical routing
- `RedirectTo`: Redirect path - Automatically redirects to specified path when route matches
- `Component`: Component type to render - Direct component reference for immediate loading
- `Guards`: List of route guard types - Array of IRouteGuard implementations for access control
- `ComponentLoader`: Async component loader for lazy loading - Returns `Task<Type>` for on-demand loading
- `Middleware`: List of route middleware types - Array of IRouteMiddleware implementations for cross-cutting concerns

## Package-Specific Features

### Blazouter (Core)
- Lazy loading support with ComponentLoader
- 14 transition types with RouteTransition enum
- Comprehensive XML documentation for IntelliSense
- Layout system support (DefaultLayout, per-route Layout)
- Fluent query string building with automatic URL encoding
- CSS animations included (blazouter.css, blazouter.min.css)
- All routing components (Router, RouterLink, RouterOutlet)
- Multi-framework support (net6.0, net7.0, net8.0, net9.0, net10.0)
- Type-safe query parameter parsing with 15+ extension methods
- Navigation services (RouterNavigationService, RouterStateService)
- Route guards interface (IRouteGuard, AuthGuard base implementation)
- Route configuration and matching (RouteConfig, RouteMatcherService)
- Attribute-based routing (RouteAttributeDiscoveryService, 8 route attributes)
- Error handling (IRouterErrorHandler, DefaultRouterErrorHandler, RouterErrorContext)
- Query string utilities (QueryStringBuilder, RouterStateExtensions, RouterNavigationExtensions)

### Blazouter.Server
- Server-side route optimization
- Framework references for ASP.NET Core
- Enhanced performance for server scenarios
- `AddBlazouterSupport()` extension for endpoint mapping

### Blazouter.WebAssembly
- SPA navigation support
- Browser platform support
- Client-side route optimization
- Browser-specific enhancements
- Reduced bundle size optimizations

### Blazouter.Hybrid
- Native navigation integration
- MAUI-specific routing features
- Native mobile/desktop integration
- Platform-specific optimizations for iOS, Android, macOS, Windows

## Sample Application

The sample application demonstrates:
1. Protected page with route guard
2. Home page with feature overview
3. About page with library information
4. Lazy loaded page with component loader
5. Users page with nested routing (list and detail views)
6. Navigation page with comprehensive query string utilities showcase
   - Type-safe query parameter parsing examples
   - Traditional vs fluent query string building comparison
   - Query parameter manipulation (update, remove, clear)
   - Interactive buttons demonstrating all query string features
7. Lazy-loaded RCL assembly pages (Support, Help) via `OnNavigateAsync` and `AdditionalAssemblies`
   - Demonstrates loading `Blazouter.LazyModule.Sample` assembly on demand
   - Uses `LazyAssemblyLoader` with `<BlazorWebAssemblyLazyLoad>` configuration

All features are interactive and can be tested by navigating through the application.

## Comparison with Traditional Blazor Routing

| Feature | Traditional Blazor | Blazouter |
|---------|-------------------|-----------|
| Transitions | ❌ None | ✅ 14 built-in transitions |
| Active Links | ⚠️ Manual | ✅ Automatic with RouterLink |
| Lazy Loading | ⚠️ Limited | ✅ ComponentLoader + WASM RCL assembly lazy loading |
| Route Guards | ❌ Manual | ✅ Built-in IRouteGuard interface |
| Layout System | ⚠️ Static @layout | ✅ Dynamic per-route with priority |
| Error Handling | ❌ Manual | ✅ Built-in with IRouterErrorHandler |
| Nested Routes | ❌ Limited | ✅ Unlimited nesting depth |
| Attribute Routes | ❌ @page only | ✅ 8 attribute types with full config |
| Parameter Access | ✅ Basic | ✅ Enhanced with RouterStateService |
| Programmatic Nav | ✅ Basic | ✅ Enhanced with RouterNavigationService |
| Query String Helpers | ❌ Manual parsing | ✅ Type-safe fluent API with 15+ methods |