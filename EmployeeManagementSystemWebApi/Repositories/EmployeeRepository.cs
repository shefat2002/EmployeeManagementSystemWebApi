using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemWebApi.Repositories;

public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
{
    private EmployeeDbContext AppDbContext => (EmployeeDbContext)_context;

    public EmployeeRepository(EmployeeDbContext context) : base(context)
    {
    }
    public async Task<Employee?> GetEmployeeWithDetailsAsync(int id)
    {
        return await AppDbContext.Employees
            .Include(e => e.Department)
            .Include(e => e.JobRole)
            .Include(e => e.EmployeeSalary)
            .FirstOrDefaultAsync(e => e.Id == id);
    }
    public async Task<IEnumerable<Employee>> GetAllEmployeesWithDetailsAsync()
    {
        return await AppDbContext.Employees
            .Include(e => e.Department)
            .Include(e => e.JobRole)
            .Include(e => e.EmployeeSalary)
            .ToListAsync();
    }
}
