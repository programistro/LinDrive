using LinDrive.Application.Services.IO.Interfaces;
using LinDrive.Contracts.Dtos.IO;

namespace FileService.Appliation.Services.IO;

public class MediaService : IMediaService
{
    public async Task UploadFile(UploadFileDto dto, CancellationToken cancellationToken)
    {
        if (dto.File.Length > 0)
        {
            var fileName = dto.File.FileName;
            var path = $@"files/{dto.Email}/{fileName}";
            
            var directory = Path.GetDirectoryName(path);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            using (var stream = System.IO.File.Create(path))
            {
                await dto.File.CopyToAsync(stream, cancellationToken);
            }
        }
    }
}