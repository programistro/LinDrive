using LinDrive.Core.Interfaces;
using LinDrive.Core.Models;
using LinDrive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Atlant.Infractructure.Repositories;

public class AccessTokenRepository : IAccessTokenRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<AccessTokenRepository> _logger;

    public AccessTokenRepository(AppDbContext context, ILogger<AccessTokenRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<AccessToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.AccessTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<AccessToken?> GetByAsync(string token, CancellationToken cancellationToken)
    {
        return await _context.AccessTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
    }

    public async Task AddAsync(AccessToken token, CancellationToken cancellationToken)
    {
        await _context.AccessTokens.AddAsync(token,  cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(AccessToken token, CancellationToken cancellationToken)
    {
        _context.AccessTokens.Attach(token);
        _context.Entry(token).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid tokenId, CancellationToken cancellationToken)
    {
        var token = await _context.AccessTokens.FirstOrDefaultAsync(x => x.Id == tokenId);

        if (token != null)
        {
            _context.AccessTokens.Remove(token);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<IEnumerable<AccessToken>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.AccessTokens.ToListAsync(cancellationToken);
    }
}