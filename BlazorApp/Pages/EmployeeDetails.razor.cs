using BlazorApp.Services;
using BlazorApp.Shared.Domain;
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

        protected override async Task OnInitializedAsync()
        {
            Employee = await EmployeeDataService.GetEmployee(int.Parse(EmployeeId));
        }

        public void NavigateBack()
        {
            NavigationManager.NavigateTo("employeeoverview");
        }
    }
}