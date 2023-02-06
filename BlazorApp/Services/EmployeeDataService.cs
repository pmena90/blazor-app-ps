using BlazorApp.Shared.Domain;
using System.Text.Json;

namespace BlazorApp.Services
{
    public class EmployeeDataService : IEmployeeDataService
    {
        private readonly HttpClient _httpClient;

        public EmployeeDataService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<Employee> AddEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }

        public Task DeleteEmployee(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Employee> GetEmployee(int id)
        {
            return await JsonSerializer.DeserializeAsync<Employee>
                 (await _httpClient.GetStreamAsync($"/api/employee/{id}"),
                 new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<Employee>>
                (await _httpClient.GetStreamAsync($"/api/employee"), new
                JsonSerializerOptions()
                { PropertyNameCaseInsensitive = true });
        }

        public Task UpdateEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}