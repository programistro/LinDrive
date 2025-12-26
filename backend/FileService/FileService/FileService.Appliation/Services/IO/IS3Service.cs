using LinDrive.Shared;
using Microsoft.AspNetCore.Http;

namespace FileService.Appliation.Services.IO;

public interface IS3Service
{
    Task<string> GetFile(string bucketName, string fileName, CancellationToken cancellationToken);
    
    Task<string> PutFile(string bucketName, string fileName, Stream stream, CancellationToken cancellationToken);
}