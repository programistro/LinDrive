using System.Security.Cryptography;
using System.Text;
using LinDrive.Application.Interfaces;
using LinDrive.Core.Interfaces;
using LinDrive.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace LinDrive.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;
    private readonly IMemoryCache _memoryCache;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger, IMemoryCache memoryCache)
    {
        _userRepository = userRepository;
        _logger = logger;
        _memoryCache = memoryCache;
    }

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _memoryCache.TryGetValue(id, out User? user);

        if (user == null)
        {
            user = await _userRepository.GetByIdAsync(id, cancellationToken);

            if (user != null)
            {
                _memoryCache.Set(user.Id, user, new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(5)));
                _logger.LogInformation("get user");
                return user;
            }
        }
        _logger.LogInformation("get user form cashe");
        return user;
    }

    public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is not null)
        {
            return user;
        }

        return null;
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken)
    {
        await _userRepository.AddAsync(user, cancellationToken);
        return user;
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        await _userRepository.UpdateAsync(user, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await _userRepository.DeleteAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
    {        
        return await _userRepository.GetAllAsync(cancellationToken);
    }

    public async Task<string> CreatePasswordHash(string password)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] hashValue = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashValue.Length; i++)
            {
                builder.Append(hashValue[i]
                    .ToString("x2")); // Преобразуем байты хэша в шестнадцатеричное представление
            }

            return builder.ToString();
        }
    }
}