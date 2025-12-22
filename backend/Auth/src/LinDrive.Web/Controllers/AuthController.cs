using LinDrive.Application.Interfaces;
using LinDrive.Contracts.Dtos;
using LinDrive.Contracts.Requestes;
using LinDrive.Core.Models;
using LinDrive.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    public AuthController(ILogger<AuthController> logger, 
        IAuthService authService, 
        ITokenService tokenService)
    {
        _logger = logger;
        _authService = authService;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(AuthRequest request, CancellationToken cancellationToken)
    {
        var userAgent = HttpContext.Request.Headers["User-Agent"];
        var ip = HttpContext.Request.Headers["X-Forwarded-For"];

        var authResult =
            await _authService.Authenticate(request, AuthType.Register, new UserAgent(ip: ip, agent: userAgent),
                cancellationToken); 
        
        if(authResult.IsFailure)
            return BadRequest(authResult.Error);
        
        return Ok(authResult.Value);
    }

    [HttpPost("login")]
    private async Task<IActionResult> Login(AuthRequest request, CancellationToken cancellationToken)
    {
        var userAgent = HttpContext.Request.Headers["User-Agent"];
        var ip = HttpContext.Request.Headers["X-Forwarded-For"];

        var authResult =
            await _authService.Authenticate(request, AuthType.Login, new UserAgent(ip: ip, agent: userAgent),
                cancellationToken); 
        
        if(authResult.IsFailure)
            return BadRequest(authResult.Error);
        
        return Ok(authResult.Value);
    }

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