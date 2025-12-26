using LinDrive.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Result;

namespace FileService.Appliation.Services.IO;

public class S3Service : IS3Service
{
    private readonly ILogger<S3Service> _logger;
    private readonly IMinioClient _minIo;

    public S3Service(ILogger<S3Service> logger, IMinioClient minIo)
    {
        _logger = logger;
        _minIo = minIo;
    }

    public async Task<ListAllMyBucketsResult> GetListBuckets(CancellationToken cancellationToken)
    {
        var result = await _minIo.ListBucketsAsync(cancellationToken);

        return result;
    }

    public async Task<string> GetFile(string bucketName, string fileName, CancellationToken cancellationToken)
    {
        var result = await _minIo.PresignedGetObjectAsync(new PresignedGetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithExpiry(3600));
        
        return result;
    }

    public async Task<string> PutFile(string bucketName, string fileName, Stream stream, CancellationToken cancellationToken)
    {
        var result = await _minIo.PutObjectAsync(new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithStreamData(stream), cancellationToken);
        
        return result.ObjectName;
    }
}