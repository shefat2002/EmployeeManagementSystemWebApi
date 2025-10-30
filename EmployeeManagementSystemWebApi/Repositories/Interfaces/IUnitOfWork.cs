namespace EmployeeManagementSystemWebApi.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IEmployeeRepository Employees { get; }
    IDepartmentRepository Departments { get; }
    IJobRoleRepository JobRoles { get; }
    IEmployeeSalaryRepository EmployeeSalaries { get; }
    IPayrollRepository Payrolls { get; }
    IAttendanceRepository Attendances { get; }
    ILeaveApplicationRepository LeaveApplications { get; }

    Task<int> SaveChangesAsync();
}