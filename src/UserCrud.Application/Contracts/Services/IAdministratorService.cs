using UserCrud.Application.DTOs.Administrator;

namespace UserCrud.Application.Contracts.Services;

public interface IAdministratorService
{
    AdministratorTokenDto? Login(AdministratorLoginDto dto);
}