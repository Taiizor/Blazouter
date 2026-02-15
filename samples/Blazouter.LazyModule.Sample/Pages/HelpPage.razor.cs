using Blazouter.Attributes;
using Blazouter.Enums;
using Microsoft.AspNetCore.Components;
using RouteAttribute = Blazouter.Attributes.RouteAttribute;

namespace Blazouter.LazyModule.Sample.Pages
{
    [Route("/help")]
    [RouteTitle("Help Center")]
    [RouteTransition(RouteTransition.Slide)]
    public partial class HelpPage : ComponentBase { }
}