namespace EmployeeManagementSystemWebApi.Models;

public class JobRole
{
    public int Id { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int BaseSalary { get; set; }
}
