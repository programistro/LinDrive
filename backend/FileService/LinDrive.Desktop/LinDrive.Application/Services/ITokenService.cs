namespace LinDrive.Application.Services;

public interface ITokenService
{
    string GetAccessToken();
    
    string GetRefreshToken();
    
    Task<string> SetAccessToken(string accessToken);
    
    Task<string> SetRefreshToken(string refreshToken);
}