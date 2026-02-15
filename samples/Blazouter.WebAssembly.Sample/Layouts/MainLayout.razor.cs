namespace Blazouter.WebAssembly.Sample.Layouts
{
    public partial class MainLayout
    {
        private bool _isMobileMenuOpen = false;

        // Features dropdown pages
        private static readonly string[] FeaturesPages = new[]
        {
            "/navigation", "/transitions", "/cache", "/middleware", "/typescript", "/attribute-examples"
        };

        // Examples dropdown pages
        private static readonly string[] ExamplesPages = new[]
        {
            "/users", "/protected", "/lazy", "/support", "/help", "/faq", "/error-example"
        };

        private void ToggleMobileMenu()
        {
            _isMobileMenuOpen = !_isMobileMenuOpen;
        }

        private void CloseMobileMenu()
        {
            _isMobileMenuOpen = false;
        }

        private string GetFeaturesDropdownClass()
        {
            string currentPath = RouterState.CurrentPath ?? "/";
            bool isActive = FeaturesPages.Any(p => currentPath.StartsWith(p, StringComparison.OrdinalIgnoreCase));

            string baseClass = "inline-flex items-center px-3 py-2 text-sm font-medium transition-colors";
            string activeClass = isActive
                ? "text-blue-600 dark:text-blue-400 border-b-2 border-blue-600 dark:border-blue-400"
                : "text-gray-700 dark:text-gray-300 hover:text-blue-600 dark:hover:text-blue-400";

            return $"{baseClass} {activeClass}";
        }

        private string GetExamplesDropdownClass()
        {
            string currentPath = RouterState.CurrentPath ?? "/";
            bool isActive = ExamplesPages.Any(p => currentPath.StartsWith(p, StringComparison.OrdinalIgnoreCase));

            string baseClass = "inline-flex items-center px-3 py-2 text-sm font-medium transition-colors";
            string activeClass = isActive
                ? "text-blue-600 dark:text-blue-400 border-b-2 border-blue-600 dark:border-blue-400"
                : "text-gray-700 dark:text-gray-300 hover:text-blue-600 dark:hover:text-blue-400";

            return $"{baseClass} {activeClass}";
        }
    }
}