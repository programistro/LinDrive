namespace LinDrive.Contracts.Dtos.IO;

public record UserFileResponse
{
    public string Name { get; set; }
    
    public string Path { get; set; }
    
    public string Extension { get; set; }
    
    public string ContentType { get; set; }
    
    public long Length { get; set; }
}