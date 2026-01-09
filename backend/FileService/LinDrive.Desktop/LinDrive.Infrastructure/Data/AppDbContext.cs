using LinDrive.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace LinDrive.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}