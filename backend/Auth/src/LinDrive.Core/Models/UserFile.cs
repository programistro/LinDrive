using System.ComponentModel.DataAnnotations;

namespace LinDrive.Core.Models;

public class UserFile
{
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Path { get; set; }
    
    public string Extension { get; set; }
    
    public string ContentType { get; set; }
    
    public long Length { get; set; }
}