using FileService.Appliation.Services.IO;
using LinDrive.Application.Services.IO.Interfaces;
using LinDrive.Contracts.Dtos.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Web.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class MediaController : ControllerBase
{
    private readonly ILogger<MediaController> _logger;
    private readonly IMediaService _mediaService;
    private readonly IS3Service _s3Service;

    public MediaController(ILogger<MediaController> logger, 
        IMediaService mediaService, 
        IS3Service s3Service)
    {
        _logger = logger;
        _mediaService = mediaService;
        _s3Service = s3Service;
    }

    [HttpGet("buckets")]
    public async Task<IActionResult> GetBuckets(CancellationToken cancellationToken)
    {
        var result = await _s3Service.GetListBuckets(cancellationToken);
        
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(UploadFileDto dto, CancellationToken cancellationToken)
    {
        var result = await _s3Service.PutFile(bucketName:dto.Bucket, dto.File.FileName, dto.File.OpenReadStream(), cancellationToken);

        return Ok(result);
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