using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystemWebApi.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    [ForeignKey("Department")]
    public int DepartmentId { get; set; }
    [ForeignKey("JobRole")]
    public int JobRoleId { get; set; }

    // Navigation property 
    [ValidateNever]
    public Department Department { get; set; } = null!;
    [ValidateNever]
    public JobRole JobRole { get; set; } = null!;


}
