using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace EmployeeMVC_Consume.ViewModels;

public class EmployeeVM
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public int JobRoleId { get; set; }
    [ValidateNever]
    public DepartmentVM Department { get; set; } = null!;
    [ValidateNever]
    public JobRoleVM JobRole { get; set; } = null!;
    [ValidateNever]
    public ICollection<DepartmentVM> Departments { get; set; } = new List<DepartmentVM>();

}
