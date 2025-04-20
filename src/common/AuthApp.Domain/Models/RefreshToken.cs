namespace AuthApp.Domain.Models;

public class RefreshToken
{
    public string Id { get; set; } = Guid.CreateVersion7().ToString();
    public string UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Revoked { get; set; }
    public bool IsRevoked => Revoked.HasValue;
}
