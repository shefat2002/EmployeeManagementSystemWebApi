using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EmployeeMVC_Consume.ViewModels;

public class DepartmentVM
{   public int Id { get; set; }
   public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    [ValidateNever]
   public ICollection<EmployeeVM> Employees { get; set; } = new List<EmployeeVM>();
}
