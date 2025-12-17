using LinDrive.Application.Interfaces;
using LinDrive.Contracts.Dtos;
using LinDrive.Contracts.Requestes;
using LinDrive.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace LinDrive.Web.Controllers;

[ApiController]
[Route("/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;

    public AuthController(ILogger<AuthController> logger, 
        IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
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
}