using LinDrive.Contracts.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LinDrive.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;

    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Register(AuthDto dto, CancellationToken cancellationToken)
    {
        
        
        return Ok();
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadFile()
    {
        return Ok();
    }
}