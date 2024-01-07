using JogoVelha.Domain.Entities;

namespace JogoVelha.Infrastructure.Repositories.Implentation;

public class UserRepository(SqliteDbContext context) : RepositoryBase<User, SqliteDbContext>(context), IUserRepository
{}