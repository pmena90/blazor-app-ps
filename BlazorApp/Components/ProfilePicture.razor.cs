using Microsoft.AspNetCore.Components;

namespace BlazorApp.Components
{
    public partial class ProfilePicture
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }
    }
}