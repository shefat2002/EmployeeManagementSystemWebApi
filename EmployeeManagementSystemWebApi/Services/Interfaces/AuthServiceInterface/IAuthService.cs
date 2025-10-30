using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Models.DTOs.AuthDto;

namespace EmployeeManagementSystemWebApi.Services.Interfaces.AuthServiceInterface;

public interface IAuthService
{

    //In future I will apply SOLID principles(Dependency Inversion Principle) here to separate registration and login functionalities into different interfaces and classes.
    Task<Employee> Register(RegisterDto register);
    Task<LoginResponseDto?> Login(LoginDto login);
}
