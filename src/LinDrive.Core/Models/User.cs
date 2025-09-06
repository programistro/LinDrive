using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace LinDrive.Core.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    public string Email { get; set; }
    
    public string PasswordHash { get; set; }

    public List<UserFile> Files = new List<UserFile>();
    
    public string? AccessToken { get; set; }

    public ICollection<AccessToken> AccessTokens { get; set; } = new List<AccessToken>();
}