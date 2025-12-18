using LinDrive.Contracts.Dtos.IO;
using LinDrive.Core.Models;
using Microsoft.AspNetCore.Http;

namespace LinDrive.Application.Extensions;

public static class FileExtension
{
    public static List<UserFileDto> FilesToDto(this List<UserFile> files)
    {
        List<UserFileDto> dtos = new List<UserFileDto>();
        
        foreach (var item in files)
        {
            dtos.Add(new  UserFileDto
            {
                ContentType = item.ContentType,
                Extension = item.Extension,
                Length = item.Length,
                Name = item.Name,
                Path = item.Path
            });
        }
        
        return dtos;
    }
}