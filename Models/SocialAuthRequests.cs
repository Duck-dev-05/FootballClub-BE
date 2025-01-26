using System.ComponentModel.DataAnnotations;

public class GoogleAuthRequest
{
    [Required]
    public string Credential { get; set; }
}

public class FacebookAuthRequest
{
    [Required]
    public string AccessToken { get; set; }
} 