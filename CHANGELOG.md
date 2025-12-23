# Changelog

All notable changes to Blazouter will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.0.13] - 2025-12-23

### Added

-   **TypeScript-Based JavaScript Interop Infrastructure**: Comprehensive type-safe browser API access
    -   TypeScript compilation pipeline with `.d.ts` generation for IntelliSense support
    -   Global namespace exposure pattern for C# interop: `window.blazouterNavigation`, `window.blazouterDocument`, `window.blazouterStorage`, `window.blazouterViewport`, `window.blazouterClipboard`
    -   Separate `Blazouter.TypeScript` project at `src/Blazouter.TypeScript/` for TypeScript source files
    -   NPM package with TypeScript compiler configuration (tsconfig.json)
    -   Compiled JavaScript modules with source maps and type definitions in `src/Blazouter/wwwroot/js/`
-   **NavigationInterop Service**: Browser history and navigation API wrapper
    -   History API: `GoBackAsync()`, `GoForwardAsync()`, `GoAsync()`, `GetHistoryLengthAsync()`, `GetHistoryStateAsync()`
    -   URL helpers: `GetCurrentUrlAsync()`, `GetPathnameAsync()`
    -   Hash navigation: `GetHashAsync()`, `SetHashAsync()`
    -   Query parameters: `GetQueryStringAsync()`, `GetQueryParamAsync()`, `GetAllQueryParamsAsync()`
    -   Page control: `ReloadAsync()`
    -   Integration with `RouterNavigationService` providing `GoBackAsync()`, `GoForwardAsync()`, `CanGoBackAsync()`
-   **DocumentInterop Service**: Document manipulation and SEO support
    -   Title management: `SetTitleAsync()`, `GetTitleAsync()`
    -   Meta tags: `SetMetaTagAsync()`, `GetMetaTagAsync()`, `RemoveMetaTagAsync()`
    -   Open Graph support: `SetOpenGraphTagAsync()`
    -   Canonical URLs: `SetCanonicalUrlAsync()`, `GetCanonicalUrlAsync()`
    -   Scroll control: `ScrollToTopAsync()`, `ScrollToBottomAsync()`, `ScrollToElementAsync()`, `GetScrollPositionAsync()`, `SetScrollPositionAsync()`
    -   Element management: `FocusElementAsync()`, `IsElementVisibleAsync()`
    -   CSS classes: `AddClassAsync()`, `RemoveClassAsync()`, `ToggleClassAsync()`
    -   Document state: `GetReadyStateAsync()`, `IsDocumentReadyAsync()`
-   **StorageInterop Service**: Type-safe localStorage and sessionStorage access
    -   LocalStorage operations: `SetLocalStorageAsync<T>()`, `GetLocalStorageAsync<T>()`, `RemoveLocalStorageAsync()`, `ClearLocalStorageAsync()`, `GetLocalStorageKeysAsync()`, `HasLocalStorageAsync()`
    -   SessionStorage operations: `SetSessionStorageAsync<T>()`, `GetSessionStorageAsync<T>()`, `RemoveSessionStorageAsync()`, `ClearSessionStorageAsync()`, `GetSessionStorageKeysAsync()`, `HasSessionStorageAsync()`
    -   Generic type support with automatic JSON serialization/deserialization
-   **ViewportInterop Service**: Viewport, screen, and device detection
    -   Viewport dimensions: `GetViewportSizeAsync()` returning `Size` record with width and height
    -   Screen information: `GetScreenSizeAsync()`
    -   Device pixel ratio: `GetPixelRatioAsync()`
    -   Orientation: `IsPortraitAsync()`, `IsLandscapeAsync()`, `GetOrientationAsync()`
    -   Device type detection: `IsMobileAsync()`, `IsTabletAsync()`, `IsDesktopAsync()`, `GetDeviceTypeAsync()`
    -   Fullscreen API: `IsFullscreenAsync()`, `RequestFullscreenAsync()`, `ExitFullscreenAsync()`
-   **ClipboardInterop Service**: Clipboard operations with permission support
    -   Text operations: `CopyTextAsync()`, `ReadTextAsync()`
    -   Feature detection: `IsClipboardSupportedAsync()`
    -   Permission checks: `HasClipboardReadPermissionAsync()`, `HasClipboardWritePermissionAsync()`
    -   Fallback support for older browsers in `copyText()` implementation
-   **Service Organization**: All interop services in `Blazouter.Interops` namespace
    -   Located at `src/Blazouter/Interops/` folder
    -   `AddBlazouterInterop()` extension method for DI registration (optional, non-breaking)
    -   Services injected as nullable dependencies for backward compatibility
-   **Documentation**: Comprehensive guides and API references

    -   `TYPESCRIPT_INTEGRATION.md`: Complete documentation with usage examples, API references, migration guide, and hosting-specific instructions for WebAssembly, Hybrid, and Server models
    -   `FEATURES.md`: Feature #13 with detailed descriptions of all 5 interop services and their capabilities
    -   `README.md`: Installation instructions with JavaScript module import requirement, updated Project Structure
    -   `src/Blazouter/README.md` (NuGet): Concise TypeScript Integration section for package consumers
    -   All documentation includes required module import: `<script type="module" src="_content/Blazouter/js/index.js"></script>`

-   **Route Caching System**: Sophisticated caching layer for optimal navigation performance

    -   `IRouteCacheService` interface defining cache operations
    -   `RouteCacheService` implementation with thread-safe concurrent operations
    -   `CachedRouteMatcherService` decorator pattern wrapping `RouteMatcherService` with caching
    -   `CacheOptions` model for configuring cache behavior:
        -   `EnableRouteMatchCache`: Enable/disable route match caching (default: true)
        -   `EnableComponentTypeCache`: Enable/disable component type caching (default: true)
        -   `MaxRouteMatchCacheSize`: Maximum cache entries (default: 100)
        -   `MaxComponentTypeCacheSize`: Maximum component cache entries (default: 50)
        -   `RouteMatchCacheTTLSeconds`: Time-to-live for cache entries (default: 0 = no expiration)
        -   `EnableStatistics`: Enable cache performance tracking
    -   `CacheStatistics` model providing performance metrics: TotalRequests, CacheHits, CacheMisses, HitRate
    -   LRU (Least Recently Used) eviction policy for route matches
    -   FIFO (First In First Out) eviction policy for component types
    -   10-100x faster subsequent navigations through cache hits
    -   `RouteCacheExtensions` with `GetFormattedStatistics()` helper method

-   **Per-Route Cache Control**: Fine-grained cache control at route level

    -   `RouteConfig.EnableCache` property for per-route cache control (true/false/null)
    -   `[RouteCache]` attribute for declarative cache configuration on components
    -   Attribute coverage increased from 83% to 92% (11/12 RouteConfig properties)
    -   Sample `CacheExample.razor` page demonstrating attribute-based cache control

-   **Cache Documentation**: Comprehensive caching documentation
    -   `CACHING.md`: Complete documentation covering architecture, configuration, usage examples, best practices, and troubleshooting
    -   `ATTRIBUTE_ROUTING.md`: Updated with `[RouteCache]` attribute documentation and feature matrix

### Changed

-   Project structure updated to include:
    -   `src/Blazouter.TypeScript/` - Separate project for TypeScript source files
    -   `src/Blazouter/Interops/` - All interop service implementations
    -   `src/Blazouter/wwwroot/js/` - Compiled JavaScript modules (.js, .d.ts, .js.map files)
-   `RouterNavigationService` enhanced with navigation interop integration
-   Service registration extended via `AddBlazouterInterop()` method

### Technical Details

-   TypeScript source files: navigation.ts, document.ts, storage.ts, viewport.ts, clipboard.ts, index.ts
-   ES module format with explicit .js extensions for browser compatibility
-   TypeScript compilation generates declaration files for IntelliSense
-   JavaScript modules exposed via window object for C# JSRuntime interop
-   Optional module import - services gracefully handle missing JavaScript modules
-   Backward compatible - existing applications continue working without changes

## [1.0.12] - 2025-11-25

### Added

-   **Route Middleware System**: Comprehensive middleware pipeline for cross-cutting concerns
    -   `IRouteMiddleware` interface for implementing custom route middleware
    -   `RouteMiddlewareContext` model providing access to route match, path, and shared data dictionary
    -   `RouteMiddlewareAttribute` for declarative middleware configuration on components
    -   Pipeline execution pattern with `InvokeAsync(context, next)` method signature
    -   Support for before/after navigation logic by placing code around the `next()` delegate call
    -   Ability to short-circuit navigation by not calling `next()`
    -   `Abort` and `RedirectPath` properties for conditional navigation control
    -   `Data` dictionary for sharing state between middleware and components
    -   Middleware instantiation via dependency injection or `Activator.CreateInstance` fallback
    -   Execution order: Middleware runs before route guards in the navigation pipeline
    -   Multiple middleware support with ordered execution based on declaration order
    -   `RouterErrorType.MiddlewareExecution` error type for middleware-specific error handling
    -   Common use cases: logging, analytics, performance monitoring, data preloading, caching, feature flags, A/B testing, session management, error tracking

### Changed

-   Router component now executes middleware pipeline before guard evaluation
-   Enhanced `RouteConfig.Middleware` property with comprehensive XML documentation
-   Updated project structure to include Middleware directory

## [1.0.11] - 2025-11-24

### Added

-   **Query String Helpers and Utilities**: Comprehensive type-safe query string manipulation
    -   `QueryStringBuilder` class with fluent API for building query strings
    -   15 type-safe `Add()` method overloads supporting string, int, long, decimal, double, bool, DateTime, Guid, enum, and nullable variants
    -   15 type-safe `Set()` method overloads for replacing values (prevents duplicates)
    -   `RouterStateExtensions` with typed query parameter parsing methods: `GetQueryInt()`, `GetQueryBool()`, `GetQueryDateTime()`, etc.
    -   `GetAllQueryParams()` method to retrieve all query parameters as a dictionary
    -   `RouterNavigationExtensions` for enhanced navigation with query strings
    -   `NavigateToWithQuery()` for fluent query string building during navigation
    -   `NavigateToWithUpdatedQuery()` for updating specific parameters while preserving others
    -   `NavigateToWithRemovedQuery()` and `NavigateToWithClearedQuery()` for parameter removal
    -   `NavigateToWithSingleQuery()` convenience methods for single parameter navigation
    -   Automatic URL encoding via `Uri.EscapeDataString`
    -   Safe parsing with `TryParse` and default value support
    -   Comprehensive XML documentation with usage examples

### Changed

-   Updated README.md with Query String Utilities documentation and usage examples
-   Enhanced UserList sample component to display all query parameters using `GetAllQueryParams()`
-   Updated project structure documentation to reflect new Utilities directory

## [1.0.10] - 2025-11-19

### Added

-   **Enhanced Route Transitions**: Expanded from 4 to 14 built-in transition types
    -   Added: `None`, `Pop`, `Blur`, `Reveal`, `Rotate`, `Curtain`, `SlideFade`, `Spotlight`, `Swipe`, `Lift`
    -   All transitions are GPU-accelerated for smooth performance
    -   Respects `prefers-reduced-motion` accessibility preference
    -   Comprehensive XML documentation for each transition type with use cases and best practices
-   **Attribute-Based Routing System**: Complete declarative routing configuration
    -   8 attribute types: `[Route]`, `[RouteGuard]`, `[RouteTransition]`, `[RouteLayout]`, `[RouteTitle]`, `[RouteData]`, `[RouteRedirect]`, `[RouteExact]`
    -   `RouteAttributeDiscoveryService` for automatic route discovery via assembly scanning
    -   `AddAttributeRoutes()` extension method for mixing programmatic and attribute-based routes
    -   `FromAttributes()` static method for pure attribute-based configuration
    -   Full support for nested routes and complex configurations via attributes
    -   Detailed documentation in ATTRIBUTE_ROUTING.md
-   **Comprehensive Error Handling System**
    -   `IRouterErrorHandler` interface for custom error handling implementations
    -   `DefaultRouterErrorHandler` with built-in console logging
    -   `RouterErrorContext` providing detailed error information and context
    -   `RouterErrorType` enum with error categories: `ComponentLoadFailed`, `GuardRejected`, `NavigationFailed`, `InvalidRoute`
    -   `ErrorContent` RenderFragment parameter in Router component for custom error UI
    -   Retry mechanism support for failed operations
    -   Error event notifications throughout routing lifecycle
    -   `AddBlazouterErrorHandler<T>()` extension method for registering custom error handlers
-   **Enhanced Layout System**
    -   `DefaultLayout` parameter on Router component for application-wide layouts
    -   Per-route layout override via `RouteConfig.Layout` property
    -   Support for no-layout routes by explicitly setting `Layout = null`
    -   Layout priority system: Route.Layout > Router.DefaultLayout > No Layout
    -   `HasExplicitLayout` internal flag for proper layout resolution
    -   Seamless layout switching during navigation with state preservation
-   **Additional Components and Services**
    -   `BlankLayout` component for routes requiring no layout structure
    -   Enhanced `RouteMatcherService` with improved pattern matching
    -   `RouterStateService` with parameter change notifications
    -   Loading state support with `<Loading>` RenderFragment parameter in Router
    -   Component caching for lazy-loaded routes after first load

### Changed

-   **Documentation Overhaul**
    -   README.md: Updated to reflect all 14 transitions, added error handling section, enhanced project structure
    -   FEATURES.md: Expanded from 8 to 10 features with detailed implementation descriptions
    -   All sample applications updated to showcase 14 transitions instead of 4
    -   Added comprehensive comparison table showing Blazouter advantages over traditional Blazor routing
    -   Project Structure section now includes all folders: Attributes/, Extensions/, Resources/, Components/Layouts/
-   **Enhanced RouteConfig Model**
    -   Added `HasExplicitLayout` property for proper layout handling
    -   Improved XML documentation for all properties
    -   Better support for complex nested route scenarios
-   **Sample Applications**
    -   All 4 sample apps (WebAssembly, Server, Hybrid, Web) updated with transition demos
    -   Added demo pages for all 14 transition types with descriptions and code examples
    -   Enhanced Home.razor with accurate feature showcase
    -   Added Transitions.razor pages demonstrating all transition types

### Fixed

-   Layout handling when explicitly set to null vs. not set
-   Documentation inconsistencies between README.md and FEATURES.md
-   Sample applications showing outdated feature counts

## [1.0.9] - 2025-11-18

### Changed

-   Documentation links updated to reflect new repository location

## [1.0.6-1.0.8] - 2025-11-16 to 2025-11-17

### Note

-   Internal version increments for package stability and distribution
-   Minor bug fixes and improvements

### Deprecated

-   **Blazouter.Web package**: The `Blazouter.Web` package has been deprecated in favor of using `Blazouter.Server` and `Blazouter.WebAssembly` for Blazor Web Applications. This provides a clearer, more consistent package structure where:
    -   Server project uses: `Blazouter.Server`
    -   Client project uses: `Blazouter.WebAssembly`

### Changed

-   **Documentation updated**: All documentation (README.md, FEATURES.md, package READMEs) updated to reflect the deprecated status of `Blazouter.Web` and provide guidance on using `Blazouter.Server` + `Blazouter.WebAssembly` for Blazor Web Applications
-   **Sample application**: Blazor Web sample now uses `Blazouter.Server` for server project and `Blazouter.WebAssembly` for client project

## [1.0.5] - 2025-11-16

### Added

-   **Multiple NuGet Packages**: Split into 4 specialized packages for better modularity
    -   `Blazouter`: Core library (required for all hosting models)
    -   `Blazouter.Hybrid`: MAUI/Hybrid support for iOS, Android, macOS, and Windows
    -   `Blazouter.Server`: Server-side Blazor extensions with `AddBlazouterSupport()`
    -   `Blazouter.WebAssembly`: WebAssembly-specific optimizations
-   Professional NuGet package metadata for all packages
    -   Package-specific README files
    -   Optimized package icons
    -   Comprehensive descriptions and tags
    -   SourceLink support for debugging
    -   Symbol packages (.snupkg) for all packages
-   Multi-framework targeting:
    -   Core library: net6.0, net7.0, net8.0, net9.0, net10.0
    -   Server: net8.0, net9.0, net10.0
    -   Hybrid: net9.0, net10.0 (platform-specific)
    -   WebAssembly: net6.0, net7.0, net8.0, net9.0, net10.0

### Changed

-   Project structure reorganized into multiple packages
-   Documentation updated to reflect 4-package architecture
-   README files now specific to each package's features and use cases

## [1.0.2] - 2025-11-15

### Added

-   Type-safe `RouteTransition` enum for better IntelliSense and compile-time safety
-   Enhanced sample application with comprehensive demos:
    -   Professional home page with feature showcase
    -   Navigation demo page with programmatic navigation examples
    -   Transitions demo page showcasing all 4 transition types
-   Loading state for lazy-loaded routes to prevent 404 flash
-   XML documentation comments for better IDE support

### Fixed

-   Component re-rendering issue when navigating between routes with same component but different parameters
-   Lazy-loaded routes showing 404 page briefly before component loads
-   Visual Studio IntelliSense warnings for multiple RenderFragment parameters

### Changed

-   Improved sample application UI/UX with modern gradient design
-   Enhanced documentation with more examples and use cases

## [1.0.1] - 2025-11-14

### Added

-   Initial release of Blazouter
-   Core routing components (Router, RouterLink, RouterOutlet)
-   Nested routes support
-   Route guards for authentication/authorization
-   Lazy loading with ComponentLoader
-   Route transitions (fade, slide, slide-up, scale)
-   Programmatic navigation service
-   Dynamic route parameters
-   Query string support
-   Active link state management

### Documentation

-   Comprehensive README with examples
-   FEATURES.md detailing all capabilities
-   Sample application demonstrating key features

[1.0.13]: https://github.com/Taiizor/Blazouter/compare/v1.0.12...v1.0.13
[1.0.12]: https://github.com/Taiizor/Blazouter/compare/v1.0.11...v1.0.12
[1.0.11]: https://github.com/Taiizor/Blazouter/compare/v1.0.10...v1.0.11
[1.0.10]: https://github.com/Taiizor/Blazouter/compare/v1.0.9...v1.0.10
[1.0.9]: https://github.com/Taiizor/Blazouter/compare/v1.0.5...v1.0.9
[1.0.6-1.0.8]: https://github.com/Taiizor/Blazouter/compare/v1.0.5...v1.0.9
[1.0.5]: https://github.com/Taiizor/Blazouter/compare/v1.0.2...v1.0.5
[1.0.2]: https://github.com/Taiizor/Blazouter/compare/v1.0.1...v1.0.2
[1.0.1]: https://github.com/Taiizor/Blazouter/releases/tag/v1.0.1