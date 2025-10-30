using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using static EmployeeManagementSystemWebApi.Models.Enums.Enums;

namespace EmployeeManagementSystemWebApi.Models;

public class Attendance
{
    public int Id { get; set; }

    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    public DateTime Date { get; set; }

    public DateTime? CheckInTime { get; set; }
    public DateTime? CheckOutTime { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Remarks { get; set; }

    // Navigation Property
    [ValidateNever]
    public Employee Employee { get; set; } = null!;
}
