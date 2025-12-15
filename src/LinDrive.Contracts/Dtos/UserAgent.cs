namespace LinDrive.Contracts.Dtos;

public record UserAgent
{
    public string Ip { get; set; }
    
    public string Agent { get; set; }
}