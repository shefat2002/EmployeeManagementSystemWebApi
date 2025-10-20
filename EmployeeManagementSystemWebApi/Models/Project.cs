using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EmployeeManagementSystemWebApi.Models;

public enum ProjectStatus
{
   NotStarted,
   InProgress,
   Completed,
   OnHold,
   Cancelled
}
public class Project
{
   public int Id { get; set; }
   public string Name { get; set; }
   public string Description { get; set; }
   public DateTime StartDate { get; set; }
   public DateTime? EndDate { get; set; }
   public ProjectStatus Status { get; set; }

   // Navigation property for related employees
   [ValidateNever]
   public ICollection<EmployeeProjectAssignment> EmployeeProjects { get; set; }

}
