using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Repositories.Interfaces;

namespace EmployeeManagementSystemWebApi.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly EmployeeDbContext _context;
    public IEmployeeRepository Employees { get; private set; }
    public IDepartmentRepository Departments { get; private set; }
    public IJobRoleRepository JobRoles { get; private set; }

    public UnitOfWork(EmployeeDbContext context)
    {
        _context = context;
        Employees = new EmployeeRepository(_context);
        Departments = new DepartmentRepository(_context);
        JobRoles = new JobRoleRepository(_context);
    }



    // Save Changes
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    // Dispose
    private bool _disposed = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }
    public void Dispose()
    {
        Dispose(true);  // Dispose managed resources
        GC.SuppressFinalize(this); // Suppress finalization. Details: https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose
    }
}
