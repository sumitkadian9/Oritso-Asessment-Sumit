using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.DTOs;

public class RegisterDto : UserDto
{
    [Required]
    public string Password { get; set; }
}