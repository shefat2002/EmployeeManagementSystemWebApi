using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static EmployeeManagementSystemWebApi.Models.Enums;

namespace EmployeeManagementSystemWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SalaryController : ControllerBase
{
    private readonly EmployeeDbContext _context;
    public SalaryController(EmployeeDbContext context)
    {
        _context = context;
    }

    // Salary Structure Retrieval

    // GET: api/Salary/structure/5
    [HttpGet("structure/{employeeId}")]
    public async Task<ActionResult<EmployeeSalary>> GetSalaryStructure(int employeeId)
    {
        var salaryStructure = await _context.EmployeeSalaries
            .FirstOrDefaultAsync(s => s.EmployeeId == employeeId);

        if (salaryStructure == null)
        {
            return NotFound("No salary structure found for this employee.");
        }

        return Ok(salaryStructure);
    }

    // POST: api/Salary/structure
    [HttpPost("structure")]
    public async Task<ActionResult<EmployeeSalary>> CreateSalaryStructure(EmployeeSalary salaryStructure)
    {
        _context.EmployeeSalaries.Add(salaryStructure);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSalaryStructure), new { employeeId = salaryStructure.EmployeeId }, salaryStructure);
    }

    // PUT: api/Salary/structure/5
    [HttpPut("structure/{employeeId}")]
    public async Task<IActionResult> UpdateSalaryStructure(int employeeId, EmployeeSalary updatedStructure)
    {
        var existingStructure = await _context.EmployeeSalaries
            .FirstOrDefaultAsync(s => s.EmployeeId == employeeId);
        if (existingStructure == null)
        {
            return NotFound("No salary structure found for this employee.");
        }
        // Update fields
        existingStructure.BaseSalary = updatedStructure.BaseSalary;
        existingStructure.HouseAllowance = updatedStructure.HouseAllowance;
        existingStructure.MedicalAllowance = updatedStructure.MedicalAllowance;
        existingStructure.TransportAllowance = updatedStructure.TransportAllowance;
        existingStructure.ProvidentFundPercentage = updatedStructure.ProvidentFundPercentage;
        existingStructure.TaxPercentage = updatedStructure.TaxPercentage;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Payroll Generation
    // POST: api/Salary/payroll/5
    [HttpPost("payroll/{employeeId}")]
    public async Task<ActionResult> GeneratePayroll([FromBody] GeneratePayrollRequestDto request)
    {
        if (request == null)
        {
            return BadRequest("Invalid payroll request.");
        }
        try
        {
            bool alreadyGenerated = await _context.Payrolls
                .AnyAsync(p => p.PayMonth == request.Month && p.PayYear == request.Year);
            if (alreadyGenerated)
            {
                return Conflict("Payroll for this month and year has already been generated.");
            }
            var employees = await _context.Employees
                .Include(e => e.EmployeeSalary)
                .Where(e => e.EmployeeSalary != null)
                .ToListAsync();
            if(employees.Count == 0)
            {
                return NotFound("No employees with salary structures found.");
            }
            var payrolls = new List<Payroll>();
            var startDate = new DateTime(request.Year, request.Month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            int totalDaysInMonth = DateTime.DaysInMonth(request.Year, request.Month);

            foreach (var employee in employees)
            {
                var salary = employee.EmployeeSalary!;
                int absentDays = await _context.Attendances
                    .Where(a => a.EmployeeId == employee.Id &&
                                a.Date >= startDate && a.Date <= endDate &&
                                a.Status == AttendanceStatus.Absent)
                    .CountAsync();
                int holidays = await _context.Attendances
                    .Where(a => a.EmployeeId == employee.Id &&
                                a.Date >= startDate && a.Date <= endDate &&
                                a.Status == AttendanceStatus.Holiday)
                    .CountAsync();
                decimal absenceDeduction = (salary.BaseSalary / totalDaysInMonth) * absentDays;
                decimal providentFundDeduction = (salary.ProvidentFundPercentage / 100) * salary.BaseSalary;
                decimal taxDeduction = (salary.TaxPercentage / 100) * salary.BaseSalary;
                decimal totalEarnings = salary.BaseSalary + salary.HouseAllowance + salary.MedicalAllowance + salary.TransportAllowance;
                decimal totalDeductions = absenceDeduction + providentFundDeduction + taxDeduction;
                decimal netSalary = totalEarnings - totalDeductions;
                var payroll = new Payroll
                {
                    EmployeeId = employee.Id,
                    PayMonth = request.Month,
                    PayYear = request.Year,
                    PayDate = DateTime.Now,
                    BaseSalaryPaid = salary.BaseSalary,
                    HouseAllowancePaid = salary.HouseAllowance,
                    MedicalAllowancePaid = salary.MedicalAllowance,
                    TransportAllowancePaid = salary.TransportAllowance,
                    OvertimePay = 0, 
                    Bonus = 0, 
                    TotalEarnings = totalEarnings,
                    AbsenceDeduction = absenceDeduction,
                    ProvidentFundDeduction = providentFundDeduction,
                    TaxDeduction = taxDeduction,
                    TotalDeductions = totalDeductions,
                    NetSalary = netSalary,
                    Remarks = "Payroll generated automatically."
                };
                payrolls.Add(payroll);
            }
            await _context.Payrolls.AddRangeAsync(payrolls);
            await _context.SaveChangesAsync();


            return Ok("Payroll generated successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }
    //GET: api/Salary/payslip/5?month=8&year=2025
    [HttpGet("payslip/{employeeId}")]
    public async Task<ActionResult<Payroll>> GetPayslip(int employeeId, [FromQuery] int month, [FromQuery] int year)
    {
        var payroll = await _context.Payrolls
            .FirstOrDefaultAsync(p => p.EmployeeId == employeeId && p.PayMonth == month && p.PayYear == year);
        if (payroll == null)
        {
            return NotFound($"Payslip not found for {month} - {year}.");
        }
        

        return Ok(payroll);
    }

}
