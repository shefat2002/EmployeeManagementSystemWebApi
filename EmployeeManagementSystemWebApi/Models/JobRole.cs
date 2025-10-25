using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystemWebApi.Models;

public class JobRole
{
    public int Id { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Column(TypeName = "decimal(18, 2)")]
    public int BaseSalary { get; set; }
    // Navigation Property
    [ValidateNever]
    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
