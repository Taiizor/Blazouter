using Blazouter.Attributes;
using Blazouter.Enums;

namespace Blazouter.LazyModule.Sample.Pages
{
    [Route("/support")]
    [RouteTitle("Support")]
    [RouteTransition(RouteTransition.Fade)]
    public partial class SupportPage
    {
        // This page is intentionally left simple to demonstrate lazy loading of routes from an RCL assembly.
        // No additional code is needed here since the routing and assembly loading are handled by Blazouter.
    }
}