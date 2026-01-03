using LinDrive.Contracts.Dtos;
using LinDrive.Contracts.Requestes;
using LinDrive.Core.Models;
using LinDrive.Shared;
using LinDrive.Shared.Enums;

namespace LinDrive.Application.Interfaces;

public interface IAuthService
{
    Task<Result<User>> Authenticate(AuthRequest request, UserAgent userAgent, CancellationToken cancellationToken);
}