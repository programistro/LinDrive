using LinDrive.Contracts.Dtos;
using LinDrive.Contracts.Responses;
using LinDrive.Core.Models;

namespace LinDrive.Application.Extensions;

public static class UserExtension
{
    public static UserResponse ToUserResponse(this User user)
    {
        return new UserResponse()
        {
            UserId = user.UserId,
            AccessToken = user.AccessToken,
            AccessTokens = user.AccessTokens,
            Files = user.Files.ToFilesResponses()
        };
    }
}