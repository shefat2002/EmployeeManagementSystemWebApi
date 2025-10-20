using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystemWebApi.Models;

public class EmployeeProjectAssignment
{
   public int Id { get; set; }
   [ForeignKey("Employee")]
   public int EmployeeId { get; set; }
   [ForeignKey("Project")]
   public int ProjectId { get; set; }
   public DateTime AssignedDate { get; set; }

   // Navigation properties
   public Employee Employee { get; set; }
   public Project Project { get; set; }

}
