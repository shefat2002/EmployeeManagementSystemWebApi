using Microsoft.AspNetCore.Mvc;
using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Repositories.Interfaces;

namespace EmployeeManagementSystemWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public EmployeesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    // GET: api/Employees
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
    {
        var employees = await _unitOfWork.Employees.GetAllEmployeesWithDetailsAsync();
        return Ok(employees);
    }

    // GET: api/Employees/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(int id)
    {
        var employee = await _unitOfWork.Employees.GetEmployeeWithDetailsAsync(id);
        if (employee == null)
        {
            return NotFound();
        }
        return Ok(employee);
    }

    // PUT: api/Employees/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEmployee( int id, [FromForm] Employee employee)
    {
        if (id != employee.Id)
        {
            return BadRequest();
        }
        var existingEmployee = await _unitOfWork.Employees.GetByIdAsync(id);
        if (existingEmployee == null)
        {
            return NotFound();
        }

        existingEmployee.Name = employee.Name;

        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }
     
    // POST: api/Employees
    [HttpPost]
    public async Task<ActionResult<Employee>> PostEmployee([FromForm] Employee employee)
    {
        await _unitOfWork.Employees.AddAsync(employee);
        await _unitOfWork.SaveChangesAsync();
        return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);

    }

    // DELETE: api/Employees/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployee(int id)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(id);
        if(employee == null)
        {
            return NotFound();
        }

        await _unitOfWork.Employees.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

}
