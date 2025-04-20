using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UserCrud.Application.Contracts.Services;
using UserCrud.Application.DTOs.User;
using UserCrud.Application.Notifications;
using UserCrud.Application.Validations;
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
        if (!await ValidationsToCreate(dto))
            return null;

        var user = _mapper.Map<User>(dto);
        user.Password = _passwordHasher.HashPassword(user, dto.Password);
        _userRepository.Create(user);

        return await CommitChanges() ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task<UserDto?> Update(int id, UpdateUserDto dto)
    {
        if (!await ValidationsToUpdate(id, dto))
            return null;

        var user = await _userRepository.GetById(id);
        user!.Name = dto.Name;
        user.Email = dto.Email;
        user.Password = dto.Password;
        user.Password = _passwordHasher.HashPassword(user, dto.Password);
        _userRepository.Update(user);

        return await CommitChanges() ? _mapper.Map<UserDto>(user) : null;
    }

    public async Task Delete(int id)
    {
        var user = await _userRepository.GetById(id);
        if (user == null)
        {
            _notificator.HandleNotFoundResource();
            return;
        }

        _userRepository.Delete(user);
        await CommitChanges();
    }

    public async Task<UserDto?> GetById(int id)
    {
        var user = await _userRepository.GetById(id);
        if (user != null)
            return _mapper.Map<UserDto>(user);

        _notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<UserDto?> GetByEmail(string email)
    {
        var user = await _userRepository.GetByEmail(email);
        if (user != null)
            return _mapper.Map<UserDto>(user);

        _notificator.HandleNotFoundResource();
        return null;
    }

    public async Task<List<UserDto>> GetAll()
    {
        var users = await _userRepository.GetAll();
        return _mapper.Map<List<UserDto>>(users);
    }

    private async Task<bool> ValidationsToCreate(CreateUserDto dto)
    {
        var user = _mapper.Map<User>(dto);
        var validator = new UserValidator();

        var validationResult = await validator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            _notificator.Handle(validationResult.Errors);
            return false;
        }

        var userExist = await _userRepository.GetByEmail(user.Email);
        if (userExist != null)
        {
            _notificator.Handle("There is already a registered user with the email provided");
            return false;
        }

        return true;
    }

    private async Task<bool> ValidationsToUpdate(int id, UpdateUserDto dto)
    {
        if (id != dto.Id)
        {
            _notificator.Handle("The ID given to the URL must be the same as the ID given in the JSON");
            return false;
        }

        var userExist = await _userRepository.GetById(id);
        if (userExist == null)
        {
            _notificator.HandleNotFoundResource();
            return false;
        }

        var user = _mapper.Map<User>(dto);
        var validator = new UserValidator();

        var validationResult = await validator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            _notificator.Handle(validationResult.Errors);
            return false;
        }

        return true;
    }

    private async Task<bool> CommitChanges()
    {
        if (await _userRepository.UnitOfWork.Commit())
            return true;

        _notificator.Handle("An error occurred while saving changes");
        return false;
    }
}