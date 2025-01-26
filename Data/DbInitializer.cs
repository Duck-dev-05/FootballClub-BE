using FootballClub_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballClub_Backend.Data;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

        // Check if there are any users
        if (await context.Users.AnyAsync())
            return;

        // Create admin user
        var adminUser = new User
        {
            Username = "admin",
            Email = "admin@fcescuela.com",
            Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
            Role = "admin",
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(adminUser);
        await context.SaveChangesAsync();
    }
} 