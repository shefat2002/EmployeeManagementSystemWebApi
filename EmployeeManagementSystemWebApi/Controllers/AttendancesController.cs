using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static EmployeeManagementSystemWebApi.Models.Enums.Enums;

namespace EmployeeManagementSystemWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AttendancesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    public AttendancesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // POST: api/Attendances/checkin
    [HttpPost("checkin")]
    public async Task<ActionResult> CheckIn([FromForm] int employeeId)
    {
        var today = DateTime.Now;
        var existingAttendance = await _unitOfWork.Attendances.GetAttendanceByEmployeeIdAndDateAsync(employeeId, today);
        if (existingAttendance != null && existingAttendance.CheckInTime != null)
        {
            return Conflict("Employee has already checked in for today.");
        }
        if (existingAttendance == null)
        {
            var attendance = new Models.Attendance
            {
                EmployeeId = employeeId,
                Date = today,
                CheckInTime = DateTime.Now,
                Status = AttendanceStatus.Present

            };
            await _unitOfWork.Attendances.AddAsync(attendance);
        }
        else
        {
            existingAttendance.CheckInTime = DateTime.Now;
            await _unitOfWork.Attendances.AddAsync(existingAttendance);
        }
        await _unitOfWork.SaveChangesAsync();
        return Ok("Check-in successful.");
    }

    // POST: api/Attendances/checkout
    [HttpPost("checkout")]
    public async Task<ActionResult> CheckOut([FromForm] int employeeId)
    {
        var today = DateTime.Now;
        var existingAttendance = await _unitOfWork.Attendances.GetAttendanceByEmployeeIdAndDateAsync(employeeId, today);
        if (existingAttendance == null || existingAttendance.CheckInTime == null)
        {
            return BadRequest("Employee has not checked in for today.");
        }
        if (existingAttendance.CheckOutTime != null)
        {
            return Conflict("Employee has already checked out for today.");
        }
        existingAttendance.CheckOutTime = DateTime.Now;
        if(existingAttendance.CheckInTime > existingAttendance.CheckOutTime)
        {
            return BadRequest("Check-out time cannot be earlier than check-in time.");
        }
        if(existingAttendance.CheckInTime != null && existingAttendance.CheckOutTime != null)
        {
            var workDuration = existingAttendance.CheckOutTime.Value - existingAttendance.CheckInTime.Value;
            if (workDuration.TotalHours >= 8)
            {
                existingAttendance.Status = AttendanceStatus.Present;
            }
            else if (workDuration.TotalHours >= 4)
            {
                existingAttendance.Status = AttendanceStatus.HalfDay;
            }
            else
            {
                existingAttendance.Status = AttendanceStatus.Absent;
            }
        }
        await _unitOfWork.Attendances.AddAsync(existingAttendance);
        await _unitOfWork.SaveChangesAsync();
        return Ok("Check-out successful.");
    }

    // PUT: api/Attendances/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAttendance(int id, [FromBody] Models.Attendance attendance)
    {
        if (id != attendance.Id)
        {
            return BadRequest("Attendance ID mismatch.");
        }
        var existingAttendance = await _unitOfWork.Attendances.GetByIdAsync(id);
        if (existingAttendance == null)
        {
            return NotFound("Attendance record not found.");
        }
        existingAttendance.CheckInTime = attendance.CheckInTime;
        existingAttendance.CheckOutTime = attendance.CheckOutTime;
        existingAttendance.Status = attendance.Status;
        existingAttendance.Remarks = attendance.Remarks;
        await _unitOfWork.Attendances.UpdateAsync(existingAttendance);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }

    // GET: api/Attendances/report/5?month=8&year=2025
    [HttpGet("report/{employeeId}")]
    public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendanceReport(int employeeId, [FromQuery] int month, [FromQuery] int year)
    {
        var attendanceRecords = await _unitOfWork.Attendances.GetAttendanceByEmployeeIdAndMonthAsync(employeeId, month, year);
        if (attendanceRecords == null || !attendanceRecords.Any())
        {
            return NotFound("No attendance records found for this employee for the specified month and year.");
        }
        return Ok(attendanceRecords);
    }

    // GET: api/Attendances/range/5?startDate=2025-08-01&endDate=2025-08-31
    [HttpGet("range/{employeeId}")]
    public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendanceByDateRange(int employeeId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var attendanceRecords = await _unitOfWork.Attendances.GetAttendanceByEmployeeIdAndDateRangeAsync(employeeId, startDate, endDate);
        if (attendanceRecords == null || !attendanceRecords.Any())
        {
            return NotFound("No attendance records found for this employee in the specified date range.");
        }
        return Ok(attendanceRecords);
    }

    // DELETE: api/Attendances/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttendance(int id)
    {
        var existingAttendance = await _unitOfWork.Attendances.GetByIdAsync(id);
        if (existingAttendance == null)
        {
            return NotFound("Attendance record not found.");
        }
        await _unitOfWork.Attendances.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
        return NoContent();
    }
}
