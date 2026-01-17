using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Common;
using TaskManagement.API.Entities;
using TaskManagement.API.Models;

namespace TaskManagement.API.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<TodoTask> TodoTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {   
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

        var adminId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        PasswordHashWithSalt pw = PasswordHelper.GetPasswordWithSalt("admin");
        modelBuilder.Entity<User>().HasData(new User
        {
            Id = adminId,
            FirstName = "Admin",
            LastName = "User",
            Email = "admin@tmapp.com",
            Role = Enums.Role.Admin,
            PasswordHash = pw.Hash,
            PasswordSalt = pw.Salt,
            CreatedOn = 1768588200,
            LastUpdatedOn = 1768588200,
            CreatedBy = adminId,
            LastUpdatedBy = adminId
        });

        base.OnModelCreating(modelBuilder);
    }
}