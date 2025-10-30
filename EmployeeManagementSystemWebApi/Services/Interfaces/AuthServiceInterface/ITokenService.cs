using EmployeeManagementSystemWebApi.Models;

namespace EmployeeManagementSystemWebApi.Services.Interfaces.AuthServiceInterface;

public interface ITokenService
{
    Task<string> GenerateToken(Employee employee);
    string GenerateRefreshToken(); // will be implemented later
}
