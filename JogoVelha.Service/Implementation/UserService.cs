using AutoMapper;
using JogoVelha.Domain.DTOs;
using JogoVelha.Infrastructure.Repositories;
using JogoVelha.Service.Interfaces;

namespace JogoVelha.Service.Implementation;

public class UserService(IUserRepository repository, IMapper mapper) : IUserService
{

    public Task<UserDTO.UserResponse> Create(UserDTO.UserRequest record)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserDTO.UserResponse>> FindAll()
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO.UserResponse> FindById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<UserDTO.UserResponse> Update(int id, UserDTO.UserRequest record)
    {
        throw new NotImplementedException();
    }
}