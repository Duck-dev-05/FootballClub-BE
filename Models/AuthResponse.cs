namespace FootballClub_Backend.Models;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public UserDto? User { get; set; }
}

public class ErrorResponse
{
    public string Title { get; set; }
    public string Message { get; set; }

    public ErrorResponse(string title, string message)
    {
        Title = title;
        Message = message;
    }
} 