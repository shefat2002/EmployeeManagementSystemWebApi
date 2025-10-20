using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemWebApi.Models;

public class EmployeeDbContext : DbContext
{
   public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
   {
   }
   public DbSet<Employee> Employees { get; set; } = null!;
   public DbSet<Department> Departments { get; set; } = null!;
   public DbSet<Project> Projects { get; set; } = null!;
   public DbSet<EmployeeProjectAssignment> EmployeeProjectAssignments { get; set; } = null!;
}
