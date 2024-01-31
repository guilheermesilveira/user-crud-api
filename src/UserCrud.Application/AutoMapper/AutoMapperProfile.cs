using AutoMapper;
using UserCrud.Application.DTOs.User;
using UserCrud.Domain.Models;

namespace UserCrud.Application.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateUserDto, User>().ReverseMap();
        CreateMap<UpdateUserDto, User>().ReverseMap();
        CreateMap<UserDto, User>().ReverseMap();
    }
}