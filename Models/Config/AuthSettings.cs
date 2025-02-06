namespace FootballClub_Backend.Models.Config;

public class AuthSettings
{
    public JwtSettings Jwt { get; set; } = new();
    public GoogleConfig Google { get; set; } = new();
    public FacebookConfig Facebook { get; set; } = new();
}

public class JwtSettings
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiryInDays { get; set; } = 7;
}

public class GoogleConfig
{
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}

public class FacebookConfig
{
    public string AppId { get; set; } = string.Empty;
    public string AppSecret { get; set; } = string.Empty;
} 