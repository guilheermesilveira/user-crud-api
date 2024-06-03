using UserCrud.Application.DTOs.Auth;

namespace UserCrud.Application.Contracts.Services;

public interface IAuthService
{
    TokenDto? Login(LoginDto dto);
}