using LinDrive.Application.Extensions;
using LinDrive.Application.Interfaces;
using LinDrive.Application.IO.Interfaces;
using LinDrive.Contracts.Dtos.IO;
using Microsoft.AspNetCore.Mvc;

namespace LinDrive.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class MediaController : ControllerBase
{
    private readonly ILogger<MediaController> _logger;
    private readonly IMediaService _mediaService;
    private readonly IUserService _userService;

    public MediaController(ILogger<MediaController> logger, IMediaService mediaService, IUserService userService)
    {
        _logger = logger;
        _mediaService = mediaService;
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(UploadFileDto dto, CancellationToken cancellationToken)
    {
        await _mediaService.UploadFile(dto, cancellationToken);

        return Ok();
    }

    [HttpGet("all-files")]
    public async Task<IActionResult> GetAllFiles(string email,CancellationToken cancellationToken)
    {
        var findUser = await _userService.GetByEmailAsync(email, cancellationToken);
        
        if(findUser == null)
            return NotFound();
        
        var files = findUser.Files.FilesToDto();
        
        return Ok(files);
    }
}