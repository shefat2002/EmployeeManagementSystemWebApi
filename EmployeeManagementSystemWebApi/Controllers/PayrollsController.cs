using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        foreach(var s in salaries)
        {
            var absenceDays = 0; // Placeholder for actual absence calculation logic
            var baseSalaryPaid = s.BaseSalary;
            var houseAllowancePaid = s.HouseAllowance;
            var medicalAllowancePaid = s.MedicalAllowance;
            var transportAllowancePaid = s.TransportAllowance;
            var totalEarnings = baseSalaryPaid + houseAllowancePaid + medicalAllowancePaid + transportAllowancePaid + payroll.OvertimePay + payroll.Bonus;
            var absenceDeduction = (s.BaseSalary / daysInMonth) * absenceDays;
            var providentFundDeduction = (s.BaseSalary * s.ProvidentFundPercentage) / 100;
            var taxDeduction = (s.BaseSalary * s.TaxPercentage) / 100;
            var totalDeductions = absenceDeduction + providentFundDeduction + taxDeduction;
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
                AbsenceDeduction = absenceDeduction,  // to do
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


}
