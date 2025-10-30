using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystemWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SalariesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public SalariesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    // Salary Structure Retrieval

    // GET: api/Salary/structure/5
    [HttpGet("structure/{employeeId}")]
    public async Task<ActionResult<EmployeeSalary>> GetEmployeeSalaryStructure(int employeeId)
    {
        var salary = await _unitOfWork.EmployeeSalaries.GetByEmployeeIdAsync(employeeId);

        if (salary == null)
        {
            return NotFound("No salary structure found for this employee.");
        }

        return Ok(salary);
    }

    // POST: api/Salary/structure
    [HttpPost("structure")]
    public async Task<ActionResult<EmployeeSalary>> CreateSalaryStructure([FromForm] EmployeeSalary salaryStructure)
    {
        var existingStructure = await _unitOfWork.EmployeeSalaries.GetByEmployeeIdAsync(salaryStructure.EmployeeId);
        if (existingStructure != null)
        {
            return Conflict("Salary structure already exists for this employee.");
        }
        await _unitOfWork.EmployeeSalaries.AddAsync(salaryStructure);
        await _unitOfWork.SaveChangesAsync();

        // reload to include navigation properties
        var createdStructure = await _unitOfWork.EmployeeSalaries.GetByEmployeeIdAsync(salaryStructure.EmployeeId);

        return CreatedAtAction(nameof(GetEmployeeSalaryStructure), new { employeeId = salaryStructure.EmployeeId }, createdStructure);
    }

    // PUT: api/Salary/structure/5
    [HttpPut("structure/{employeeId}")]
    public async Task<IActionResult> UpdateSalaryStructure(int employeeId, [FromForm] EmployeeSalary updatedStructure)
    {
        
        if (employeeId != updatedStructure.EmployeeId)
        {
            return BadRequest("Employee ID mismatch.");
        }

        var salary = await _unitOfWork.EmployeeSalaries.GetByEmployeeIdAsync(employeeId);

        if (salary == null)
        {
            return NotFound("Salary structure not found for this employee.");
        }

        // Update fields
        salary.BaseSalary = updatedStructure.BaseSalary;
        salary.HouseAllowance = updatedStructure.HouseAllowance;
        salary.MedicalAllowance = updatedStructure.MedicalAllowance;
        salary.TransportAllowance = updatedStructure.TransportAllowance;
        salary.ProvidentFundPercentage = updatedStructure.ProvidentFundPercentage;
        salary.TaxPercentage = updatedStructure.TaxPercentage;
        await _unitOfWork.EmployeeSalaries.UpdateAsync(salary);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/Salary/structure/5
    [HttpDelete("structure/{employeeId}")]
    public async Task<IActionResult> DeleteSalaryStructure(int employeeId)
    {
        var salary = await _unitOfWork.EmployeeSalaries.GetByEmployeeIdAsync(employeeId);
        if (salary == null)
        {
            return NotFound("Salary structure not found for this employee.");
        }
        await _unitOfWork.EmployeeSalaries.DeleteAsync(employeeId);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }



}
