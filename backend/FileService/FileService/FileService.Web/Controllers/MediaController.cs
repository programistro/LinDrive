using LinDrive.Application.Services.IO.Interfaces;
using LinDrive.Contracts.Dtos.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Web.Controllers;

// [Authorize]
[ApiController]
[Route("[controller]")]
public class MediaController : ControllerBase
{
    private readonly ILogger<MediaController> _logger;
    private readonly IMediaService _mediaService;

    public MediaController(ILogger<MediaController> logger, 
        IMediaService mediaService)
    {
        _logger = logger;
        _mediaService = mediaService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok("okay");
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(UploadFileDto dto, CancellationToken cancellationToken)
    {
        await _mediaService.UploadFile(dto, cancellationToken);

        return Ok();
    }
    
    // [HttpGet("{name}")]
    // public async Task<IActionResult> GetFile(string name, CancellationToken cancellationToken)
    // {
        // if (HttpContext.Request.Cookies.TryGetValue("jwt_token", out var jwtToken))
        // {
        //     var findUserId = _tokenService.GetEmailFromToken(jwtToken, cancellationToken);
        //
        //     if (findUserId == null)
        //         return NotFound("Пользователь с указанным id не найден.");
        //
        //     var findUser = await _userService.GetByEmailAsync(findUserId, cancellationToken);
        //     
        //     var path = $@"files/{findUser.Email}/{name}";
        //
        //     if (!System.IO.File.Exists(path))
        //         return NotFound();
        //
        //     var memoryStream = new MemoryStream();
        //     using (var stream = new FileStream(path, FileMode.Open))
        //     {
        //         await stream.CopyToAsync(memoryStream);
        //     }
        //     memoryStream.Position = 0;
        //
        //     var contentType = "application/octet-stream";
        //     return File(memoryStream, contentType, name);
        // }
        // else
        //     return Unauthorized("Token not fonud in cookie");
    // }
}