using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static EmployeeManagementSystemWebApi.Models.Enums;

namespace EmployeeManagementSystemWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaveApplicationsController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    public LeaveApplicationsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // POST: api/LeaveApplications/apply
    [HttpPost("apply")]
    public async Task<ActionResult> ApplyLeave([FromForm] LeaveApplication leaveApplication)
    {
        leaveApplication.ApplicationDate = DateTime.Now;
        leaveApplication.Status = LeaveStatus.Pending;
        await _unitOfWork.LeaveApplications.AddAsync(leaveApplication);
        await _unitOfWork.SaveChangesAsync();
        return CreatedAtAction(nameof(GetApplication), new { id = leaveApplication.Id }, leaveApplication);
    }

    // GET: api/LeaveApplications/5
    [HttpGet("{id}")]
    public async Task<ActionResult<LeaveApplication>> GetApplication(int id)
    {
        var application = await _unitOfWork.LeaveApplications.GetByIdAsync(id);
        if (application == null)
        {
            return NotFound("Leave application not found.");
        }
        return Ok(application);
    }

    // GET: api/LeaveApplications/employee/5
    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<LeaveApplication>>> GetEmployeeApplications(int employeeId)
    {
        var applications = await _unitOfWork.LeaveApplications.GetLeaveApplicationsByEmployeeIdAsync(employeeId);
        return Ok(applications);
    }

    // GET: api/LeaveApplications/pending
    [HttpGet("pending")]
    public async Task<ActionResult<IEnumerable<LeaveApplication>>> GetPendingApplications()
    {
        var applications = await _unitOfWork.LeaveApplications.GetPendingAsync();
        return Ok(applications);
    }

    // GET: api/LeaveApplications/rejected
    [HttpGet("rejected")]
    public async Task<ActionResult<IEnumerable<LeaveApplication>>> GetRejectedApplications()
    {
        var applications = await _unitOfWork.LeaveApplications.GetRejectedAsync();
        return Ok(applications);
    }

    // GET: api/LeaveApplications/approved
    [HttpGet("approved")]
    public async Task<ActionResult<IEnumerable<LeaveApplication>>> GetApprovedApplications()
    {
        var applications = await _unitOfWork.LeaveApplications.GetApprovedAsync();
        return Ok(applications);
    }

    // GET: api/LeaveApplications/employee/5/approved/8/2025
    [HttpGet("employee/{employeeId}/approved/{month}/{year}")]
    public async Task<ActionResult<IEnumerable<LeaveApplication>>> GetApprovedApplicationsByMonth(int employeeId, int month, int year)
    {
        var applications = await _unitOfWork.LeaveApplications.GetApprovedByMonthAsync(employeeId, month, year);
        return Ok(applications);
    }

    // PUT: api/LeaveApplications/5/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateApplicationStatus(int id, [FromForm] LeaveStatus status)
    {
        var application = await _unitOfWork.LeaveApplications.GetByIdAsync(id);
        if (application == null)
        {
            return NotFound("Leave application not found.");
        }
        application.Status = status;
        await _unitOfWork.LeaveApplications.UpdateAsync(application);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    // POST: api/LeaveApplications/approve/5
    [HttpPost("approve/{id}")]
    public async Task<IActionResult> ApproveApplication(int id)
    {
        var application = await _unitOfWork.LeaveApplications.GetByIdAsync(id);
        if (application == null)
        {
            return NotFound("Leave application not found.");
        }
        application.Status = LeaveStatus.Approved;
        

        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    // POST: api/LeaveApplications/reject/5
    [HttpPost("reject/{id}")]
    public async Task<IActionResult> RejectApplication(int id)
    {
        var application = await _unitOfWork.LeaveApplications.GetByIdAsync(id);
        if (application == null)
        {
            return NotFound("Leave application not found.");
        }
        application.Status = LeaveStatus.Rejected;
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/LeaveApplications/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteApplication(int id)
    {
        var application = await _unitOfWork.LeaveApplications.GetByIdAsync(id);
        if (application == null)
        {
            return NotFound("Leave application not found.");
        }
        await _unitOfWork.LeaveApplications.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

}
