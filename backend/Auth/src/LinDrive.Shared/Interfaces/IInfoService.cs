namespace LinDrive.Shared.Interfaces;

public interface IInfoService
{
    Task<string> GetLocation(string ip, CancellationToken cancellationToken);
}