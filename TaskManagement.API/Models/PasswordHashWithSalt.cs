namespace TaskManagement.API.Models;

public class PasswordHashWithSalt
{
    public string Hash { get; set; }
    public string Salt { get; set; }
}