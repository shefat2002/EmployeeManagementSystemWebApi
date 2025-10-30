using EmployeeManagementSystemWebApi.Models.DTOs.AuthDto;
using EmployeeManagementSystemWebApi.Services.Interfaces.AuthServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagementSystemWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST: api/Auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterDto registerDto)
    {
        try
        {
            var employee = await _authService.Register(registerDto);
            return CreatedAtAction(nameof(Register), new { id = employee.Id }, employee);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // POST: api/Auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
    {
        var loginResponse = await _authService.Login(loginDto);
        if (loginResponse == null)
        {
            return Unauthorized(new { message = "Invalid email or password." });
        }
        return Ok(loginResponse);
    }

}
