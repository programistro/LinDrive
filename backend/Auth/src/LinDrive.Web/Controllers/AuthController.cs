using LinDrive.Application.Interfaces;
using LinDrive.Contracts.Dtos;
using LinDrive.Contracts.Requestes;
using LinDrive.Core.Models;
using LinDrive.Shared.Enums;
using LinDrive.Shared.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace LinDrive.Web.Controllers;

[ApiController]
[Route("/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    private readonly ICryptoService _cryptoService;
    private readonly IDistributedCache _cache;
        
    public AuthController(ILogger<AuthController> logger, 
        IAuthService authService, 
        ITokenService tokenService, 
        ICryptoService cryptoService, 
        IDistributedCache cache)
    {
        _logger = logger;
        _authService = authService;
        _tokenService = tokenService;
        _cryptoService = cryptoService;
        _cache = cache;
    }

    [HttpGet("challenge/{userId}")]
    public async Task<IActionResult> Chanllenge(string userId, CancellationToken cancellationToken)
    {
        var nonce = _cryptoService.RandomNonce();
        var key = $"challenge:{userId}:{nonce}";
        await _cache.SetStringAsync(key, DateTime.UtcNow.ToString(), new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
        }, cancellationToken);
        return Ok(nonce);
    }

    [HttpPost("verify")]
    public async Task<IActionResult> Verify(AuthRequest request, CancellationToken cancellationToken)
    {
        var key = $"challenge:{request.UserId}:{request.Challenge}";
        var stored = await _cache.GetStringAsync(key, cancellationToken);
        if(string.IsNullOrEmpty(stored))
            return Unauthorized("Nonce not found");

        var userAgent = HttpContext.Request.Headers["User-Agent"];
        var ip = HttpContext.Request.Headers["X-Forwarded-For"];

        var result = await _authService.Authenticate(request, new UserAgent(userAgent, ip), cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error);
        }

        var jwt = _tokenService.GenerateJwtToken(result.Value);
        
        return Ok(new
        {
            AccessToken = jwt,
            RefreshToken = result.Value.AccessToken,
        });
    }
    
    // [HttpPost("register")]
    // public async Task<IActionResult> Register(AuthRequest request, CancellationToken cancellationToken)
    // {
    //     var userAgent = HttpContext.Request.Headers["User-Agent"];
    //     var ip = HttpContext.Request.Headers["X-Forwarded-For"];
    //
    //     var authResult =
    //         await _authService.Authenticate(request, AuthType.Register, new UserAgent(ip: ip, agent: userAgent),
    //             cancellationToken); 
    //     
    //     if(authResult.IsFailure)
    //         return BadRequest(authResult.Error);
    //     
    //     return Ok(authResult.Value);
    // }
    //
    // [HttpPost("login")]
    // private async Task<IActionResult> Login(AuthRequest request, CancellationToken cancellationToken)
    // {
    //     var userAgent = HttpContext.Request.Headers["User-Agent"];
    //     var ip = HttpContext.Request.Headers["X-Forwarded-For"];
    //
    //     var authResult =
    //         await _authService.Authenticate(request, AuthType.Login, new UserAgent(ip: ip, agent: userAgent),
    //             cancellationToken); 
    //     
    //     if(authResult.IsFailure)
    //         return BadRequest(authResult.Error);
    //     
    //     return Ok(authResult.Value);
    // }

    [SwaggerResponse(200, "Success", typeof(User))]
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me(CancellationToken cancellationToken)
    {
        HttpContext.Request.Cookies.TryGetValue("jwt_token", out var jwtToken);
        if(jwtToken == null)
            return Unauthorized();

        var user = await _tokenService.GetUserFromToken(jwtToken, cancellationToken);
        
        if(user.IsFailure && user.ErrorCode == 404)
            return NotFound(user.Error);
        
        return Ok(user.Value);
    }
}