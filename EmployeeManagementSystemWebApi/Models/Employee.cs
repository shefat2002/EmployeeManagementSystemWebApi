using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystemWebApi.Models;

public class Employee
{
   public int Id { get; set; }
   public string Name { get; set; } = string.Empty;
   public string Position { get; set; } = string.Empty;
   [ForeignKey("Department")]
   public int DepartmentId { get; set; }

   // Navigation property for related department
   [ValidateNever]
   public Department Department { get; set; } = null!;
   [ValidateNever]
   public ICollection<EmployeeProjectAssignment> ProjectAssignments { get; set; } = new List<EmployeeProjectAssignment>();

}
