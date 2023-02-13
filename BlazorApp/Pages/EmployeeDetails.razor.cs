using BlazorApp.Services;
using BlazorApp.Shared.Domain;
using BlazorApp.Shared.Model;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Pages
{
    public partial class EmployeeDetails
    {
        [Inject]
        public IEmployeeDataService? EmployeeDataService { get; set; }

        [Parameter]
        public string EmployeeId { get; set; }

        public Employee Employee { get; set; } = new Employee();

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        public List<Marker> MapMarkers { get; set; } = new List<Marker>();

        protected override async Task OnInitializedAsync()
        {
            Employee = await EmployeeDataService.GetEmployee(int.Parse(EmployeeId));

            if (Employee.Longitude.HasValue && Employee.Latitude.HasValue)
            {
                MapMarkers = new List<Marker>
                {
                    new Marker
                    {
                        Description = $"{Employee.FirstName} {Employee.LastName}",
                        ShowPopup = false,
                        X = Employee.Longitude.Value,
                        Y = Employee.Latitude.Value
                    }
                };
            }
        }

        public void NavigateBack()
        {
            NavigationManager.NavigateTo("employeeoverview");
        }
    }
}