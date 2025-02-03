using FootballClub_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FootballClub_Backend.Data;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();
        try
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            
            logger.LogInformation("Starting database initialization");
            
            // Add your initialization logic here
            if (!context.Users.Any())
            {
                logger.LogInformation("Adding default admin user");
                var adminUser = new User
                {
                    Username = "admin",
                    Email = "admin@example.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Role = "admin"
                };
                context.Users.Add(adminUser);
                await context.SaveChangesAsync();
                logger.LogInformation("Added default admin user successfully");
            }
            
            logger.LogInformation("Database initialization completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }
} 