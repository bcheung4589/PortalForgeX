namespace PortalForgeX.Infrastructure.Configurations;

public record EmailSettingsOptions
{
    public const string Section = "EmailSettings";

    public string FromName { get; set; } = null!;
    public string FromEmail { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public bool UseSsl { get; set; }
}
