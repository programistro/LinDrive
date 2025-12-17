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
    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        return user;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
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

    public string CreatePasswordHash(string password)
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