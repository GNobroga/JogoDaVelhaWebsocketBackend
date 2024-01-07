using JogoVelha.Domain.Entities;
using JogoVelha.Infrastructure.Repositories.Base;

namespace JogoVelha.Infrastructure.Repositories;

public interface IUserRepository : IRepository<User> {

    Task<User?> FindByEmail(string email);

    Task<User?> FindByUsername(string username);

    Task<bool> ExistsEmail(string email);

    Task<bool> ExistsUsername(string username);
}
