using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystemWebApi.Models;

public class Employee
{
    public int Id { get; set; }
    [Required]
    [StringLength(30)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [ForeignKey("Department")]
    public int DepartmentId { get; set; }
    [ForeignKey("JobRole")]
    public int JobRoleId { get; set; }

    // Password hashing properties
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;

    // Navigation properties
    [ValidateNever]
    public Department Department { get; set; } = null!;
    [ValidateNever]
    public JobRole JobRole { get; set; } = null!;

    [ValidateNever]
    public EmployeeSalary? EmployeeSalary { get; set; } 

    [ValidateNever]
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    [ValidateNever]
    public ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();

    [ValidateNever]
    public ICollection<LeaveApplication> LeaveApplications { get; set; } = new List<LeaveApplication>();


}
