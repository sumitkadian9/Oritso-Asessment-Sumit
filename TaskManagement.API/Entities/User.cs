using TaskManagement.API.Enums;

namespace TaskManagement.API.Entities;

public class User : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public Role Role { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }

    // Navigation prop
    public ICollection<TodoTask> TodoTasks { get; set; } = [];
}