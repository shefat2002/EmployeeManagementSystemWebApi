using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Repositories.Interfaces;

namespace EmployeeManagementSystemWebApi.Repositories;

public class JobRoleRepository : Repository<JobRole>, IJobRoleRepository
{
    private EmployeeDbContext AppDbContext => (EmployeeDbContext)_context;
    public JobRoleRepository(EmployeeDbContext context) : base(context)
    {
    }
}