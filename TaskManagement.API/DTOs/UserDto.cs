using System.ComponentModel.DataAnnotations;
using TaskManagement.API.Enums;

namespace TaskManagement.API.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
    public Role Role  { get; set; } = Role.Member;
}