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
        if (!await ValidationsToCreateUser(dto))
            return null;

        var createUser = _mapper.Map<User>(dto);
        createUser.Password = _passwordHasher.HashPassword(createUser, dto.Password);

        _userRepository.Create(createUser);
        return await CommitChanges() ? _mapper.Map<UserDto>(createUser) : null;
    }

    public async Task<UserDto?> Update(int id, UpdateUserDto dto)
    {
        if (!await ValidationsToUpdateUser(id, dto))
            return null;

        var updateUser = await _userRepository.GetById(id);
        MappingToUpdateUser(updateUser!, dto);

        _userRepository.Update(updateUser!);
        return await CommitChanges() ? _mapper.Map<UserDto>(updateUser) : null;
    }

    public async Task Delete(int id)
    {
        var deleteUser = await _userRepository.GetById(id);
        if (deleteUser == null)
        {
            _notificator.HandleNotFoundResource();
            return;
        }

        _userRepository.Delete(deleteUser);
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

    private async Task<bool> ValidationsToCreateUser(CreateUserDto dto)
    {
        var user = _mapper.Map<User>(dto);
        var userValidator = new ValidatorToCreateUser();

        var validationResult = await userValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            _notificator.Handle(validationResult.Errors);
            return false;
        }

        var existingUserByEmail = await _userRepository.GetByEmail(user.Email);
        if (existingUserByEmail != null)
        {
            _notificator.Handle("Já existe um usuário cadastrado com o email informado.");
            return false;
        }

        return true;
    }

    private async Task<bool> ValidationsToUpdateUser(int id, UpdateUserDto dto)
    {
        var existingUserById = await _userRepository.GetById(id);
        if (existingUserById == null)
        {
            _notificator.HandleNotFoundResource();
            return false;
        }

        var user = _mapper.Map<User>(dto);
        var userValidator = new ValidatorToUpdateUser();

        var validationResult = await userValidator.ValidateAsync(user);
        if (!validationResult.IsValid)
        {
            _notificator.Handle(validationResult.Errors);
            return false;
        }

        if (!string.IsNullOrEmpty(user.Email))
        {
            var existingUserByEmail = await _userRepository.GetByEmail(user.Email);
            if (existingUserByEmail != null)
            {
                _notificator.Handle("Já existe um usuário cadastrado com o email informado.");
                return false;
            }
        }

        if (string.IsNullOrEmpty(user.Name) && string.IsNullOrEmpty(user.Email) && string.IsNullOrEmpty(user.Password))
        {
            _notificator.Handle("Nenhum campo fornecido para atualização.");
            return false;
        }

        return true;
    }

    private void MappingToUpdateUser(User user, UpdateUserDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Name))
            user.Name = dto.Name;

        if (!string.IsNullOrEmpty(dto.Email))
            user.Email = dto.Email;

        if (!string.IsNullOrEmpty(dto.Password))
        {
            user.Password = dto.Password;
            user.Password = _passwordHasher.HashPassword(user, dto.Password);
        }
    }

    private async Task<bool> CommitChanges()
    {
        if (await _userRepository.UnitOfWork.Commit())
            return true;

        _notificator.Handle("Ocorreu um erro ao salvar as alterações.");
        return false;
    }
}