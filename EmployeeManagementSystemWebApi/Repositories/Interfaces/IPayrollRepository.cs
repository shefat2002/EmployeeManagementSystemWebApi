using EmployeeManagementSystemWebApi.Controllers;
using EmployeeManagementSystemWebApi.Models;

namespace EmployeeManagementSystemWebApi.Repositories.Interfaces;

public interface IPayrollRepository : IRepository<Payroll>
{
    Task<IEnumerable<Payroll>> GetPayrollsByEmployeeIdAsync(int employeeId);
    Task<Payroll?> GetPayrollByEmployeeIdAndMonthAsync(int employeeId, int month, int year);
}
