using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.DTOs;

public class LoginResponseDto : UserDto
{
    public string Token { get; set; }
}