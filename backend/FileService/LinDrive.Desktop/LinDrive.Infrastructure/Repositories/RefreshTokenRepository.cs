using LinDrive.Core.Models;
using LinDrive.Core.Repositories;
using LinDrive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace LinDrive.Infrastructure.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly ILogger<RefreshTokenRepository> _logger;
    private readonly AppDbContext _context;

    public RefreshTokenRepository(ILogger<RefreshTokenRepository> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<RefreshToken?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<RefreshToken?> GetByTokensync(string token, CancellationToken cancellationToken)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token, cancellationToken);
    }

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        var find = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Id == refreshToken.Id, 
            cancellationToken);
        
        if(find == null)
            return;
        
        _context.RefreshTokens.Update(find);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        var find = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Id == refreshToken.Id, 
            cancellationToken);
        
        if(find == null)
            return;
        
        _context.RefreshTokens.Remove(find);
        await _context.SaveChangesAsync(cancellationToken);
    }
}