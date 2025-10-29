using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Repositories.Interfaces;

namespace EmployeeManagementSystemWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public DepartmentsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    // GET: api/Departments
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
    {
        var departments = await _unitOfWork.Departments.GetAllAsync();
        return Ok(departments);
    }

    // GET: api/Departments/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Department>> GetDepartment(int id)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id);

        if (department == null)
        {
            return NotFound();
        }

        return department;
    }

    // PUT: api/Departments/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutDepartment(int id, [FromForm] Department department)
    {
        if (id != department.Id)
        {
            return BadRequest();
        }

        var existingDepartment = await _unitOfWork.Departments.GetByIdAsync(id);
        if (existingDepartment == null)
        {
            return NotFound();
        }

        existingDepartment.Name = department.Name;
        existingDepartment.Description = department.Description;

        await _unitOfWork.Departments.UpdateAsync(department);

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if ( await _unitOfWork.Departments.GetByIdAsync(id) == null)
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
     
        return NoContent();
    }

    // POST: api/Departments
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Department>> PostDepartment([FromForm] Department department)
    {
        if (department == null)
        {
            return BadRequest();
        }

        // Existing department with same name check validation will be added in future

        await _unitOfWork.Departments.AddAsync(department);
        await _unitOfWork.SaveChangesAsync();
        return CreatedAtAction("GetDepartment", new { id = department.Id }, department);

    }

    // DELETE: api/Departments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDepartment(int id)
    {
        var department = await _unitOfWork.Departments.GetByIdAsync(id);
        if (department == null)
        {
            return NotFound();
        }

        await _unitOfWork.Departments.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

    
}
