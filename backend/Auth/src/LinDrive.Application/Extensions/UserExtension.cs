using LinDrive.Contracts.Dtos;
using LinDrive.Core.Models;

namespace LinDrive.Application.Extensions;

public static class UserExtension
{
    public static UserResponseDto ToResponseDto(this User user)
    {
        return new UserResponseDto()
        {
            Email = user.Email,
            Id = user.Id,
        };
    }
}