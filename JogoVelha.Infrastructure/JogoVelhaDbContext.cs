using JogoVelha.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JogoVelha.Infrastructure;

public class JogoVelhaDbContext(DbContextOptions<JogoVelhaDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}