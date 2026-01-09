using System.ComponentModel.DataAnnotations;

namespace LinDrive.Core.Models;

public class RefreshToken
{
    [Key]
    public Guid Id { get; set; }
    
    public string Token { get; set; }
    
    public DateTime Expires { get; set; }
    
    public string UserId { get; set; }
    
    public bool IsActive => Expires > DateTime.UtcNow;
}