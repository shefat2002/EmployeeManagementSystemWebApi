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
    public DbSet<JobRole> JobRoles { get; set; } = null!;
    public DbSet<EmployeeSalary> EmployeeSalaries { get; set; } = null!;
    public DbSet<Attendance> Attendances { get; set; } = null!;
    public DbSet<Payroll> Payrolls { get; set; } = null!;
    public DbSet<LeaveApplication> LeaveApplications { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Employee>() // Many-to-One relationship with Department
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId);
        modelBuilder.Entity<Employee>() // Many-to-One relationship with JobRole
            .HasOne(e => e.JobRole)
            .WithMany(j => j.Employees)
            .HasForeignKey(e => e.JobRoleId);

        modelBuilder.Entity<EmployeeSalary>() // One-to-One relationship with Employee
            .HasOne(es => es.Employee)
            .WithOne()
            .HasForeignKey<EmployeeSalary>(es => es.EmployeeId)
            .IsRequired();
        modelBuilder.Entity<EmployeeSalary>() // Unique index on EmployeeId
            .HasIndex(es => es.EmployeeId)
            .IsUnique();
        modelBuilder.Entity<Payroll>() // Many-to-One relationship with Employee
            .HasOne(p => p.Employee)
            .WithMany()
            .HasForeignKey(p => p.EmployeeId)
            .IsRequired();


        modelBuilder.Entity<Attendance>() // Many-to-One relationship with Employee
            .HasOne(a => a.Employee)
            .WithMany(e => e.Attendances)
            .HasForeignKey(a => a.EmployeeId);

        modelBuilder.Entity<LeaveApplication>() // Many-to-One relationship with Employee
            .HasOne(la => la.Employee)
            .WithMany(e => e.LeaveApplications)
            .HasForeignKey(la => la.EmployeeId);
    }
}
