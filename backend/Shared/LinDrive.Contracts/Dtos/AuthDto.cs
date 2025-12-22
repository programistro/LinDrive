namespace LinDrive.Contracts.Dtos;

public record AuthDto
{
    public required string Email { get; set; }
    
    public required string Password { get; set; }
}