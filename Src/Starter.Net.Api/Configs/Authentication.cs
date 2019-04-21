namespace Starter.Net.Api.Configs
{
    public class PasswordRequirements
    {
        public bool RequireDigit { set; get; }
        public int RequiredLength { set; get; }
        public bool RequireAlphanumeric { set; get; }
        public bool RequireUppercase { set; get; }
        public bool RequireLowercase { set; get; }
    }

    public class Lockouts
    {
        public bool AllowedForNewUsers { set; get; }
        public int DefaultLockoutTimeInMinutes { set; get; }
        public int MaxAllowedFailedAttempts { set; get; }
    }

    public class UsernameRequirements
    {
        public bool RequireUniqueEmail { set; get; }
        public string AllowedCharactersInUsername { set; get; }
    }

    public class JwtBearerOptions
    {
        public string Audience { set; get; }
        public string SigningKey { set; get; }
        public string Issuer { set; get; }
        public int JwtTtl { set; get; }
    }

    public class Authentication
    {
        public PasswordRequirements PasswordRequirements { set; get; }
        public Lockouts Lockouts { set; get; }
        public UsernameRequirements UsernameRequirements { set; get; }
        public JwtBearerOptions JwtBearerOptions { set; get; }
        public bool InvitationOnlyMode { set; get; }
    }
}
