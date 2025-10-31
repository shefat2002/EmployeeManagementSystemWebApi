using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystemWebApi.Models.DTOs.AuthDto;

public class RegisterDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; } = string.Empty;
    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
    [Required]
    [ForeignKey("Department")]
    public int DepartmentId { get; set; }
    [Required]
    [ForeignKey("JobRole")]
    public int JobRoleId { get; set; }
}
