namespace LinDrive.Contracts.Requestes;

public record AuthRequest
{
    public required string UserId { get; set; }
    
    public required string Challenge { get; set; }
    
    public required string PubKey { get; set; }
}