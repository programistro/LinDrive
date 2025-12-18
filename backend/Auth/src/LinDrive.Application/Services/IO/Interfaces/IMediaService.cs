using LinDrive.Contracts.Dtos.IO;

namespace LinDrive.Application.Services.IO.Interfaces;

public interface IMediaService
{
    Task UploadFile(UploadFileDto dto, CancellationToken cancellationToken);
}