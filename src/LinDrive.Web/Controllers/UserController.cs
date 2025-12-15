using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LinDrive.Application.Interfaces;
using LinDrive.Contracts.Dtos;
using LinDrive.Contracts.Dtos.IO;
using LinDrive.Core.Models;
using LinDrive.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LinDrive.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(AuthDto dto, CancellationToken cancellationToken)
    {
        var findUser = await _userService.GetByEmailAsync(dto.Email, cancellationToken);

        if (findUser == null)
        {
            User newUser = new()
            {
                Email = dto.Email,
                SeedHash = _userService.CreatePasswordHash(dto.Password),
                Id = Guid.NewGuid(),
            };
            
            await _userService.AddAsync(newUser, cancellationToken);
            
            return Ok(newUser);
        }
        
        return BadRequest();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(AuthDto dto, CancellationToken cancellationToken)
    {
        var findUser = await _userService.GetByEmailAsync(dto.Email, cancellationToken);
        
        if(findUser == null)
            return NotFound();

        var claims = new List<Claim> { new Claim(ClaimTypes.Email, findUser.Email) };
        
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            
        var token= new JwtSecurityTokenHandler().WriteToken(jwt);
        
        return Ok(jwt);
    }
}