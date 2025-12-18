namespace LinDrive.Contracts.Dtos;

public record UserResponseDto
{
    public required string Email { get; set; }
    
    public required Guid Id { get; set; }
}