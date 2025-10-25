using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystemWebApi.Models;

public class Payroll
{
    public int Id { get; set; }

    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    public int PayMonth { get; set; } 
    public int PayYear { get; set; } 
    public DateTime PayDate { get; set; }

    // Calculated Earnings for the month
    [Column(TypeName = "decimal(18, 2)")]
    public decimal BaseSalaryPaid { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal HouseAllowancePaid { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal MedicalAllowancePaid { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TransportAllowancePaid { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal OvertimePay { get; set; } 

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Bonus { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalEarnings { get; set; }

    // Calculated Deductions for the month
    [Column(TypeName = "decimal(18, 2)")]
    public decimal AbsenceDeduction { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal ProvidentFundDeduction { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TaxDeduction { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalDeductions { get; set; }

    // Final Net Pay
    [Column(TypeName = "decimal(18, 2)")]
    public decimal NetSalary { get; set; }

    public string? Remarks { get; set; }

    // Navigation Property
    [ValidateNever]
    public Employee Employee { get; set; } = null!;
}
