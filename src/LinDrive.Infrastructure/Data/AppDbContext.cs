using LinDrive.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LinDrive.Infrastructure.Data;

public class AppDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AppDbContext> _logger;

    public AppDbContext(DbContextOptions<AppDbContext> options, 
        IConfiguration configuration, ILogger<AppDbContext> logger) : base(options)
    {
        _configuration = configuration;
        _logger = logger;
    }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<AccessToken> AccessTokens { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(u => u.AccessTokens)
            .WithOne(a => a.User)
            .IsRequired();
    }
}