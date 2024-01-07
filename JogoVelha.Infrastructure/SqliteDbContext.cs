using JogoVelha.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JogoVelha.Infrastructure;

public class SqliteDbContext(DbContextOptions<SqliteDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}