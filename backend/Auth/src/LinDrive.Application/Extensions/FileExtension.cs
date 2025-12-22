using LinDrive.Contracts.Dtos.IO;
using LinDrive.Core.Models;
using Microsoft.AspNetCore.Http;

namespace LinDrive.Application.Extensions;

public static class FileExtension
{
    public static List<UserFileResponse> ToFilesResponses(this List<UserFile> files)
    {
        List<UserFileResponse> dtos = new List<UserFileResponse>();
        
        foreach (var item in files)
        {
            dtos.Add(new  UserFileResponse
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