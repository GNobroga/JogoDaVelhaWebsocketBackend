using AutoMapper;
using JogoVelha.Domain.DTOs;
using JogoVelha.Domain.Entities;
using JogoVelha.Infrastructure.Repositories;
using JogoVelha.Service.Interfaces;

namespace JogoVelha.Service.Implementation;

public class UserService(IUserRepository repository, IMapper mapper) : IUserService
{

    public async Task<UserDTO.UserResponse> Create(UserDTO.UserRequest record)
    {   
        
        await ExistEmailOrUsername(record.Email, record.Username);
        var entity = mapper.Map<User>(record);
        entity.Id = default;
        entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);
        return mapper.Map<UserDTO.UserResponse>(await repository.CreateAsync(entity));
    }

    public async Task<bool> Delete(int id)
    {
       await GetUserOrThrowException(id);
       return await repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<UserDTO.UserResponse>> FindAll()
    {
        return mapper.Map<List<UserDTO.UserResponse>>(await repository.FindAllAsync());
    }

    public async Task<UserDTO.UserResponse> FindById(int id)
    {
        var user = await GetUserOrThrowException(id);
        return mapper.Map<UserDTO.UserResponse>(user);
    }

    public async Task<UserDTO.UserResponse> Update(int id, UserDTO.UserRequest record)
    {
        var user = await GetUserOrThrowException(id);
        await ExistEmailOrUsername(user, record);
        var password = user.Password;
        mapper.Map(record, user);
        user.Id = id;
        if (!BCrypt.Net.BCrypt.Verify(record.Password, password)) 
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(record.Password);
        }
        await repository.UpdateAsync(user);
        return mapper.Map<UserDTO.UserResponse>(user);
    }

    private async Task<User> GetUserOrThrowException(int id)
    {
        return await repository.FindByIdAsync(id) 
            ?? throw new ArgumentException($"Usuário com ${id} não encontrado.");
    }

    private async Task ExistEmailOrUsername(string email, string username)
    {
        if (await repository.ExistsEmail(email) || await repository.ExistsUsername(username)) 
        {
            throw new ArgumentException($"Email ou username estão em uso.");
        }
    }

    private async Task ExistEmailOrUsername(User user, UserDTO.UserRequest dto)
    {

        if (!string.Equals(user.Email, dto.Email, StringComparison.OrdinalIgnoreCase) && await repository.ExistsEmail(dto.Email)) 
        {
            throw new ArgumentException($"Email ou username estão em uso.");
        }

        if (!string.Equals(user.Username, dto.Username, StringComparison.OrdinalIgnoreCase) && await repository.ExistsUsername(dto.Username)) 
        {
            throw new ArgumentException($"Email ou username estão em uso.");
        }
    }
}