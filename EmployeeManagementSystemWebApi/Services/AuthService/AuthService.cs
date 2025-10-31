using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Models.DTOs.AuthDto;
using EmployeeManagementSystemWebApi.Repositories.Interfaces;
using EmployeeManagementSystemWebApi.Services.Interfaces.AuthServiceInterface;
using System.Security.Cryptography;
using System.Text;

namespace EmployeeManagementSystemWebApi.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration, ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _tokenService = tokenService;
    }

    public async Task<LoginResponseDto?> Login(LoginDto login)
    {
        var employee = await _unitOfWork.Employees
            .GetEmployeeByEmailAsync(login.Email);

        if (employee == null)
        {
            return null;
        }
        if (!VerifyPasswordHash(login.Password, employee.PasswordHash, employee.PasswordSalt))
        {
            return null;
        }

        // Generate JWT token
        string token = await _tokenService.GenerateToken(employee);

        return new LoginResponseDto
        {
            Token = token,
            Name =  employee.Name,
            Role = employee.JobRole?.JobTitle ?? "Role Not Assigned",
        };
    }

    public async Task<Employee> Register(RegisterDto register)
    {
        var existingEmployee = await _unitOfWork.Employees
            .GetEmployeeByEmailAsync(register.Email);
        if (existingEmployee != null)
        {
            throw new Exception("Employee with this email already exists.");
        }

        CreatePasswordHash(register.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var employee = new Employee
        {
            Name = register.Name,
            Email = register.Email,
            DepartmentId = register.DepartmentId,
            JobRoleId = register.JobRoleId,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        await _unitOfWork.Employees.AddAsync(employee);
        await _unitOfWork.SaveChangesAsync();

        return employee;
    }

    // Helper method to create password hash and salt
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using (var hmac = new HMACSHA512(storedSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(storedHash);
        }
    }
    // How VerifyPasswordHash works:
    // 1. It takes the plain text password, stored hash, and stored salt as inputs.
    // 2. It creates a new HMACSHA512 instance using the stored salt.
    // 3. It computes the hash of the provided password using the same salt.
    // 4. It compares the computed hash with the stored hash.

}