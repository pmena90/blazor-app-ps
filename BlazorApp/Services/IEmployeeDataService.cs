using BlazorApp.Shared.Domain;

namespace BlazorApp.Services
{
    public interface IEmployeeDataService
    {
        Task<IEnumerable<Employee>> GetEmployees();

        Task<Employee> GetEmployee(int id);

        Task<Employee> AddEmployee(Employee employee);

        Task UpdateEmployee(Employee employee);

        Task DeleteEmployee(int id);
    }
}