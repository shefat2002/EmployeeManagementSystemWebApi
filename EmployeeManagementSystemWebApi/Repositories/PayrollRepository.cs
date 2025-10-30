using EmployeeManagementSystemWebApi.Data;
using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemWebApi.Repositories;

public class PayrollRepository : Repository<Payroll>, IPayrollRepository
{
    private EmployeeDbContext AppDbContext => (EmployeeDbContext)_context;
    public PayrollRepository(EmployeeDbContext context) : base(context)
    {
    }
    public async Task<IEnumerable<Payroll>> GetPayrollsByEmployeeIdAsync(int employeeId)
    {
        return await AppDbContext.Payrolls
            .Where(p => p.EmployeeId == employeeId)
            .OrderByDescending(p => p.PayYear)
            .ThenByDescending(p => p.PayMonth)
            .ToListAsync();
    }
    public async Task<Payroll?> GetPayrollByEmployeeIdAndMonthAsync(int employeeId, int month, int year)
    {
        return await AppDbContext.Payrolls
            .Include(p => p.Employee)
            .FirstOrDefaultAsync(p => p.EmployeeId == employeeId && p.PayMonth == month && p.PayYear == year);
    }
}