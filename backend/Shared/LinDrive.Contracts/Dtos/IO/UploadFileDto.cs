using Microsoft.AspNetCore.Http;

namespace LinDrive.Contracts.Dtos.IO;

public record UploadFileDto
{
    public required IFormFile File { get; set; }
    
    public required string Bucket { get; set; }
}