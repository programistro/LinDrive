namespace LinDrive.Infrastructure;

public record JwtOptions
{
    public required string Issuer { get; set; }
    
    public required string Audience { get; set; }
    
    public required string Key { get; set; }
}