namespace FileService.Core;

public record S3Options
{
    public required string Bucket { get; set; }
    
    public required string AccessKey { get; set; }
    
    public required string SecretKey { get; set; }
    
    public required string ServiceUrl { get; set; }
}