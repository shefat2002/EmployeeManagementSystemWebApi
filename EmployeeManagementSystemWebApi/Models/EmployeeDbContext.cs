using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystemWebApi.Models;

namespace EmployeeManagementSystemWebApi.Models;

public class EmployeeDbContext : DbContext
{
   public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
   {
   }
   public DbSet<Employee> Employees { get; set; } = null!;
   public DbSet<Department> Departments { get; set; } = null!;

public DbSet<EmployeeManagementSystemWebApi.Models.JobRole> JobRole { get; set; } = default!;
}
