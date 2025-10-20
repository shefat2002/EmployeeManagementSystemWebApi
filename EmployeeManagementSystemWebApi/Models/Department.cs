using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EmployeeManagementSystemWebApi.Models;

public class Department
{
   public int Id { get; set; }
   public string Name { get; set; }
   public string Description { get; set; }
   [ValidateNever]
   public ICollection<Employee> Employees { get; set; }

}
