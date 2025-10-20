using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EmployeeMVC_Consume.ViewModels;

public class DepartmentVM
{   public int Id { get; set; }
   public string Name { get; set; }
   public string Description { get; set; }
   [ValidateNever]
   public ICollection<EmployeeVM> Employees { get; set; }
}
