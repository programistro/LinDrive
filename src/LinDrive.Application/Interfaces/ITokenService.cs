using LinDrive.Application.Results;
using LinDrive.Contracts.Dtos;
using LinDrive.Core.Models;

namespace LinDrive.Application.Interfaces;

public interface ITokenService
{
    Task<Result<AccessToken>> ValidateToken(string token, CancellationToken cancellationToken);
    
    Task<AccessToken> GenerateToken(User user, UserAgent agent, CancellationToken cancellationToken);

    public string GenerateJwtToken(User user);
    
    string? GetUserIdFromToken(string token, CancellationToken cancellationToken);
    
    string? GetRoleFromToken(string token, CancellationToken cancellationToken);
    
    Task DeleteTokenAsync(string token, CancellationToken cancellationToken);

    Task<IEnumerable<string>> GetTokensByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}