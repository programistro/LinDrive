using LinDrive.Contracts.Dtos.IO;

namespace FileService.Appliation.Services.IO;

public interface IMediaService
{
    Task UploadFile(UploadFileDto dto, CancellationToken cancellationToken);
}