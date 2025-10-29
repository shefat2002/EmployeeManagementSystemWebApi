using EmployeeManagementSystemWebApi.Models;

namespace EmployeeManagementSystemWebApi.Repositories.Interfaces;

public interface IEmployeeRepository : IRepository<Employee>
{
    Task<IEnumerable<Employee>> GetAllEmployeesWithDetailsAsync();
    Task<Employee?> GetEmployeeWithDetailsAsync(int id);
}