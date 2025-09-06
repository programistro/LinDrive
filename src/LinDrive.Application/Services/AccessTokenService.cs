using Atlant.Application.Interfaces;
using LinDrive.Application.Interfaces;
using LinDrive.Core.Interfaces;
using LinDrive.Core.Models;

namespace LinDrive.Application.Services;

public class AccessTokenService : IAccessTokenService
{
    private IAccessTokenRepository _repository { get; set; }
    private IUserService UserService { get; set; }
    
    public AccessTokenService(IAccessTokenRepository repository, IUserService userService)
    {
        _repository = repository;
        UserService = userService;
    }
    
    public async Task<AccessToken> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByIdAsync(id, cancellationToken);

        if (user is not null)
            return user;
        
        return null;
    }

    public async Task<AccessToken> AddAsync(string token, Guid userId, CancellationToken cancellationToken)
    {
        var user = await UserService.GetByIdAsync(userId, cancellationToken);
        
        if (user == null)
            throw new ArgumentException($"User with id {userId} not found", nameof(userId));
        
        AccessToken accessToken = new()
        {
            Id = Guid.NewGuid(),
            Token = token,
            User = user,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(2)
        };

        await _repository.AddAsync(accessToken, cancellationToken);
        return accessToken;
    }

    public async Task UpdateAsync(AccessToken token, CancellationToken cancellationToken)
    {
        await _repository.UpdateAsync(token, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<AccessToken>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}