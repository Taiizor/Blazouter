using Blazouter.Attributes;
using Blazouter.Enums;
using Microsoft.AspNetCore.Components;
using RouteAttribute = Blazouter.Attributes.RouteAttribute;

namespace Blazouter.LazyModule.Sample.Pages
{
    [Route("/support")]
    [RouteTitle("Support")]
    [RouteTransition(RouteTransition.Fade)]
    public partial class SupportPage : ComponentBase { }
}