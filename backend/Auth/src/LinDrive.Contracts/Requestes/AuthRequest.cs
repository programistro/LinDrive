namespace LinDrive.Contracts.Requestes;

public record AuthRequest
{
    public required string Email { get; set; }
    
    public required string Password { get; set; }
}