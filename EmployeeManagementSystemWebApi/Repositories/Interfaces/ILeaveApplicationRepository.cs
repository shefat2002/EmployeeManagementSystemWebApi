using EmployeeManagementSystemWebApi.Models;

namespace EmployeeManagementSystemWebApi.Repositories.Interfaces;

public interface ILeaveApplicationRepository : IRepository<LeaveApplication>
{
    Task<IEnumerable<LeaveApplication>> GetLeaveApplicationsByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<LeaveApplication>> GetPendingAsync();
}
