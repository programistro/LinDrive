using System.Text.Json;
using LinDrive.Core.Interfaces;
using LinDrive.Core.Models;
using LinDrive.Infrastructure.Data;
using LinDrive.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LinDrive.Infrastructure.Repositories;

public class AccessTokenRepository : IAccessTokenRepository
{
    private readonly ILogger<AccessTokenRepository> _logger;
    private readonly IDistributedCache _cache;

    private readonly JsonSerializerSettings _options = new()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        NullValueHandling = NullValueHandling.Include
    };
    
    public AccessTokenRepository(ILogger<AccessTokenRepository> logger, IDistributedCache cache)
    {
        _logger = logger;
        _cache = cache;
    }

    public async Task<Result<AccessToken>?> GetByTokenAsync(string token, CancellationToken cancellationToken)
    {
        var findString =  await _cache.GetStringAsync(token, cancellationToken);
        if(string.IsNullOrWhiteSpace(findString))
            return Result<AccessToken>.Failure("AccessToken not found");
        _logger.LogDebug($"Getting access token from cache: {findString}");
        var findToken = JsonConvert.DeserializeObject<AccessToken>(findString, _options);
        
        if(findToken == null)
            return Result<AccessToken>.Failure("AccessToken not found");
        if(!findToken.IsExpired)
            return Result<AccessToken>.Failure("AccessToken expired at " + findToken.ExpiresAt);
        if (findToken.ExpiresAt < DateTime.UtcNow)
        {
            findToken.Revoked = DateTime.UtcNow;
            var tokenStrnig = JsonConvert.SerializeObject(findToken, _options);
            await _cache.SetStringAsync(findToken.Token, tokenStrnig, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(10)
            }, cancellationToken);
            return Result<AccessToken>.Failure("Срок действия сессии истечен");
        }

        return Result<AccessToken>.Success(findToken);
    }

    public async Task AddAsync(AccessToken token, CancellationToken cancellationToken)
    {
        var tokenStrnig = JsonConvert.SerializeObject(token, _options);
        _logger.LogInformation($"Adding refresh token {tokenStrnig}");
        await _cache.SetStringAsync(token.Token, tokenStrnig, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(10)
        }, cancellationToken);

        var listKey = $"list:{token.UserId}";
        var listString = await _cache.GetStringAsync(listKey, cancellationToken);
        var tokenList = string.IsNullOrEmpty(listString)
            ? new List<AccessToken>()
            : JsonConvert.DeserializeObject<List<AccessToken>>(listString, _options) ?? new List<AccessToken>();
        
        if (!tokenList.Contains(token))
        {
            tokenList.Add(token);
            var updatedListJson = JsonConvert.SerializeObject(tokenList, _options);
            await _cache.SetStringAsync(listKey, updatedListJson, cancellationToken);
        }
    }

    public async Task UpdateAsync(AccessToken token, CancellationToken cancellationToken)
    {
        var tokenStrnig = JsonConvert.SerializeObject(token, _options);
        _logger.LogInformation($"Updating refresh token {tokenStrnig}");
        await _cache.SetStringAsync(token.Token, tokenStrnig, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(10)
        }, cancellationToken);

        var listKey = $"list:{token.UserId}";
        var listString = await _cache.GetStringAsync(listKey, cancellationToken);
        var tokenList = string.IsNullOrEmpty(listString)
            ? new List<AccessToken>()
            : JsonConvert.DeserializeObject<List<AccessToken>>(listString, _options) ?? new List<AccessToken>();

        var index = tokenList.FindIndex(t => t.Token == token.Token);

        if (index >= 0)
        {
            // Заменяем существующий токен на новый
            tokenList[index] = token;

            // Сохраняем обновлённый список в кэш
            var updatedListJson = JsonConvert.SerializeObject(tokenList, _options);
            await _cache.SetStringAsync(listKey, updatedListJson, cancellationToken);
        }
        else
        {
            // Если токена не было — добавляем
            tokenList.Add(token);
            var updatedListJson = JsonConvert.SerializeObject(tokenList, _options);
            await _cache.SetStringAsync(listKey, updatedListJson, cancellationToken);
        }
    }

    public async Task DeleteAsync(string token, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Deleted refresh token {token}");
        await _cache.RemoveAsync(token, cancellationToken);
    }

    public async Task DisableAllRefreshAsync(List<AccessToken> refreshTokens, CancellationToken cancellationToken, string? refreshToken = null)
    {
        foreach (var item in refreshTokens)
        {
            if(refreshToken != null && item.Token == refreshToken)
                continue;
            
            item.Revoked = DateTime.UtcNow;
            await UpdateAsync(item, cancellationToken);
        }
    }
}