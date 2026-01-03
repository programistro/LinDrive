using LinDrive.Core.Models;

namespace LinDrive.Core.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<User?> GetByUseridAsync(string userId, CancellationToken cancellationToken);

    Task AddAsync(User user, CancellationToken cancellationToken);
    
    Task UpdateAsync(User user, CancellationToken cancellationToken);
    
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
}
