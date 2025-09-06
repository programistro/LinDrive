using System.ComponentModel.DataAnnotations;
using LinDrive.Core.Models;

namespace LinDrive.Core.Models;

public class AccessToken
{
    [Key]
    public Guid Id { get; set; }
    
    public User User { get; set; }
    
    public DateTime ExpiresAt { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string Token { get; set; }
}