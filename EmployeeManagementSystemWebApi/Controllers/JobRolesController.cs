using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Repositories.Interfaces;

namespace EmployeeManagementSystemWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobRolesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public JobRolesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    // GET: api/JobRoles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobRole>>> GetJobRole()
    {
        var jobRoles = await _unitOfWork.JobRoles.GetAllAsync();
        return Ok(jobRoles);
    }

    // GET: api/JobRoles/5
    [HttpGet("{id}")]
    public async Task<ActionResult<JobRole>> GetJobRole(int id)
    {
        var jobRole = await _unitOfWork.JobRoles.GetByIdAsync(id);

        if (jobRole == null)
        {
            return NotFound();
        }

        return jobRole;
    }

    // PUT: api/JobRoles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutJobRole(int id, [FromForm] JobRole jobRole)
    {
        if (id != jobRole.Id)
        {
            return BadRequest();
        }

        var existingJobRole = await _unitOfWork.JobRoles.GetByIdAsync(id);
        if (existingJobRole == null)
        {
            return NotFound();
        }

        existingJobRole.JobTitle = jobRole.JobTitle;
        existingJobRole.Description = jobRole.Description;

        await _unitOfWork.JobRoles.UpdateAsync(existingJobRole);

        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (await _unitOfWork.JobRoles.GetByIdAsync(id) == null)
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

    // POST: api/JobRoles
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<JobRole>> PostJobRole([FromForm]JobRole jobRole)
    {
        await _unitOfWork.JobRoles.AddAsync(jobRole);
        await _unitOfWork.SaveChangesAsync();

        return CreatedAtAction("GetJobRole", new { id = jobRole.Id }, jobRole);
    }

    // DELETE: api/JobRoles/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteJobRole(int id)
    {
        var jobRole = await _unitOfWork.JobRoles.GetByIdAsync(id);
        if (jobRole == null)
        {
            return NotFound();
        }

        await _unitOfWork.JobRoles.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return NoContent();
    }

}
