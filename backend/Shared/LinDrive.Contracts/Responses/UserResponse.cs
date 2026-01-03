using LinDrive.Contracts.Dtos.IO;

namespace LinDrive.Contracts.Responses;

public record UserResponse
{
    public required string UserId { get; set; }
    
    public List<UserFileResponse> Files { get; set; }
    
    public string? AccessToken { get; set; }

    public ICollection<string> AccessTokens { get; set; } = new List<string>();
}