using Microsoft.EntityFrameworkCore;
using Api.Domain.Entities;

namespace Api.Data.Context;

public sealed class SqliteContext(DbContextOptions<SqliteContext> options) : DbContext(options)
{
    public DbSet<ApplicationUser> Users { get; set; }

}