<<<<<<< HEAD
=======
using System.ComponentModel.DataAnnotations;

>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
namespace FootballClub_Backend.Models.Responses;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
<<<<<<< HEAD
    public UserDto User { get; set; } = new();
=======
    public UserDto User { get; set; } = null!;
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
} 