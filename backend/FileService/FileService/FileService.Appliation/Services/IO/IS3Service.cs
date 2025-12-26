using LinDrive.Shared;
using Microsoft.AspNetCore.Http;
using Minio.DataModel.Result;

namespace FileService.Appliation.Services.IO;

public interface IS3Service
{
    Task<ListAllMyBucketsResult> GetListBuckets(CancellationToken cancellationToken);
    
    Task<string> GetFile(string bucketName, string fileName, CancellationToken cancellationToken);
    
    Task<string> PutFile(string bucketName, string fileName, Stream stream, CancellationToken cancellationToken);
}