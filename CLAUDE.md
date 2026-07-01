# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Blazouter is a React Router-like routing library for Blazor applications. It provides nested routes, route guards, lazy loading, transitions, middleware, and programmatic navigation across all Blazor hosting models.

## Build & Development Commands

```bash
# Restore dependencies
dotnet restore Blazouter.sln

# Build entire solution
dotnet build Blazouter.sln

# Build in Release mode (as CI does)
dotnet build Blazouter.sln -c Release -p:UseSharedCompilation=false --no-restore

# Build a specific project
dotnet build src/Blazouter/Blazouter.csproj

# Run sample apps
dotnet run --project samples/Blazouter.Server.Sample/Blazouter.Server.Sample.csproj
dotnet run --project samples/Blazouter.WebAssembly.Sample/Blazouter.WebAssembly.Sample.csproj
dotnet run --project samples/Blazouter.Web.Sample/Blazouter.Web.Sample/Blazouter.Web.Sample.csproj
```

There are no tests in this repository. CI only runs `dotnet build` in Release mode on Windows.

## SDK & Framework Requirements

- `global.json` pins to .NET SDK 11.0.x (preview, with rollForward: latestMajor)
- Core library (`Blazouter`) multi-targets: net6.0, net7.0, net8.0, net9.0, net10.0
- `Blazouter.Server` and `Blazouter.Web` target: net8.0, net9.0, net10.0
- `Blazouter.WebAssembly` targets: net6.0, net7.0, net8.0, net9.0, net10.0
- `Blazouter.Hybrid` targets: net10.0 only (iOS, Android, macOS, Windows via MAUI)
- All projects use `LangVersion=preview` and `Nullable=enable`

## Architecture

### Package Structure

```
src/
├── Blazouter/              # Core library - all routing logic lives here
├── Blazouter.Server/       # Server extension: AddBlazouterSupport() for MapRazorComponents
├── Blazouter.WebAssembly/  # WebAssembly extension (thin wrapper)
├── Blazouter.Hybrid/       # MAUI extension: AddBlazouterSupport() for MauiAppBuilder
└── Blazouter.Web/          # DEPRECATED - use Server + WebAssembly instead
```

Platform packages (Server, WebAssembly, Hybrid, Web) are thin wrappers that reference the core `Blazouter` project. All meaningful routing logic is in the core package.

### Core Library Organization (`src/Blazouter/`)

- **Components/**: `Router.razor`, `RouterOutlet.razor`, `RouterLink.razor` - the Blazor components users place in their markup. Each has a `.razor.cs` code-behind.
- **Services/**: `RouterStateService` (singleton, global state), `RouterNavigationService` (scoped, per-circuit navigation), `RouteMatcherService` (route matching), `CachedRouteMatcherService` (decorator with LRU cache), `RouteCacheService`, `RouteAttributeDiscoveryService`
- **Interfaces/**: `IRouteGuard`, `IRouteMiddleware`, `IRouteMatcherService`, `IRouterErrorHandler`, `IRouteCacheService`
- **Models/**: `RouteConfig`, `RouteMatch`, `CacheOptions`, `CacheStatistics`, `RouterErrorContext`, `RouterErrorInfo`, `RouteMiddlewareContext`
- **Attributes/**: 10 attribute types for declarative routing (`[Route]`, `[RouteGuard]`, `[RouteTitle]`, `[RouteTransition]`, `[RouteMiddleware]`, `[RouteLayout]`, `[RouteData]`, `[RouteExact]`, `[RouteRedirect]`, `[RouteCache]`)
- **Enums/**: `RouteTransition` (15 transition types), `RouterErrorType`
- **Extensions/**: `ServiceCollectionExtensions` (DI registration: `AddBlazouter()`, `AddBlazouterInterop()`, `AddBlazouterErrorHandler<T>()`), plus extensions for query strings, navigation, transitions, and cache
- **Interops/**: JS interop wrappers - `NavigationInterop`, `DocumentInterop`, `StorageInterop`, `ViewportInterop`, `ClipboardInterop`
- **Utilities/**: `QueryStringBuilder` - fluent API for query string construction
- **wwwroot/**: CSS (`blazouter.css`, `blazouter.min.css`) and compiled JS modules from TypeScript

### DI Registration Pattern

```csharp
// Core services
services.AddBlazouter();           // RouterStateService (singleton), RouterNavigationService (scoped), cached route matcher (singleton)
services.AddBlazouterInterop();    // Optional JS interop services (all scoped)
services.AddBlazouterErrorHandler<T>(); // Optional custom error handler (scoped)
```

### InternalsVisibleTo

The core `Blazouter` project exposes internals to `Blazouter.Web`, `Blazouter.Hybrid`, `Blazouter.Server`, and `Blazouter.WebAssembly`.

## Versioning

All packages share the same version, currently `1.0.13`. Version is set in each `.csproj` `<Version>` property. When bumping versions, update all 5 project files (`Blazouter`, `Blazouter.Server`, `Blazouter.WebAssembly`, `Blazouter.Hybrid`, `Blazouter.Web`).

## Code Style

- Indent: 4 spaces, CRLF line endings (enforced via `.editorconfig`)
- XML indent: 2 spaces
- PascalCase for types, properties, methods; interfaces prefixed with `I`
- `var` usage is set to `false` (explicit types preferred) though this is silent severity
- Block-scoped namespaces (`namespace X { }`)
- Expression-bodied members for accessors/properties/lambdas; not for constructors/methods
- XML documentation comments on all public APIs (`GenerateDocumentationFile=true`, warning 1591 suppressed)
- AOT/Trimming compatible: all projects enable trim analyzers, AOT analyzers, and are marked `IsAotCompatible`

## CI Suppressed Warnings

The CI build suppresses: `CS0067, CS0108, CS0109, CS0114, CS0169, CS0414, CS0618, CS0649, CS8632, CA1416, NU5104, NETSDK1138, SYSLIB0003`

## Key Design Decisions

- `Blazouter.Web` is deprecated; Blazor Web Apps should use `Blazouter.Server` + `Blazouter.WebAssembly`
- Route matching uses a decorator pattern: `CachedRouteMatcherService` wraps `RouteMatcherService` with LRU caching
- `RouterStateService` is singleton (shared state), `RouterNavigationService` is scoped (per-circuit in Server mode)
- The Router component auto-filters both middleware data and route data parameters to match only what the target component declares
