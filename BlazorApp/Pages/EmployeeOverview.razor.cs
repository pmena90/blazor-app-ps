using BlazorApp.Services;
using BlazorApp.Shared.Domain;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Pages
{
    public partial class EmployeeOverview
    {
        [Inject]
        public IEmployeeDataService? EmployeeDataService { get; set; }

        public List<Employee>? Employees
        { get; set; } = default!;

        private Employee? _selectedEmployee;

        private string Title = "Employee overview";

        protected override async Task OnInitializedAsync()
        {
            Employees = (await EmployeeDataService.GetEmployees()).ToList();
        }

        public void ShowQuickViewPopup(Employee employee)
        {
            _selectedEmployee = employee;
        }
    }
}