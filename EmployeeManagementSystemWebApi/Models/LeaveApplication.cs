using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using static EmployeeManagementSystemWebApi.Models.Enums;

namespace EmployeeManagementSystemWebApi.Models;

public class LeaveApplication
{
    public int Id { get; set; }

    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    public LeaveType Type { get; set; }
    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public DateTime ApplicationDate { get; set; }

    
    //public int? ApprovedByEmployeeId { get; set; }

    // Navigation Property
    [ValidateNever]
    public Employee Employee { get; set; } = null!;
}
