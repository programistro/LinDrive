using LinDrive.Contracts.Requestes;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace LinDrive.Web.Controllers;

[ApiController]
[Route("/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(AuthRequest request, CancellationToken cancellationToken)
    {
        
        
        return Ok();
    }
}