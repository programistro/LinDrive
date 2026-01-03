using System.Security.Cryptography;

namespace LinDrive.Shared.Interfaces;

public interface ICryptoService
{
    string CreatePasswordHash(string password);

    string RandomNonce(int lenght = 32)
    {
        byte[] bytes = RandomNumberGenerator.GetBytes(lenght);
        return Convert.ToBase64String(bytes);
    }
}