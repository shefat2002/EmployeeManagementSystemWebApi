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
    public DbSet<JobRole> JobRoles { get; set; } = default!;
    public DbSet<EmployeeSalary> EmployeeSalaries { get; set; } = default!;
    public DbSet<Attendance> Attendances { get; set; } = default!;
    public DbSet<Payroll> Payrolls { get; set; } = default!;
    public DbSet<LeaveApplication> LeaveApplications { get; set; } = default!;

}
