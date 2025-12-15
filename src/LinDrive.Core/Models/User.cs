using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace LinDrive.Core.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    
    public string Email { get; set; }
    
    public string SeedHash { get; set; }
    
    public string SeedSalt { get; set; }

    public List<UserFile> Files = new List<UserFile>();
    
    public string? AccessToken { get; set; }

    public ICollection<string> AccessTokens { get; set; } = new List<string>();
}