using EmployeeManagementSystemWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystemWebApi.Data;

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


    // Fluent API configurations
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Foreign key relationships --------------
        modelBuilder.Entity<Employee>() // Many-to-One relationship with Department
            .HasOne(e => e.Department)
            .WithMany(d => d.Employees)
            .HasForeignKey(e => e.DepartmentId);
        modelBuilder.Entity<Employee>() // Many-to-One relationship with JobRole
            .HasOne(e => e.JobRole)
            .WithMany(j => j.Employees)
            .HasForeignKey(e => e.JobRoleId);

        //modelBuilder.Entity<EmployeeSalary>() // One-to-One relationship with Employee
        //    .HasOne(es => es.Employee)
        //    .WithOne()
        //    .HasForeignKey<EmployeeSalary>(es => es.EmployeeId);
        
        //modelBuilder.Entity<Payroll>() // Many-to-One relationship with Employee
        //    .HasOne(p => p.Employee)
        //    .WithMany()
        //    .HasForeignKey(p => p.EmployeeId);

        modelBuilder.Entity<Attendance>() // Many-to-One relationship with Employee
            .HasOne(a => a.Employee)
            .WithMany(e => e.Attendances)
            .HasForeignKey(a => a.EmployeeId);

        modelBuilder.Entity<LeaveApplication>() // Many-to-One relationship with Employee
            .HasOne(la => la.Employee)
            .WithMany(e => e.LeaveApplications)
            .HasForeignKey(la => la.EmployeeId);

        // -------------------- Foreign Key Relationships ---------------------


        // Indexes for performance optimization ------------

        // Employee indexes
        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Email)
            .IsUnique();
        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.Name);

        // Attendance indexes
        modelBuilder.Entity<Attendance>()
            .HasIndex(a => new { a.EmployeeId, a.Date });

        // Payroll indexes
        modelBuilder.Entity<Payroll>()
            .HasIndex(p => new { p.EmployeeId, p.PayDate });

        // LeaveApplication indexes
        modelBuilder.Entity<LeaveApplication>()
            .HasIndex(la => new { la.EmployeeId, la.StartDate, la.EndDate });

        // Department indexes
        modelBuilder.Entity<Department>()
            .HasIndex(d => d.Name)
            .IsUnique();
        // JobRole indexes
        modelBuilder.Entity<JobRole>()
            .HasIndex(j => j.JobTitle)
            .IsUnique();
        // EmployeeSalary indexes
        modelBuilder.Entity<EmployeeSalary>() 
            .HasIndex(es => es.EmployeeId)
            .IsUnique();
        // --------------- Indexes for Performance Optimization ---------------
    }



}
