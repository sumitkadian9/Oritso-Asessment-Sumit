namespace TaskManagement.API.DTOs;

public class UpdatePasswordDto
{
    public Guid UserId { get; set; }
    public string Password { get; set; }
}