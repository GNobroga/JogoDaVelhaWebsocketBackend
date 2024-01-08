using JogoVelha.Domain.DTOs;
using JogoVelha.Domain.Entities;

namespace JogoVelha.Service.Interfaces;

public interface IAccountService 
{
    Task<UserDTO.UserResponse> Update(ForgotAccountDTO dto);
}