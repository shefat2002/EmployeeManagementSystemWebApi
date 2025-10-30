using EmployeeManagementSystemWebApi.Models;

namespace EmployeeManagementSystemWebApi.Repositories.Interfaces;

public interface ILeaveApplicationRepository : IRepository<LeaveApplication>
{
    Task<IEnumerable<LeaveApplication>> GetLeaveApplicationsByEmployeeIdAsync(int employeeId);
    Task<IEnumerable<LeaveApplication>> GetPendingAsync();
    Task<IEnumerable<LeaveApplication>> GetRejectedAsync();
    Task<IEnumerable<LeaveApplication>> GetApprovedAsync();
    Task<IEnumerable<LeaveApplication>> GetApprovedByMonthAsync(int employeeId, int month, int year);
}
