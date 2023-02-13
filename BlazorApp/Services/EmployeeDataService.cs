using BlazorApp.Models;
using BlazorApp.Shared.Domain;
using Blazored.LocalStorage;
using System.Text;
using System.Text.Json;

namespace BlazorApp.Services
{
    public class EmployeeDataService : IEmployeeDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorageService;

        public EmployeeDataService(HttpClient httpClient, ILocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<Employee> AddEmployee(Employee employee)
        {
            var employeeJson = new StringContent(JsonSerializer.Serialize(employee),
                Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/employee", employeeJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<Employee>(await response.Content.ReadAsStreamAsync());
            }
            else
            {
                var response1 = await response.Content.ReadAsStreamAsync();
                var response2 = response1;
            }

            return null;
        }

        public async Task UpdateEmployee(Employee employee)
        {
            var employeeJson = new StringContent(JsonSerializer.Serialize(employee),
                Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/employee", employeeJson);
        }

        public async Task DeleteEmployee(int id)
        {
            await _httpClient.DeleteAsync("api/employee/" + id.ToString());
        }

        public async Task<Employee> GetEmployee(int id)
        {
            return await JsonSerializer.DeserializeAsync<Employee>
                 (await _httpClient.GetStreamAsync($"/api/employee/{id}"),
                 new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<Employee>> GetEmployees(bool refreshRequierd = false)
        {
            if (refreshRequierd)
            {
                bool employeeExpirationExists = await _localStorageService.ContainKeyAsync(LocalStorageConstants.EmployeesListExpirationKey);
                if (employeeExpirationExists)
                {
                    DateTime employeeListExpiration = await _localStorageService
                        .GetItemAsync<DateTime>(LocalStorageConstants.EmployeesListExpirationKey);
                    if (employeeListExpiration > DateTime.Now)//get from local storage
                    {
                        bool employeListExist = await _localStorageService
                            .ContainKeyAsync(LocalStorageConstants.EmployeesListKey);
                        if (employeListExist)
                        {
                            return await _localStorageService.GetItemAsync<List<Employee>>(LocalStorageConstants.EmployeesListKey);
                        }
                    }
                }
            }

            var list = await GetEmployeeListFromDatabaseAsync();

            await _localStorageService.SetItemAsync(LocalStorageConstants.EmployeesListKey, list);
            await _localStorageService.SetItemAsync(LocalStorageConstants.EmployeesListExpirationKey, DateTime.Now.AddMinutes(1));

            return list;
        }

        private async Task<IEnumerable<Employee>> GetEmployeeListFromDatabaseAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Employee>>
                        (await _httpClient.GetStreamAsync($"/api/employee"),
                        new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}