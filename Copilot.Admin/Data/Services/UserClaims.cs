namespace Copilot.Admin.Data.Services;

public class UserClaims
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }

    public DateTime ExpiredAt { get; set; }
}