using EmployeeManagementSystemWebApi.Models;

namespace EmployeeManagementSystemWebApi.Repositories.Interfaces;

public interface IEmployeeSalaryRepository : IRepository<EmployeeSalary>
{
    Task<EmployeeSalary?> GetByEmployeeIdAsync(int employeeId);
}
