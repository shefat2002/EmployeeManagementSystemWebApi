using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemWebApi.Repositories;

public class EmployeeSalaryRepository : Repository<EmployeeSalary>, IEmployeeSalaryRepository
{
    private EmployeeDbContext AppDbContext => (EmployeeDbContext)_context;
    public EmployeeSalaryRepository(EmployeeDbContext context) : base(context)
    {
    }
    public async Task<EmployeeSalary?> GetByEmployeeIdAsync(int employeeId)
    {
        return await AppDbContext.EmployeeSalaries
            .Include(es => es.Employee)
            .FirstOrDefaultAsync(es => es.EmployeeId == employeeId);
    }
}
