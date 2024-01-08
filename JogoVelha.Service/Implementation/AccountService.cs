using AutoMapper;
using JogoVelha.Domain.DTOs;
using JogoVelha.Infrastructure.Repositories;
using JogoVelha.Service.Interfaces;

namespace JogoVelha.Service.Implementation;

public class AccountService(IUserRepository repository, IMapper mapper) : IAccountService
{
    public async Task<UserDTO.UserResponse> Update(ForgotAccountDTO dto)
    {
        var user = await repository.FindByEmail(dto.Email) ??
            throw new ArgumentException("Email não existe");
        
        if (!string.Equals(dto.Password, dto.ConfirmPassword)) 
            throw new ArgumentException("A confirm senha não é igual a password");
        
        user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        await repository.UpdateAsync(user);
        
        return mapper.Map<UserDTO.UserResponse>(user);
    }
}