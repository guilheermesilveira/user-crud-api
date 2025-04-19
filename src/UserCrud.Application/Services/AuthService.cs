using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserCrud.Application.Contracts.Services;
using UserCrud.Application.DTOs.Auth;
using UserCrud.Application.Notifications;

namespace UserCrud.Application.Services;

public class AuthService : IAuthService
{
    private readonly INotificator _notificator;
    private readonly IConfiguration _configuration;

    public AuthService(INotificator notificator, IConfiguration configuration)
    {
        _notificator = notificator;
        _configuration = configuration;
    }

    public TokenDto? Login(LoginDto dto)
    {
        var name = _configuration["Jwt:Name"];
        var password = _configuration["Jwt:Password"];

        if (dto.Name == name && dto.Password == password)
            return GenerateToken();

        _notificator.Handle("Name and/or password are incorrect");
        return null;
    }

    private TokenDto GenerateToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Role, "User"),
                new(ClaimTypes.Name, _configuration["Jwt:Name"])
            }),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Expires = DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:HoursToExpire"]))
        });

        var encodedToken = tokenHandler.WriteToken(token);

        return new TokenDto
        {
            Token = encodedToken
        };
    }
}