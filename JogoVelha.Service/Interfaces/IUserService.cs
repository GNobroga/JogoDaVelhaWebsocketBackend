using JogoVelha.Domain.DTOs;
using JogoVelha.Domain.Entities;

namespace JogoVelha.Service.Interfaces;

public interface IUserService
{

    Task<IEnumerable<UserDTO.UserResponse>> FindAll();

    Task<UserDTO.UserResponse> FindById(int id);

    Task<UserDTO.UserResponse> Create(UserDTO.UserRequest record);

    Task<UserDTO.UserResponse> Update(int id, UserDTO.UserRequest record);

    Task<bool> Delete(int id);
}