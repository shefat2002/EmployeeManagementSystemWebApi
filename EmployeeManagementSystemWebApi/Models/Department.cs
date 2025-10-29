using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystemWebApi.Models;

public class Department
{
    public int Id { get; set; }
    [Required]
    [StringLength(20)]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Navigation property
    [ValidateNever]
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();

}
