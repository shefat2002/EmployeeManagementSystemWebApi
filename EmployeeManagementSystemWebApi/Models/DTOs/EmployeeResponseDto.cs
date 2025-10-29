namespace EmployeeManagementSystemWebApi.Models.DTOs;

public class EmployeeResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public DateTime HireDate { get; set; }
    public string DepartmentName { get; set; } = null!;
    public string JobRoleName { get; set; } = null!;
}
