using AutoMapper;
using JogoVelha.Domain.DTOs;
using JogoVelha.Domain.Entities;

namespace JogoVelha.Domain.AutoMapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile() 
    {
        CreateMap<UserDTO.UserRequest, User>().ReverseMap();
        CreateMap<User, UserDTO.UserResponse>();
    }
}