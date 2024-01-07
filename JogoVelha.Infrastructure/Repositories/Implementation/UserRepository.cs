using JogoVelha.Domain.Entities;

namespace JogoVelha.Infrastructure.Repositories.Implementation;

public class UserRepository(SqliteDbContext context) : RepositoryBase<User, SqliteDbContext>(context), IUserRepository
{}