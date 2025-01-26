using System.ComponentModel.DataAnnotations;

namespace FootballClub_Backend.Models
{
    public class RoleUpdateRequest
    {
        [Required]
        [RegularExpression("^(admin|user)$", ErrorMessage = "Role must be either 'admin' or 'user'.")]
        public required string Role { get; set; }
    }
} 