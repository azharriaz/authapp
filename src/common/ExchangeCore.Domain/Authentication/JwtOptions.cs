namespace AuthApp.Domain.Authentication;

public class JwtOptions
{
    public static string SectionName => "JWT";
    public string Secret { get; set; }

    public string ValidIssuer { get; set; }

    public string ValidAudience { get; set; }
}
