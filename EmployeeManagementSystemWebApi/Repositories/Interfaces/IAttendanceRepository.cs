using EmployeeManagementSystemWebApi.Models;

namespace EmployeeManagementSystemWebApi.Repositories.Interfaces;

public interface IAttendanceRepository : IRepository<Attendance>
{
    Task<Attendance?> GetAttendanceByEmployeeIdAndDateAsync(int employeeId, DateTime date);
    Task<IEnumerable<Attendance>> GetAttendanceByEmployeeIdAndMonthAsync(int employeeId, int month, int year);
    Task<IEnumerable<Attendance>> GetAttendanceByEmployeeIdAndDateRangeAsync(int employeeId, DateTime startDate, DateTime endDate);
}
