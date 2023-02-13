using BlazorApp.Services;
using BlazorApp.Shared.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorApp.Pages
{
    public partial class EmployeeEdit
    {
        [Inject]
        public IEmployeeDataService? EmployeeDataService { get; set; }

        [Inject]
        public ICountryDataService? CountryDataService { get; set; }

        [Inject]
        public IJobCategoryDataService? JobCategoryDataService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string? EmployeeId { get; set; }

        public Employee Employee { get; set; } = new Employee();

        public List<Country> CountryList { get; set; } = new List<Country>();
        public List<JobCategory> JobCategories { get; set; } = new List<JobCategory>();

        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        protected override async Task OnInitializedAsync()
        {
            CountryList = (await CountryDataService.GetCountries()).ToList();
            JobCategories = (await JobCategoryDataService.GetJobCategories()).ToList();

            int.TryParse(EmployeeId, out var employeeId);

            if (employeeId == 0)
            {
                Employee = new Employee
                {
                    CountryId = 1,
                    ExitDate = DateTime.Now,
                    Gender = Gender.Male,
                    JobCategoryId = 1,
                    JoinedDate = DateTime.Now,
                    Latitude = 0,
                    Longitude = 0,
                };
            }
            else
            {
                Employee = await EmployeeDataService.GetEmployee(employeeId);
            }
        }

        public async Task HandleValidSubmit()
        {
            Saved = false;
            if (Employee.EmployeeId == 0)
            {
                // Image adding area
                if (selectedFile != null)
                {
                    var file = selectedFile;
                    var stream = file.OpenReadStream(3 * 1024 * 1024);
                    MemoryStream ms = new();
                    await stream.CopyToAsync(ms);
                    stream.Close();

                    Employee.ImageName = file.Name;
                    Employee.ImageContent = ms.ToArray();
                }

                var addedEmployee = await EmployeeDataService.AddEmployee(Employee);
                if (addedEmployee != null)
                {
                    StatusClass = "alert-success";
                    Message = "New employee added succesfuly.";
                    Saved = true;
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong adding the new employee. Please try again.";

                }
            }
            else
            {
                await EmployeeDataService.UpdateEmployee(Employee);
                StatusClass = "alert-success";
                Message = "Employee updated succesfuly.";
                Saved = true;
            }
        }

        public async Task HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = "There are some validation errors. Please fix them and try again.";
        }

        public async Task DeleteEmployee()
        {
            await EmployeeDataService.DeleteEmployee(int.Parse(EmployeeId));
            StatusClass = "alert-success";
            Message = "Employee deleted succesfuly.";
            Saved = true;
        }

        public void NavigateToOverview()
        {
            NavigationManager.NavigateTo("/employeeoverview");
        }

        private IBrowserFile selectedFile;

        public void OnInputFileChange(InputFileChangeEventArgs e)
        {
            selectedFile = e.File;
            StateHasChanged();
        }
    }
}