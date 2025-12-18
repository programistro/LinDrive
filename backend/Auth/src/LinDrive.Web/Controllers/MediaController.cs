using LinDrive.Application.Extensions;
using LinDrive.Application.Interfaces;
using LinDrive.Application.Services.IO.Interfaces;
using LinDrive.Contracts.Dtos.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LinDrive.Web.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class MediaController : ControllerBase
{
    private readonly ILogger<MediaController> _logger;
    private readonly IMediaService _mediaService;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public MediaController(ILogger<MediaController> logger, IMediaService mediaService, IUserService userService, ITokenService tokenService)
    {
        _logger = logger;
        _mediaService = mediaService;
        _userService = userService;
        _tokenService = tokenService;
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

    [HttpGet("{name}")]
    public async Task<IActionResult> GetFile(string name, CancellationToken cancellationToken)
    {
        if (HttpContext.Request.Cookies.TryGetValue("jwt_token", out var jwtToken))
        {
            var findUserId = _tokenService.GetUserIdFromToken(jwtToken, cancellationToken);

            if (findUserId == null)
                return NotFound("Пользователь с указанным id не найден.");

            var findUser = await _userService.GetByEmailAsync(findUserId, cancellationToken);
            
            var path = $@"files/{findUser.Email}/{name}";

            if (!System.IO.File.Exists(path))
                return NotFound();

            var memoryStream = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream);
            }
            memoryStream.Position = 0;

            var contentType = "application/octet-stream";
            return File(memoryStream, contentType, name);
        }
        else
            return Unauthorized("Token not fonud in cookie");
    }
}