namespace LinDrive.Shared.Interfaces;

public interface ICryptoService
{
    string CreatePasswordHash(string password);
}