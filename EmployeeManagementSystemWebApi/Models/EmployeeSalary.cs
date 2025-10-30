using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementSystemWebApi.Models;

public class EmployeeSalary
{
    public int Id { get; set; }

    [ForeignKey("Employee")]
    public int EmployeeId { get; set; } 

    [Column(TypeName = "decimal(18, 2)")]
    public decimal BaseSalary { get; set; }

    // (Earnings)
    [Column(TypeName = "decimal(18, 2)")]
    public decimal HouseAllowance { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal MedicalAllowance { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal TransportAllowance { get; set; }

    // Deductions 
    [Column(TypeName = "decimal(5, 2)")]
    public decimal ProvidentFundPercentage { get; set; }

    [Column(TypeName = "decimal(5, 2)")]
    public decimal TaxPercentage { get; set; }

    // Navigation Property
    [ValidateNever]
    public virtual Employee Employee { get; set; } = null!;
}
