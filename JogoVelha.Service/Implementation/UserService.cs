using System.Runtime.CompilerServices;
using System.Text.Json;
using AutoMapper;
using JogoVelha.Domain.DTOs;
using JogoVelha.Domain.Entities;
using JogoVelha.Infrastructure.Repositories;
using JogoVelha.Service.Interfaces;
using Microsoft.Extensions.Caching.Distributed;

namespace JogoVelha.Service.Implementation;

public class UserService(IUserRepository repository, IMapper mapper, IDistributedCache cache) : IUserService
{

    public async Task<UserDTO.UserResponse> Create(UserDTO.UserRequest record)
    {   
        if (record.ConfirmPassword is null)
        {
            throw new ArgumentException("É necessário ter a senha de confirmação para se cadastrar.");
        }
        
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
        string cacheKey = "users";
        string? cachedUsersJson = await cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedUsersJson)) 
            return JsonSerializer.Deserialize<List<UserDTO.UserResponse>>(cachedUsersJson)!;

        var users = mapper.Map<List<UserDTO.UserResponse>>(await repository.FindAllAsync());

        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(users));

        return users;
    }

    public async Task<UserDTO.UserResponse> FindById(int id)
    {
        string cacheKey = $"user:{id}";
        string? cachedUserJson = await cache.GetStringAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedUserJson)) 
            return JsonSerializer.Deserialize<UserDTO.UserResponse>(cachedUserJson)!;

        var user = await GetUserOrThrowException(id);
        var response = mapper.Map<UserDTO.UserResponse>(user);

        await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(response));

        return response;
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

        if (!string.Equals(user.Email, dto.Email, StringComparison.InvariantCultureIgnoreCase) && await repository.ExistsEmail(dto.Email)) 
        {
            throw new ArgumentException($"Email ou username estão em uso.");
        }

        if (!string.Equals(user.Username, dto.Username, StringComparison.InvariantCultureIgnoreCase) && await repository.ExistsUsername(dto.Username)) 
        {
            throw new ArgumentException($"Email ou username estão em uso.");
        }
    }
}