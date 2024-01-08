using JogoVelha.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JogoVelha.Infrastructure.Repositories.Implementation;

public class UserRepository(AppDbContext context) : RepositoryBase<User, AppDbContext>(context), IUserRepository
{
    public async Task<bool> ExistsEmail(string email)
    {
        return (await FindByEmail(email)) is not null;
    }

    public async Task<bool> ExistsUsername(string username)
    {
        return (await FindByUsername(username)) is not null;
    }

    public async Task<User?> FindByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(
            user =>  EF.Functions.Like(user.Email, email.ToLower()));  

    }

    public async Task<User?> FindByUsername(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(
            user => EF.Functions.Like(user.Username, username.ToLower())); 
    }
}