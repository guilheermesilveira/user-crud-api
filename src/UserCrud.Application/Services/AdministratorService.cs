using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserCrud.Application.Contracts.Services;
using UserCrud.Application.DTOs.Administrator;
using UserCrud.Application.Notifications;

namespace UserCrud.Application.Services;

public class AdministratorService : IAdministratorService
{
    private readonly INotificator _notificator;
    private readonly IConfiguration _configuration;

    public AdministratorService(INotificator notificator, IConfiguration configuration)
    {
        _notificator = notificator;
        _configuration = configuration;
    }

    public AdministratorTokenDto? Login(AdministratorLoginDto dto)
    {
        var tokenLogin = _configuration["Jwt:Login"];
        var tokenPassword = _configuration["Jwt:Password"];

        if (dto.Login == tokenLogin && dto.Password == tokenPassword)
            return GenerateToken();

        _notificator.Handle("Login e/ou senha estão incorretos.");
        return null;
    }

    private AdministratorTokenDto GenerateToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Role, "User"),
                new(ClaimTypes.Name, _configuration["Jwt:Login"])
            }),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Expires = DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:HoursToExpire"]))
        });

        var encodedToken = tokenHandler.WriteToken(token);

        return new AdministratorTokenDto
        {
            Token = encodedToken
        };
    }
}