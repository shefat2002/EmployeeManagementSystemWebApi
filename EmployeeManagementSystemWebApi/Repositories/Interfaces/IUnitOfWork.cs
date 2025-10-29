namespace EmployeeManagementSystemWebApi.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IEmployeeRepository Employees { get; }
    IDepartmentRepository Departments { get; }
    IJobRoleRepository JobRoles { get; }

    Task<int> SaveChangesAsync();
}