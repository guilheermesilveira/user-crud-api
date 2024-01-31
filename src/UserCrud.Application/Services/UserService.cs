using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UserCrud.Application.Contracts.Services;
using UserCrud.Application.DTOs.User;
using UserCrud.Application.Notifications;
using UserCrud.Domain.Contracts.Repositories;
using UserCrud.Domain.Models;

namespace UserCrud.Application.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly INotificator _notificator;
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IMapper mapper, INotificator notificator, IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher)
    {
        _mapper = mapper;
        _notificator = notificator;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<UserDto?> Create(CreateUserDto dto)
    {
        if (!dto.Validate(out var validationResult))
        {
            _notificator.Handle(validationResult.Errors);
            return null;
        }

        var getUser = await _userRepository.GetByEmail(dto.Email);
        if (getUser != null)
        {
            _notificator.Handle("Já existe um usuário cadastrado com o email informado.");
            return null;
        }

        var user = _mapper.Map<User>(dto);
        user.Password = _passwordHasher.HashPassword(user, dto.Password);

        _userRepository.Create(user);
        return await CommitChanges() ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<UserDto?> Update(int id, UpdateUserDto dto)
    {
        var getUser = await _userRepository.GetById(id);
        if (getUser == null)
        {
            _notificator.HandleNotFoundResource();
            return null;
        }

        if (!dto.Validate(out var validationResult))
        {
            _notificator.Handle(validationResult.Errors);
            return null;
        }

        if (!string.IsNullOrEmpty(dto.Name))
            getUser.Name = dto.Name;

        if (!string.IsNullOrEmpty(dto.Email))
        {
            var getUserEmail = await _userRepository.GetByEmail(dto.Email);
            if (getUserEmail != null)
            {
                _notificator.Handle("Já existe um usuário cadastrado com o email informado.");
                return null;
            }

            getUser.Email = dto.Email;
        }

        if (!string.IsNullOrEmpty(dto.Password))
        {
            getUser.Password = dto.Password;
            getUser.Password = _passwordHasher.HashPassword(getUser, dto.Password);
        }

        _userRepository.Update(getUser);
        return await CommitChanges() ? _mapper.Map<UserDto>(getUser) : null;
    }

    public async Task Delete(int id)
    {
        var getUser = await _userRepository.GetById(id);
        if (getUser == null)
        {
            _notificator.HandleNotFoundResource();
            return;
        }

        _userRepository.Delete(getUser);
        await CommitChanges();
    }

    public async Task<UserDto?> GetById(int id)
    {
        var getUser = await _userRepository.GetById(id);
        if (getUser != null)
            return _mapper.Map<UserDto>(getUser);

        _notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<UserDto?> GetByEmail(string email)
    {
        var getUser = await _userRepository.GetByEmail(email);
        if (getUser != null)
            return _mapper.Map<UserDto>(getUser);

        _notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<List<UserDto>> GetAll()
    {
        var getUserList = await _userRepository.GetAll();
        return _mapper.Map<List<UserDto>>(getUserList);
    }

    private async Task<bool> CommitChanges()
    {
        if (await _userRepository.UnitOfWork.Commit())
            return true;

        _notificator.Handle("Ocorreu um erro ao salvar as alterações!");
        return false;
    }
}