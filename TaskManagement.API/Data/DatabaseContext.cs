using Microsoft.EntityFrameworkCore;

namespace TaskManagement.API.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    
}