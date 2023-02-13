using BlazorApp.Shared.Domain;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Components
{
    public partial class EmployeeCard
    {
        [Parameter]
        public Employee Employee { get; set; } = default!;

        [Parameter]
        public EventCallback<Employee> EmployeeQuickViewClicked { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        protected override void OnInitialized()
        {
            if (string.IsNullOrEmpty(Employee.LastName))
            {
                throw new Exception("LastName is required.");
            }
        }

        public void NavigateToDetails(Employee employee)
        {
            NavigationManager.NavigateTo($"employeedetails/{employee.EmployeeId}");
        }

        public void NavigateToEdit(Employee employee)
        {
            NavigationManager.NavigateTo($"employeeedit/{employee.EmployeeId}");
        }
    }
}