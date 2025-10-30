using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemWebApi.Repositories;

public class AttendanceRepository : Repository<Attendance>, IAttendanceRepository
{
    private EmployeeDbContext AppDbContext => (EmployeeDbContext)_context;
    public AttendanceRepository(EmployeeDbContext context) : base(context)
    {
    }
    public async Task<Attendance?> GetAttendanceByEmployeeIdAndDateAsync(int employeeId, DateTime date)
    {
        return await AppDbContext.Attendances
            .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date.Date == date.Date);
    }
    public async Task<IEnumerable<Attendance>> GetAttendanceByEmployeeIdAndMonthAsync(int employeeId, int month, int year)
    {
        return await AppDbContext.Attendances
            .Where(a => a.EmployeeId == employeeId && a.Date.Month == month && a.Date.Year == year)
            .ToListAsync();
    }
    public async Task<IEnumerable<Attendance>> GetAttendanceByEmployeeIdAndDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate)
    {
        return await AppDbContext.Attendances
            .Where(a => a.EmployeeId == employeeId && a.Date.Date >= startDate.Date && a.Date.Date <= endDate.Date)
            .OrderBy(a => a.Date)
            .ToListAsync();
    }
}