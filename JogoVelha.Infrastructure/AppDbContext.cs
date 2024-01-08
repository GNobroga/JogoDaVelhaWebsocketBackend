using JogoVelha.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JogoVelha.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}