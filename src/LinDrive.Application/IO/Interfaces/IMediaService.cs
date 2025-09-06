using LinDrive.Contracts.Dtos.IO;

namespace LinDrive.Application.IO.Interfaces;

public interface IMediaService
{
    Task UploadFile(UploadFileDto dto, CancellationToken cancellationToken);
}