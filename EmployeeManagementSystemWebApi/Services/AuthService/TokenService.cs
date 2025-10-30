using EmployeeManagementSystemWebApi.Models;
using EmployeeManagementSystemWebApi.Services.Interfaces.AuthServiceInterface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeManagementSystemWebApi.Services.AuthService;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public Task<string> GenerateToken(Employee employee)
    {
        // Create claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new Claim(ClaimTypes.Name, employee.Name),
            new Claim(ClaimTypes.Email, employee.Email),
            new Claim(ClaimTypes.Role, employee.JobRole.JobTitle)
        };

        // get secret key from configuration
        var key = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(_configuration.GetSection("Jwt:Key").Value!));

        // Create signing credentials
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature); // Use HMAC SHA512 for better security

        // Create token descriptor -> meaning: what will be in the token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7), // Token expiration ****(Important)****
            SigningCredentials = creds
        };
        // explanation of token Descriptor:
        // Subject: The claims identity that contains the claims to be included in the token.
        // Expires: The expiration time of the token.
        // SigningCredentials: The credentials used to sign the token, ensuring its integrity and authenticity.


        // Create token handler -> meaning: to create the token (responsible for creating and writing tokens)
        var tokenHandler = new JwtSecurityTokenHandler();

        // Create token
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Return token as string
        return Task.FromResult(tokenHandler.WriteToken(token));
    }

    // I will implement refresh token generation later
    public string GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }
}