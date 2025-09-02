using LinDrive.Core.Models;

namespace LinDrive.Application.Interfaces;

public interface IUserService
{
    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);
    
    Task<User> AddAsync(User user, CancellationToken cancellationToken);
    
    Task UpdateAsync(User user, CancellationToken cancellationToken);
    
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
    
    Task<string> CreatePasswordHash(string password);
}