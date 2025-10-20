using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EmployeeMVC_Consume.ViewModels;

public class EmployeeVM
{
   public int Id { get; set; }
   public string Name { get; set; }
   public string Position { get; set; }
   public int DepartmentId { get; set; }
   [ValidateNever]
   public ICollection<DepartmentVM> Departments { get; set; }

}
