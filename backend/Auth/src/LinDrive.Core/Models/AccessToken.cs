using System.ComponentModel.DataAnnotations;
using LinDrive.Core.Models;

namespace LinDrive.Core.Models;

public class AccessToken
{
    [Key]
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public DateTime ExpiresAt { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? Revoked { get; set; }
    
    public string Token { get; set; }

    public bool IsActive => Revoked == null && !IsExpired;
    
    public bool IsExpired => ExpiresAt < DateTime.UtcNow;
    
    public string Agent { get; set; }
    
    public string Location { get; set; }
}