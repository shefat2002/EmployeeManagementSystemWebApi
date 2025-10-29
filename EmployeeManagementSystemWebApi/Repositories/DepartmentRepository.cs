using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Repositories.Interfaces;

namespace EmployeeManagementSystemWebApi.Repositories;

public class DepartmentRepository : Repository<Department>, IDepartmentRepository
{
    private EmployeeDbContext AppDbContext => (EmployeeDbContext)_context;
    public DepartmentRepository(EmployeeDbContext context) : base(context)
    {
    }
}