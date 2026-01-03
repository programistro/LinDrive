using LinDrive.Core.Interfaces;
using LinDrive.Core.Models;
using LinDrive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LinDrive.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Id == userId,  cancellationToken);
    }

    public async Task<User?> GetByUseridAsync(string userId, CancellationToken cancellationToken)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.UserId == userId,  cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId,  cancellationToken);

        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Users.ToListAsync(cancellationToken);
    }
}