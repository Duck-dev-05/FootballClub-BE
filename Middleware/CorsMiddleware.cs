using Microsoft.AspNetCore.Http;

namespace FootballClub_Backend.Middleware;

public class CorsMiddleware
{
    private readonly RequestDelegate _next;

    public CorsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Method == "OPTIONS")
        {
<<<<<<< HEAD
            context.Response.Headers["Access-Control-Allow-Origin"] = "http://localhost:3000";
            context.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization";
            context.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE, OPTIONS";
            context.Response.Headers["Access-Control-Allow-Credentials"] = "true";
=======
            context.Response.Headers.Add("Access-Control-Allow-Origin", "http://localhost:3000");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Authorization");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS");
            context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
            context.Response.StatusCode = 200;
            return;
        }

        await _next(context);
    }
} 