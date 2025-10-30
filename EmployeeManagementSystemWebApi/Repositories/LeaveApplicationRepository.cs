using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemWebApi.Repositories;

public class LeaveApplicationRepository : Repository<LeaveApplication>, ILeaveApplicationRepository
{
    private EmployeeDbContext AppDbContext => (EmployeeDbContext)_context;
    public LeaveApplicationRepository(EmployeeDbContext context) : base(context)
    {
    }
    public async Task<IEnumerable<LeaveApplication>> GetLeaveApplicationsByEmployeeIdAsync(int employeeId)
    {
        return await AppDbContext.LeaveApplications
            .Where(la => la.EmployeeId == employeeId)
            .OrderByDescending(la => la.StartDate)
            .ToListAsync();
    }
    public async Task<IEnumerable<LeaveApplication>> GetPendingAsync()
    {
        return await AppDbContext.LeaveApplications
            .Where(la => la.Status == Enums.LeaveStatus.Pending)
            .OrderByDescending(la => la.StartDate)
            .ToListAsync();
    }
}