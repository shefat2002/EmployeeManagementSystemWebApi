using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static EmployeeManagementSystemWebApi.Models.Enums;

namespace EmployeeManagementSystemWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PayrollsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    public PayrollsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET : api/Payroll/employee/5
    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<Payroll>>> GetEmployeePayroll(int employeeId)
    {
        var payroll = await _unitOfWork.Payrolls.GetPayrollsByEmployeeIdAsync(employeeId);

        if (payroll == null)
        {
            return NotFound("No payroll information found for this employee.");
        }

        return Ok(payroll);
    }

    // GET : api/Payroll/employee/5/8/2025
    [HttpGet("employee/{employeeId}/{month}/{year}")]
    public async Task<ActionResult<Payroll>> GetEmployeePayrollByMonth(int employeeId, int month, int year)
    {
        var payroll = await _unitOfWork.Payrolls.GetPayrollByEmployeeIdAndMonthAsync(employeeId, month, year);
        if (payroll == null)
        {
            return NotFound("No payroll information found for this employee for the specified month and year.");
        }
        return Ok(payroll);
    }

    // POST : api/Payroll/generate
    [HttpPost("generate")]
    public async Task<ActionResult<Payroll>> GeneratePayroll([FromForm] Payroll payroll)
    {
        var existingPayroll = await _unitOfWork.Payrolls.GetPayrollByEmployeeIdAndMonthAsync(0, payroll.PayMonth, payroll.PayYear);
        if (existingPayroll != null)
        {
            return Conflict($"Payroll already exists for {payroll.PayMonth}/{payroll.PayYear}");
        }

        var salaries = await _unitOfWork.EmployeeSalaries.GetAllAsync();

        var newPayrolls = new List<Payroll>();
        int daysInMonth = DateTime.DaysInMonth(payroll.PayYear, payroll.PayMonth);

        var attendanceRecords = await _unitOfWork.Attendances.GetAttendanceByEmployeeIdAndMonthAsync(payroll.EmployeeId, payroll.PayMonth, payroll.PayYear);
        var approvedLeaves = await _unitOfWork.LeaveApplications.GetApprovedByMonthAsync(payroll.EmployeeId, payroll.PayMonth, payroll.PayYear);

        foreach (var s in salaries)
        {
            // Calculate absence days
            var absenceDays = attendanceRecords.Count(a => a.Status == AttendanceStatus.Absent && a.EmployeeId == s.EmployeeId);
            var leaveDays = approvedLeaves.Count(l => l.EmployeeId == s.EmployeeId);
            absenceDays -= leaveDays;
            if (absenceDays < 0) absenceDays = 0;

            // Calculate earnings
            var baseSalaryPaid = s.BaseSalary;
            var houseAllowancePaid = s.HouseAllowance;
            var medicalAllowancePaid = s.MedicalAllowance;
            var transportAllowancePaid = s.TransportAllowance;
            var totalEarnings = baseSalaryPaid + houseAllowancePaid + medicalAllowancePaid + transportAllowancePaid + payroll.OvertimePay + payroll.Bonus;

            // Calculate deductions
            var absenceDeduction = Math.Round(s.BaseSalary / daysInMonth) * absenceDays;
            var providentFundDeduction = Math.Round((s.BaseSalary * s.ProvidentFundPercentage) / 100);
            var taxDeduction = Math.Round((s.BaseSalary * s.TaxPercentage) / 100);
            var totalDeductions = absenceDeduction + providentFundDeduction + taxDeduction;

            // Calculate net salary
            var netSalary = totalEarnings - totalDeductions;
            var newPayroll = new Payroll
            {
                EmployeeId = s.EmployeeId,
                PayMonth = payroll.PayMonth,
                PayYear = payroll.PayYear,
                PayDate = DateTime.Now,
                BaseSalaryPaid = baseSalaryPaid,
                HouseAllowancePaid = houseAllowancePaid,
                MedicalAllowancePaid = medicalAllowancePaid,
                TransportAllowancePaid = transportAllowancePaid,
                OvertimePay = payroll.OvertimePay, // to do 
                Bonus = payroll.Bonus, // to do
                TotalEarnings = totalEarnings,
                AbsenceDeduction = absenceDeduction,
                ProvidentFundDeduction = providentFundDeduction,
                TaxDeduction = taxDeduction,
                TotalDeductions = totalDeductions,
                NetSalary = netSalary,
                Remarks = payroll.Remarks
            };
            newPayrolls.Add(newPayroll);
        }
        await _unitOfWork.Payrolls.AddRangeAsync(newPayrolls);
        await _unitOfWork.SaveChangesAsync();
        return Ok(new {message = "Payroll generated successfully.", count = newPayrolls.Count});
    }

    // PUT : api/Payroll/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePayroll(int id, [FromForm] Payroll updatedPayroll)
    {
        if (id != updatedPayroll.Id)
        {
            return BadRequest("Payroll ID mismatch.");
        }
        var existingPayroll = await _unitOfWork.Payrolls.GetByIdAsync(id);
        if (existingPayroll == null)
        {
            return NotFound("Payroll record not found.");
        }
        // Update fields
        existingPayroll.PayMonth = updatedPayroll.PayMonth;
        existingPayroll.PayYear = updatedPayroll.PayYear;
        existingPayroll.PayDate = updatedPayroll.PayDate;
        existingPayroll.BaseSalaryPaid = updatedPayroll.BaseSalaryPaid;
        existingPayroll.HouseAllowancePaid = updatedPayroll.HouseAllowancePaid;
        existingPayroll.MedicalAllowancePaid = updatedPayroll.MedicalAllowancePaid;
        existingPayroll.TransportAllowancePaid = updatedPayroll.TransportAllowancePaid;
        existingPayroll.OvertimePay = updatedPayroll.OvertimePay;
        existingPayroll.Bonus = updatedPayroll.Bonus;
        existingPayroll.TotalEarnings = updatedPayroll.TotalEarnings;
        existingPayroll.AbsenceDeduction = updatedPayroll.AbsenceDeduction;
        existingPayroll.ProvidentFundDeduction = updatedPayroll.ProvidentFundDeduction;
        existingPayroll.TaxDeduction = updatedPayroll.TaxDeduction;
        existingPayroll.TotalDeductions = updatedPayroll.TotalDeductions;
        existingPayroll.NetSalary = updatedPayroll.NetSalary;
        existingPayroll.Remarks = updatedPayroll.Remarks;
        await _unitOfWork.Payrolls.UpdateAsync(existingPayroll);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    // DELETE : api/Payroll/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayroll(int id)
    {
        var payroll = await _unitOfWork.Payrolls.GetByIdAsync(id);
        if (payroll == null)
        {
            return NotFound("Payroll record not found.");
        }
        await _unitOfWork.Payrolls.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

}
