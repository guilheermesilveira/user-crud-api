using UserCrud.Application.DTOs.User;

namespace UserCrud.Application.Contracts.Services;

public interface IUserService
{
    Task<UserDto?> Create(CreateUserDto dto);
    Task<UserDto?> Update(int id, UpdateUserDto dto);
    Task Delete(int id);
    Task<UserDto?> GetById(int id);
    Task<UserDto?> GetByEmail(string email);
    Task<List<UserDto>> GetAll();
}